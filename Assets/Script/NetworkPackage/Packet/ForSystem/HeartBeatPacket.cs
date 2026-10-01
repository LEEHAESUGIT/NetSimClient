
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForSystem
{
	internal struct C2SHeartBeat : IPacket
	{
		// Info
		public EPacketID packetID => EPacketID.C_HeartBeat;
		// Data
		public long WasSendTime { get; set; }
		public long ServerTick { get; set; }
		public long SessionTick { get; set; }

		public C2SHeartBeat(long sendTime, long serverTick , long sessionTick)
		{
			this.WasSendTime = sendTime;
			this.ServerTick = serverTick;
			this.SessionTick = sessionTick;
		}
		public void Serialize(PacketWriter writer)
		{
			writer.Write(WasSendTime);
			writer.Write(ServerTick);
			writer.Write(SessionTick);
				
		}
		public static C2SHeartBeat Deserialize(PacketReader reader)
		{
			return new C2SHeartBeat
			{
				WasSendTime = reader.ReadLong(),
				ServerTick = reader.ReadLong(),
				SessionTick = reader.ReadLong()
			};
		}
	}

	internal struct S2CHeartBeat : IPacket
	{
		// Info
		public EPacketID packetID => EPacketID.S_HeartBeat;
		// Data
		public long WasSendTime { get; set; }
		public long ServerTick { get; set; }
		public long SessionTick { get; set; }


		public S2CHeartBeat(long sendTime, long serverTick, long sessionTick)
		{
			this.WasSendTime = sendTime;
			this.ServerTick = serverTick;
			this.SessionTick = sessionTick;
		}
		public void Serialize(PacketWriter writer)
		{
			writer.Write(WasSendTime);
			writer.Write(ServerTick);
			writer.Write(SessionTick);

		}
		public static S2CHeartBeat Deserialize(PacketReader reader)
		{
			return new S2CHeartBeat
			{
				WasSendTime = reader.ReadLong(),
				ServerTick = reader.ReadLong(),
				SessionTick = reader.ReadLong()
			}; ;
		}
	}

}

