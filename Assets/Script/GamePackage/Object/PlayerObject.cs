


using NETSIM_Ver2.Assets.Script.Component;
using NETSIM_Ver2.Assets.Script.GamePackage.Function;
using NETSIM_Ver2.Assets.Script.GamePackage.MetaData.Enum;
using NETSIM_Ver2.Assets.Script.NetworkPackage.MetaData.Interface;
using System.Linq.Expressions;
using TMPro;
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



		public TextMeshProUGUI TransformOverlay;


		internal void Awake()
		{

		}
		internal void FixedUpdate()
		{
			if (_playerController != null)
				_playerController.OnUpdate(this);
			Move();

			TransformOverlay.text = $" Pos : {this.transform.position} , Dir : {this.transform.forward}";

		}


		internal void Init(float deltaTime, Player player, IPlayerController playerController)
		{
			this._deltaTime = deltaTime;
			this.PlayerData = player;
			this._playerController = playerController;

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
			Vector3 tempDir = MoveConverter.ToDirection(PlayerData.StatusInfo.MoveFlag);

			PositionOfTick(tempDir);
			DirectionOfTick(tempDir);
			PlayerData.InterpolationInfo.ConsumeTick();
			_elapsedTime = 0;
		}

		private void PositionOfTick(Vector3 tempDir)
		{

			if (PlayerData.ClientInfo.IsPlayable)
			{
				// Update Target Position
				this.PlayerData.TransformInfo.NowPosition = this.transform.position;

				Vector3 currentPos = (tempDir * (PlayerData.StatusInfo.MoveSpeed * _deltaTime) + PlayerData.InterpolationInfo.NowInterpolationPosition());


				this.PlayerData.TransformInfo.NextPosition = this.PlayerData.TransformInfo.NowPosition + currentPos;
			}
			else
			{
				this.PlayerData.TransformInfo.NowPosition = this.transform.position;

				Vector3 currentPos = PlayerData.InterpolationInfo.NowInterpolationPosition();

				this.PlayerData.TransformInfo.NextPosition = this.PlayerData.TransformInfo.NowPosition + currentPos;
			}

		}
		private void DirectionOfTick(Vector3 tempDir)
		{
			// Update Target Rotation;
			this.PlayerData.TransformInfo.NowDirection = this.transform.forward;
			this.PlayerData.TransformInfo.NextDirection = tempDir + PlayerData.InterpolationInfo.NowInterpolationDirection();

			this._startRotation = this.transform.rotation;
			if (PlayerData.TransformInfo.NextDirection.sqrMagnitude > 0.001f)
			{
				this._targetRotation = Quaternion.LookRotation(PlayerData.TransformInfo.NextDirection);
			}
			else
			{
				this._targetRotation = this._startRotation;
			}
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