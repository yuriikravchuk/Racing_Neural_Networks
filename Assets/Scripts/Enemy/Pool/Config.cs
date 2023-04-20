using UnityEngine;

public abstract class Config<T> : ScriptableObject where T : Config<T>
{
	private static T _instance;
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = (T)Resources.Load($"{typeof(T).Name}");
				if (_instance == null)
				{
					_instance = CreateInstance<T>();
				}
			}
			return _instance;
		}
	}
}
