using Microsoft.Win32;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForSystem;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Handle;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Socket;
using System;
using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Manager
{
	internal class SessionManager
	{
		private Session _session;

		internal Session LiveSession { get => _session; }


	
		internal SessionManager()
		{
			_session = new Session();

		}

		internal void ActiveSession(int portNum , ModulePipe modulePipe,  CancellationToken shutDownToken)
		{
			_session.Init(modulePipe , shutDownToken);

			_ = _session.Start(portNum);
		}
		internal void Stop()
		{
			_session.Disconnect();
		}
		internal void TryExcute(Action<Session> foreachAction)
		{
			foreachAction(_session);
		}

	}
}
