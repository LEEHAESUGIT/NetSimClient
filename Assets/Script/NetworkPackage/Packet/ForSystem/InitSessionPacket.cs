



using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.Tool;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForSystem
{
	internal struct C2S_InitRequest : IPacket
	{
		public EPacketID packetID => EPacketID.C_Init;
		public C2S_InitRequest(int sessionID) { }
		public void Serialize(PacketWriter writer) { }

		public static C2S_InitRequest Deserialize(PacketReader reader)
		{
			return new C2S_InitRequest { };
		}

	}
	internal struct S2C_InitResponse : IPacket
	{
		public EPacketID packetID => EPacketID.S_Init;
		public int ForInitSessionID { get; private set; }
		public S2C_InitResponse(int sessionID)
		{
			this.ForInitSessionID = sessionID;
		}
		public void Serialize(PacketWriter writer)
		{
			writer.Write(ForInitSessionID);
		}

		public static S2C_InitResponse Deserialize(PacketReader reader)
		{
			return new S2C_InitResponse
			{
				ForInitSessionID = reader.ReadInt()
			};
		}
	}


}
