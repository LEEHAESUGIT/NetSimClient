//using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Pipe;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace NETSIM_Ver2.Assets.Script.NetworkPackage.AsyncTask
//{
//	internal class IngameSendTask : ILoopTask
//	{
//		private IngameProcessPipe _ingameProcessPipe;
//		private PacketSender _sendManager;
//		internal IngameSendTask(IngameProcessPipe ingameProcessPipe, PacketSender sendManager)
//		{
//			this._ingameProcessPipe = ingameProcessPipe;
//			this._sendManager = sendManager;
//		}


//		public async Task LoopAsync(CancellationToken serverShutDownToken)
//		{
//			while (!serverShutDownToken.IsCancellationRequested)
//			{
//				if (await _ingameProcessPipe.OutBoundPipe.WaitForPipe(serverShutDownToken))
//				{
//					while (_ingameProcessPipe.OutBoundPipe.TryRead(out ResultPacketContext context))
//					{
//						_sendManager.UniCast(context.Packet);
//					}
//				}
//			}
//		}
//	}
//}
