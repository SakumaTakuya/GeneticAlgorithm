using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnedCar : MonoBehaviour {
	
	[SerializeField] private float _power = 10;
	[SerializeField] private CarData _data;
	
	private Rigidbody _rigidbody;
	private CarGene _nowGene;
	private int _pointer;

	// Use this for initialization
	private void Start ()
	{
		Time.timeScale = 30;
		_rigidbody = GetComponent<Rigidbody>();
		MoveNext();
	}
	
	// Update is called once per frame
	private void Update () 
	{
		
		if (_nowGene.CurrentTime > 0f)
		{
			var dir = new Vector3(_nowGene.Right, 0, _nowGene.Front).normalized;		
			_rigidbody.AddForce(dir * _power);
			_nowGene.CurrentTime -= Time.deltaTime;
		}
		else
		{
			MoveNext();
		}
	}

	private void MoveNext()
	{
		_nowGene = _data.Genes[_pointer++];
		_nowGene.Reset();
		print(_nowGene.CurrentTime);
	}
}
