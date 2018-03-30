using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TicTacToe
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private GameController _controller;
		[SerializeField] private Brain _brainRed;
		[SerializeField] private Brain _brainBlue;

		private IEnumerator<bool> _play;
		
		private void Start()
		{
			ResetGame();
		}

		private void Update()
		{
			if (_play.Current) _play.MoveNext();
			else ResetGame();
		}

		private void ResetGame()
		{
			_play = _controller.Play(_brainBlue, _brainRed);
			_play.MoveNext();
		}
	}
}
