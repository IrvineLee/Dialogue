using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Dico.HyperCasualGame.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager sSingleton { get; private set; }

    void Awake()
    {
        if (sSingleton != null && sSingleton != this) Destroy(this.gameObject);
        else sSingleton = this;
    }

    public SpawnablePool GetPool(PoolType poolType)
    {
        return SpawnablePool.GetPool(poolType);
    }

    public void SetToDefaultObjectParent(SpawnablePool spawnablePool, SpawnableObject pooledObject)
	{
        spawnablePool.SetToDefaultObjectParent(pooledObject);
    }

    public void ReturnEverythingBackToPool()
	{
        foreach (PoolType poolType in Enum.GetValues(typeof(PoolType)))
        {
            SpawnablePool.GetPool(poolType)?.ReturnAllObjects();
        }
    }
}
