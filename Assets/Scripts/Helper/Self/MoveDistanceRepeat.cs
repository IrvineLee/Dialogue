using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dico.Helper;

namespace Dico.HyperCasualGame.Stage
{
	public class MoveDistanceRepeat : MonoBehaviour
	{
		[SerializeField] Vector3 moveRange = Vector3.zero;

		[Tooltip("ディフォルト所から１方向に行く時間。ディフォルト所に戻る時間。")]
		[SerializeField] float duration = 0.5f;

		[Tooltip("スタート遅延。")]
		[SerializeField] float startDelay = 0;

		public Vector3 MoveRange { get => moveRange; }
		public float Duration { get => duration; }
		public float StartDelay { get => startDelay; }

		Vector3 moveRangeValue;
		bool isMovingLeft, isMovingUp, isMovingForward;
		CoroutineRun cr;

		void Start()
		{
			if (moveRange.x < 0) isMovingLeft = true;
			if (moveRange.y > 0) isMovingUp = true;
			if (moveRange.z > 0) isMovingForward = true;

			moveRangeValue = moveRange.With(x: Mathf.Abs(moveRange.x), y: Mathf.Abs(moveRange.y), z: Mathf.Abs(moveRange.z));

			cr = CoroutineHelper.WaitFor(startDelay, () =>
			{
				if (this != null)
					MoveDefaultToOneSide(GetTopLeftPosition(transform.localPosition, isMovingUp, isMovingLeft, isMovingForward));
			});
		}

		public void Initalize(Vector3 moveRange, float duration, float startDelay)
		{
			this.moveRange = moveRange;
			this.duration = duration;
			this.startDelay = startDelay;
		}

		void MoveDefaultToOneSide(Vector3 newPos)
		{
			cr = CoroutineHelper.LerpTo(transform, newPos, duration, () => Move(newPos));
		}

		void MoveTheOtherEnd(Vector3 newPos)
		{
			cr = CoroutineHelper.LerpTo(transform, newPos, duration * 2f, () => Move(newPos));
		}

		void Move(Vector3 newPos)
		{
			isMovingLeft = !isMovingLeft;
			isMovingUp = !isMovingUp;
			isMovingForward = !isMovingForward;

			newPos = GetTopLeftPosition(transform.localPosition, isMovingUp, isMovingLeft, isMovingForward);
			newPos = GetTopLeftPosition(newPos, isMovingUp, isMovingLeft, isMovingForward);

			MoveTheOtherEnd(newPos);
		}

		Vector3 GetTopLeftPosition(Vector3 localPosition, bool isTopTrue = true, bool isLeftTrue = true, bool isForwardTrue = true)
		{
			Vector3 newPos = Vector3.zero;

			newPos = isLeftTrue ? localPosition.With(x: localPosition.x - moveRangeValue.x) : localPosition.With(x: localPosition.x + moveRangeValue.x);
			newPos = isTopTrue ? newPos.With(y: newPos.y + moveRangeValue.y) : newPos.With(y: newPos.y - moveRangeValue.y);
			newPos = isForwardTrue ? newPos.With(z: newPos.z + moveRangeValue.z) : newPos.With(z: newPos.z - moveRangeValue.z);

			return newPos;
		}

		void OnDisable()
		{
			cr?.StopCoroutine();
		}
	}

}