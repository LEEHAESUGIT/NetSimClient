
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
//using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Channels;
//using System.Threading.Tasks;

//namespace NETSIM_Ver2.Assets.Script.NetworkPackage.AsyncTask
//{
//	internal class SystemSendTask : ILoopTask
//	{
//		private SystemProcessPipe _systemProcessPipe;
//		private PacketSender _sendManager;

//		internal SystemSendTask(SystemProcessPipe systemProcessPipe, PacketSender sendManager)
//		{
//			this._systemProcessPipe = systemProcessPipe;
//			this._sendManager = sendManager;
//		}

//		public async Task LoopAsync(CancellationToken serverShutDownToken)
//		{
//			while (!serverShutDownToken.IsCancellationRequested)
//			{
//				if (await _systemProcessPipe.OutBoundPipe.WaitForPipe(serverShutDownToken))
//				{
//					while (_systemProcessPipe.OutBoundPipe.TryRead(out ResultPacketContext context))
//					{
//						_sendManager.UniCast( context.Packet);
//					}
//				}
//			}
//		}
//	}
//}
