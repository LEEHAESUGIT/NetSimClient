



using NETSIM_Ver2.Assets.Script.GamePackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Handle;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Init;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Function
{
	internal class GamePacketProcess
	{
		// Manageer
		private PlayerManager _playerManager;

		// Pipe
		private GamePacketPipe _gamePacketPipe;

		// Router
		private GameHandle _gameHandle;
		private GamePacketRouter _gamePacketRouter;
		private GamePacketRouterInit _gamePacketRouterInit;


		internal GamePacketProcess(GameManager gameManager, PlayerManager playerManager, GamePacketPipe gamePacketPipe)
		{
			// Manageer
			this._playerManager = playerManager;

			// Pipe
			this._gamePacketPipe = gamePacketPipe;

			// Router
			this._gameHandle = new GameHandle(gameManager, _playerManager, _gamePacketPipe);
			this._gamePacketRouterInit = new GamePacketRouterInit(_gameHandle);
			this._gamePacketRouter = new GamePacketRouter(_gamePacketRouterInit.InitIngamePacketRouter());
		}

		private void GameAndPacketWire()
		{
			//this._gameHandle.OnCreatePlayerObject (player) => _game 
		}

		internal void Start(CancellationToken shutDownToken)
		{
			_ = GameProcessLoopAsync(shutDownToken);
		}






		public async Task GameProcessLoopAsync(CancellationToken shutDownToken)
		{
			try
			{
				while (!shutDownToken.IsCancellationRequested)
				{
					if (await _gamePacketPipe.InBoundPipe.WaitForPipe(shutDownToken))
					{
						while (_gamePacketPipe.InBoundPipe.TryRead(out ReceivePacketContext context))
						{
							_gamePacketRouter.Apply(context);
						}
					}
				}
			}
			catch (Exception ex) { Console.WriteLine($"[Error] : {ex}"); }
		}



	}

}