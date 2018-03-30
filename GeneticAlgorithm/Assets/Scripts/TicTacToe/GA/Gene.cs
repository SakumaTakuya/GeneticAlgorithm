using UnityEngine;

namespace TicTacToe.GA
{
	public struct Gene 
	{
		//public Matrix MiddleWeight;
		//public Matrix OutputWeight;

		public Matrix[] Weight;

		public Gene(Matrix middle, Matrix output)
		{
			//MiddleWeight = middle;
			//OutputWeight = output;
			Weight = new[] {middle, output};	
		}

		public override string ToString()
		{
			return Weight[0].ToString() + "\n\n" + Weight[1].ToString();
		}
	}
}
