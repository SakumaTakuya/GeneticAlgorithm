using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GeneticAlgorithm
{
	public abstract class GeneticAlgorithmController<TGene> : MonoBehaviour
	{
		protected List<Individual<TGene>> CurrentGeneration = new List<Individual<TGene>>();

		[SerializeField] private int _poplation = 10;
		[SerializeField] private float _timeScale = 10;
		[SerializeField] private int _keepNumber = 2;
		[SerializeField] private GameObject _individual;

		public int Generation { get; private set; }
		
		// Use this for initialization
		protected virtual void Start ()
		{
			Time.timeScale = _timeScale;
			//染色体の初期化
			var individual = _individual.GetComponent<Individual<TGene>>();
			individual.Initialize(0);
			CurrentGeneration.Add(individual);
			for (var i = 1; i < _poplation; i++)
			{
				var ind = Instantiate(individual);
				//print(ind);
				ind.Initialize(i);
				CurrentGeneration.Add(ind);
			}
		}

		protected IEnumerator Play()
		{
			while (!Convergence())
			{
				var results = new List<Individual<TGene>>();
				
				//それぞれの個体の行動を開始する
//				print(CurrentGeneration[0]);
				foreach (var individual in CurrentGeneration)
				{
					//print(individual);
					StartCoroutine(individual.Play(results));
				}
				
				//全ての結果が揃うまで待機
				yield return new WaitPlayReslut<Individual<TGene>>(_poplation, results);
				
				//点数が高い順に並べ替える
				results = results.OrderByDescending(ret =>  ret.Fitness).ToList();
				
				OnEndPlaying(results);
				
				//次世代の子孫を作成
				//実装上は新しい染色体を作成して各個体に割り当てている(これはDestroyが重いため)
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
				
				Generation++;
				
				OnChangeNextGeneration(Generation);
			}
			
			OnEndSimulation();
		}

		protected virtual void OnEndPlaying(IList<Individual<TGene>> individuals)
		{
			print("endPlaying:" + individuals);
		}

		protected virtual void OnChangeNextGeneration(int generation)
		{
			print("Generation:" + generation);
		}

		protected virtual void OnEndSimulation()
		{
			
		}

		/// <summary>
		/// 収束判定を行う
		/// </summary>
		/// <returns>収束した場合true</returns>
		protected abstract bool Convergence();
		
		/// <summary>
		/// 交配に使用する染色体を選ぶ
		/// </summary>
		/// <param name="individuals"></param>
		/// <returns>選択した親の染色体(Chromosome)</returns>
		protected abstract List<TGene> SelectChromosome(IList<Individual<TGene>> individuals);
		
		/// <summary>
		/// 二つの染色体から新たな染色体を生成する
		/// </summary>
		/// <param name="dad"></param>
		/// <param name="mom"></param>
		/// <returns>新しい染色体</returns>
		protected abstract List<TGene> Crossover(List<TGene> dad, List<TGene> mom);
		
		/// <summary>
		/// 一定確率で突然変異を起こす
		/// </summary>
		/// <param name="chromosome"></param>
		protected abstract void Mutate(ref List<TGene> chromosome);
		
		public class WaitPlayReslut<T> : CustomYieldInstruction
		{
			private readonly int _reslutNum;
			private readonly List<T> _list;

			public override bool keepWaiting
			{
				get
				{
					//Debug.Log("wait:"+ _list.Count);
					return _list.Count < _reslutNum;
				}
			}
			
			public WaitPlayReslut(int resultNum, List<T> list)
			{
				_reslutNum = resultNum;
				_list = list;
			}
		}
	}
	
	
}
