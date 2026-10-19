
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.Tool;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.WrapperForPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame
{

	internal struct S2C_LivePlayerRenewalPacket : IPacket
	{
		// Info
		public EPacketID packetID => EPacketID.S_LivePlayerRenewal;

		public uint Tick { get; private set; }
		public int LivePlayerCount { get; private set; }
		public List<PlayerInfoWrapper> InfoWrapper { get; private set; }
		public List<PlayersPositionSnapshot> Snapshots { get; private set; }

		public S2C_LivePlayerRenewalPacket(uint tick, int playerCount, List<PlayerInfoWrapper> infos, List<PlayersPositionSnapshot> snapshots)
		{
			this.Tick = tick;
			this.LivePlayerCount = playerCount;
			this.InfoWrapper = infos;
			this.Snapshots = snapshots;
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Write(Tick);
			writer.Write(LivePlayerCount);
			writer.Write((ushort)InfoWrapper.Count);
			writer.Write((ushort)Snapshots.Count);
			foreach (var info in InfoWrapper)
			{
				writer.Write(info.PlayerColor);
			}
			foreach (var snapshot in Snapshots)
			{
				writer.Write(snapshot.PlayerID);
				writer.Write(snapshot.PosX);
				writer.Write(snapshot.PosY);
				writer.Write(snapshot.PosZ);

				writer.Write(snapshot.DirX);
				writer.Write(snapshot.DirY);
				writer.Write(snapshot.DirZ);
			}
		}
		public static S2C_LivePlayerRenewalPacket Deserialize(PacketReader reader)
		{
			uint tick = reader.ReadUInt();
			int playerCount = reader.ReadInt();
			int infoWrapperCount = (int)reader.ReadUshort();
			int snapshotCount = (int)reader.ReadUshort();

			List<PlayerInfoWrapper> deserializeInfos = new List<PlayerInfoWrapper>(infoWrapperCount);
			List<PlayersPositionSnapshot> deserializeSnapshots = new List<PlayersPositionSnapshot>(snapshotCount);

			for (int infoIndex = 0; infoIndex < infoWrapperCount; infoIndex++)
			{
				deserializeInfos.Add(new PlayerInfoWrapper
				(
					reader.ReadInt()
				));
			}
			for (int snapshotIndex = 0; snapshotIndex < snapshotCount; snapshotIndex++)
			{
				deserializeSnapshots.Add(new PlayersPositionSnapshot
				(
					reader.ReadInt(),
					reader.ReadFloat(),
					reader.ReadFloat(),
					reader.ReadFloat(),

					reader.ReadFloat(),
					reader.ReadFloat(),
					reader.ReadFloat()
				));
			}

			return new S2C_LivePlayerRenewalPacket
			{
				Tick = tick,
				LivePlayerCount = playerCount,
				InfoWrapper = deserializeInfos,
				Snapshots = deserializeSnapshots

			};
		}
	}
}