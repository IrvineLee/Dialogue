using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Dico.Helper;

namespace Dico.HyperCasualGame.Pool
{
	public class MoveToImage : SpawnableObject
	{
		[SerializeField] float fadeInDuration = 0;

		public Image Image { get => image; }

		SpawnablePool pool;

		Image image;
		float defaultAlpha;

		void Awake()
		{
			pool = GetComponentInParent<SpawnablePool>();
			image = GetComponentInChildren<Image>();

			defaultAlpha = image.color.a;
		}

		void OnEnable()
		{
			CoroutineHelper.FadeFromTo(image, 0, 1, fadeInDuration);
		}

		void OnDisable()
		{
			if (image)
			{
				// It will not return back to default value when changing scenes, so reset it back here.
				image.color = image.color.With(null, null, null, defaultAlpha);
			}

			pool?.ReturnObject(this);
		}
	}
}