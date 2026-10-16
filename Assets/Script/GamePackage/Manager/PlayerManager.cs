
using NETSIM_Ver2.Assets.Script.Configration;
using NETSIM_Ver2.Assets.Script.GamePackage;
using NETSIM_Ver2.Assets.Script.GamePackage.Object;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace NETSIM_Ver2.Assets.Script.GamePackage.Manager
{
	internal class PlayerManager
	{
		private ConcurrentStack<Player> _waitPool = new();
		private ConcurrentDictionary<int, Player> _registry = new();
		internal IReadOnlyDictionary<int, Player> Players => _registry;

		internal int PoolCount = 0;
		internal int RegistryCount = 0;

		private Action<Player> _playerObjectCreateTrigger;
		private Action<Player> _playerObjectDeleteTrigger;


		internal PlayerManager(Action<Player> playerObjectCreateTrigger , Action<Player> playerObjectDeleteTrigger )
		{
			for (int i = 7; i > -1; i--)
			{
				_waitPool.Push(new Player());
				Interlocked.Increment(ref PoolCount);
			}
			this._playerObjectCreateTrigger += playerObjectCreateTrigger;
			this._playerObjectDeleteTrigger += playerObjectDeleteTrigger;
		}
		#region Function
		internal bool TryGetPlayer(int playerID, out Player player)
		{
			return _registry.TryGetValue(playerID, out player);
		}
		internal bool TryExcute(int playerID, Action<Player> action)
		{
			if (_registry.TryGetValue(playerID, out var player))
			{
				action(player);
				return true;
			}
			return false;
		}
		internal void ForEachActionPlayer(Action<Player> foreachAction)
		{
			foreach (var player in _registry.Values)
			{
				foreachAction(player);
			}
		}
		#endregion

		private PlayerStartPositionList _playerStartPositionList = new();

		internal bool CreatePlayer(int issuancePlayerID)
		{
			try
			{
				if (_waitPool.TryPop(out Player item))
				{
					Interlocked.Decrement(ref PoolCount);
					if (_registry.TryAdd(issuancePlayerID, item))
					{
						Interlocked.Increment(ref RegistryCount);
						
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log($"[CreatePlayer Error] : {ex}");
				return false;
			}
		}

		internal void InitPlayer(int playerID, ClientInfo clientInfo, StatusInfo statusInfo, TransformInfo transformInfo)
		{
			if (_registry.TryGetValue(playerID, out Player player))
			{
				player.Init(clientInfo, statusInfo, transformInfo);
				UnityEngine.Debug.Log($"CreatePlayerData");
				this._playerObjectCreateTrigger.Invoke(player);
			}

		}


		internal bool DeletePlayer(int playerID)
		{
			try
			{
				if (_registry.TryRemove(playerID, out Player item))
				{
					Interlocked.Decrement(ref RegistryCount);

					_waitPool.Push(item);
					Interlocked.Increment(ref PoolCount);

					this._playerObjectDeleteTrigger.Invoke(item);
					return true;
				}
				return false;

			}
			catch (Exception ex)
			{
				Console.WriteLine($"[Error] : {ex}");
				return false;
			}

		}



	}
}
