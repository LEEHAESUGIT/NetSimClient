using NETSIM_Ver2.Assets.Script.Module;
using NETSIM_Ver2.Assets.Script.NetworkPackage;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Manager;
using NETSIM_Ver2.Assets.Script.NetworkPackage.Pipe;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace NETSIM_Ver2.Assets.Script.NetworkPackage.Manager
{

	public class NetworkBehaviour : MonoBehaviour
	{
		//private ClientMain _clientMain;


		private NetworkModule _networkModule;


		internal void Init(ModulePipe modulePipe , CancellationTokenSource CTS)
		{
			_networkModule = new NetworkModule(modulePipe , CTS);
			//this._clientMain = new ClientMain(_ingameProcessPipe);
		}

		internal void CallStart()
		{
			_networkModule.Strart();
			//_clientMain.Start();
		}


		private void OnApplicationQuit()
		{
			_networkModule.Stop();
			//_clientMain.Stop();
		}



		internal void Log(string text)
		{
			Debug.Log($"{text}");
		}


	}
}
