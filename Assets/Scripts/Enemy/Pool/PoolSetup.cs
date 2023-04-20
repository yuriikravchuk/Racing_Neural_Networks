namespace pool
{
	[System.Serializable]
	public class PoolSetup
	{
		public PoolObject Prefab;
		//public int Id;
		public bool DisableOnReturn;

		public PoolSetup(PoolObject prefab, bool disableOnReturn = false)
		{
			Prefab = prefab;
			//Id = id;
			DisableOnReturn = disableOnReturn;
		}
	}
}
