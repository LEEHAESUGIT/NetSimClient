
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


		

		internal PlayerManager()
		{
			for (int i = 7; i > -1; i--)
			{
				_waitPool.Push(new Player());
				Interlocked.Increment(ref PoolCount);
			}
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



		internal bool CreatePlayer(int issuancePlayerID, out Player player)
		{
			try
			{
				if (_waitPool.TryPop(out Player item))
				{
					Interlocked.Decrement(ref PoolCount);
					if (_registry.TryAdd(issuancePlayerID, item))
					{
						Interlocked.Increment(ref RegistryCount);

						//bool isPlayable = true;

						//item.Init(
						//	new ClientInfo(issuancePlayerID, isPlayable),
						//	new StatusInfo());
						player = item;
						
						return true;
					}
				}
				player = null;
				return false;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"[Error] : {ex}");
				player = null;
				//issuancePlayerID = -1;
				return false;
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
