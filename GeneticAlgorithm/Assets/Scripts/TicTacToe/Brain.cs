using UnityEngine;

namespace TicTacToe
{
	public abstract class Brain : MonoBehaviour
	{
		public Game.Piece MyTurn;
		public bool DeterminePosition;

		public void SetMyTurn(Game.Piece turn)
		{
			MyTurn = turn;
		}
		
		public abstract void CalculateBestMove(Game.Piece myTurn , out int x, out int y, int sizeX, int sizeY);
	}
}
