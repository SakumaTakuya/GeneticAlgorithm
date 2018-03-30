using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
	public class GameRenderer : MonoBehaviour
	{
		[SerializeField] private Panel _panel;
		[SerializeField] private GameObject _red;
		[SerializeField] private GameObject _blue;
		[SerializeField] private float _space;

		private readonly List<GameObject> _gameObjects = new List<GameObject>();
		
		// Use this for initialization
		private void Start () 
		{

		}

		public void SetStage(int sizeX, int sizeY)
		{
			for (var i = 0; i < sizeX; i++)
			{
				for (var j = 0; j < sizeY; j++)
				{
					var p = Instantiate(_panel, new Vector3(i, 0, j) * _space, _panel.transform.rotation);
					p.X = i;
					p.Y = j;
				}
			}
		}

		public void Render(int x, int y, Game.Piece turn)
		{
			if(x == -1 ||  y == -1) return;
			var go = turn == Game.Piece.Blue ? _blue : _red;
			var obj = Instantiate(go, new Vector3(x, 0.25f, y) * _space, go.transform.rotation);
			_gameObjects.Add(obj);
		}

		public void Clean()
		{
			foreach (var go in _gameObjects)
			{
				Destroy(go);
			}
		}
	}
}
