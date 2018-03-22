using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticAlgorithm;

public class OneMaxIndividual : Individual<int>
{
	public float Rate;
	
	public override void Initialize(int index)
	{
		Chromosome = new List<int>();
		for (var i = 0; i < OneMaxController.ChromosomeLength; i++)
		{
			Chromosome.Add(Random.Range(0,2));
		}
	}

	protected override void Reset()
	{
		
	}

	protected override float Action(int gene)
	{
		GoNext();
		return gene;
	}
}
