
using NETSIM_Ver2.Assets.Script.Configration;
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

		internal StatusInfo(float moveSpeed, EPlayerColor playerColor, EMoveDirFlag moveFlag)
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


		internal TransformInfo(Vector3 nowPos, Vector3 nowDir)
		{
			this.NowPosition = nowPos;
			this.NowDirection = nowDir;
			this.NextPosition = nowPos;
			this.NextDirection = nowDir;

		}
	}

	[Serializable]
	public class InterpolationInfo
	{
		/// <summary>
		/// 보간 틱레이틀를 10 틱으로 기준을 잡는다.													ex) 64틱 기준으로 1틱은 약 0.0156초 정도이므로 156ms가 보간을 처리하는 시간이 된다.
		/// 현재 타겟이 되는 오브젝트의 현재 포지션과 보간되는 포지션의 위치를 계산하여 오차를 낸다.	ex) CoordinateErrorPos = interpos Vector3 - transform.position 
		/// 오차를 보간틱 기준으로 나누어 틱당 이동 포지션을 구한다.									ex) OneTickInterpolationPos = CoordinateErrorPos / 10
		/// 틱당 이동포지션을 매틱마다 오브젝트의 이동에 추가한다.										ex) OneTickMovePos = NextMovePos + OneTickInterpolationPos
		/// 만약 10틱 안에 보간패킷이 도착한 경우, 최신의 보간패킷을 기준으로 다시 연산한다.
		/// </summary>
		// PacketData
		public Vector3 InterpolationPosition { get; private set; }
		public Vector3 InterpolationDirection { get; private set; }

		// Calculation Data
		public Vector3 PositionCoordinateError { get; private set; }
		public Vector3 DirectionCoordinateError { get; private set; }

		// OnTickData
		public Vector3 OnTickPosition { get; private set; }
		public Vector3 OnTickDirection { get; private set; }
		public int RemainingTicks { get; private set; } = 0;


		Action OnCalculationCoordinate;
		internal InterpolationInfo(Action OnCoordinate)
		{
			this.InterpolationPosition = Vector3.zero;
			this.InterpolationDirection = Vector3.zero;

			this.PositionCoordinateError = Vector3.zero;
			this.DirectionCoordinateError = Vector3.zero;

			this.OnTickPosition = Vector3.zero;
			this.OnTickDirection = Vector3.zero;

			OnCalculationCoordinate += OnCoordinate;
		}

		internal void InputInterpolationData(Vector3 pos, Vector3 dir)
		{
			this.InterpolationPosition = pos;
			this.InterpolationDirection = dir;

			OnCalculationCoordinate?.Invoke();
		}

		internal void CalculationCoordinateError(Vector3 nowTransformPos, Vector3 nowTransformDir)
		{
			this.PositionCoordinateError = InterpolationPosition - nowTransformPos;
			this.DirectionCoordinateError = InterpolationDirection - nowTransformDir;

			this.RemainingTicks = ServerConfigurationData.InterpolationTickUnit;

			CalculationOnTickPos();
			CalculationOnTickDir();
		}

		private void CalculationOnTickPos()
		{
			this.OnTickPosition = this.PositionCoordinateError / ServerConfigurationData.InterpolationTickUnit;
		}
		private void CalculationOnTickDir()
		{
			this.OnTickDirection = this.DirectionCoordinateError / ServerConfigurationData.InterpolationTickUnit;
		}


		internal Vector3 NowInterpolationPosition()
		{
			if (RemainingTicks > 0)
			{
				
				return OnTickPosition;
			}
			return Vector3.zero;
		}
		internal Vector3 NowInterpolationDirection()
		{
			if (RemainingTicks > 0)
			{
				
				return OnTickDirection;
			}
			return Vector3.zero;
		}
		internal void ConsumeTick()
		{
			if (RemainingTicks > 0)
			{
				RemainingTicks--;
			}
		}

	}


}