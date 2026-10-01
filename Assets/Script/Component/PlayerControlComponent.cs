


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

			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");

			EMoveDirFlag currentFlag = EMoveDirFlag.None;
			if (h > 0)
			{
				currentFlag |= EMoveDirFlag.Right;
			}
			else if (h < 0)
			{
				currentFlag |= EMoveDirFlag.Left;
			}

			// 세로 방향 플래그 설정 (동시 입력 시 대각선 이동 지원)
			if (v > 0)
			{
				currentFlag |= EMoveDirFlag.Up;
			}
			else if (v < 0)
			{
				currentFlag |= EMoveDirFlag.Down;
			}

			_gamePacketPipe.OutBoundPipe.TryWrite(new ResultPacketContext
			{
				Packet = new C2S_MovePacket(player.PlayerData.ClientInfo.PlayerID , currentFlag ,0 , 0)
			});

			Vector3 moveDir = FlagToVector(currentFlag);
			player.Move(moveDir);

		}

		public Vector3 FlagToVector(EMoveDirFlag flag)
		{
			float x = 0f;
			float z = 0f;

			if ((flag & EMoveDirFlag.Right) != 0) x += 1f;
			if ((flag & EMoveDirFlag.Left) != 0) x -= 1f;
			if ((flag & EMoveDirFlag.Up) != 0) z += 1f;
			if ((flag & EMoveDirFlag.Down) != 0) z -= 1f;

			Vector3 dir = new Vector3(x, 0f, z);
			return dir.sqrMagnitude > 0f ? dir.normalized : Vector3.zero;
		}

	}


	internal class RemotePacketController : IPlayerController
	{
		private MoveFlagWraper _moveFlagWraper;


		internal RemotePacketController(MoveFlagWraper moveFlagWraper)
		{
			this._moveFlagWraper = moveFlagWraper;
		}

		public void OnUpdate(PlayerObject player)
		{
			if (_moveFlagWraper.Output() == EMoveDirFlag.None)
				player.InterlockedMove(Vector3.zero);
			if (_moveFlagWraper.Output() == EMoveDirFlag.Up)
				player.InterlockedMove(Vector3.up);
			if (_moveFlagWraper.Output() == EMoveDirFlag.Down)
				player.InterlockedMove(Vector3.down);
			if (_moveFlagWraper.Output() == EMoveDirFlag.Left)
				player.InterlockedMove(Vector3.up);
			if (_moveFlagWraper.Output() == EMoveDirFlag.Right)
				player.InterlockedMove(Vector3.right);
		}

	}

}