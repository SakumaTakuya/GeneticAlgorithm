using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GeneticAlgorithm;

public class OneMaxController : GeneticAlgorithmController<int>
{
	[SerializeField] private int _max;
	[SerializeField] private int _maxGeneration = 100;
	
	private readonly List<float> _rates = new List<float>();
	public static int ChromosomeLength = 100;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			StartCoroutine(Play());
		}
	}

	protected override bool Convergence()
	{
		return Generation == _maxGeneration || _max == ChromosomeLength;
	}

	protected override List<int> SelectChromosome(IList<Individual<int>> individuals)
	{		
		var val = Random.value;
		var totalRate = 0.0f;

		for (var i = 0; i < individuals.Count; i++)
		{
			totalRate += _rates[i];
			if (totalRate >= val)
			{
				return individuals[i].Chromosome;
			}
		}
		
		return individuals[0].Chromosome;
	}

	protected override List<int> Crossover(List<int> dad, List<int> mom)
	{
		//二点交叉
		/*var chromosome = new List<int>();
		
		var pointOne = Random.Range(0, ChromosomeLength);
		var pointTwo = Random.Range(pointOne + 1, ChromosomeLength);
		var lengthOne = pointTwo - pointOne;
		var lengthTwo = ChromosomeLength - pointTwo;
		//print("point1:" + pointOne + "_" + lengthOne + "/point2:" + pointTwo + "_" + lengthTwo);
		chromosome.AddRange(dad.Take(pointOne));
		chromosome.AddRange(mom.Skip(pointOne).Take(lengthOne));
		chromosome.AddRange(dad.Skip(pointTwo).Take(lengthTwo));
		
	//	print("dad:"+dad.Skip(pointOne).Take(lengthOne).Count());
	//	print("mom:"+mom.Skip(pointTwo).Take(lengthTwo).Count());
		
		var s = "Chromosome:" + chromosome.Count + "_";
		foreach (var gene in chromosome)
		{
			s += gene.ToString();
		}
	//	print(s);
		return chromosome;*/
		
		//一様交叉
		var chromosome = new List<int>();
		for (var i = 0; i < ChromosomeLength; i++)
		{
			var r = Random.Range(0, 2);
			chromosome.Add(r == 0 ? dad[i] : mom[i]);
		}
		return chromosome;
	}

	protected override void Mutate(ref List<int> chromosome)
	{
		var r = Random.Range(0, ChromosomeLength * 10);
		if(r >= ChromosomeLength) return;
		chromosome[r] = Random.Range(0,2);
	}

	protected override void OnEndPlaying(IList<Individual<int>> individuals)
	{
		//個体はfitnessが大きい順に並んでいる
		_max = (int) individuals[0].Fitness;

		var sum = individuals.Sum(i => i.Fitness);
		
		_rates.Clear();
		foreach (var individual in individuals)
		{
			_rates.Add(individual.Fitness / sum);
		}
	}
	
	protected override void OnChangeNextGeneration(int generation)
	{
		print("Generation:" + generation);
		print("max:" + _max);
	}

	protected override void OnEndSimulation()
	{
		print("Max:" + _max);
		var s = "";
		foreach (var individual in CurrentGeneration)
		{
			s += "ind:" + individual.Chromosome.Count + "_";
			foreach (var c in individual.Chromosome)
			{
				s += c.ToString() ;
			}
			s += "_" + individual.Fitness + "\n";
		}
		print(s);
	}
}
