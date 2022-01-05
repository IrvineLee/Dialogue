using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dico.Helper
{
	public class OnEnableFadeInOut : MonoBehaviour
	{
		[SerializeField] float fadeInDuration = 1f;
		[SerializeField] float fadeOutDuration = 1f;
		[SerializeField] bool isLoop = false;

		SpriteRenderer sr;

		CoroutineRun cr;

		void Awake()
		{
			sr = GetComponentInChildren<SpriteRenderer>();
		}

		void OnEnable()
		{
			FadeInOut();
		}

		void FadeInOut()
		{
			cr = CoroutineHelper.FadeFromTo(sr, 0, 1, fadeInDuration, () =>
			{
				cr = CoroutineHelper.FadeTo(sr, 0, fadeOutDuration, () => 
				{
					if (isLoop) FadeInOut();
				});
			});
		}

		void OnDisable()
		{
			cr.StopCoroutine();
		}
	}
}
