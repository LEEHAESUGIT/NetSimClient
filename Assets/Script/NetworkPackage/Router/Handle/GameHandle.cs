



using NETSIM_Ver2.Assets.Script.GamePackage;
using NETSIM_Ver2.Assets.Script.GamePackage.Manager;
using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.GamePackage.Object;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using System;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Handle
{
	internal class GameHandle
	{
		// Manager
		private PlayerManager _playerManager;
		
		// Pipe
		private GamePacketPipe _gamePacketPipe;

		// Delegate
		internal Action<Player> OnCreatePlayerObject;

		internal GameHandle(PlayerManager playerManager, GamePacketPipe gamePacketPipe)
		{
			this._playerManager = playerManager;
			this._gamePacketPipe = gamePacketPipe;
		}

		internal void SpawnResponseHandle(ReceivePacketContext context)
		{
			if (context.Packet is S2C_SpawnResponse Packet)
			{
				if (_playerManager.CreatePlayer(Packet.ResponsePlayerID, out Player createPlayer))
				{
					createPlayer.Init(
						new ClientInfo(Packet.ResponsePlayerID, true),
						new StatusInfo(5f, (EPlayerColor)Packet.TargetColorEnum , EMoveDirFlag.None)
						);

					UnityEngine.Debug.Log($"{Packet.ResponsePlayerID}");
					UnityEngine.Debug.Log($"SpawnPlayer");
				}
			}
		}

		internal void PlayerMoveHandle(ReceivePacketContext context)
		{
			if(context.Packet is S2C_MovePacket Packet)
			{
				if(_playerManager.TryGetPlayer(Packet.PlayerID , out Player player))
				{
					player.MoveTick(Packet.MoveFlag);
				}
			}
		}

		internal void TransformInterpolationHandle(ReceivePacketContext context)
		{
			if(context.Packet is S2C_TransformInterpolationPacket Packet)
			{
				// TODO
			}
		}
	}
}

