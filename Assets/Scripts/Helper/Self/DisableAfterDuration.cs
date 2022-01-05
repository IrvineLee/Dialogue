using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dico.HyperCasualGame.Pool;

namespace Dico.Helper
{
	public class DisableAfterDuration : SpawnableObject
	{
		[SerializeField] float duration = 1f;

		float currentDuration;

		SpawnablePool pool;
		Collider col;
		Rigidbody rgBody;

		CoroutineRun cr;

		void Awake()
		{
			pool = GetComponentInParent<SpawnablePool>();
			col = GetComponentInChildren<Collider>();
			rgBody = GetComponentInChildren<Rigidbody>();

			currentDuration = duration;
		}

		void OnEnable()
		{
			cr?.StopCoroutine();
			cr = CoroutineHelper.WaitFor(currentDuration, OnDisable);
		}

		public void SetDuration(float duration) 
		{
			currentDuration = duration;
			OnEnable();
		}

		public void DisableCollider(float reenableAfter = Mathf.Infinity)
		{
			col.enabled = false;
			if (reenableAfter != Mathf.Infinity)
				CoroutineHelper.WaitFor(reenableAfter, () => col.enabled = true);
		}

		public void AddForce(Vector3 direction, ForceMode forceMode)
		{
			rgBody.AddForce(direction, forceMode);
		}

		void OnDisable()
		{
			currentDuration = duration;

			col.enabled = true;
			cr?.StopCoroutine();
			pool.ReturnObject(this);
		}
	}
}
