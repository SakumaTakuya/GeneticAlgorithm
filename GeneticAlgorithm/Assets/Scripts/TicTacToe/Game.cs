using System;
using UnityEngine;

namespace TicTacToe
{
	public class Game : MonoBehaviour 
	{
		public enum Piece
		{
			Red = -1,
			Null = 0,
			Blue = 1
		}

		/// <summary>
		/// 盤面
		/// </summary>
		public Piece[/*盤面のx軸*/,/*盤面のy軸*/] Board;

		private int _sizeX;
		private int _sizeY;

		public int WinCount = 4;
	
		/// <summary>
		/// 現在駒を撃っているプレイヤー
		/// </summary>
		public Piece Turn = Piece.Blue;
		public Piece Winner = Piece.Null;

		public int TurnCount;

		/// <summary>
		/// 盤面の駒を全て空にする
		/// </summary>
		/// <param name="sizeX"></param>
		/// <param name="sizeY"></param>
		public void InitializeBoad(int sizeX, int sizeY)
		{
			_sizeX = sizeX;
			_sizeY = sizeY;
			Board = new Piece[sizeX,sizeY];
			ResetBoad();
		}

		/// <summary>
		/// ボードを全て空にする
		/// </summary>
		public void ResetBoad()
		{
			TurnCount = 0;
			Winner = Piece.Null;
			for (var x = 0; x < _sizeX; x++)
			{
				for (var y = 0; y < _sizeY; y++)
				{
					Board[x,y] = Piece.Null;
				}
			}
		}

		/// <summary>
		/// 指定した座標に一手を打つ
		/// 通常のゲームとして遊ぶ際にはこの関数を利用する
		/// </summary>
		/// <returns>勝敗が決しているかどうか.指定した座標にすでに駒が存在している場合はfalse</returns>
		public bool Move(int x, int y)
		{
			var set = SetPiece(x, y, ref Board);
			if (!set)
			{
				/*var s = "無効な一手:x=" + x + " ,y=" + y + ":\n";
				for (var i = 0; i < _sizeX; i++)
				{
					for (var j = 0; j < _sizeY; j++)
					{
						s += Board[i, j] + " ";
					}
					s += '\n';
				}
				throw new Exception(s);*/
				return false;
			}
			TurnCount++;
			var ret = Judge(x, y, ref Board);
			if(!ret) ChangeTurn();
			return ret;
		}

		/// <summary>
		/// 指定した場所に駒を置く
		/// </summary>
		/// <returns>設置できたかどうか</returns>
		public bool SetPiece(int x, int y, ref Piece[,] board)
		{
			if(x < 0 || x >= board.GetLength(0) ||
			   y < 0 || y >= board.GetLength(1)) return false;
			if(board[x,y] != Piece.Null) return false;	
			board[x,y] = Turn;
			return true;
		}

		/// <summary>
		/// 勝敗の判定を行う
		/// </summary>
		/// <returns>勝敗が決しているかどうか</returns>
		private bool Judge(int x, int y,ref Piece[,] board)
		{
			var count = CountSequence(Turn, x, y, board);
			//CountSequence(Turn, x, y, ref count, ref board, ref map);
			
			var ret = count >= WinCount;
			if (ret) Winner = Turn;
			return TurnCount >= _sizeX * _sizeY || ret;
		}

		/// <summary>
		/// ターンの交代を行う
		/// </summary>
		public void ChangeTurn()
		{
			Turn = (Piece)(-1 * (int) Turn);
		}

		/// <summary>
		/// ターゲットの駒がその座標の付近でいくつ連結しているかカウントする
		/// </summary>
		/*public void CountSequence(Piece target, int x, int y, ref int count, ref Piece[,] source, ref bool[,] map)
		{
			if (source[x,y] == target) count++;
			else return;
	
			if (count >= 3) return;
			
			map[x, y] = true;
			
			if (x - 1 >= 0)
			{
				if(!map[x-1, y])CountSequence(target, x - 1, y, ref count, ref source, ref map);	
				if (y - 1 >= 0 && !map[x-1,y-1]) CountSequence(target, x - 1, y - 1, ref count, ref source, ref map);
			}

			if (y - 1 >= 0)
			{
				if(!map[x, y-1])CountSequence(target, x, y - 1, ref count, ref source, ref map);
				if (x + 1 < Board.GetLength(0) && !map[x+1,y-1]) CountSequence(target, x + 1, y - 1, ref count, ref source, ref map);	
			}

			if (x + 1 < Board.GetLength(0))
			{
				if(!map[x+1,y])CountSequence(target, x + 1, y, ref count, ref source, ref map);
				if (y + 1 < Board.GetLength(1) && !map[x+1,y+1]) CountSequence(target, x + 1, y + 1, ref count, ref source, ref map);
			}
		
			if (y + 1 < Board.GetLength(1))
			{
				if(!map[x,y+1])CountSequence(target, x, y + 1, ref count, ref source, ref map);
				if (x - 1 >= 0 && !map[x-1,y+1]) CountSequence(target, x - 1, y + 1, ref count, ref source, ref map);
			}
			
			
		}*/

		public int CountSequence(Piece target, int x, int y, Piece[,] source)
		{
			var count = 0;
			var map = new bool[_sizeX, _sizeY];
			CountDir(target, x, y, ref count, source, 1, 0, ref map);//[-]
			
			if (count >= WinCount) return count;
			count = 0;
			map = new bool[_sizeX, _sizeY];
			CountDir(target, x, y, ref count, source, 0, 1, ref map);//[|]
			
			if (count >= WinCount) return count;
			count = 0;
			map = new bool[_sizeX, _sizeY];
			CountDir(target, x, y, ref count, source, 1, -1, ref map);//[\]
			
			if (count >= WinCount) return count;
			count = 0;
			map = new bool[_sizeX, _sizeY];
			CountDir(target, x, y, ref count, source, 1, 1, ref map);//[/]
			return count;
		}

		private static void CountDir(Piece target, int x, int y, ref int count, Piece[,] source, int dirX, int dirY, ref bool[,]map)
		{	
			//範囲外や捜査済みなら終了
			if(x < 0 || x >= source.GetLength(0) ||
			   y < 0 || y >= source.GetLength(1)) return;
			
			if(map[x,y]) return;
			map[x, y] = true;
			
			
			//自分の位置に対象の駒がおいてあれば＋、そうでなければ捜査終了
			if (source[x,y] == target) count++;
			else return;
			

			
			CountDir(target, x - dirX, y - dirY, ref count, source, dirX, dirY, ref map);
			CountDir(target, x + dirX, y + dirY, ref count, source, dirX, dirY, ref map);
		}
	}
}