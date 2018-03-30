using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
	public class GameController : MonoBehaviour
	{
		[SerializeField] private bool _doRendering;
		[SerializeField] private bool _writeConsole;
		[SerializeField] private Game _game;
		[SerializeField] private GameRenderer _renderer;
		
		public int SizeX;
		public int SizeY;

		public Game.Piece Winner = Game.Piece.Null;

		private void Start()
		{
			_game.InitializeBoad(SizeX, SizeY);
			if(_doRendering) _renderer.SetStage(SizeX, SizeY);
		}

		/// <summary>
		/// Play中はtrueを返す
		/// </summary>
		/// <param name="red"></param>
		/// <param name="blue"></param>
		/// <returns></returns>
		public IEnumerator<bool> Play(Brain red, Brain blue)
		{
			var players = new Dictionary<Game.Piece, Brain> {{Game.Piece.Red, red}, {Game.Piece.Blue, blue}, {Game.Piece.Null, null}};
			
			//print("start");
			
			red.SetMyTurn(Game.Piece.Red);
			blue.SetMyTurn(Game.Piece.Blue);
			
			var x = 0;
			var y = 0;
			
			_game.ResetBoad();
		
			do
			{
//				print(_game.Turn);
				var player = players[_game.Turn];
				player.DeterminePosition = false;
				player.CalculateBestMove(_game.Turn , out x, out y, SizeX, SizeY);
				if(_doRendering) _renderer.Render(x, y, _game.Turn);
				yield return true;
				
			} while (!_game.Move(x, y));
			
			Winner = _game.Winner;
			if (_writeConsole)
			{
				var s = "Winner:" + Winner + "(player:" + players[Winner] + "):\n";
				for (var i = 0; i < _game.Board.GetLength(0); i++)
				{
					for (var j = 0; j < _game.Board.GetLength(1); j++)
					{
						s += _game.Board[i, j] + " ";
					}
					s += '\n';
				}
				print(s);
			}
			if(_doRendering) _renderer.Clean();
			
			yield return false;
		}
		
		public class WaitDetermine : CustomYieldInstruction
		{
			private readonly Brain _brain;
			public override bool keepWaiting
			{
				get { return !_brain.DeterminePosition; }
			}

			public WaitDetermine(Brain brain)
			{
				_brain = brain;
			}
		}
	}
}
