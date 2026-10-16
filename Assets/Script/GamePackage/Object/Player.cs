



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


		internal Action OnMoveTick;

		internal Player()
		{
			this.ClientInfo = new ClientInfo();
			this.StatusInfo = new StatusInfo();
		}

		internal void Init(ClientInfo clientInfoInit , StatusInfo statusInfoInit)
		{
			this.ClientInfo = clientInfoInit;	
			this.StatusInfo = statusInfoInit;
		}

		internal void Clear()
		{
			this.ClientInfo = new ClientInfo();
			this.StatusInfo = new StatusInfo();
		}
		
		internal void MoveTick(EMoveDirFlag moveflag)
		{
			this.StatusInfo.MoveFlag = moveflag;
			OnMoveTick.Invoke();

		}



	}
}