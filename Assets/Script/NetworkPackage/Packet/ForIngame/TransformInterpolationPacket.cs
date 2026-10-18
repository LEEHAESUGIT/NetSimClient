using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.Tool;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.WrapperForPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame
{

	internal struct S2C_TransformInterpolationPacket : IPacket
	{
		public EPacketID packetID => EPacketID.S_Transform;

		public List<PlayersPositionSnapshot> Snapshots;


		public S2C_TransformInterpolationPacket(List<PlayersPositionSnapshot> snapshot)
		{
			this.Snapshots = snapshot;
		}

		public void Serialize(PacketWriter writer)
		{
			writer.Write((ushort)Snapshots.Count);
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

		public static S2C_TransformInterpolationPacket Deserialize(PacketReader reader)
		{
			int snapshotCount = (int)reader.ReadUshort();
			List<PlayersPositionSnapshot> temp = new(snapshotCount);

			for (int i = 0; i < snapshotCount; i++)
			{
				temp.Add(new PlayersPositionSnapshot
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
			return new S2C_TransformInterpolationPacket
			{
				Snapshots = temp
			};
		}

	}
}

