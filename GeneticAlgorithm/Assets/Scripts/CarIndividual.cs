using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneticAlgorithm;

[RequireComponent(typeof(Rigidbody))]
public class CarIndividual : Individual<CarGene>
{
	[SerializeField] private float _power = 10;
	[SerializeField] private Transform _goal;

	//private float _time;
	private bool _isDead;
	private Rigidbody _rigidbody;
//	private Vector3 _prePos;

//	private int _num;
	
	public override void Initialize(int index)
	{
		transform.position = Vector3.up;
	//	_num = index;
		_rigidbody = GetComponent<Rigidbody>();
		Chromosome = new List<CarGene>();
		for (var i = 0; i < CarController.ChromosomeLength; i++)
		{
			Chromosome.Add(new CarGene(Random.Range(0f,1f),Random.Range(-1,2),Random.Range(-1,2)));
		}
	}

	protected override void Reset()
	{
		_isDead = false;
		//print(_num + ":" + Chromosome.Count);
		//_time = 0;
		foreach (var gene in Chromosome)
		{
			gene.Reset();
		}
		transform.position = Vector3.up;
		//_prePos = transform.position;
		//print("reset");
	}

	protected override float Action(CarGene gene)
	{
		if (_isDead)
		{
			GoNext();
			return 0f;
		}
		
		
		var dir = new Vector3(gene.Right, 0, gene.Front).normalized;
		//print(_num + ":" + gene.CurrentTime + "dir:" + dir + "delta:" + Time.deltaTime);
		
		_rigidbody.AddForce(dir * _power);
		//_time += Time.deltaTime;

		
		gene.CurrentTime -= Time.deltaTime;
		if(gene.CurrentTime < 0) GoNext();

		//var dist = Vector3.Distance(transform.position, _prePos);
		//_prePos = transform.position;
		return 1 / Vector3.Distance(transform.position, _goal.position);
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Pole"))
		{
			//print("dead");
			_isDead = true;
		}
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Goal"))
		{
			_isDead = true;
			Fitness += 1000;
			CarController.IsGoaled = true;
		}
	}
}
