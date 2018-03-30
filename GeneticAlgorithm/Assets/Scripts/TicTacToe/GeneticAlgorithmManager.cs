using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace TicTacToe
{
	public class GeneticAlgorithmManager : MonoBehaviour
	{
/*		[SerializeField] private int _middle;
		[SerializeField] private int _poplation;
		[SerializeField] private int _keepNumber;
		[SerializeField] private int _maxGeneration;
		[SerializeField] private GameController _controller;
		[SerializeField] private Game _game;
		[SerializeField] private NeuralNetworkBrain _neuralNetwork;
		[SerializeField] private Brain _enemy;


		private int _generation;
		private IEnumerator<bool>[] _play;

		private void Update()
		{
			if (_play != null) return;
			if (!Input.GetKeyDown(KeyCode.A)) return;
			_neuralNetwork.Initialize(_middle);
			StartCoroutine(Reproduct());
		}

		private void SetPlay()
		{
			_play = new IEnumerator<bool>[_poplation];
			for (var i = 0; i < _poplation; i++)
			{
				_play[i] = Random.Range(0, 2) == 1 ? 
					_controller.Play(_neuralNetwork, _enemy) : 
					_controller.Play(_enemy, _neuralNetwork);
			}
		}


		private IEnumerator Reproduct()
		{
			while (_maxGeneration > _generation)
			{
				/*SetPlay();
				foreach (var play in _play)
				{
					//勝負 //1フレームで1勝負を終わらせる
					do { play.MoveNext();} while (play.Current);
				}
				
				yield return null;
				
				//世代交代
				for (var i = 0; i < _poplation; i++)
				{
					List<TGene> child;
					if (i < _keepNumber)
					{
						child = results[i].Chromosome;
					}
					else
					{
						var dad = SelectChromosome(results);
						var mom = SelectChromosome(results);				
						child = Crossover(dad, mom);
						Mutate(ref child);
					}

					CurrentGeneration[i].Chromosome = child;
				}
				
				_generation++;
			}
			yield return null;
		}*/
	}
}
