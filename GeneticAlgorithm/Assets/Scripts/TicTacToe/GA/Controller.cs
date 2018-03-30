using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TicTacToe.GA
{
	public class Controller : GeneticAlgorithmController<Gene>
	{
		[SerializeField] private int _maxGenerations = 100;
		[SerializeField] private float _plotSize = 20;
		[SerializeField] private float _stop = 80;
		[SerializeField] private BestWeight _best;
		[SerializeField] private BestWeight _source;
		[SerializeField] private string _dataPath;
		
		public static int ChromosomeLength = 20;
		
		private readonly List<float> _rates = new List<float>();
		private float _max;

		private Matrix[] _bestWeights;
		
		private readonly List<Vector2> _points = new List<Vector2>();
		
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				print("Start");
				StartCoroutine(Play());
			}
		}
		
		protected override bool Convergence()
		{
			return Generation == _maxGenerations || _max >= _stop;
		}

		protected override List<Gene>[] SelectChromosome(IList<Individual<Gene>> individuals)
		{
			//return SelectRoulette(individuals, _rates, 0.5f);
			return SelectElite(individuals);
		}

		protected override List<Gene> Crossover(List<Gene> dad, List<Gene> mom)
		{
			//print("dad:" + dad[0].Weight[0]);
			//一様交叉
			var chromosome = new List<Gene>();
			var weights = dad[0].Weight;
			var newWeight = new Matrix[weights.Length];
			
			for (var index = 0; index < weights.Length; index++)
			{
				var weight = new Matrix(weights[index].Row, weights[index].Column);
				for (var i = 0; i < weight.Row; i++)
				{
					for (var j = 0; j < weight.Column; j++)
					{
						var r = Random.Range(0, 2) == 0;
						weight[i, j] =  r ? 
							dad[0].Weight[index][i, j] : 
							mom[0].Weight[index][i, j];
					}
				}
				newWeight[index] = weight;
			}
			//print("new:" + newWeight[0]+ "\n" + newWeight[1]);
			var gene = new Gene(newWeight[0], newWeight[1]);

			for (var i = 0; i < ChromosomeLength; i++)
			{
				chromosome.Add(gene);
			}
			
			return chromosome;
		}

		protected override void Mutate(ref List<Gene> chromosome)
		{
			var wei = chromosome[0].Weight;

			var accord = true;
			for (var i = 1; i < CurrentGeneration.Count; i++)
			{
				accord &= CurrentGeneration[i].Chromosome[0].Weight[0] == CurrentGeneration[i - 1].Chromosome[0].Weight[0];
			}
			if(accord || Random.Range(0,4) == 0) return;
			
			
			for (var index = 0; index < wei.Length; index++)
			{
				for (var i = 0; i < wei[index].Row; i++)
				{
					for (var j = 0; j < wei[index].Column; j++)
					{
						wei[index][i, j] = Random.value;
					}
				}
			}
			
			for (var i = 0; i < ChromosomeLength; i++)
			{
				Array.Copy(wei, chromosome[i].Weight, wei.Length);
			}
		}
		
		protected override void OnEndPlaying(IList<Individual<Gene>> individuals)
		{
			//個体はfitnessが大きい順に並んでいる
			_max =  individuals[0].Fitness / ChromosomeLength;
			_bestWeights = individuals[0].Chromosome[0].Weight;
			
			var sum = individuals.Sum(i => i.Fitness);
		
			_rates.Clear();
			foreach (var individual in individuals)
			{
				_rates.Add(individual.Fitness / sum);
			}
		}

		protected override void OnEndSimulation()
		{
			var b = DataManager.WriteData(_bestWeights, _dataPath);
			print("save:" + _max);
			var mat = DataManager.ReadData<Matrix[]>(_dataPath);
			print(mat[0]);
			print(mat[1]);
		}

		protected override void OnChangeNextGeneration(int generation)
		{
			_points.Add(new Vector2(Generation, _max));
			print("Generation:" + generation);
			print("Max:" + _max);
		}
	
		private void OnGUI()
		{
			foreach (var point in _points)
			{
				GUI.Label(new Rect(point.x, 400 - point.y * _plotSize,50,50), "o");			
			}
		}
	}
}
