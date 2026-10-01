


using NETSIM_Ver2.Assets.Script.Component;
using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

namespace NETSIM_Ver2.Assets.Script.GamePackage.Object
{
	internal class PlayerObject : MonoBehaviour
	{
		[SerializeField]
		internal Player PlayerData;
		internal IPlayerController _playerController = null;

		private Transform _playerTransform;
		


		internal void Awake()
		{
			this._playerTransform = this.GetComponent<Transform>();
		}

		internal void Update()
		{
			if (_playerController != null)
				_playerController.OnUpdate(this);
		}


		internal void Init(Player player, IPlayerController playerController )
		{
			this.PlayerData = player;
			this._playerController = playerController;
		}

		// Local
		internal void Move(Vector3 newPos)
		{
			this._playerTransform.position += newPos * (PlayerData.StatusInfo.MoveSpeed * Time.deltaTime);
		}

		// Remote
		internal void InterlockedMove(Vector3 newPos)
		{
			this._playerTransform.position += newPos * (PlayerData.StatusInfo.MoveSpeed * Time.deltaTime);
		}



	}
}