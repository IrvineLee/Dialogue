using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dico.HyperCasualGame.Pool
{
	public class SpawnableObject : MonoBehaviour
	{
		//SpawnablePool pool;

		//public void SetPool(PoolType poolType)
		//{
		//	pool = PoolManager.sSingleton.GetPool(poolType);
		//	Debug.Log(pool);
		//}

		public virtual void Spawn(Vector3 position)
		{
			gameObject.SetActive(true);
			gameObject.transform.position = position;
		}

		//void OnDisable()
		//{
		//	pool?.ReturnObject(this);
		//}
	}
}