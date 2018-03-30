using System;
using System.Linq;
using System.Threading;
using TicTacToe.GA;
using UnityEngine;

namespace TicTacToe
{
	public class NeuralNetworkBrain : Brain
	{
		
		private Matrix _middleWeight;
		private Matrix _outputWeight;

		[SerializeField] private bool _useLearned;
		[SerializeField] private string _dataPath;
		[SerializeField] private Game _game;

		private void Start()
		{
			if (_useLearned)
			{
				var mats = DataManager.ReadData<Matrix[]>(_dataPath);
				SetWeight(mats[0], mats[1]);
				print(mats[0]);
				print(mats[1]);
			}
		}

		public void SetWeight(Matrix middle, Matrix output)
		{
			_outputWeight = output;
			_middleWeight = middle;
//			print("middle:" + middle.ToString() + "\noutput:" + output.ToString());
		}
		
		public override void CalculateBestMove(Game.Piece myTurn ,out int x, out int y, int sizeX, int sizeY)
		{
			x = 4;
			y = 4;
			var max = float.MinValue;
			for (var i = 0; i < sizeX; i++)
			{
				for (var j = 0; j < sizeY; j++)
				{
					//仮想の盤面を作成
					var board = new Game.Piece[sizeX, sizeY];
					Array.Copy(_game.Board, board, _game.Board.Length);

				
					var set = _game.SetPiece(i, j, ref board);
					if (!set) continue;
				
					var point = Net(board, (int)myTurn);//Red=-1なので掛けることで符号反転
					if (point <= max) continue;
				
					x = i;
					y = j;
					max = point;
				}
			}
		}

		private float Net(Game.Piece[,] input, int myTurn)
		{
			var i = input.Cast<int>().ToArray();
//			print(_outputWeight.ToString());
//			print(_middleWeight.ToString());
			var mid = Activate(_middleWeight * i * myTurn);
			
			var point = _outputWeight * mid;
			
/*			var s = i.Aggregate("point:\n", (current, im) => current + (im + "\n"));

			for (var k = 0; k < _game.Board.GetLength(0); k++)
			{
				for (var j = 0; j < _game.Board.GetLength(1); j++)
				{
					s += input[k, j] + " ";
				}
				s += '\n';
			}
			
			print(_middleWeight+"\n"+(_middleWeight * i * myTurn)+s+point.ToString());*/
			return point[0,0];
		}

		private static Matrix Activate(Matrix matrix)
		{
			for (var i = 0; i < matrix.Row ; i++)
			{
				for (var j = 0; j < matrix.Column; j++)
				{
					 matrix[i,j] = Activate(matrix[i,j]);
				}
			}

			return matrix;
		}

		private static float Activate(float num)
		{
			return num > 0 ? num : 0;
		}
	}
}
