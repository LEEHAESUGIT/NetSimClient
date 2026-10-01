using Microsoft.Win32;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Service
{
	internal class SessionLifeCycleService
	{
		// Manager
		private SessionManager _sessionManager;

		// Pipe
		private ModulePipe _modulePipe;

		internal SessionLifeCycleService(SessionManager sessionManager, ModulePipe modulePipe)
		{
			this._sessionManager = sessionManager;
			this._modulePipe = modulePipe;
		}

		internal void Start(int portNum, CancellationToken shutDownToken)
		{
			OnSessionConnected(portNum, _modulePipe, shutDownToken);


			//_ = SessionHeartbeatCycle(shutDownToken);
		}


		//private async Task SessionHeartbeatCycle(CancellationToken shutDownToken)
		//{
		//	try
		//	{
		//		var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

		//		while (await timer.WaitForNextTickAsync(shutDownToken))
		//		{
		//			long currentTick = Environment.TickCount64;

		//			if (currentTick - _sessionManager.LiveSession.LastHeartBeatTick > 15000)
		//			{
		//				_sessionManager.Stop();
		//			}
		//		}
		//	}
		//	catch (Exception)
		//	{
		//	}
		//}


		private void OnSessionConnected(int portNum, ModulePipe modulePipe, CancellationToken shutDownToken)
		{
			try
			{
				_sessionManager.ActiveSession(portNum, modulePipe, shutDownToken);
			}
			catch (Exception) { }
		}
		internal void OnSessionDisconnected(int sessionID)
		{
			_sessionManager.Stop();
		}


	}
}
