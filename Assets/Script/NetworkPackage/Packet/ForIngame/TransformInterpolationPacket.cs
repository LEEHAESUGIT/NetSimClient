using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.ForIngame
{

	internal struct PlayersPositionSnapshot
	{
		public int PlayerID { get; private set; }
		public float PosX { get; private set; }
		public float PosY { get; private set; }
		public float PosZ { get; private set; }

		public float DirX { get; private set; }
		public float DirY { get; private set; }
		public float DirZ { get; private set; }

		internal PlayersPositionSnapshot(int playerID, float posX, float posY, float posZ, float dirX, float dirY, float dirZ)
		{
			this.PlayerID = playerID;
			this.PosX = posX;
			this.PosY = posY;
			this.PosZ = posZ;
			this.DirX = dirX;
			this.DirY = dirY;
			this.DirZ = dirZ;
		}
	}

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

