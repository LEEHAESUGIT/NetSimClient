


using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.LiveData;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Context;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForSystem;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Packet
{
	internal class GamePacketOperator
	{
		#region Singletone
		/// <summary>
		/// 기존 알고있던 static형식의 싱글톤과 차이점
		/// 1. Lazy<>를 사용해 C#내부의 스레드 안전성 보장.
		/// 2. 프로그램 Runtime시 즉시 메모리에 객체가 올라가는게 아닌 Instance를 호출시 메모리에 올리는 
		///		방식.
		/// </summary>
		private static readonly Lazy<GamePacketOperator> _instance = new Lazy<GamePacketOperator>(() => new GamePacketOperator());
		public static GamePacketOperator Instance { get => _instance.Value; }
		private GamePacketOperator() { }
		#endregion

		private Queue<ResultPacketContext> TickPerOutbox = new();

		#region GamePacket
		internal void GenerateMovePacket(int playerID, EMoveDirFlag moveFlag)
		{
			TakeIn(new C2S_MovePacket(
				LiveServerData.Instance.ServerTickCount,
				playerID,
				moveFlag)
			);

			UnityEngine.Debug.Log($"Tick : {LiveServerData.Instance.ServerTickCount} , GenerateMovePacket");
		}
		
		internal void GeneratePlayableSpawnPacket()
		{
			TakeIn(new C2S_PlayableObjectSpawnRequest(LiveServerData.Instance.ServerTickCount));
			UnityEngine.Debug.Log($"Tick : {LiveServerData.Instance.ServerTickCount} , GeneratePlayableSpawnPacket");
		}
		
		internal void GenerateNonPlayableSpawnPacket()
		{
			TakeIn(new C2S_NonPlayableObjectSpawnRequest(LiveServerData.Instance.ServerTickCount));
			UnityEngine.Debug.Log($"Tick : {LiveServerData.Instance.ServerTickCount} , GenerateNonPlayableSpawnPacket");
		}

		#endregion

		private void TakeIn(IPacket packet)
		{
			this.TickPerOutbox.Enqueue(new ResultPacketContext
			{
				Packet = packet
			});
		}
		internal void TakeOut(GamePacketPipe gamePacketPipe)
		{
			while (TickPerOutbox.TryDequeue(out ResultPacketContext context))
			{
				gamePacketPipe.OutBoundPipe.TryWrite(context);
			}

		}


	}


	internal class SystemPacketOperator
	{
		#region Singletone
		private static readonly Lazy<SystemPacketOperator> _instance = new Lazy<SystemPacketOperator>(() => new SystemPacketOperator());
		public static SystemPacketOperator Instance { get => _instance.Value; }
		private SystemPacketOperator() { }
		#endregion

		private Queue<ResultPacketContext> TickPerOutbox = new();

		#region SystemPacketGenerate
		internal void GenerateHeartbeatPacket(long sendTime, long serverTick, long sessionTick)
		{
			TakeIn(
				new C2S_HeartBeat(
				LiveServerData.Instance.ServerTickCount,
				sendTime,
				serverTick,
				sessionTick)
			);

			//UnityEngine.Debug.Log($"Tick : {LiveServerData.Instance.ServerTickCount} , GenerateHeartbeatPacket");
		}
		internal void GenerateInitSessionRequestPacket()
		{
			TakeIn(
				new C2S_InitRequest(
					LiveServerData.Instance.ServerTickCount)
				);

			UnityEngine.Debug.Log($"Tick : {LiveServerData.Instance.ServerTickCount} , GenerateInitSessionRequestPacket");
		}
		#endregion

		private void TakeIn(IPacket packet)
		{
			this.TickPerOutbox.Enqueue(new ResultPacketContext
			{
				Packet = packet
			});
		}
		internal void TakeOut(SystemPacketPipe systemPacketPipe)
		{
			while (TickPerOutbox.TryDequeue(out ResultPacketContext context))
			{
				systemPacketPipe.OutBoundPipe.TryWrite(context);
			}

		}
	}


}