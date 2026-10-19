

using NETSIM_Ver2.Assets.Script.Component;
using NETSIM_Ver2.Assets.Script.Configration;
using NETSIM_Ver2.Assets.Script.GamePackage.Manager;
using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.GamePackage.Object;
using NETSIM_Ver2.Assets.Script.GamePackage.OverlayScript;
using NETSIM_Ver2.Assets.Script.LiveData;
using NETSIM_Ver2.Assets.Script.Module;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForSystem;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.WrapperForPacket;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Handle;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Init;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace NETSIM_Ver2.Assets.Script.GamePackage.Manager
{
	internal class GameBehaviour : MonoBehaviour
	{
		// prefab 
		public GameObject PlayerPrefab;
		public List<Material> PlayerColor;

		public OverlayManager OverlayManager;


		//public GameObject GameManager;
		[SerializeField]
		public List<GameObject> LivePlayers = new List<GameObject>();

		private Dictionary<Player, GameObject> _playerMapper = new();

		private GameModule _gameModule;
		private ModulePipe _modulePipe;




		private readonly float _oneSec = 1f;
		//private readonly int _tickRate = 64;
		private double _targetTickMS;

		//private int _interpolationCount = 0;

		private uint _tickCount = 0;


		private void Awake()
		{
			this._targetTickMS = _oneSec / ServerConfigurationData.TickRate;

			// FixedUpdate 를 1초 / 64틱 으로 제어함 
			// 0.015625
			Time.fixedDeltaTime = (float)_targetTickMS;

		}

		internal void Init(ModulePipe modulePipe, CancellationTokenSource CTS)
		{
			this._modulePipe = modulePipe;
			this._gameModule = new GameModule(_modulePipe.GamePacketPipe, CTS);
			LinkWire();
		}
			

		private void FixedUpdate()
		{
			OnTick(Time.fixedDeltaTime);
		
		}


		internal void OnTick(float targetTickMs)
		{
			//if (_tickCount % ServerConfigurationData.TickRate == 0)
			//	SystemPacketOperator.Instance.GenerateHeartbeatPacket();

			// received packet process
			_gameModule.PacketProcess();
			// logic process
			NextMoveLogic();
			// send packet process
			SystemPacketOperator.Instance.TakeOut(_modulePipe.SystemPacketPipe);
			GamePacketOperator.Instance.TakeOut(_modulePipe.GamePacketPipe);

			// IncreaseTick
			LiveServerData.Instance.IncreamentServerTick(_tickCount);
			_tickCount++;
		}



		private void LinkWire()
		{
			_gameModule.OnCreatePlayer += (player) => PlayerObjectSpawn(player);
			_gameModule.OnDeletePlayer += (player) => PlayerObjectDelete(player);
		}
		private void PlayerObjectSpawn(Player playerData)
		{
			Vector3 setPos = playerData.TransformInfo.NowPosition;
			Vector3 setDir = playerData.TransformInfo.NowDirection;
			Quaternion setRot = setDir.sqrMagnitude > 0.001f ? Quaternion.LookRotation(setDir) : Quaternion.identity;

			GameObject Object = Instantiate(PlayerPrefab , setPos , setRot);
			PlayerObject PlayerObject = Object.GetComponent<PlayerObject>();
			PlayerObject.TransformOverlay = OverlayManager.TransformTexts[(int)playerData.StatusInfo.PlayerColor - 1];
			PlayerObject.GetComponent<MeshRenderer>().material = PlayerColor[(int)playerData.StatusInfo.PlayerColor];
			//PlayerData.Init(playerData.ClientInfo, playerData.StatusInfo, playerData.TransformInfo);

			if(playerData.ClientInfo.IsPlayable)
				PlayerObject.Init((float) _targetTickMS, playerData, new LocalInputController(_modulePipe.GamePacketPipe));
			if(!playerData.ClientInfo.IsPlayable)
				PlayerObject.Init((float) _targetTickMS, playerData, new RemotePacketController());
			
			this.LivePlayers.Add(Object);
			this._playerMapper.Add(playerData, Object);
		}
		private void PlayerObjectDelete(Player playerData)
		{
			if(_playerMapper.TryGetValue(playerData , out GameObject playerObject))
			{
				this.LivePlayers.Remove(playerObject);
				this._playerMapper.Remove(playerData);
			}

		}

		// + 보간
		private void NextMoveLogic()
		{
			foreach (var player in LivePlayers)
			{
				player.GetComponent<PlayerObject>().OnUpdateMoveTick();
			}
		}

		

		internal void CallStart()
		{
			_gameModule.Start();
		}


	}


	
}