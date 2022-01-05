using System.Collections.Generic;
using UnityEngine;

namespace Dico.Helper
{
	public static class ListExtensions
	{
		// Stop all CoroutineRun.
		public static void StopAllCoroutineRun(this List<CoroutineRun> originalList)
		{
			foreach (CoroutineRun cr in originalList)
			{
				cr.StopCoroutine();
			}
		}
	}
}
