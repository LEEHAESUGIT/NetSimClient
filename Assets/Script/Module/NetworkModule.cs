



using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Service;
using System.Threading;

namespace NETSIM_Ver2.Assets.Script.Module
{
	internal class NetworkModule
	{
		// Local
		private CancellationTokenSource _cts;

		// Manager
		private SessionManager _sessionManager;

		// Pipe
		private ModulePipe _moudlePipe;


		// Service
		private SessionLifeCycleService _sessionLifeCycleService;
		private PacketService _packetService;


		internal NetworkModule(ModulePipe modulePipe , CancellationTokenSource clientCTS)
		{
			// Local
			this._cts = clientCTS;

			// Manager
			this._sessionManager = new SessionManager();

			//Pipe
			this._moudlePipe = modulePipe;

			// Service
			this._sessionLifeCycleService = new SessionLifeCycleService(_sessionManager , _moudlePipe);
			this._packetService = new PacketService(_sessionManager , _moudlePipe);
		}

		internal void Strart()
		{
			this._sessionLifeCycleService.Start(9999,_cts.Token);
			this._packetService.Start(_cts.Token);


		}
		internal void Stop()
		{


		}




	}
}


