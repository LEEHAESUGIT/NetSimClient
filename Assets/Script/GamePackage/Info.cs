
using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using System;
using UnityEngine;
using UnityEngine.InputSystem.Composites;

namespace NETSIM_Ver2.Assets.Script.GamePackage
{
	[Serializable]
	public struct StatusInfo
	{
		public float MoveSpeed;
		[SerializeField] internal EPlayerColor PlayerColor;
		[SerializeField] internal EMoveDirFlag MoveFlag;

		internal StatusInfo(float moveSpeed, EPlayerColor playerColor , EMoveDirFlag moveFlag)
		{
			this.MoveSpeed = moveSpeed;
			this.PlayerColor = playerColor;
			this.MoveFlag = moveFlag;
		}
	}

	[Serializable]
	public struct ClientInfo
	{
		public int PlayerID;
		public bool IsPlayable;
		internal ClientInfo(int playerID, bool isPlayable)
		{
			this.PlayerID = playerID;
			this.IsPlayable = isPlayable;
		}
	}

	[Serializable]
	public struct TransformInfo
	{
		public Vector3 NowPosition;
		public Vector3 NowDirection;

		public Vector3 NextPosition;
		public Vector3 NextDirection;
		
		internal TransformInfo(Vector3 nowPos , Vector3 nowDir) 
		{
			this.NowPosition = nowPos;
			this.NowDirection = nowDir;
			this.NextPosition = nowPos;
			this.NextDirection = nowDir;
		}
	}

}