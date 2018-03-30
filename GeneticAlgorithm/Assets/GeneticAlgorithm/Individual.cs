using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneticAlgorithm
{
    /// <summary>
    /// 遺伝的アルゴリズムを適応する個体
    /// 正しく動作させるためには、実行中何らかの形でGoNext()が呼ばれる必要がある
    /// </summary>
    /// <typeparam name="TGene">遺伝子の型</typeparam>
    public abstract class Individual<TGene> : MonoBehaviour
    {
        [HideInInspector] public List<TGene> Chromosome;
        [HideInInspector] public float Fitness;

        private bool _next;

        
        /// <summary>
        /// 次の遺伝子へ移動する
        /// </summary>
        public void GoNext()
        {
            _next = true;
        }
        
        public IEnumerator Play(IList<Individual<TGene>> results)
        {
//            print("Play");
            ResetIndividual();
            Fitness = 0;
            foreach (var gene in Chromosome)
            {
                _next = false;
                do
                {
                    Fitness += Phenotype(gene);
                    yield return null;
                } while (!_next);
            }
            
            results.Add(this);
        }

        /// <summary>
        /// 染色体の初期生成
        /// </summary>
        /// <param name="index"></param>
        public abstract void Initialize(int index);

        /// <summary>
        /// キャラクタの初期位置などの定義が必要であればここに書く
        /// </summary>
        protected abstract void ResetIndividual();
        
        /// <summary>
        /// 実際に各個体に行わせる行動を定義する
        /// </summary>
        /// <param name="gene">行動を抽象化した遺伝子</param>
        /// <returns>行動の結果として得られた報酬</returns> 
        protected abstract float Phenotype(TGene gene);

    }
}


