using System;
using UnityEngine;

namespace TicTacToe
{
	public class ProbabilisticBrain : Brain 
	{	
		[SerializeField] private Game _game;


		public override void CalculateBestMove(Game.Piece myTurn, out int x, out int y, int sizeX, int sizeY)
		{
			x = 3;
			y = 3;
			var max = float.MinValue;
			var unbest = UnityEngine.Random.Range(0, 5) == 0;
			for (var i = 0; i < sizeX; i++)
			{
				for (var j = 0; j < sizeY; j++)
				{
					var bestx = x;
					var besty = y;
					//仮想の盤面を作成
					var board = new Game.Piece[sizeX, sizeY];
					Array.Copy(_game.Board, board, _game.Board.Length);


					var set = _game.SetPiece(i, j, ref board);
					if (!set) continue;

					x = i;
					y = j;
					
					if(unbest && UnityEngine.Random.Range(0, 5) == 0) break;
					
					var point = _game.CountSequence(MyTurn, i, j, board);
					if (point <= max)
					{
						x = bestx;
						y = besty;
						continue;
					}
					
					max = point;
				}
			}
		}
	}
}
