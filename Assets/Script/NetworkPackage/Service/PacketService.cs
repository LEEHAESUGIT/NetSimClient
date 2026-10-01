


using NETSIM_Ver2.Assets.Script.NetworkPackage.Function;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using System.Threading;
using UnityEditor;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Service
{
	internal class PacketService
	{
		// Manager
		private SessionManager _sessionManager;

		// Pipe
		private ModulePipe _modulePipe;

		// Function
		private PacketSender _packetSender;
		private SystemPacketProcess _systemPacketProcess;
		
		internal PacketService(SessionManager sessionManager , ModulePipe modulePipe)
		{
			// Manager
			this._sessionManager = sessionManager;
			// Pipe
			this._modulePipe = modulePipe;

			// Function
			this._packetSender = new PacketSender(_sessionManager , _modulePipe);
			this._systemPacketProcess = new SystemPacketProcess(_sessionManager , _modulePipe);
		}

		internal void Start(CancellationToken shutDownToken)
		{
			this._packetSender.Start(shutDownToken);
			this._systemPacketProcess.Start(shutDownToken);
		}


	}

}