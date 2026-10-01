
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Router;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace NETSIM_Ver2.Assets.Script.NetworkPackage.AsyncTask
//{
//	internal class IngameProcessTask 
//	{
//		private GamePacketRouter _packetRouter;
//		private GamePacketPipe _gamePacketPipe;

//		internal IngameProcessTask(GamePacketPipe gamePacketPipe, GamePacketRouter packetRouter)
//		{
//			this._gamePacketPipe = gamePacketPipe;	
//			this._packetRouter = packetRouter;
//		}


//		public async Task LoopAsync(CancellationToken cancellationToken)
//		{
//			try
//			{
//				while (!cancellationToken.IsCancellationRequested)
//				{
//					if (await _gamePacketPipe.InBoundPipe.WaitForPipe(cancellationToken))
//					{
//						while (_gamePacketPipe.InBoundPipe.TryRead(out ReceivePacketContext context))
//						{
//							_packetRouter.Apply(context);
//						}
//					}
//				}
//			}
//			catch (Exception) { }
//		}
//	}



//	internal class SystemProcessTask
//	{
//		private SystemProcessPipe _systemProcessPipe;
//		private SystemPacketRouter _packetRouter;


//		internal SystemProcessTask(SystemProcessPipe systemProcessPipe, SystemPacketRouter packetRouter)
//		{
//			this._systemProcessPipe = systemProcessPipe;
//			this._packetRouter = packetRouter;
//		}


//		public async Task LoopAsync(CancellationToken cancellationToken)
//		{
//			try

//			{
//				while (!cancellationToken.IsCancellationRequested)
//				{
//					if (await _systemProcessPipe.InBoundPipe.WaitForPipe(cancellationToken))
//					{
//						while (_systemProcessPipe.InBoundPipe.TryRead(out ReceivePacketContext context))
//						{
//							_packetRouter.Apply(context);
//						}
//					}
//				}
//			}
//			catch (Exception) { }
//		}
//	}
//}




