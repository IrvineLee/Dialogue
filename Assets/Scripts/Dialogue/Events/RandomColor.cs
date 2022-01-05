using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dico.Helper;

namespace Events
{
	public class RandomColor : MonoBehaviour
	{
		void Start()
		{
			GameEvent.sSingleton.randomColorEvent += GetRandomColor;
		}

		void GetRandomColor()
		{
			Debug.Log(MathHelper.RandomColor());
		}

		void OnDestroy()
		{
			GameEvent.sSingleton.randomColorEvent -= GetRandomColor;
		}
	}
}