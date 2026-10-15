

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


		//
		//private GameManager _gameManager;


		private GameModule _gameModule;



		private Queue<Vector3> _tickDelayOfNextTransform = new();

		private readonly float _oneSec = 1f;
		private readonly int _tickRate = 64;
		private double _targetTickMS;


		private void Awake()
		{
			//if(GameManager != null)
			//this._gameManager = GameManager.GetComponent<GameManager>();

			this._targetTickMS = _oneSec / _tickRate;


			// FixedUpdate 를 1초 / 64틱 으로 제어함 
			// 0.015625
			Time.fixedDeltaTime = (float)_targetTickMS;

		}


		//private void Start()
		//{

		//}
		internal void Init(GamePacketPipe gamePacketPipe, CancellationTokenSource CTS)
		{
			//if (this._gameManager == null && GameManager != null)
			//{
			//	this._gameManager = GameManager.GetComponent<GameManager>();
			//}
			this._gameModule = new GameModule(gamePacketPipe, CTS);
		}



		private void FixedUpdate()
		{
			_gameModule.OnTick(Time.fixedDeltaTime);

		}

		private void Update()
		{

		}



		private void PlayerSpawn()
		{
			var obj = Instantiate(PlayerPrefab);
			var objComp = obj.GetComponent<Player>();
			objComp.Init(new ClientInfo() , new StatusInfo());

			this.LivePlayers.Add(obj);
		}





		internal void CallStart()
		{
			_gameModule.Start();
		}











	}
}