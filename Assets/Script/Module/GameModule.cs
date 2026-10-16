using NETSIM_Ver2.Assets.Script.GamePackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Handle;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Init;
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

		// Router
		private GameHandle _gameHandle;
		private GamePacketRouter _gamePacketRouter;
		private GamePacketRouterInit _gamePacketRouterInit;

		internal GameModule(GamePacketPipe gamePacketPipe, CancellationTokenSource clientCTS)
		{
			// Local
			this._cts = clientCTS;

			// Manager
			this._playerManager = new PlayerManager();
			//this._gameManager = gameManager;

			// Pipe
			this._gamePacketPipe = gamePacketPipe;
			
			// Router
			this._gameHandle = new GameHandle(_playerManager, _gamePacketPipe);
			this._gamePacketRouterInit = new GamePacketRouterInit(_gameHandle);
			this._gamePacketRouter = new GamePacketRouter(_gamePacketRouterInit.InitIngamePacketRouter());


		}

		internal void Start()
		{
			//_gamePacketApplyService.Start(_cts.Token);
			_gamePacketPipe.OutBoundPipe.TryWrite(new ResultPacketContext
			{
				Packet = new C2S_SpawnRequest()
			});
		}


		internal void OnTick(float targetTickMs)
		{
			PacketProcess();
		}

		private void PacketProcess()
		{
			int maxProcessCount = 500;
			int currentCount = 0;

			while(currentCount < maxProcessCount && _gamePacketPipe.InBoundPipe.TryRead(out ReceivePacketContext context))
			{
				_gamePacketRouter.Apply(context);
				currentCount++;
			}
		}
		
	}
}