


using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NETSIM_Ver2.Assets.Script.Configration
{
	internal class PlayerStartPositionList
	{
		private readonly Dictionary<int , Vector3> _startPositions = new();

		//internal ReadOnlyMemory<Vector3> StartPositions
		internal PlayerStartPositionList() 
		{
			for(int i = 0; i < 8 ; i++)
			{
				_startPositions.Add(i , new Vector3(i,i,i));
			}
		}
		internal Vector3 GetStartPosition(int slotKey)
		{
			return _startPositions[slotKey];
		}



	}
}