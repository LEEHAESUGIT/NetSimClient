using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Handle;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Init
{
	internal class SystemPacketRouterInit
	{

		private SystemHandle _systemHandle;

		internal SystemPacketRouterInit(SystemHandle systemHandle)
		{
			this._systemHandle = systemHandle;
		}
		internal FrozenDictionary<EPacketID, Action<ReceivePacketContext>> InitSystemPacketRouter()
		{
			var tempHandlerMap = new Dictionary<EPacketID, Action<ReceivePacketContext>>();
			// Add HanlderAction;
			tempHandlerMap.Add(EPacketID.S_HeartBeat, (packet) => _systemHandle.HeartBeatHeandle(packet));
			tempHandlerMap.Add(EPacketID.S_Init, (packet) => _systemHandle.InitSessionHandle(packet));




			return tempHandlerMap.ToFrozenDictionary();
		}
	}
}
