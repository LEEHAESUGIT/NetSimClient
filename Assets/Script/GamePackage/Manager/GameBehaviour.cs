

using NETSIM_Ver2.Assets.Script.Component;
using NETSIM_Ver2.Assets.Script.Configration;
using NETSIM_Ver2.Assets.Script.GamePackage.Manager;
using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.GamePackage.Object;
using NETSIM_Ver2.Assets.Script.GamePackage.OverlayScript;
using NETSIM_Ver2.Assets.Script.Module;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame;
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
		private GamePacketPipe _gamePacketPipe;




		private readonly float _oneSec = 1f;
		//private readonly int _tickRate = 64;
		private double _targetTickMS;

		private int _interpolationCount = 0;




		private void Awake()
		{
			this._targetTickMS = _oneSec / ServerConfigurationData.TickRate;

			// FixedUpdate 를 1초 / 64틱 으로 제어함 
			// 0.015625
			Time.fixedDeltaTime = (float)_targetTickMS;

		}

		internal void Init(GamePacketPipe gamePacketPipe, CancellationTokenSource CTS)
		{
			this._gamePacketPipe = gamePacketPipe;
			this._gameModule = new GameModule(gamePacketPipe, CTS);
			LinkWire();
		}
			

		private void FixedUpdate()
		{
			OnTick(Time.fixedDeltaTime);
		}

		private void Update()
		{

		}

		internal void OnTick(float targetTickMs)
		{
			_gameModule.PacketProcess();
			NextMoveLogic();
		}


		private void LinkWire()
		{
			_gameModule.OnCreatePlayer += (player) => PlayerObjectSpawn(player);
			_gameModule.OnDeletePlayer += (player) => PlayerObjectDelete(player);
		}
		private void PlayerObjectSpawn(Player playerData)
		{
			GameObject Object = Instantiate(PlayerPrefab);
			PlayerObject PlayerObject = Object.GetComponent<PlayerObject>();
			PlayerObject.TransformOverlay = OverlayManager.TransformTexts[(int)playerData.StatusInfo.PlayerColor - 1];
			PlayerObject.GetComponent<MeshRenderer>().material = PlayerColor[(int)playerData.StatusInfo.PlayerColor];
			//PlayerData.Init(playerData.ClientInfo, playerData.StatusInfo, playerData.TransformInfo);

			if(playerData.ClientInfo.IsPlayable)
				PlayerObject.Init((float) _targetTickMS, playerData, new LocalInputController(_gamePacketPipe));
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


	internal class InterpolationSystem
	{
		/// <summary>
		/// 보간 틱레이틀를 10 틱으로 기준을 잡는다.													ex) 64틱 기준으로 1틱은 약 0.0156초 정도이므로 156ms가 보간을 처리하는 시간이 된다.
		/// 현재 타겟이 되는 오브젝트의 현재 포지션과 보간되는 포지션의 위치를 계산하여 오차를 낸다.	ex) CoordinateErrorPos = interpos Vector3 - transform.position 
		/// 오차를 보간틱 기준으로 나누어 틱당 이동 포지션을 구한다.									ex) OneTickInterpolationPos = CoordinateErrorPos / 10
		/// 틱당 이동포지션을 매틱마다 오브젝트의 이동에 추가한다.										ex) OneTickMovePos = NextMovePos + OneTickInterpolationPos
		/// 만약 10틱 안에 보간패킷이 도착한 경우, 최신의 보간패킷을 기준으로 다시 연산한다.
		/// </summary>

		private int _internpolationTickCount = 0;


		internal InterpolationSystem()
		{

		}
		
		// 
		internal void UpdateNewInterpolation()
		{
			this._internpolationTickCount = 0;
		}
		// Return Tick of InterpolationPosition
		internal Vector3 NewInterpolationPositionOfTick( )
		{
			return new Vector3(0, 0, 0) / ServerConfigurationData.InterpolationTickUnit;
		}




	}
	internal struct InterpolationData
	{
		internal Vector3 TickbyInterpolation;
		internal ushort nextInterpolatiobeforetime;

	}


}