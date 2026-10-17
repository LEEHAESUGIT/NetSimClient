
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
			tempHandlerMap.Add(EPacketID.S_PlayableObjectSpawn, (packet) => _ingameHandle.PlayableObjectSpawnResponseHandle(packet));
			tempHandlerMap.Add(EPacketID.S_NonPlayableObjectSpawn, (packet) => _ingameHandle.NonPlayableObjectSpawnResponseHandle(packet));

			tempHandlerMap.Add(EPacketID.S_LivePlayerRenewal, (packet) => _ingameHandle.AllPlayerSpawnHandle(packet));

			tempHandlerMap.Add(EPacketID.S_Move, (packet) => _ingameHandle.PlayerMoveHandle(packet));
			tempHandlerMap.Add(EPacketID.S_Transform, (packet) => _ingameHandle.TransformInterpolationHandle(packet));

			return tempHandlerMap.ToFrozenDictionary();
		}
	}
}

