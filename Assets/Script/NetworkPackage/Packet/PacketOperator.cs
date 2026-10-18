


using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
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
using UnityEditor.Rendering;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Packet
{
	internal class PacketOperator
	{
		#region Singletone
		/// <summary>
		/// 기존 알고있던 static형식의 싱글톤과 차이점
		/// 1. Lazy<>를 사용해 C#내부의 스레드 안전성 보장.
		/// 2. 프로그램 Runtime시 즉시 메모리에 객체가 올라가는게 아닌 Instance를 호출시 메모리에 올리는 
		///		방식.
		/// </summary>
		private static readonly Lazy<PacketOperator> _instance = new Lazy<PacketOperator>(() => new PacketOperator());
		public static PacketOperator Instance { get => _instance.Value; }
		private PacketOperator() { }
		#endregion

		private Queue<IPacket> TickPerOutbox = new();

		private uint _tickCount = 0;

		#region SystemPacket
		internal void GenerateHeartbeatPacket()
		{
			TakeIn(new C2SHeartBeat(this._tickCount));
		}
		internal void GenerateInitSessionPacket()
		{
			TakeIn(new C2S_InitRequest(this._tickCount));
		}
		#endregion
		#region GamePacket
		internal void GenerateMovePacket(int playerID , EMoveDirFlag moveFlag) 
		{
			TakeIn(new C2S_MovePacket(this._tickCount , playerID , moveFlag ));
		}
		internal void GenerateReqPlayableSpawnPacket() 
		{
			TakeIn(new C2S_ReqPlayableObjectSpawnRequest(this._tickCount));
		}
		
		#endregion




		private void TakeIn(IPacket packet)
		{
			this.TickPerOutbox.Enqueue(packet);
		}
		internal void TakeOut(GamePacketPipe gamePacketPipe, uint tickCount)
		{
			while (TickPerOutbox.TryDequeue(out IPacket packet))
			{
				gamePacketPipe.OutBoundPipe.TryWrite(new ResultPacketContext
				{
					Packet = packet
				});
			}
			this._tickCount = tickCount + 1;

		}


	}

}