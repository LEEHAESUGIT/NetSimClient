


using NETSIM_Ver2.Assets.Script.Component;
using NETSIM_Ver2.Assets.Script.GamePackage.Function;
using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace NETSIM_Ver2.Assets.Script.GamePackage.Object
{
	internal class PlayerObject : MonoBehaviour
	{
		// PlayerDataContainer
		[SerializeField]
		public Player PlayerData;

		// 1sec / Tickrate
		private float _deltaTime = 0;
		private float _elapsedTime = 0f;

		// PlayerFunction
		internal IPlayerController _playerController = null;

	
		private Quaternion _startRotation;
		private Quaternion _targetRotation = Quaternion.identity;

		internal void Awake()
		{
			
		}
		internal void Update()
		{
			if (_playerController != null)
				_playerController.OnUpdate(this);
		}


		internal void Init(float deltaTime, Player player, IPlayerController playerController)
		{
			this._deltaTime = deltaTime;
			this.PlayerData = player;
			this._playerController = playerController;

			PlayerData.OnMoveTick += OnUpdateMoveTick;
		}

		internal void OnUpdateClientInfo(ClientInfo info)
		{
			this.PlayerData.ClientInfo = info;
		}
		internal void OnUpdateStatusInfo(StatusInfo info)
		{
			this.PlayerData.StatusInfo = info;
		}
		
		internal void OnUpdateMoveTick()
		{
			var tempDir = MoveConverter.ToDirection(PlayerData.StatusInfo.MoveFlag);

			// Update Target Position
			this.PlayerData.TransformInfo.NowPosition = this.transform.position;
			this.PlayerData.TransformInfo.NextPosition += tempDir * (PlayerData.StatusInfo.MoveSpeed * _deltaTime);

			// Update Target Rotation;
			this.PlayerData.TransformInfo.NowDirection = this.transform.forward;
			this.PlayerData.TransformInfo.NextDirection = tempDir;

			this._startRotation = this.transform.rotation;
			if(PlayerData.TransformInfo.NextDirection.sqrMagnitude > 0.001f)
			{
				this._targetRotation = Quaternion.LookRotation(PlayerData.TransformInfo.NextDirection);
			}
			else
			{
				this._targetRotation = this._startRotation;
			}

			_elapsedTime = 0;
		}

		internal void Move()
		{
			_elapsedTime += Time.deltaTime;

			var t = Mathf.Clamp01(this._elapsedTime / _deltaTime);

			this.transform.position = Vector3.Lerp(
				PlayerData.TransformInfo.NowPosition,
				PlayerData.TransformInfo.NextPosition,
				t);

			this.transform.rotation = Quaternion.Slerp(
				_startRotation,
				_targetRotation,
				t);

		}
	}
}