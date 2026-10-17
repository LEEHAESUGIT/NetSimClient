using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Packet.WrapperForPacket
{
	// PlayerColor
	internal struct PlayerInfoWrapper
	{
		internal int PlayerColor { get; private set; }

		internal PlayerInfoWrapper(int playerColor)
		{
			this.PlayerColor = playerColor;
		}

	}

	// Position
	// Direction
	// PlayerID
	internal struct PlayersPositionSnapshot
	{
		internal int PlayerID { get; private set; }
		internal float PosX { get; private set; }
		internal float PosY { get; private set; }
		internal float PosZ { get; private set; }

		internal float DirX { get; private set; }
		internal float DirY { get; private set; }
		internal float DirZ { get; private set; }

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
}
