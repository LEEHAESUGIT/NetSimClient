using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.Tool;
using UnityEngine;


namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Packet
{
	internal struct PacketHeader
	{
		public ushort Size;
		public EPacketID packetID;

		public void Serialize(PacketWriter writer)
		{
			writer.Write(Size);
			writer.Write((int)packetID);
		}

		public static PacketHeader Deserialize(PacketReader reader)
		{
			return new PacketHeader
			{
				Size = reader.ReadUshort(),
				packetID = (EPacketID)reader.ReadInt()
			};
		}

	}
}

