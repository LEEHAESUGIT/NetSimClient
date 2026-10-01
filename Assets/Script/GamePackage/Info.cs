
using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;

namespace NETSIM_Ver2.Assets.Script.GamePackage
{
	[System.Serializable]
	internal class StatusInfo
	{
		internal float MoveSpeed { get; private set; } = 5f;
		internal EPlayerColor PlayerColor { get; private set; } = EPlayerColor.NONE;
		internal MoveFlagWraper MoveFlagWraper { get; private set; }

		internal StatusInfo() { }
		internal StatusInfo(float moveSpeed, EPlayerColor playerColor)
		{
			this.MoveSpeed = moveSpeed;
			this.PlayerColor = playerColor;
			this.MoveFlagWraper = new();
		}

		internal void OnUpdateMoveFlag(EMoveDirFlag moveFlag)
		{
			this.MoveFlagWraper.Input(moveFlag);
		}

	}
	[System.Serializable]
	internal class ClientInfo
	{
		internal int PlayerID { get; set; } = -1;
		internal bool IsPlayable { get; set; } = false;

		internal ClientInfo() { }
		internal ClientInfo(int playerID, bool isPlayable)
		{
			this.PlayerID = playerID;
			this.IsPlayable = isPlayable;
		}


	}
}