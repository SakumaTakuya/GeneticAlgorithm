using System.Collections;
using UnityEngine;

namespace TicTacToe
{
	public class PlayerBrain : Brain
	{

		private int _x;
		private int _y;

		private bool _isGetting;
		
		public override void CalculateBestMove(Game.Piece myTurn, out int x, out int y, int sizeX, int sizeY)
		{
			if (!_isGetting) StartCoroutine(GetPlayerMove());
			x = _x;
			y = _y;
		}

		private IEnumerator GetPlayerMove()
		{
			_isGetting = true;
			
			_x = -1;
			_y = -1;
			while (_x == -1 && _y == -1)
			{
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 999, LayerMask.GetMask("Panel")))
				{
					var panel = hit.collider.GetComponent<Panel>();
					_x = panel.X;
					_y = panel.Y;
				}
				
				yield return null;
			}

			_isGetting = false;
		}
	}
}
