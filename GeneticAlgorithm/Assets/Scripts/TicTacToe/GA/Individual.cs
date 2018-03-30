using System.Collections.Generic;
using GeneticAlgorithm;
using UnityEngine;

namespace TicTacToe.GA
{
	public class Individual : Individual<Gene>
	{
		[SerializeField] private int _middleSize;
		[SerializeField] private NeuralNetworkBrain _neuralNetworkBrain;
		[SerializeField] private Brain _enemy;
		[SerializeField] private GameController _gameController;


		private IEnumerator<bool> _play;
		
		public override void Initialize(int index)
		{
			Chromosome = new List<Gene>();
			var middle = new Matrix(_middleSize, _gameController.SizeX * _gameController.SizeY);
			var output = new Matrix(1, _middleSize);
			
			for (var i = 0; i < _middleSize; i++)
			{
				for (var j = 0; j < middle.Column; j++)
				{
					middle[i,j] = Random.value;
				}
			}

			for (var i = 0; i < _middleSize; i++)
			{
				output[0, i] = Random.value;
			}
			
			for (var i = 0; i < Controller.ChromosomeLength; i++)
			{
				Chromosome.Add(new Gene(middle, output));				
			}
		}

		protected override void ResetIndividual()
		{
			//_neuralNetworkBrain.SetGameSize(_gameController.SizeX, _gameController.SizeY);
			//_enemy.SetGameSize(_gameController.SizeX, _gameController.SizeY);
			SetPlay();
//			print(Chromosome[0].Weight[0] + "," + Chromosome[0].Weight[1]);
		}

		private void SetPlay()
		{
			_play = Random.Range(0,2) == 0 ? 
				_gameController.Play(_neuralNetworkBrain, _enemy) :
				_gameController.Play(_enemy, _neuralNetworkBrain);
		}

		protected override float Phenotype(Gene gene)
		{			
			_neuralNetworkBrain.SetWeight(gene.Weight[0], gene.Weight[1]);
			
			_play.MoveNext();

			if (_play.Current) return 0;
			
			SetPlay();
			GoNext();
			//自分の駒と勝者が一致している時、1を返し、そうでないとき-1
			return (float)_neuralNetworkBrain.MyTurn * (float)_gameController.Winner;
		}
	}
}
