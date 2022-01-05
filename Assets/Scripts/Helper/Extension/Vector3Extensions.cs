using UnityEngine;

namespace Dico.Helper
{
	public static class Vector3Extensions
	{
		// Replace with
		public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
		{
			return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
		}

		// Flatten it with y = 0
		public static Vector3 Flat(this Vector3 original)
		{
			return new Vector3(original.x, 0, original.z);
		}

		// DirectionTo
		public static Vector3 DirectionTo(this Vector3 source, Vector3 destination)
		{
			return Vector3.Normalize(destination - source);
		}

		// SpanTo
		public static Vector3 SpanTo(this Vector3 source, Vector3 destination)
		{
			return destination - source;
		}
	}
}
