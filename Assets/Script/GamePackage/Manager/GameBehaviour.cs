

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

		private GameModule _gameModule;


		public GameObject GameManager;

		private GameManager _gameManager;


		private void Awake()
		{
			if(GameManager != null)
			this._gameManager = GameManager.GetComponent<GameManager>();
			
		}


		private void Start()
		{

		}
		internal void Init(GamePacketPipe gamePacketPipe, CancellationTokenSource CTS)
		{
			if (this._gameManager == null && GameManager != null)
			{
				this._gameManager = GameManager.GetComponent<GameManager>();
			}
			this._gameModule = new GameModule(_gameManager, gamePacketPipe, CTS);
		}

		internal void CallStart()
		{
			_gameModule.Start();
		}









	}
}