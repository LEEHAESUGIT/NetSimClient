



using NETSIM_Ver2.Assets.Script.GamePackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Service;
using System.Threading;

namespace NETSIM_Ver2.Assets.Script.Module
{

	internal class GameModule
	{
		// Local
		private CancellationTokenSource _cts;

		// Manager
		private PlayerManager _playerManager;

		// Pipe
		private GamePacketPipe _gamePacketPipe;

		// Service
		private GamePacketApplyService _gamePacketApplyService;

		private GameManager _gameManager;

		internal GameModule(GameManager gameManager, GamePacketPipe gamePacketPipe, CancellationTokenSource clientCTS)
		{
			// Local
			this._cts = clientCTS;

			// Manager
			this._playerManager = new PlayerManager();
			this._gameManager = gameManager;

			// Pipe
			this._gamePacketPipe = gamePacketPipe;

			// Service
			this._gamePacketApplyService = new GamePacketApplyService(gameManager, _playerManager, _gamePacketPipe);


			this._gameManager.Init(_playerManager, gamePacketPipe);

		}

		internal void Start()
		{
			_gamePacketApplyService.Start(_cts.Token);
			_gamePacketPipe.OutBoundPipe.TryWrite(new ResultPacketContext
			{
				Packet = new C2S_SpawnRequest()
			});
		}

		internal void Stop()
		{

		}



	}



}