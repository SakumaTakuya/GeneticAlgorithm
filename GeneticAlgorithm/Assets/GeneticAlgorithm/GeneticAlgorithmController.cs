using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
				//ind.name = individual.name + i;
				CurrentGeneration.Add(ind);
			}
		}

		protected IEnumerator Play()
		{
			while (true)
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
				
				if(Convergence()) break;
				
				//次世代の子孫を作成
				//実装上は新しい染色体を作成して各個体に割り当てている(これはDestroyが重いため)
				var newChromosomes = new List<TGene>[_poplation];
				for (var i = 0; i < _poplation; i++)
				{
					
					if (i < _keepNumber)
					{
						newChromosomes[i] = new List<TGene>(results[i].Chromosome);
					}
					else
					{
						var parents = SelectChromosome(results);
						//var dad = new List<TGene>(parents[0]);
						//print(parents[0][0].GetHashCode() + "," + new List<TGene>(parents[0])[0].GetHashCode() + "," + dad[0].GetHashCode());
						newChromosomes[i] = Crossover(parents[0], parents[1]);
						//print("pre:" + CurrentGeneration[i].name + newChromosomes[i][0] + "\ndad:" + parents[0][0] + "\nmom:" + parents[1][0]);
						Mutate(ref newChromosomes[i]);
					}
				}
				
				for (var i = 0; i < _poplation; i++)
				{
					CurrentGeneration[i].Chromosome = newChromosomes[i];
					//print(CurrentGeneration[i].name + newChromosomes[i][0]);
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
		protected abstract List<TGene>[] SelectChromosome(IList<Individual<TGene>> individuals);
		
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

		/// <summary>
		/// ルーレット方式で選択する
		/// 各個体に対応した選出率をあらかじめ決めておく必要がある
		/// </summary>
		/// <param name="individuals">固体のリスト</param>
		/// <param name="rates">個体に対応した選出率</param>
		/// <returns>選択した個体の染色体</returns>
		protected static List<TGene>[] SelectRoulette(IList<Individual<TGene>> individuals, IList<float> rates, float rangeRate)
		{
			var val = UnityEngine.Random.Range(0f,rangeRate);
			var totalRate = 0.0f;
			var ret = new List<TGene>[2];
			
			for (var i = 0; i < 2; i++)
			{
				for (var j = 0; j < individuals.Count; j++)
				{
					totalRate += rates[j];
					if (totalRate >= val)
					{
						ret[i] = individuals[j].Chromosome;
						print("select:" + j);
						break;
					}
				}
			}

			return ret;
		}

		/// <summary>
		/// エリート方式で選択する
		/// </summary>
		/// <param name="individuals"></param>
		/// <returns></returns>
		protected static List<TGene>[] SelectElite(IList<Individual<TGene>> individuals)
		{
			return new []{ individuals[0].Chromosome, individuals[1].Chromosome};
		}
	}
}
