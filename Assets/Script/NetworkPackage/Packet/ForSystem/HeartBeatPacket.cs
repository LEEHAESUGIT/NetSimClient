
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
		public uint Tick { get; set; }

		public C2SHeartBeat(uint tick)
		{ 
			this.Tick = tick;
		}
		public void Serialize(PacketWriter writer)
		{
			writer.Write(Tick);	
		}
		public static C2SHeartBeat Deserialize(PacketReader reader)
		{
			return new C2SHeartBeat
			{
				Tick = reader.ReadUInt()
			};
		}
	}

	internal struct S2CHeartBeat : IPacket
	{
		// Info
		public EPacketID packetID => EPacketID.S_HeartBeat;
		// Data
		public uint Tick { get; set; }


		public S2CHeartBeat(uint tick)
		{
			this.Tick = tick;
		}
		public void Serialize(PacketWriter writer)
		{
			writer.Write(Tick);

		}
		public static S2CHeartBeat Deserialize(PacketReader reader)
		{
			return new S2CHeartBeat
			{
				Tick = reader.ReadUInt()
			}; ;
		}
	}

}

