using NETSIM_Ver2.Assets.Script.GamePackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Function;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using System.Threading;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Service
{


	internal class GamePacketApplyService
	{
		// Manager
		private PlayerManager _playerManager;

		// Pipe
		private GamePacketPipe _gamePacketPipe;

		// Function
		private GamePacketProcess _gamePacketProcess;


		internal GamePacketApplyService(GameManager gameManager , PlayerManager playerManager, GamePacketPipe gamePacketPipe)
		{
			// Manager
			this._playerManager = playerManager;
			// Pipe
			this._gamePacketPipe = gamePacketPipe;
			// Function
			this._gamePacketProcess = new GamePacketProcess( gameManager, _playerManager, _gamePacketPipe);
		}
		internal void Start(CancellationToken shutDownToken)
		{
			_gamePacketProcess.Start(shutDownToken);
		}


	}


}