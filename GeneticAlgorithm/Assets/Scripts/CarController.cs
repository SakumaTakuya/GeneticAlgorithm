using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GeneticAlgorithm;

public class CarController : GeneticAlgorithmController<CarGene>
{
	public static bool IsGoaled;
	public static int ChromosomeLength = 100;
	
	[SerializeField] private int _maxGeneration = 100;
	[SerializeField] private float _size = 20;
	[SerializeField] private CarData _data;
	
	private readonly List<float> _rates = new List<float>();
	private float _max;
	private List<Vector2> _points = new List<Vector2>();

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			StartCoroutine(Play());
		}
	}

	protected override bool Convergence()
	{
		return Generation == _maxGeneration || IsGoaled;
	}

	protected override List<CarGene>[] SelectChromosome(IList<Individual<CarGene>> individuals)
	{
		return SelectRoulette(individuals, _rates, 0.5f);
	}

	protected override List<CarGene> Crossover(List<CarGene> dad, List<CarGene> mom)
	{
		var chromosome = new List<CarGene>();
		
		var pointOne = Random.Range(0, ChromosomeLength);
		var pointTwo = Random.Range(pointOne + 1, ChromosomeLength);
		var lengthOne = pointTwo - pointOne;
		var lengthTwo = ChromosomeLength - pointTwo;

		chromosome.AddRange(dad.Take(pointOne));
		chromosome.AddRange(mom.Skip(pointOne).Take(lengthOne));
		chromosome.AddRange(dad.Skip(pointTwo).Take(lengthTwo));

		return chromosome;
	}

	protected override void Mutate(ref List<CarGene> chromosome)
	{
		var r = Random.Range(0, ChromosomeLength * 10);
		if(r >= ChromosomeLength) return;
		//chromosome.Add(new CarGene(Random.Range(0f,2f),Random.Range(-1,2),Random.Range(-1,2)));
		chromosome[r] = new CarGene(Random.Range(0f,0.5f),Random.Range(-1,2),Random.Range(-1,2));
	}

	protected override void OnEndPlaying(IList<Individual<CarGene>> individuals)
	{
		//個体はfitnessが大きい順に並んでいる
		_max =  individuals[0].Fitness;
		
		//print("0:" +  _max + "max:" + individuals.Max(i => i.Fitness));
		
		var sum = individuals.Sum(i => i.Fitness);
		
		_rates.Clear();
		foreach (var individual in individuals)
		{
			_rates.Add(individual.Fitness / sum);
		}
	}
	
	protected override void OnChangeNextGeneration(int generation)
	{
		_points.Add(new Vector2(Generation, _max));
		print("Generation:" + generation);
		print("max:" + _max);
	}
	
	protected override void OnEndSimulation()
	{
		var max = CurrentGeneration.First(i => i.Fitness.Equals(CurrentGeneration.Max(j => j.Fitness)));
		_data.Genes = max.Chromosome.ToArray();
	}

	private void OnGUI()
	{
		foreach (var point in _points)
		{
			GUI.Label(new Rect(point.x, 400 - point.y * _size,50,50), "o");	
			
		}
		
	}
}
