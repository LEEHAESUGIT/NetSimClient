




//using NETSIM_Ver2.Assets.Script.NetworkPackage.AsyncTask;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Router;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Handle;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Init;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Pipe;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Socket;
//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using System.Threading.Tasks;
//using Unity.VisualScripting;

//namespace NETSIM_Ver2.Assets.Script.NetworkPackage
//{
//	internal class ClientMain
//	{
//		//private readonly TcpClient _client;
//		private CancellationTokenSource _clientCTS;

//		private SystemProcessPipe _systemProcessPipe;
//		private IngameProcessPipe _ingameProcessPipe;

//		private SessionManager _sessionManager;
//		private PacketSender _sendManager;

//		private SystemHandle _systemHandle;
//		//private IngameHandle _ingameHandle;

//		private SystemPacketRouterInit _systemPacketRouterInit;
//		//private IngamePacketRouterInit _ingamePacketRouterInit;

//		private SystemPacketRouter _systemPacketRouter;
//		//private InGamePacketRouter _ingamePacketRouter;

//		private SystemProcessTask _systemProcessTask;
//		//private IngameProcessTask _ingameProcessTask;
//		private SystemSendTask _systemSendTask;
//		private IngameSendTask _ingameSendTask;



//		internal ClientMain(IngameProcessPipe ingameProcessPipe)
//		{
//			this._clientCTS = new CancellationTokenSource();

//			this._systemProcessPipe = new SystemProcessPipe();
//			this._ingameProcessPipe = ingameProcessPipe;

//			this._sessionManager = new SessionManager();
//			this._sendManager = new PacketSender(_sessionManager);

//			this._systemHandle = new SystemHandle(_systemProcessPipe, _sessionManager);
//			//this._ingameHandle = new IngameHandle(_ingameProcessPipe, );

//			this._systemPacketRouterInit = new SystemPacketRouterInit(_systemHandle);
//			//this._ingamePacketRouterInit = new IngamePacketRouterInit(_ingameHandle);

//			this._systemPacketRouter = new SystemPacketRouter(_systemPacketRouterInit.InitSystemPacketRouter());
//			//this._ingamePacketRouter = new InGamePacketRouter(_ingamePacketRouterInit.InitIngamePacketRouter());

//			this._systemProcessTask = new SystemProcessTask(_systemProcessPipe, _systemPacketRouter);
//			//this._ingameProcessTask = new IngameProcessTask(_ingameProcessPipe, _ingamePacketRouter);
//			this._systemSendTask = new SystemSendTask(_systemProcessPipe, _sendManager);
//			this._ingameSendTask = new IngameSendTask(_ingameProcessPipe, _sendManager);
//		}


//		internal void Start()
//		{
//			try
//			{
//				_sessionManager.ActiveSession(_sendManager, _ingameProcessPipe, _systemProcessPipe , _clientCTS.Token);
				
//				CallLoop();
//				//return true;
//			}
//			catch (Exception)
//			{
//				//return false;
//			}
//		}
//		internal bool Stop()
//		{
//			try
//			{
//				_sessionManager.Stop();
//				return true;
//			}
//			catch (Exception) 
//			{
//				return false;
//			}
//		}



//		private void CallLoop()
//		{
//			_ = _systemProcessTask.LoopAsync(_clientCTS.Token);
//			//_ = _ingameProcessTask.LoopAsync(_clientCTS.Token);

//			_ = _systemSendTask.LoopAsync(_clientCTS.Token);
//			_ = _ingameSendTask.LoopAsync(_clientCTS.Token);
//		}

//	}
//}