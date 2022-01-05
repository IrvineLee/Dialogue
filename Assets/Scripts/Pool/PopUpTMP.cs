using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Dico.Helper;

namespace Dico.HyperCasualGame.Pool
{
	public class PopUpTMP : SpawnableObject
	{
		public TextMeshProUGUI TmpUI { get => tmpUI; }

		TextMeshProUGUI tmpUI;
		SpawnablePool pool;

		float defaultAlpha;

		void Awake()
		{
			tmpUI = GetComponentInChildren<TextMeshProUGUI>();
			pool = GetComponentInParent<SpawnablePool>();

			defaultAlpha = tmpUI.color.a;
		}

		void OnDisable()
		{
			if (tmpUI)
			{
				// It will not return back to default value when changing scenes, so reset it back here.
				tmpUI.color = tmpUI.color.With(null, null, null, defaultAlpha);
			}

			pool?.ReturnObject(this);
		}
	}
}