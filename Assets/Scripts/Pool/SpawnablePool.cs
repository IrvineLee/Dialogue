using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dico.HyperCasualGame.Pool
{
	public class SpawnablePool : GenericPool<SpawnableObject, SpawnablePool>
	{
		static Dictionary<PoolType, SpawnablePool> SpawnablePoolDictionary = new Dictionary<PoolType, SpawnablePool>();

		protected override void Awake()
		{
			base.Awake();
			SpawnablePoolDictionary.Add(PoolType, this);
		}

		public static SpawnablePool GetPool(PoolType poolType)
		{
			return GetPool(poolType, SpawnablePoolDictionary);
		}

		public SpawnableObject SpawnOut(Vector3 startPosition, Transform underParent = null)
		{
			SpawnableObject spawnable = Get();
			spawnable.Spawn(startPosition);
			if (underParent) spawnable.transform.SetParent(underParent);

			return spawnable;
		}

		void OnDestroy()
		{
			SpawnablePoolDictionary.Clear();
		}
	}
}
