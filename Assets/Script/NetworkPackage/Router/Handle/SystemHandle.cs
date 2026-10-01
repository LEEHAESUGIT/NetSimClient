using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForSystem;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Handle
{
	internal class SystemHandle
	{
		// Manager
		private SessionManager _sessionManager;
		// Pipe
		private SystemPacketPipe _systemPacketPipe;


		internal SystemHandle(SessionManager sessionManager, SystemPacketPipe systemPacketPipe)
		{
			this._sessionManager = sessionManager;
			this._systemPacketPipe = systemPacketPipe;
		}

		internal void HeartBeatHeandle(ReceivePacketContext context)
		{
			// 세션 객체를 context 로 받는건 잘못되었음 세션 매니저를 통해 직접 관리해야함.
			if (context.Packet is S2CHeartBeat Packet)
			{
				// Session Send Time
				DateTime WasSessionSendTime = DateTimeOffset.FromUnixTimeMilliseconds(Packet.WasSendTime).UtcDateTime;

				long nowTick = Environment.TickCount;
				DateTime nowTime = DateTime.UtcNow;
				long nowTimeTypeLong = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();


				UnityEngine.Debug.Log("HeartBeats");
				_systemPacketPipe.OutBoundPipe.TryWrite(
					new ResultPacketContext
					{
						Packet = new C2SHeartBeat(nowTimeTypeLong, Packet.ServerTick, nowTick)
					});

			}
		}


		internal void InitSessionHandle(ReceivePacketContext context)
		{
			if (context.Packet is S2C_InitResponse Packet)
			{
				_sessionManager.TryExcute(session =>
				{
					session.InitSessionID(Packet.ForInitSessionID);
					UnityEngine.Debug.Log(Packet.ForInitSessionID);
				});
			}
		}





	}
}
