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
	#region Playable
	internal struct C2S_ReqPlayableObjectSpawnRequest : IPacket
	{
		// Info
		public EPacketID packetID => EPacketID.C_PlayableObjectSpawn;

		public uint Tick { get; private set; }

		public C2S_ReqPlayableObjectSpawnRequest(uint tick)
		{
			this.Tick = tick;
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Write(Tick);
		}
		public static C2S_ReqPlayableObjectSpawnRequest Deserialize(PacketReader reader)
		{
			return new C2S_ReqPlayableObjectSpawnRequest
			{
				Tick = reader.ReadUInt()
			};
		}
	}
	internal struct S2C_AnsPlayableObjectSpawnResponse : IPacket
	{
		// Info
		public EPacketID packetID => EPacketID.S_PlayableObjectSpawn;
		public uint Tick { get; private set; }
		public int TargetSessionID { get; private set; }
		public int ResponsePlayerID { get; private set; }
		public int TargetColorEnum { get; private set; }

		public S2C_AnsPlayableObjectSpawnResponse(uint tick, int sessionID, int issuanceID, int colorEnum)
		{
			this.Tick = tick;
			this.TargetSessionID = sessionID;
			this.ResponsePlayerID = issuanceID;
			this.TargetColorEnum = colorEnum;
		}


		public void Serialize(PacketWriter writer)
		{
			writer.Write(Tick);
			writer.Write(TargetSessionID);
			writer.Write(ResponsePlayerID);
			writer.Write(TargetColorEnum);
		}
		public static S2C_AnsPlayableObjectSpawnResponse Deserialize(PacketReader reader)
		{
			return new S2C_AnsPlayableObjectSpawnResponse
			{
				Tick = reader.ReadUInt(),
				TargetSessionID = reader.ReadInt(),
				ResponsePlayerID = reader.ReadInt(),
				TargetColorEnum = reader.ReadInt()
			};
		}
	}
	#endregion
	#region NonPlayable
	internal struct C2S_ReqNonPlayableObjectSpawnRequest : IPacket
	{
		// Info
		public EPacketID packetID => EPacketID.C_NonPlayableObjectSpawn;
		//public int TargetSessionID { get; private set; }
		public C2S_ReqNonPlayableObjectSpawnRequest(int sessionID)
		{
			//this.TargetSessionID = sessionID;
		}


		public void Serialize(PacketWriter writer)
		{
			//writer.Write(TargetSessionID);
		}
		public static C2S_ReqNonPlayableObjectSpawnRequest Deserialize(PacketReader reader)
		{
			return new C2S_ReqNonPlayableObjectSpawnRequest
			{
				//TargetSessionID = reader.ReadInt()
			};
		}
	}
	internal struct S2C_AnsNonPlayableObjectSpawnResponse : IPacket
	{
		// Info
		public EPacketID packetID => EPacketID.S_NonPlayableObjectSpawn;

		public int TargetSessionID { get; private set; }
		public int ResponsePlayerID { get; private set; }
		public int TargetColorEnum { get; private set; }

		public S2C_AnsNonPlayableObjectSpawnResponse(int sessionID, int issuanceID, int colorEnum)
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
		public static S2C_AnsNonPlayableObjectSpawnResponse Deserialize(PacketReader reader)
		{
			return new S2C_AnsNonPlayableObjectSpawnResponse
			{
				TargetSessionID = reader.ReadInt(),
				ResponsePlayerID = reader.ReadInt(),
				TargetColorEnum = reader.ReadInt()
			};
		}
	}
	#endregion



}
