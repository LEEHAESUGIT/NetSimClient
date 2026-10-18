



using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using System;
using UnityEngine;

namespace NETSIM_Ver2.Assets.Script.GamePackage.Object
{
	[Serializable]
	public class Player
	{
		[SerializeField]
		internal ClientInfo ClientInfo;
		[SerializeField]
		internal StatusInfo StatusInfo;
		[SerializeField]
		internal TransformInfo TransformInfo;
		[SerializeField]
		internal InterpolationInfo InterpolationInfo;

		internal Player()
		{
			this.ClientInfo = new ClientInfo();
			this.StatusInfo = new StatusInfo();
			this.TransformInfo = new TransformInfo();
			this.InterpolationInfo = new InterpolationInfo(CalculationCoordinate);

		}

		internal void Init(ClientInfo clientInfoInit , StatusInfo statusInfoInit , TransformInfo transformInfo)
		{
			this.ClientInfo = clientInfoInit;	
			this.StatusInfo = statusInfoInit;
			this.TransformInfo = transformInfo;
		}

		internal void Clear()
		{
			this.ClientInfo = new ClientInfo();
			this.StatusInfo = new StatusInfo();
			this.TransformInfo = new TransformInfo();
		}
		
		internal void UpdateMoveFlag(EMoveDirFlag moveflag)
		{
			this.StatusInfo.MoveFlag = moveflag;
		}

		internal void CalculationCoordinate()
		{
			this.InterpolationInfo.CalculationCoordinateError(
				this.TransformInfo.NowPosition,
				this.TransformInfo.NowDirection
				);
		}


	}
}