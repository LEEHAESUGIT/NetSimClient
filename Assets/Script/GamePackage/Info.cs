
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
				
				return OnTickPosition;
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