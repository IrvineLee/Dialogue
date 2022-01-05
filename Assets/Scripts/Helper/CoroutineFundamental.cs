﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dico.Helper
{
	public class CoroutineFundamental
	{
		protected static IEnumerator IEWaitFor(CoroutineRun cr, float seconds, Action<float> callbackMethod, bool isRealSeconds)
		{
			float startTime = Time.time;
			if (isRealSeconds) startTime = Time.realtimeSinceStartup;

			while ((!isRealSeconds ? Time.time : Time.realtimeSinceStartup) - startTime < seconds)
			{
				if (callbackMethod != default) callbackMethod((!isRealSeconds ? Time.time : Time.realtimeSinceStartup) - startTime);

				while (cr.IsPause)
				{
					startTime += Time.unscaledDeltaTime;
					yield return null;
				}

				yield return null;
			}
		}

		protected static IEnumerator IEWaitNextFrame()
		{
			yield return null;
		}

		protected static IEnumerator IEWaitUntilCurrentAnimationEnds(CoroutineRun cr, Animator animator)
		{
			while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
			{
				if (cr.IsPause) yield return IEPause(cr);

				yield return null;
			}
		}

		protected static IEnumerator IEWaitUntilFuncReturnsTrue(CoroutineRun cr, Func<bool> checkFunc, float timeOutDuration = Mathf.Infinity)
		{
			bool isTimeout = false;

			if (timeOutDuration != Mathf.Infinity)
				CoroutineHelper.WaitFor(timeOutDuration, () => isTimeout = true);

			while (!checkFunc())
			{
				if (cr.IsPause) yield return IEPause(cr);
				if (isTimeout) break;

				yield return null;
			}
		}


		protected static IEnumerator IEPause(CoroutineRun cr)
		{
			while (cr.IsPause)
			{
				yield return null;
			}
		}

		//protected static IEnumerator IEWaitTillTrue(Ref<bool> reference)
		//{
		//	while (!reference.Value)
		//	{
		//		yield return null;
		//	}
		//}

		protected static IEnumerator IEMove(CoroutineRun cr, Transform instanceTransform, Action moveAct, Vector3 goToPosition, Action lookAct = default, bool isFixedUpdate = false)
		{
			float minSqrMag = (goToPosition - instanceTransform.position).sqrMagnitude;
			float lowestSqrMag = minSqrMag;

			while (minSqrMag >= 0.01f)
			{
				if (instanceTransform == null) yield break;
				if (cr.IsPause) yield return IEPause(cr);

				minSqrMag = (goToPosition - instanceTransform.position).sqrMagnitude;
				if (lowestSqrMag >= minSqrMag)
				{
					lowestSqrMag = minSqrMag;

					moveAct();
					if (lookAct != default) lookAct();
				}
				else
				{
					if (goToPosition.x == 0) goToPosition.x = instanceTransform.position.x;
					if (goToPosition.y == 0) goToPosition.y = instanceTransform.position.y;
					if (goToPosition.z == 0) goToPosition.z = instanceTransform.position.z;

					instanceTransform.position = goToPosition;
					break;
				}

				if (isFixedUpdate) yield return new WaitForFixedUpdate();
				else yield return null;
			}
		}

		protected static IEnumerator IEMove(CoroutineRun cr, Action moveAct, float duration, bool isDeltaTime)
		{
			float timer = 0;
			while (timer < duration)
			{
				if (cr.IsPause) yield return IEPause(cr);

				timer += isDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime;
				moveAct();
				yield return null;
			}
		}

		protected static IEnumerator IEFadeFromTo(CoroutineRun cr, Transform trans, Color currentColor, Action<Color> callbackMethod, float fromAlpha, float toAlpha, float duration, bool isDeltaTime)
		{
			// Make sure it's enabled.
			trans.gameObject.SetActive(true);

			float alpha = fromAlpha;
			for (float t = 0.0f; t < 1.0f; t += (isDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime) / duration)
			{
				if (cr.IsPause) yield return IEPause(cr);

				Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(alpha, toAlpha, t));
				callbackMethod(newColor);
				yield return null;
			}

			Color color = new Color(currentColor.r, currentColor.g, currentColor.b, toAlpha);
			callbackMethod(color);
		}

		protected static IEnumerator IEPopUPDisappear<T>(CoroutineRun cr, Transform trans, Vector3 direction, float distance, float duration, float waitBeforeFade, float fadeTime, bool isDeltaTime)
									   where T : Component
		{
			// Make sure it's enabled.
			trans.gameObject.SetActive(true);

			Color color;
			Action<Color> callbackMethod = PopUpSetCallbackMethod<T>(trans, out color);

			float defaultAlpha = color.a;

			// Couldn't use the fadeTime because if the reached time is lower then it will not be fully alpha.
			IEFadeFromTo(new CoroutineRun(), trans, color, callbackMethod, 0, 1, duration * 0.5f, isDeltaTime);

			Vector3 fromPos = trans.position;
			Vector3 toPos = trans.position + (direction * distance);
			float speed = distance / duration;

			for (float d = 0f; d < distance; d += Time.deltaTime * speed)
			{
				if (cr.IsPause) yield return IEPause(cr);

				float time = isDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime;

				trans.Translate(direction * speed * time);
				yield return null;
			}

			trans.position = toPos;

			yield return new WaitForSeconds(waitBeforeFade);
			yield return IEFadeFromTo(new CoroutineRun(), trans, color, callbackMethod, 1, 0, fadeTime, isDeltaTime);

			trans.position = fromPos;
			trans.gameObject.SetActive(false);

			callbackMethod(new Color(color.r, color.g, color.b, defaultAlpha));
		}

		static Action<Color> PopUpSetCallbackMethod<T>(Transform trans, out Color color) where T : Component
		{
			color = Color.white;
			Action<Color> callbackMethod = (result) => { };

			if (typeof(T) == typeof(SpriteRenderer))
			{
				SpriteRenderer spriteRenderer = trans.GetComponent<SpriteRenderer>();
				color = spriteRenderer.color;
				callbackMethod = (result) => { spriteRenderer.color = result; };
			}
			else if (typeof(T) == typeof(Image))
			{
				Image image = trans.GetComponent<Image>();
				color = image.color;
				callbackMethod = (result) => { image.color = result; };
			}
			else if (typeof(T) == typeof(TextMeshProUGUI))
			{
				TextMeshProUGUI tmpUI = trans.GetComponent<TextMeshProUGUI>();
				color = tmpUI.color;
				callbackMethod = (result) => { tmpUI.color = result; };
			}

			return callbackMethod;
		}

		protected static IEnumerator IERotateWithinSeconds(CoroutineRun cr, Transform target, Vector3 rotateAmount, float duration, bool isDeltaTime, Space space)
		{
			Vector3 endRotate = target.eulerAngles + rotateAmount;

			float timer = isDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime;
			Vector3 rotateOverTime = rotateAmount / duration;

			while (timer < duration)
			{
				if (cr.IsPause) yield return IEPause(cr);

				float time = isDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime;
				Vector3 currentRotate = rotateOverTime * time;
				target.Rotate(currentRotate, space);

				// This needs further testing.
				Vector3 euler = target.eulerAngles;
				euler.x = Mathf.Clamp(euler.x, endRotate.x < 0 ? 360 + endRotate.x : 0, endRotate.x > 0 ? endRotate.x : 360);
				euler.y = Mathf.Clamp(euler.y, endRotate.y < 0 ? 360 + endRotate.y : 0, endRotate.y > 0 ? endRotate.y : 360);
				euler.z = Mathf.Clamp(euler.z, endRotate.z < 0 ? 360 + endRotate.z : 0, endRotate.z > 0 ? endRotate.z : 360);

				timer += time;
				target.eulerAngles = euler;

				yield return null;
			}

			target.eulerAngles = endRotate;
		}

		protected static IEnumerator IELerpWithinSeconds<T1, T2>(CoroutineRun cr, T1 startValue, T1 endValue, float duration, Action<T2> callbackMethod, Func<bool> breakMethod, bool isDeltaTime)
		{
			float timer = 0;

			Vector3 vect = Vector3.zero;
			if (typeof(T1) == typeof(Transform))
				vect = ((Transform)(object)startValue).position;

			while (timer <= duration)
			{
				if (cr.IsPause) yield return IEPause(cr);

				float time = isDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime;
				timer += time;

				float t = timer / duration;
				T2 value;
				if (typeof(T1) == typeof(int))
				{
					value = (T2)(object)(int)Mathf.Lerp((int)(object)startValue, (int)(object)endValue, t);
				}
				else if (typeof(T1) == typeof(float))
				{
					value = (T2)(object)Mathf.Lerp((float)(object)startValue, (float)(object)endValue, t);
				}
				else if (typeof(T1) == typeof(Vector2))
				{
					value = (T2)(object)Vector2.Lerp((Vector2)(object)startValue, (Vector2)(object)endValue, t);
				}
				else if (typeof(T1) == typeof(Vector3))
				{
					value = (T2)(object)Vector3.Lerp((Vector3)(object)startValue, (Vector3)(object)endValue, t);
				}
				else if (typeof(T1) == typeof(Color))
				{
					value = (T2)(object)Color.Lerp((Color)(object)startValue, (Color)(object)endValue, t);
				}
				else if (typeof(T1) == typeof(Transform))
				{
					value = (T2)(object)Vector3.Lerp(vect, ((Transform)(object)endValue).position, t);
				}
				else
				{
					Debug.Log("IELerpWithinSeconds unsupported type!");
					yield break;
				}

				callbackMethod((T2)(object)value);

				if (breakMethod != null && breakMethod()) yield break;
				yield return null;
			}

			if (typeof(T1) == typeof(T2)) callbackMethod((T2)(object)endValue);
		}

		protected static IEnumerator IEQuaternionLerpWithinSeconds(CoroutineRun cr, Transform instance, Quaternion startValue, Quaternion endValue,
																   float duration, Func<bool> breakMethod, bool isDeltaTime)
		{
			float timer = 0;
			while (timer < duration)
			{
				if (cr.IsPause) yield return IEPause(cr);

				float time = isDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime;
				timer += time;

				float t = timer / duration;
				Quaternion quaternion = Quaternion.Lerp(startValue, endValue, t);

				instance.rotation = quaternion;

				if (breakMethod != null && breakMethod()) yield break;
				yield return null;
			}
		}

		protected static IEnumerator IERunActionUntilBreak(CoroutineRun cr, float runEvery, Action action, Func<bool> breakMethod,
														   bool isRunAtStart, bool isRealSeconds, bool isFixedUpdate, float timeOutDuration)
		{
			if (isRunAtStart) action();

			CoroutineRun newCr;

			if (runEvery != 0) newCr = CoroutineHelper.WaitFor(runEvery, action, default, isRealSeconds);
			else newCr = CoroutineHelper.WaitNextFrame(action);

			bool isSelfBreak = false;
			CoroutineHelper.WaitFor(timeOutDuration, () => isSelfBreak = true);

			while (!breakMethod())
			{
				if (cr.IsPause) yield return IEPause(cr);

				if (newCr.IsDone)
				{
					if (runEvery != 0) newCr = CoroutineHelper.WaitFor(runEvery, action, default, isRealSeconds);
					else newCr = CoroutineHelper.WaitNextFrame(action);
				}

				if (isSelfBreak) break;

				yield return isFixedUpdate ? new WaitForFixedUpdate() : null;
			}

			newCr.StopCoroutine();
		}

		protected static Vector3 GetDirection(Vector3 pos1, Vector3 pos2)
		{
			return (pos2 - pos1).normalized;
		}

		protected static Vector3 DisregardAxisNewPosition(Vector3 position, Vector3 disregardAxis)
		{
			if (disregardAxis.x != 0) position.x = disregardAxis.x;
			if (disregardAxis.y != 0) position.y = disregardAxis.y;
			if (disregardAxis.z != 0) position.z = disregardAxis.z;

			return position;
		}

		protected static void LookAt2D(Transform instanceTransform, Vector3 lookAt)
		{
			instanceTransform.up = (lookAt - instanceTransform.position).normalized;
		}

		protected static void RigidbodyMove<T>(Transform instanceTransform, T rgbd, Vector3 direction, float moveSpeed, bool isDeltaTime) where T : Component
		{
			float time = isDeltaTime ? Time.fixedDeltaTime : Time.fixedUnscaledDeltaTime;
			Vector3 pos = instanceTransform.position + (direction * moveSpeed * time);

			if (typeof(T) == typeof(Rigidbody2D)) (rgbd as Rigidbody2D).MovePosition(pos);
			else if (typeof(T) == typeof(Rigidbody)) (rgbd as Rigidbody).MovePosition(pos);
		}

		protected static void TransformMove(Transform instanceTransform, Vector3 direction, float moveSpeed, bool isDeltaTime, Space space)
		{
			float time = isDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime;
			instanceTransform.Translate(direction * moveSpeed * time, space);
		}

		protected static CoroutineRun Fade(SpriteRenderer spriteRenderer, float fromAlpha, float toAlpha, float duration, Action doLast = default, bool isDeltaTime = true)
		{
			Action<Color> callbackMethod = (result) => { spriteRenderer.color = result; };

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEFadeFromTo(cr, spriteRenderer.transform, spriteRenderer.color, callbackMethod, fromAlpha, toAlpha, duration, isDeltaTime), doLast);
		}

		protected static CoroutineRun Fade(Image image, float fromAlpha, float toAlpha, float duration, Action doLast = default, bool isDeltaTime = true)
		{
			Action<Color> callbackMethod = (result) => { image.color = result; };

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEFadeFromTo(cr, image.transform, image.color, callbackMethod, fromAlpha, toAlpha, duration, isDeltaTime), doLast);
		}

		protected static CoroutineRun Fade(Renderer rend, float fromAlpha, float toAlpha, float duration, Action doLast = default, bool isDeltaTime = true)
		{
			Action<Color> callbackMethod = (result) => { rend.material.color = result; };

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEFadeFromTo(cr, rend.transform, rend.material.color, callbackMethod, fromAlpha, toAlpha, duration, isDeltaTime), doLast);
		}

		protected static CoroutineRun FadeUsingShader(Renderer rend, string propertyName, float fromAlpha, float toAlpha, float duration, Action doLast = default, bool isDeltaTime = true)
		{
			MaterialPropertyBlock propBlock = ShaderHelper.GetPropertyBlock_InitializeColor(rend, rend.material.color, propertyName);

			Action<Color> callbackMethod = (result) =>
			{
				propBlock.SetColor(propertyName, result);
				rend.SetPropertyBlock(propBlock);
			};

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEFadeFromTo(cr, rend.transform, rend.material.color, callbackMethod, fromAlpha, toAlpha, duration, isDeltaTime), doLast);
		}

		protected static CoroutineRun FadeUsingURP(Renderer rend, string propertyName, float fromAlpha, float toAlpha, float duration, Action doLast = default, bool isDeltaTime = true)
		{
			Action<Color> callbackMethod = (result) =>
			{
				rend.material.SetColor(propertyName, result);
			};

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IEFadeFromTo(cr, rend.transform, rend.material.color, callbackMethod, fromAlpha, toAlpha, duration, isDeltaTime), doLast);
		}

		protected static CoroutineRun ColorChangeUsingURP(Renderer rend, string propertyName, Color fromColor, Color toColor, float duration,
														  Action doLast = default, int materialIndex = 0, bool isDeltaTime = true)
		{
			Action<Color> callbackMethod = (result) =>
			{
				rend.materials[materialIndex].SetColor(propertyName, result);
			};

			CoroutineRun cr = new CoroutineRun();
			return cr.Initialize(IELerpWithinSeconds(cr, fromColor, toColor, duration, callbackMethod, null, isDeltaTime), doLast);
		}
	}
}
