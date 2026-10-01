



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
		private GameManager _gameManager;



		// Pipe
		private GamePacketPipe _gamePacketPipe;

		// Delegate
		internal Action<Player> OnCreatePlayerObject;


		internal GameHandle(GameManager gameManager, PlayerManager playerManager, GamePacketPipe gamePacketPipe)
		{
			this._playerManager = playerManager;
			this._gamePacketPipe = gamePacketPipe;
			
			this._gameManager = gameManager;
		}

		internal void SpawnResponseHandle(ReceivePacketContext context)
		{
			if (context.Packet is S2C_SpawnResponse Packet)
			{
				if (_playerManager.CreatePlayer(Packet.ResponsePlayerID, out Player createPlayer))
				{
					createPlayer.Init(
						new ClientInfo(Packet.ResponsePlayerID, true),
						new StatusInfo(5f, (EPlayerColor)Packet.TargetColorEnum)
						);

					UnityEngine.Debug.Log($"{Packet.ResponsePlayerID}");
					UnityEngine.Debug.Log($"SpawnPlayer");
					

					_gameManager.SpawnPlayer(Packet.ResponsePlayerID);
				}
			}
		}

		internal void PlayerMoveHandle(ReceivePacketContext context)
		{
			if(context.Packet is S2C_MovePacket Packet)
			{
				if(_playerManager.TryGetPlayer(Packet.PlayerID , out Player player))
				{
					player.StatusInfo.OnUpdateMoveFlag(Packet.MoveFlag);
				}
			}
		}



	}


}

