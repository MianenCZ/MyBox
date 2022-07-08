// ---------------------------------------------------------------------------- 
// Author: Richard Fine
// Source: https://bitbucket.org/richardfine/scriptableobjectdemo
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace MyBox
{
	public class MinMaxRangeAttribute : PropertyAttribute
	{
		public MinMaxRangeAttribute(float min, float max)
		{
			Min = min;
			Max = max;
		}

		public readonly float Min;
		public readonly float Max;
	}

	[Serializable]
	public struct RangedFloat
	{
		public float Min;
		public float Max;

		public RangedFloat(float min, float max)
		{
			Min = min;
			Max = max;
		}
	}

	[Serializable]
	public struct RangedInt
	{
		public int Min;
		public int Max;

		public RangedInt(int min, int max)
		{
			Min = min;
			Max = max;
		}
	}

	public static class RangedExtensions
	{
		public static float LerpFromRange(this RangedFloat ranged, float t)
		{
			return Mathf.Lerp(ranged.Min, ranged.Max, t);
		}

		public static float LerpFromRangeUnclamped(this RangedFloat ranged, float t)
		{
			return Mathf.LerpUnclamped(ranged.Min, ranged.Max, t);
		}

		public static float LerpFromRange(this RangedInt ranged, float t)
		{
			return Mathf.Lerp(ranged.Min, ranged.Max, t);
		}

		public static float LerpFromRangeUnclamped(this RangedInt ranged, float t)
		{
			return Mathf.LerpUnclamped(ranged.Min, ranged.Max, t);
		}

		/// <summary>
		/// Returns <c>true</c> if closed inteval defined by <see cref="RangedFloat"/> <paramref name="interval"/> contains specified <paramref name="value"/>
		/// </summary>
		/// <remarks>Closed interval means that bounds values belong into the inteval</remarks>
		/// <param name="interval">Base interval</param>
		/// <param name="value">Value to be tested</param>
		/// <returns></returns>
		public static bool CloseContains(this RangedFloat interval, float value)
		{
			return interval.Min <= value && value <= interval.Max;
		}

		/// <summary>
		/// Returns <c>true</c> if open inteval defined by <see cref="RangedFloat"/> <paramref name="interval"/> contains specified <paramref name="value"/>
		/// </summary>
		/// <remarks>Open interval means that bounds values do NOT belong into the inteval</remarks>
		/// <param name="interval">Base interval</param>
		/// <param name="value">Value to be tested</param>
		/// <returns></returns>
		public static bool OpenContains(this RangedFloat interval, float value)
		{
			return interval.Min < value && value < interval.Max;
		}

		/// <summary>
		/// Returns <c>true</c> if left-open inteval defined by <see cref="RangedFloat"/> <paramref name="interval"/> contains specified <paramref name="value"/>
		/// </summary>
		/// <remarks>left-open interval means that left bound do NOT belong into interval and right bound DO belong into interval</remarks>
		/// <param name="interval">Base interval</param>
		/// <param name="value">Value to be tested</param>
		/// <returns></returns>
		public static bool LeftOpenContains(this RangedFloat interval, float value)
		{
			return interval.Min < value && value <= interval.Max;
		}

		/// <summary>
		/// Returns <c>true</c> if right-open inteval defined by <see cref="RangedFloat"/> <paramref name="interval"/> contains specified <paramref name="value"/>
		/// </summary>
		/// <remarks>right-open interval means that left bound DO belong into interval and right bound do NOT belong into interval</remarks>
		/// <param name="interval">Base interval</param>
		/// <param name="value">Value to be tested</param>
		/// <returns></returns>
		public static bool RightOpenContains(this RangedFloat interval, float value)
		{
			return interval.Min <= value && value < interval.Max;
		}
		/// <summary>
		/// Returns <c>true</c> if closed inteval defined by <see cref="RangedInt"/> <paramref name="interval"/> contains specified <paramref name="value"/>
		/// </summary>
		/// <remarks>Closed interval means that bounds values belong into the inteval</remarks>
		/// <param name="interval">Base interval</param>
		/// <param name="value">Value to be tested</param>
		/// <returns></returns>
		public static bool CloseContains(this RangedInt interval, int value)
		{
			return interval.Min <= value && value <= interval.Max;
		}

		/// <summary>
		/// Returns <c>true</c> if open inteval defined by <see cref="RangedInt"/> <paramref name="interval"/> contains specified <paramref name="value"/>
		/// </summary>
		/// <remarks>Open interval means that bounds values do NOT belong into the inteval</remarks>
		/// <param name="interval">Base interval</param>
		/// <param name="value">Value to be tested</param>
		/// <returns></returns>
		public static bool OpenContains(this RangedInt interval, int value)
		{
			return interval.Min < value && value < interval.Max;
		}

		/// <summary>
		/// Returns <c>true</c> if left-open inteval defined by <see cref="RangedInt"/> <paramref name="interval"/> contains specified <paramref name="value"/>
		/// </summary>
		/// <remarks>left-open interval means that left bound do NOT belong into interval and right bound DO belong into interval</remarks>
		/// <param name="interval">Base interval</param>
		/// <param name="value">Value to be tested</param>
		/// <returns></returns>
		public static bool LeftOpenContains(this RangedInt interval, int value)
		{
			return interval.Min < value && value <= interval.Max;
		}

		/// <summary>
		/// Returns <c>true</c> if right-open inteval defined by <see cref="RangedInt"/> <paramref name="interval"/> contains specified <paramref name="value"/>
		/// </summary>
		/// <remarks>right-open interval means that left bound DO belong into interval and right bound do NOT belong into interval</remarks>
		/// <param name="interval">Base interval</param>
		/// <param name="value">Value to be tested</param>
		/// <returns></returns>
		public static bool RightOpenContains(this RangedInt interval, int value)
		{
			return interval.Min <= value && value < interval.Max;
		}
	}
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
	using UnityEditor;
	
	[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
	public class MinMaxRangeIntAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty minProp = property.FindPropertyRelative("Min");
			SerializedProperty maxProp = property.FindPropertyRelative("Max");
			if (minProp == null || maxProp == null)
			{
				WarningsPool.LogWarning("MinMaxRangeAttribute used on <color=brown>" +
				                 property.name +
				                 "</color>. Must be used on types with Min and Max fields",
					property.serializedObject.targetObject);

				return;
			}

			var minValid = minProp.propertyType == SerializedPropertyType.Integer || minProp.propertyType == SerializedPropertyType.Float;
			var maxValid = maxProp.propertyType == SerializedPropertyType.Integer || maxProp.propertyType == SerializedPropertyType.Float;
			if (!maxValid || !minValid || minProp.propertyType != maxProp.propertyType)
			{
				WarningsPool.LogWarning("MinMaxRangeAttribute used on <color=brown>" +
				                 property.name +
				                 "</color>. Min and Max fields must be of int or float type",
					property.serializedObject.targetObject);

				return;
			}

			MinMaxRangeAttribute rangeAttribute = (MinMaxRangeAttribute) attribute;

			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, label);

			bool isInt = minProp.propertyType == SerializedPropertyType.Integer;

			float minValue = isInt ? minProp.intValue : minProp.floatValue;
			float maxValue = isInt ? maxProp.intValue : maxProp.floatValue;
			float rangeMin = rangeAttribute.Min;
			float rangeMax = rangeAttribute.Max;


			const float rangeBoundsLabelWidth = 40f;

			var rangeBoundsLabel1Rect = new Rect(position);
			rangeBoundsLabel1Rect.width = rangeBoundsLabelWidth;
			GUI.Label(rangeBoundsLabel1Rect, new GUIContent(minValue.ToString(isInt ? "F0" : "F2")));
			position.xMin += rangeBoundsLabelWidth;

			var rangeBoundsLabel2Rect = new Rect(position);
			rangeBoundsLabel2Rect.xMin = rangeBoundsLabel2Rect.xMax - rangeBoundsLabelWidth;
			GUI.Label(rangeBoundsLabel2Rect, new GUIContent(maxValue.ToString(isInt ? "F0" : "F2")));
			position.xMax -= rangeBoundsLabelWidth;

			EditorGUI.BeginChangeCheck();
			EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, rangeMin, rangeMax);

			if (EditorGUI.EndChangeCheck())
			{
				if (isInt)
				{
					minProp.intValue = Mathf.RoundToInt(minValue);
					maxProp.intValue = Mathf.RoundToInt(maxValue);
				}
				else
				{
					minProp.floatValue = minValue;
					maxProp.floatValue = maxValue;
				}
			}

			EditorGUI.EndProperty();
		}
	}
}
#endif