using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GeneticAlgorithm
{
	public abstract class GenericController<T> : GeneticAlgorithmController<T>
	{
		public int ChromosomeLength = 100;
		[SerializeField, Range(0f, 1f)] private float _mutateRate = 0.1f;
		[SerializeField] private float _plotSize = 20;

		protected readonly List<float> Rates = new List<float>();
		protected float Max;
		
		private readonly List<Vector2> _points = new List<Vector2>();

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				StartCoroutine(Play());
			}
		}

		protected override List<T>[] SelectChromosome(IList<Individual<T>> individuals)
		{
			return SelectRoulette(individuals, Rates, 0.5f);
		}

		protected override List<T> Crossover(List<T> dad, List<T> mom)
		{
			var chromosome = new List<T>();
		
			var pointOne = Random.Range(0, ChromosomeLength);
			var pointTwo = Random.Range(pointOne + 1, ChromosomeLength);
			var lengthOne = pointTwo - pointOne;
			var lengthTwo = ChromosomeLength - pointTwo;

			chromosome.AddRange(dad.Take(pointOne));
			chromosome.AddRange(mom.Skip(pointOne).Take(lengthOne));
			chromosome.AddRange(dad.Skip(pointTwo).Take(lengthTwo));

			return chromosome;
		}

		protected override void Mutate(ref List<T> chromosome)
		{
			var r = Random.Range(0,(int)(ChromosomeLength / _mutateRate));
			if(r >= ChromosomeLength) return;
			CreateNewGene(ref chromosome, r);
		}

		protected abstract void CreateNewGene(ref List<T> chromosome, int index);

		protected override void OnEndPlaying(IList<Individual<T>> individuals)
		{
			//個体はfitnessが大きい順に並んでいる
			Max =  individuals[0].Fitness;
		
			var sum = individuals.Sum(i => i.Fitness);
		
			Rates.Clear();
			foreach (var individual in individuals)
			{
				Rates.Add(individual.Fitness / sum);
			}
		}
	
		protected override void OnChangeNextGeneration(int generation)
		{
			_points.Add(new Vector2(Generation, Max));
			print("Generation:" + generation);
			print("Max:" + Max);
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

