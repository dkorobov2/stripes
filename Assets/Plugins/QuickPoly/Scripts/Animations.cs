using System;
using UnityEngine;
using System.Collections;

namespace QuickPoly
{
	public static class Animations
	{
		public static Coroutine activeAnimation;
	#region AnimationsPublic Calls
		public static void PlayChangeScaleAnimation(QuickPolygon shape, float endWidth, float endHeight, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeScale(shape, endWidth, endHeight, interpolateType, duration));
		}
		public static void PlayChangeUniformScaleAnimation(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeUniformScale(shape, endValue, interpolateType, duration));
		}
		public static void PlayChangeResolution(QuickPolygon shape, int endValue, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeResolution(shape, endValue, interpolateType, duration));
		}
		public static void PlayChangeUniformRoundingResolution(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeUniformRoundingResolution(shape, endValue, interpolateType, duration));
		}
		public static void PlayChangeRoundingResolutionOnCornerAnimation(QuickPolygon shape, int cornerID, float endValue, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeRoundingResolutionOnCorner(shape, cornerID, endValue, interpolateType, duration));
		}
		public static void PlayChangeInnerBorderAnimation(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeInnerBorder(shape, endValue, interpolateType, duration));
		}
		public static void PlayChangeOuterBorderAnimation(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeOuterBorder(shape, endValue, interpolateType, duration));
		}
		public static void PlayChangeBorderSizeAnimation(QuickPolygon shape, float endOuterScale, float endInnerScale, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeBorderSize(shape, endOuterScale, endInnerScale, interpolateType, duration));
		}
		public static void PlayChangeStarInnerRadius(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeStarInnerRadius(shape, endValue, interpolateType, duration));
		}
		public static void PlayChangeDiamondTopWidth(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeDiamondTopWidth(shape, endValue, interpolateType, duration));
		}
		public static void PlayChangeDiamondMiddleHeight(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeDiamondMiddleHeight(shape, endValue, interpolateType, duration));
		}
		public static void PlayChangePositionAnimation(QuickPolygon shape, Vector3 endValue, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangePosition(shape, endValue, interpolateType, duration));
		}
		public static void PlayChangeRotationAnimation(QuickPolygon shape, Vector3 addValue, InterpolateType interpolateType, float duration)
		{
			activeAnimation = shape.StartCoroutine(ChangeRotation(shape, addValue, interpolateType, duration));
		}
		public static void StopActiveAnimation(QuickPolygon shape)
		{
			shape.StopCoroutine(activeAnimation);
		}
	#endregion
	#region AnimationsCooroutines
		private static IEnumerator ChangeStarInnerRadius(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			float startValue = shape.GetStarInnerRadius();
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				float newvalue = GetInterpolate(interpolateType, startValue, endValue, timeElapsed, duration);
				shape.SetStarInnerRadius(newvalue, true);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.SetStarInnerRadius(endValue, true);
		}
		private static IEnumerator ChangeDiamondTopWidth(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			float startValue = shape.GetDiamondTopWidth();
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				float newvalue = GetInterpolate(interpolateType, startValue, endValue, timeElapsed, duration);
				shape.SetDiamondTopWidth(newvalue, true);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.SetDiamondTopWidth(endValue, true);
		}
		private static IEnumerator ChangeDiamondMiddleHeight(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			float startValue = shape.GetDiamondMiddleHeight();
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				float newvalue = GetInterpolate(interpolateType, startValue, endValue, timeElapsed, duration);
				shape.SetDiamondMiddleHeight(newvalue, true);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.SetDiamondMiddleHeight(endValue, true);
		}
		private static IEnumerator ChangeUniformScale(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			float startValue = shape.GetUniformScale();
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				float newvalue = GetInterpolate(interpolateType, startValue, endValue, timeElapsed , duration);
				shape.SetUniformScale(newvalue, true);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.SetUniformScale(endValue, true);
		}
		private static IEnumerator ChangeScale(QuickPolygon shape, float endWidth, float endHeight, InterpolateType interpolateType, float duration)
		{
			float startW = shape.GetWidth();
			float startH = shape.GetHeight();
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				float newW = GetInterpolate(interpolateType, startW, endWidth, timeElapsed, duration);
				float newH = GetInterpolate(interpolateType, startH, endHeight, timeElapsed, duration);
				shape.SetScale(newW,newH, true);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.SetScale(endWidth, endHeight, true);
		}
		private static IEnumerator ChangeResolution(QuickPolygon shape, int endValue, InterpolateType interpolateType, float duration)
		{
			int startValue = shape.GetResolution();
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				float value = GetInterpolate(interpolateType, startValue, endValue, timeElapsed, duration);
				int newvalue = Mathf.RoundToInt(value);
				shape.SetResolution(newvalue,true);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.SetResolution(endValue, true);
		}
		private static IEnumerator ChangeUniformRoundingResolution(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			float startValue = shape.GetUniformRoundingResolution();
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				float newvalue = GetInterpolate(interpolateType, startValue, endValue, timeElapsed, duration);
				shape.SetUniformRoundingResolution(newvalue,true);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.SetUniformRoundingResolution(endValue, true);
		}
		private static IEnumerator ChangeRoundingResolutionOnCorner(QuickPolygon shape, int cornerID, float endValue, InterpolateType interpolateType, float duration)
		{
			if (shape.Shape.roundings.Length > cornerID)
			{
				float startValue = shape.GetRoundingResolution(cornerID);
				float timeElapsed = 0;
				while (timeElapsed < duration)
				{
					float newvalue = GetInterpolate(interpolateType, startValue, endValue, timeElapsed, duration);
					shape.SetRoundingResolution(cornerID, newvalue, true);
					yield return new WaitForEndOfFrame();
					timeElapsed += Time.deltaTime;
				}
				shape.SetRoundingResolution(cornerID, endValue, true);
			}
			else
			{
				Debug.LogError(MSG.Errors.WRONG_ROUNDING_CORNER);
			}
		}
		private static IEnumerator ChangeInnerBorder(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			float startValue = shape.GetBorderInnerScale();
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				float newvalue = GetInterpolate(interpolateType, startValue, endValue, timeElapsed, duration);
				shape.SetBorderInnerScale(newvalue, true);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.SetBorderInnerScale(endValue, true);
		}
		private static IEnumerator ChangeOuterBorder(QuickPolygon shape, float endValue, InterpolateType interpolateType, float duration)
		{
			float startValue = shape.GetBorderOuterScale();
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				float newvalue = GetInterpolate(interpolateType, startValue, endValue, timeElapsed, duration);
				shape.SetBorderOuterScale(newvalue, true);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.SetBorderOuterScale(endValue, true);
		}
		private static IEnumerator ChangeBorderSize(QuickPolygon shape, float endOuterScale, float endInnerScale, InterpolateType interpolateType, float duration)
		{
			float startInnerScale = shape.GetBorderInnerScale();
			float startOuterScale = shape.GetBorderOuterScale();
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				float newinnervalue = GetInterpolate(interpolateType, startInnerScale, endInnerScale, timeElapsed, duration);
				float newoutervalue = GetInterpolate(interpolateType, startOuterScale, endOuterScale, timeElapsed, duration);
				shape.SetBorderInnerScale(newinnervalue, true);
				shape.SetBorderOuterScale(newoutervalue, true);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.SetBorderInnerScale(endInnerScale, true);
			shape.SetBorderOuterScale(endOuterScale, true);
		}
		private static IEnumerator ChangePosition(QuickPolygon shape, Vector3 endValue, InterpolateType interpolateType, float duration)
		{
			Vector3 startValue = shape.GetPosition();
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				Vector3 newPosition = GetInterpolate(interpolateType, startValue, endValue, timeElapsed, duration);
				shape.SetPosition(newPosition);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.SetPosition(endValue);
		}
		private static IEnumerator ChangeRotation(QuickPolygon shape, Vector3 addValue, InterpolateType interpolateType, float duration)
		{
			Vector3 startRotation = shape.transform.rotation.eulerAngles;
			Vector3 targetRotation = startRotation + addValue;
			float timeElapsed = 0;
			while (timeElapsed < duration)
			{
				Vector3 newRotation = GetInterpolate(interpolateType, startRotation, targetRotation, timeElapsed, duration);
				shape.transform.rotation = Quaternion.Euler(newRotation);
				yield return new WaitForEndOfFrame();
				timeElapsed += Time.deltaTime;
			}
			shape.transform.rotation = Quaternion.Euler(targetRotation);
		}
	#endregion
#region InterpolationTypes
	#region Vector3Easing
		private static Vector3 GetInterpolate(InterpolateType interpolateType, Vector3 startValue, Vector3 endValue, float currentTime, float length)
		{
			switch (interpolateType)
			{
				case InterpolateType.Linear:
					return LinearInterpolate(startValue, endValue, currentTime, length);
				case InterpolateType.Cosinus:
					return CosineInterpolate(startValue, endValue, currentTime, length);
				case InterpolateType.EaseInOutQuint:
					return QuintEaseInOut(startValue, endValue, currentTime, length);
				case InterpolateType.EaseOutBack:
					return BackEaseOut(startValue, endValue, currentTime, length);
				case InterpolateType.EaseOutElastic:
					return ElasticEaseOut(startValue, endValue, currentTime, length);
				default:
					Debug.LogError(MSG.Errors.WRONG_INTERPOLATE_TYPE);
					return endValue;
			}
		}
		private static Vector3 LinearInterpolate(Vector3 startVal, Vector3 finalVal, float currentTime, float length)
		{
			Vector3 moveTo = finalVal - startVal;
			return startVal + moveTo * (currentTime / length);
		}
		private static Vector3 CosineInterpolate(Vector3 startVal, Vector3 finalVal, float currentTime, float length)
		{
			float progress = currentTime / length;
			float mu2 = (1 - Mathf.Cos(progress * Mathf.PI)) / 2;
			return (startVal * (1 - mu2) + finalVal * mu2);
		}
		private static Vector3 QuintEaseInOut(Vector3 startVal, Vector3 finalVal, float curTime, float length)
		{
			Vector3 moveTo = finalVal - startVal;
			curTime /= length / 2;
			if (curTime < 1) return moveTo / 2 * curTime * curTime * curTime * curTime * curTime + startVal;
			curTime -= 2;
			return moveTo / 2 * (curTime * curTime * curTime * curTime * curTime + 2) + startVal;
		}
		private static Vector3 BackEaseOut(Vector3 startVal, Vector3 finalVal, float curTime, float length)
		{
			return startVal + (finalVal - startVal) * (1 * ((curTime = curTime / length - 1) * curTime * ((1.70158f + 1) * curTime + 1.70158f) + 1));
		}
		private static Vector3 ElasticEaseOut(Vector3 startVal, Vector3 finalVal, float curTime, float length)
		{
			if (Math.Abs((curTime /= length) - 1) < 0.001f)
				return startVal + finalVal;
			float p = length * 0.3f;
			float s = p / 4;
			return startVal + (finalVal - startVal) * ((1 * Mathf.Pow(2, -10 * curTime) * Mathf.Sin((curTime * length - s) * (2 * Mathf.PI) / p) + 1));
		}
	#endregion
	#region floatEasing
		private static float GetInterpolate(InterpolateType interpolateType, float startValue, float endValue, float currentTime, float length)
		{
			switch (interpolateType)
			{
				case InterpolateType.Linear:
					return LinearInterpolate(startValue, endValue, currentTime, length);
				case InterpolateType.Cosinus:
					return CosineInterpolate(startValue, endValue, currentTime, length);
				case InterpolateType.EaseInOutQuint:
					return QuintEaseInOut(startValue, endValue, currentTime, length);
				case InterpolateType.EaseOutBack:
					return BackEaseOut(startValue, endValue, currentTime, length);
				case InterpolateType.EaseOutElastic:
					return ElasticEaseOut(startValue, endValue, currentTime, length);
				default:
					Debug.LogError(MSG.Errors.WRONG_INTERPOLATE_TYPE);
					return 0;
			}
		}
		private static float LinearInterpolate(float startVal, float finalVal, float currentTime, float length)
		{
			float progress = currentTime/length;
			return (startVal * (1 - progress) + finalVal * progress);
		}
		private static float CosineInterpolate(float startVal, float finalVal, float currentTime, float length)
		{
			float progress = currentTime / length;
			float mu2 = (1 - Mathf.Cos(progress * Mathf.PI)) / 2;
			return (startVal * (1 - mu2) + finalVal * mu2);
		}
		public static float QuintEaseInOut(float startVal, float finalVal, float curTime, float length)
		{
			float c = finalVal - startVal;
			curTime /= length / 2;
			if (curTime < 1) return c / 2 * curTime * curTime * curTime * curTime * curTime + startVal;
			curTime -= 2;
			return c / 2 * (curTime * curTime * curTime * curTime * curTime + 2) + startVal;

		}
		public static float BackEaseOut(float startVal, float finalVal, float curTime, float length)
		{
			return startVal + (finalVal-startVal) *(1 * ((curTime = curTime / length - 1) * curTime * ((1.70158f + 1) * curTime + 1.70158f) + 1));
		}
		public static float ElasticEaseOut(float startVal, float finalVal, float curTime, float length)
		{
			if (Math.Abs((curTime /= length) - 1) < 0.001f)
				return startVal + finalVal;
			float p = length * 0.3f;
			float s = p / 4;
			return startVal + (finalVal - startVal) * ((1 * Mathf.Pow(2, -10 * curTime) * Mathf.Sin((curTime * length - s) * (2 * Mathf.PI) / p) + 1));
		}
	#endregion
	
#endregion
	}
}