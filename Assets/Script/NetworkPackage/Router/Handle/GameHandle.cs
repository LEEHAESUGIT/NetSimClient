



using NETSIM_Ver2.Assets.Script.GamePackage;
using NETSIM_Ver2.Assets.Script.GamePackage.Manager;
using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.GamePackage.Object;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using System;
using UnityEngine;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Router.Handle
{
	internal class GameHandle
	{
		// Manager
		private PlayerManager _playerManager;

		// Pipe
		private GamePacketPipe _gamePacketPipe;

		// Delegate
		//internal Action<Player> OnCreatePlayerObject;

		internal GameHandle(PlayerManager playerManager, GamePacketPipe gamePacketPipe)
		{
			this._playerManager = playerManager;
			this._gamePacketPipe = gamePacketPipe;
		}

		internal void AllPlayerSpawnHandle(ReceivePacketContext context)
		{
			if (context.Packet is S2C_LivePlayerRenewalPacket Packet)
			{
				for (int i = 0; i < Packet.LivePlayerCount; i++)
				{
					if (_playerManager.CreatePlayer(Packet.Snapshots[i].PlayerID))
					{
						_playerManager.InitPlayer(
							Packet.Snapshots[i].PlayerID,
							new ClientInfo(Packet.Snapshots[i].PlayerID, false),
							new StatusInfo(5f, (EPlayerColor)Packet.InfoWrapper[i].PlayerColor, EMoveDirFlag.None),
							new TransformInfo(
								new Vector3(
									Packet.Snapshots[i].PosX,
									Packet.Snapshots[i].PosY,
									Packet.Snapshots[i].PosZ),
								new Vector3(
									Packet.Snapshots[i].DirX,
									Packet.Snapshots[i].DirY,
									Packet.Snapshots[i].DirZ
									))
							);

						UnityEngine.Debug.Log($"{Packet.Snapshots[i].PlayerID}");
						UnityEngine.Debug.Log($"PlayerSpawn : NonPlayable, PlayerID : {Packet.Snapshots[i].PlayerID} ");
					}
				}


			}
		}

		internal void PlayableObjectSpawnResponseHandle(ReceivePacketContext context)
		{
			if (context.Packet is S2C_AnsPlayableObjectSpawnResponse Packet)
			{
				if (_playerManager.CreatePlayer(Packet.ResponsePlayerID))
				{
					_playerManager.InitPlayer(
						Packet.ResponsePlayerID,
						new ClientInfo(Packet.ResponsePlayerID, true),
						new StatusInfo(5f, (EPlayerColor)Packet.TargetColorEnum, EMoveDirFlag.None),
						new TransformInfo(new Vector3(0, 0, 0), new Vector3(0, 0, 0))
						);

					UnityEngine.Debug.Log($"{Packet.ResponsePlayerID}");
					UnityEngine.Debug.Log($"PlayerSpawn : Playable, PlayerID : {Packet.ResponsePlayerID} ");
				}
			}
		}
		internal void NonPlayableObjectSpawnResponseHandle(ReceivePacketContext context)
		{
			if (context.Packet is S2C_AnsNonPlayableObjectSpawnResponse Packet)
			{
				if (_playerManager.CreatePlayer(Packet.ResponsePlayerID))
				{
					_playerManager.InitPlayer(
						Packet.ResponsePlayerID,
						new ClientInfo(Packet.ResponsePlayerID, false),
						new StatusInfo(5f, (EPlayerColor)Packet.TargetColorEnum, EMoveDirFlag.None),
						new TransformInfo(new Vector3(0, 0, 0), new Vector3(0, 0, 0))
						);

					UnityEngine.Debug.Log($"{Packet.ResponsePlayerID}");
					UnityEngine.Debug.Log($"PlayerSpawn : NonPlayable, PlayerID : {Packet.ResponsePlayerID}");

				}

			}
		}

		internal void PlayerMoveHandle(ReceivePacketContext context)
		{
			if (context.Packet is S2C_MovePacket Packet)
			{
				if (_playerManager.TryGetPlayer(Packet.PlayerID, out Player player))
				{
					UnityEngine.Debug.Log($"MovePacket Target:{Packet.PlayerID}");
					player.UpdateMoveFlag(Packet.MoveFlag);
				}
			}
		}

		internal void TransformInterpolationHandle(ReceivePacketContext context)
		{
			if (context.Packet is S2C_TransformInterpolationPacket Packet)
			{
				foreach (var snapshot in Packet.Snapshots)
				{
					if (_playerManager.TryGetPlayer(snapshot.PlayerID, out Player player))
					{
						//if (player.ClientInfo.IsPlayable)
						//	continue;
						player.InterpolationInfo.InputInterpolationData(
							new Vector3(snapshot.PosX, snapshot.PosY, snapshot.PosZ),
							new Vector3(snapshot.DirX, snapshot.DirY, snapshot.DirZ));

					}
				}
			}
		}
	}
}

