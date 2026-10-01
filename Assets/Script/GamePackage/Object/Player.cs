



using UnityEngine;

namespace NETSIM_Ver2.Assets.Script.GamePackage.Object
{
	[System.Serializable]
	internal class Player
	{
		internal ClientInfo ClientInfo { get; set; }
		internal StatusInfo StatusInfo { get; set; }

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


	}
}