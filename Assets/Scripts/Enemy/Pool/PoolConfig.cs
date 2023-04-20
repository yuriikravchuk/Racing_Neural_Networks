using pool;
using System.Collections.Generic;
using UnityEngine;

namespace pool
{
	[CreateAssetMenu(fileName = "PoolConfig")]
	public class PoolConfig : ScriptableObject, IPoolsProvider
	{
		[SerializeField]private List<PoolSetup> _poolsSetup = new List<PoolSetup>();

		public IReadOnlyList<PoolSetup> PoolsSetup => _poolsSetup;
    }

	public interface IPoolsProvider
    {
		IReadOnlyList<PoolSetup> PoolsSetup { get; }
	}
}