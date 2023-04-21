using System.Collections.Generic;
using UnityEngine;

namespace pool
{
    public class Pool<T>: IObjectProvider<T> where T : Behaviour
	{
		private readonly T _prefab;
		private readonly List<T> _objects;
		private readonly Transform _parent;

		public Pool(T prefab, Transform parent = null){
			_prefab = prefab;
			_objects = new List<T>();
			_parent = parent ?? new GameObject(typeof(Pool<T>).Name).transform;
		}

		public T Get()
		{
			T result = _objects.Find(item => item.isActiveAndEnabled) ?? Create();
			result.gameObject.SetActive(true);
			return result;
		}

		public void ReturnAll()
		{
			foreach(var element in _objects)
				element.gameObject.SetActive(false);
		}

		public void Clear()
        {
			foreach (var element in _objects)
				Object.Destroy(element);

			_objects.Clear();
		}

		private T Create()
		{
			T elment = Object.Instantiate(_prefab, _parent);
			_objects.Add(elment);
			return elment;
		}
    }
}