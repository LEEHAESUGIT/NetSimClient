
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.Tool;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Function
{
	internal class PacketSender
	{
		// Local
		private PacketFormatter _formatter = new();

		// Manager
		private SessionManager _sessionManager;

		// Pipe
		private ModulePipe _modulePipe;

		internal PacketSender(SessionManager sessionManager , ModulePipe modulePipe)
		{
			// Manager
			this._sessionManager = sessionManager;

			// Pipe
			this._modulePipe = modulePipe;
		}

		internal void Start(CancellationToken shutDownToken)
		{
			_ = SystemSendLoopAsync(shutDownToken);
			_ = GameSendLoopAsync(shutDownToken);
		}


		internal void UniCast(IPacket packet)
		{
			ReadOnlyMemory<byte> packetData = _formatter.Format(packet);

			_sessionManager.TryExcute(session =>
			{
				_ = session.Send(packetData);
			});

		}



		private async Task SystemSendLoopAsync(CancellationToken shutDownToken)
		{
			while (!shutDownToken.IsCancellationRequested)
			{
				if (await _modulePipe.SystemPacketPipe.OutBoundPipe.WaitForPipe(shutDownToken))
				{
					while (_modulePipe.SystemPacketPipe.OutBoundPipe.TryRead(out ResultPacketContext context))
					{
						UniCast(context.Packet);
					}
				}
			}
		}
		private async Task GameSendLoopAsync(CancellationToken shutDownToken)
		{
			while (!shutDownToken.IsCancellationRequested)
			{
				if (await _modulePipe.GamePacketPipe.OutBoundPipe.WaitForPipe(shutDownToken))
				{
					while (_modulePipe.GamePacketPipe.OutBoundPipe.TryRead(out ResultPacketContext context))
					{
						UniCast(context.Packet);
					}
				}
			}
		}





	}
}
