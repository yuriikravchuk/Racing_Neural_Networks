using UnityEngine;
using System;
namespace pool
{
	public class PoolObject : MonoBehaviour
	{
		public bool Free { get; private set; } = true;

		public event Action Returned;

		private bool _isSpawned = false;
		private bool _disableObjectOnSpawn = true;

		public void Init(Pool pool)
		{
			if (!_isSpawned)
			{
				_isSpawned = true;
				_disableObjectOnSpawn = pool.Setup.DisableOnReturn;
				if (_disableObjectOnSpawn)
					gameObject.SetActive(false);
			}
		}

		public void Push()
		{
			Free = false;
			if (_disableObjectOnSpawn)
				gameObject.SetActive(true);
		}

		public void Return()
		{
			if (_isSpawned)
			{
				Free = true;
				//transform.SetParent(PoolsContainer.Parent);
				if (_disableObjectOnSpawn)
					gameObject.SetActive(false);
			}
			else
				Destroy(gameObject);
		}
	}
}
