



using NETSIM_Ver2.Assets.Script.Component;
using NETSIM_Ver2.Assets.Script.GamePackage.Manager;
using NETSIM_Ver2.Assets.Script.GamePackage.Object;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using System.Collections.Generic;
using System.Threading.Channels;
using UnityEngine;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Manager
{
	internal class GameManager : MonoBehaviour
	{
		// Manager
		private PlayerManager _playerManager;


		// LivePlayer
		private Channel<GameObject> _players = Channel.CreateUnbounded<GameObject>();

		// Pipe
		private GamePacketPipe _gamePacketPipe;


		// Prefab
		public GameObject PlayerPrefab;
		// Material
		public List<Material> PlayerColor;

		internal void Init(PlayerManager playerManager , GamePacketPipe gamePacketPipe)
		{
			this._playerManager = playerManager;
			this._gamePacketPipe = gamePacketPipe;
		}



		internal void SpawnPlayer(int playerID)
		{
			var instancePlayer = Instantiate(PlayerPrefab);
			var playerScript = instancePlayer.GetComponent<PlayerObject>();

			if(_playerManager.TryGetPlayer(playerID , out Player player))
			{
				instancePlayer.GetComponent<MeshRenderer>().material = PlayerColor[(int)player.StatusInfo.PlayerColor];

				if (player.ClientInfo.IsPlayable)
					playerScript.Init(player, new LocalInputController(_gamePacketPipe));
				if (!player.ClientInfo.IsPlayable)
					playerScript.Init(player, new RemotePacketController(player.StatusInfo.MoveFlagWraper));
			}
			_players.Writer.TryWrite(instancePlayer);
		}

	}

}