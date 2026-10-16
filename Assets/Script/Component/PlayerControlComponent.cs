


using NETSIM_Ver2.Assets.Script.GamePackage.Object;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;
using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame;

namespace NETSIM_Ver2.Assets.Script.Component
{
	internal class LocalInputController : IPlayerController
	{
		private GamePacketPipe _gamePacketPipe;
		internal LocalInputController(GamePacketPipe gamePacketPipe)
		{
			this._gamePacketPipe = gamePacketPipe;
		}

		public void OnUpdate(PlayerObject player)
		{

			float h = Input.GetAxisRaw("Horizontal");
			float v = Input.GetAxisRaw("Vertical");

			EMoveDirFlag currentFlag = EMoveDirFlag.None;
			if (h > 0) currentFlag |= EMoveDirFlag.Right;
			else if (h < 0) currentFlag |= EMoveDirFlag.Left;
			if (v > 0) currentFlag |= EMoveDirFlag.Up;
			else if (v < 0) currentFlag |= EMoveDirFlag.Down;


			_gamePacketPipe.OutBoundPipe.TryWrite(new ResultPacketContext
			{
				Packet = new C2S_MovePacket(player.PlayerData.ClientInfo.PlayerID, currentFlag)
			});

			player.Move();

		}
	}


	internal class RemotePacketController : IPlayerController
	{

		internal RemotePacketController()
		{
		}

		public void OnUpdate(PlayerObject player)
		{
			player.Move();
		}

	}

}