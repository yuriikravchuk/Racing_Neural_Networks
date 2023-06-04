using UnityEngine;
namespace pool
{
	public class PoolObject : MonoBehaviour
	{
		public bool Free { get; private set; } = true;

        private void Awake() => gameObject.SetActive(false);

        public void Push()
		{
			Free = false;
			gameObject.SetActive(true);
		}

		public void Return()
		{
			Free = true;
			gameObject.SetActive(false);
		}
	}
}
