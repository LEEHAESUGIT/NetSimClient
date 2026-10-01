
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Handle;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Init
{
	internal class GamePacketRouterInit
	{
		private GameHandle _ingameHandle;

		internal GamePacketRouterInit(GameHandle ingameHandle)
		{
			this._ingameHandle = ingameHandle;
		}

		internal FrozenDictionary<EPacketID, Action<ReceivePacketContext>> InitIngamePacketRouter()
		{

			var tempHandlerMap = new Dictionary<EPacketID, Action<ReceivePacketContext>>();
			// Add HanlderAction;
			tempHandlerMap.Add(EPacketID.S_Spawn, (packet) => _ingameHandle.SpawnResponseHandle(packet));
			tempHandlerMap.Add(EPacketID.S_Move, (packet) => _ingameHandle.PlayerMoveHandle(packet));

			return tempHandlerMap.ToFrozenDictionary();
		}
	}
}

