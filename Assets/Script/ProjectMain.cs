

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



		public GameBehaviour GameBehaviour;
		public NetworkBehaviour NetworkBehaviour;

		private void Awake()
		{
			_projectCTS = new CancellationTokenSource();
			this._modulePipe = new ModulePipe();

			this.NetworkBehaviour.Init(_modulePipe , _projectCTS);
			this.GameBehaviour.Init(_modulePipe.GamePacketPipe , _projectCTS);
			//this.NetManager.Init(_ingameProcessPipe);
		}

		private void Start()
		{

			this.NetworkBehaviour.CallStart();
			this.GameBehaviour.CallStart();
			
		}

	}

}