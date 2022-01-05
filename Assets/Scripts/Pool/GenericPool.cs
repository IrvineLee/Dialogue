using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

using Dico.Helper;

namespace Dico.HyperCasualGame.Pool
{
	public class GenericPool<T1, T2> : MonoBehaviour where T1 : Component where T2 : Component
	{
		[SerializeField] T1 prefab = null;
		[SerializeField] AssetReference assetReference = null;

		[SerializeField] PoolType poolType = PoolType.None;
		[SerializeField] int size = 20;

		List<T1> freeList = new List<T1>();
		List<T1> usedList = new List<T1>();

		public PoolType PoolType { get => poolType; }

		Collider prefabCollider;
		Rigidbody prefabRgBody;

		protected virtual void Awake()
		{
			prefabCollider = prefab.GetComponentInChildren<Collider>();
			prefabRgBody = prefab.GetComponentInChildren<Rigidbody>();

			Addressables.LoadAssetAsync<GameObject>(assetReference).Completed += OnLoadDone;
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		protected static T2 GetPool(PoolType poolType, Dictionary<PoolType, T2> dictionary)
		{
			if (dictionary.Count == 0) return null;

			T2 pool = null;
			dictionary.TryGetValue(poolType, out pool);

			return pool;
		}

		protected T1 Get()
		{
			var numFree = freeList.Count;
			if (numFree == 0)
			{
				var instance = assetReference.InstantiateAsync(transform).Result.GetComponent<T1>();

				freeList.Add(instance);
				freeList[0].gameObject.SetActive(false);
				numFree = freeList.Count;
			}

			var pooledObject = freeList[numFree - 1];
			freeList.RemoveAt(numFree - 1);
			usedList.Add(pooledObject);

			return pooledObject;
		}

		public void ReturnObject(T1 pooledObject)
		{
			if (pooledObject == null || freeList.Contains(pooledObject)) return;
			Debug.Assert(usedList.Contains(pooledObject));

			usedList.Remove(pooledObject);
			freeList.Add(pooledObject);

			Transform pooledObjectTransform = ResetTransform(pooledObject);
			ResetColliderAndRigidbody(pooledObjectTransform);

			if (pooledObject.gameObject.activeSelf)
			{
				pooledObject.transform.SetParent(transform);
				CoroutineHelper.WaitNextFrame(() => pooledObject?.gameObject.SetActive(false));
			}
			else
			{
				// Return it back to the hierarchy.
				// Since most of the pooled objects are returned when the object is disabled,
				// set the parent on the next frame to avoid errors.
				CoroutineHelper.WaitNextFrame(() => pooledObject?.transform.SetParent(transform));
			}

			// Make sure the object is disabled.
			pooledObject.gameObject.SetActive(false);
		}

		public void ReturnAllObjects()
		{
			for (int i = 0; i < usedList.Count;)
			{
				ReturnObject(usedList[i]);
			}
		}

		public void SetToDefaultObjectParent(T1 pooledObject)
		{
			pooledObject.transform.SetParent(transform);
		}

		Transform ResetTransform(T1 pooledObject)
		{
			var pooledObjectTransform = pooledObject.transform;

			ResetTransformChildRecursive(prefab.transform, pooledObjectTransform);
			ResetRigidbody(pooledObjectTransform);

			return pooledObjectTransform;
		}

		void ResetTransform(Transform sourceTrans, Transform toResetTrans)
		{
			toResetTrans.localPosition = sourceTrans.localPosition;
			toResetTrans.localRotation = sourceTrans.localRotation;

			toResetTrans.position = sourceTrans.position;
			toResetTrans.rotation = sourceTrans.rotation;
			toResetTrans.gameObject.SetActive(sourceTrans.gameObject.activeSelf);
		}

		void ResetTransformChildRecursive(Transform sourceTrans, Transform toResetTrans)
		{
			ResetTransform(sourceTrans, toResetTrans);

			for (int i = 0; i < sourceTrans.childCount; i++)
			{
				Transform sourceTransChild = sourceTrans.GetChild(i);
				
				for (int j = 0; j < toResetTrans.childCount; j++)
				{
					Transform toResetTransChild = toResetTrans.GetChild(j);

					if (sourceTransChild.name == toResetTransChild.name)
					{
						ResetTransformChildRecursive(sourceTransChild, toResetTransChild);
						break;
					}
				}
			}
		}

		void ResetRigidbody(Transform toResetTrans)
		{
			Rigidbody[] rigidbodies = toResetTrans.GetComponentsInChildren<Rigidbody>();

			foreach (Rigidbody rgBody in rigidbodies)
			{
				rgBody.velocity = new Vector3(0f, 0f, 0f);
				rgBody.angularVelocity = new Vector3(0f, 0f, 0f);
			}
		}

		void ResetColliderAndRigidbody(Transform pooledObjectTransform)
		{
			if (prefabCollider)
			{
				Collider collider = pooledObjectTransform.GetComponentInChildren<Collider>();
				ColliderHelper.CopyValueOnto(prefabCollider, collider);
			}

			if (prefabRgBody)
			{
				Rigidbody rgBody = pooledObjectTransform.GetComponentInChildren<Rigidbody>();
				ColliderHelper.CopyValueOnto(prefabRgBody, rgBody);
			}
		}

		public void Print()
		{
			Debug.Log("<color=red>[Prefab]</color> " + prefab.name + " <color=red>[Size]</color> " + size +
				" <color=red>[Free]</color> " + freeList.Count + " <color=red>[Used]</color> " + usedList.Count);
		}


		// Instantiate the prefabs.
		void OnLoadDone(AsyncOperationHandle<GameObject> obj)
		{
			if (obj.Status == AsyncOperationStatus.Succeeded)
			{
				prefab = obj.Result.GetComponent<T1>();

				if (prefab == null)
				{
					Debug.Log("Prefab does not have the right component attached.");
					return;
				}

				for (int i = 0; i < size; i++)
				{
					var instance = assetReference.InstantiateAsync(transform).Result.GetComponent<T1>();
					instance.gameObject.SetActive(false);
					//instance.GetComponentInChildren<SpawnableObject>().SetPool(poolType);
					freeList.Add(instance);
				}
			}
			Addressables.ReleaseInstance(obj);
		}

		void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			ReturnAllObjects();
		}

		void OnValidate()
		{
			if (prefab)
				gameObject.name = prefab.name;
		}
	}
}
