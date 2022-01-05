using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
	public class GameEvent : MonoBehaviour
	{
		public static GameEvent sSingleton { get; private set; }

		public event Action randomColorEvent;

		void Awake()
		{
			if (sSingleton != null && sSingleton != this) Destroy(this.gameObject);
			else sSingleton = this;
		}

		public void CallRandomColorEvent()
		{
			randomColorEvent?.Invoke();
		}
	}
}