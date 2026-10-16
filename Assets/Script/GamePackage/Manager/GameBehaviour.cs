

using NETSIM_Ver2.Assets.Script.Component;
using NETSIM_Ver2.Assets.Script.GamePackage.Manager;
using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.GamePackage.Object;
using NETSIM_Ver2.Assets.Script.Module;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Handle;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Init;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace NETSIM_Ver2.Assets.Script.GamePackage.Manager
{
	internal class GameBehaviour : MonoBehaviour
	{
		// prefab 
		public GameObject PlayerPrefab;
		public List<Material> PlayerColor;


		public GameObject GameManager;
		[SerializeField]
		public List<GameObject> LivePlayers = new List<GameObject>();

		private Dictionary<Player, GameObject> _playerMapper = new();

		private GameModule _gameModule;
		private GamePacketPipe _gamePacketPipe;



		private Queue<Vector3> _tickDelayOfNextTransform = new();

		private readonly float _oneSec = 1f;
		private readonly int _tickRate = 64;
		private double _targetTickMS;


		private void Awake()
		{
			this._targetTickMS = _oneSec / _tickRate;

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
			_gameModule.OnTick(Time.fixedDeltaTime);
		}

		private void Update()
		{

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
			Player PlayerData = PlayerObject.PlayerData;
			
			PlayerObject.GetComponent<MeshRenderer>().material = PlayerColor[(int)PlayerData.StatusInfo.PlayerColor];
			PlayerData.Init(playerData.ClientInfo, playerData.StatusInfo, playerData.TransformInfo);

			if(playerData.ClientInfo.IsPlayable)
				PlayerObject.Init((float) _targetTickMS, PlayerData , new LocalInputController(_gamePacketPipe));
			if(!playerData.ClientInfo.IsPlayable)
				PlayerObject.Init((float) _targetTickMS, PlayerData , new RemotePacketController());
			
			this.LivePlayers.Add(Object);
			this._playerMapper.Add(PlayerData , Object);
		}
		private void PlayerObjectDelete(Player playerData)
		{
			if(_playerMapper.TryGetValue(playerData , out GameObject playerObject))
			{
				this.LivePlayers.Remove(playerObject);
				this._playerMapper.Remove(playerData);
			}

		}




		internal void CallStart()
		{
			_gameModule.Start();
		}











	}
}