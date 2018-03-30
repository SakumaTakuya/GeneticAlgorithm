using System;
using UnityEngine;

namespace TicTacToe.GA
{
	[CreateAssetMenu, Serializable]
	public class BestWeight : ScriptableObject
	{
		public Matrix[] BestWeights;
	}
}
