

using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.GamePackage;
using UnityEngine;
using NETSIM_Ver2.Assets.Script.GamePackage.Manager;
using NETSIM_Ver2.Assets.Script.Module;
using System.Threading;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;

namespace NETSIM_Ver2.Assets.Script
{
	internal class ProjectMain : MonoBehaviour
	{
		private CancellationTokenSource _projectCTS;
		private ModulePipe _modulePipe;
		private NetworkModule _networkModule;

		public GameBehaviour GameBehaviour;
		
		private void Awake()
		{
			this._projectCTS = new CancellationTokenSource();
			this._modulePipe = new ModulePipe();
			this._networkModule = new NetworkModule(_modulePipe , _projectCTS);

			this.GameBehaviour.Init(_modulePipe.GamePacketPipe , _projectCTS);
		}

		private void Start()
		{
			if(this._networkModule.Start())
			{
				this.GameBehaviour.CallStart();
			}
			else
			{
				UnityEngine.Debug.Log("Connected Failed");
			}
		}

		private void OnApplicationQuit()
		{
			this._networkModule.Stop();
		}


	}

}