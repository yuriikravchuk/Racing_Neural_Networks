using System.Collections.Generic;
using UnityEngine;
using pool;
using System.Collections.Generic;
	public class PoolsContainer : MonoBehaviour
	{
		private List<Pool> _pools = new List<Pool>();
		public Transform Parent => transform;


	public void Init(List<Pool> pools)
	{
		_pools = pools;
	}

	public PoolObject Get<T>() 
		{
			Pool p = _pools.Find(x => x.Setup.Prefab is T);
			return p.Get();
		}



        public void ReturnAll()
        {
            for(int i = 0; i < _pools.Count; i++)
            {
				_pools[i].ReturnAll();
            }
        }
    }