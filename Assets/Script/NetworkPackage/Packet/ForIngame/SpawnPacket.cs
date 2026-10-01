using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame
{
	internal struct C2S_SpawnRequest : IPacket
	{
		// Info
		public EPacketID packetID => EPacketID.C_Spawn;
		//public int TargetSessionID { get; private set; }
		public C2S_SpawnRequest(int sessionID)
		{
			//this.TargetSessionID = sessionID;
		}


		public void Serialize(PacketWriter writer)
		{
			//writer.Write(TargetSessionID);
		}
		public static C2S_SpawnRequest Deserialize(PacketReader reader)
		{
			return new C2S_SpawnRequest
			{
				//TargetSessionID = reader.ReadInt()
			};
		}
	}
	internal struct S2C_SpawnResponse : IPacket
	{
		// Info
		public EPacketID packetID => EPacketID.S_Spawn;

		public int TargetSessionID { get; private set; }
		public int ResponsePlayerID { get; private set; }
		public int TargetColorEnum { get; private set; }

		public S2C_SpawnResponse(int sessionID, int issuanceID, int colorEnum)
		{
			this.TargetSessionID = sessionID;
			this.ResponsePlayerID = issuanceID;
			this.TargetColorEnum = colorEnum;
		}


		public void Serialize(PacketWriter writer)
		{
			writer.Write(TargetSessionID);
			writer.Write(ResponsePlayerID);
			writer.Write(TargetColorEnum);
		}
		public static S2C_SpawnResponse Deserialize(PacketReader reader)
		{
			return new S2C_SpawnResponse
			{
				TargetSessionID = reader.ReadInt(),
				ResponsePlayerID = reader.ReadInt(),
				TargetColorEnum = reader.ReadInt()
			};
		}
	}
}
