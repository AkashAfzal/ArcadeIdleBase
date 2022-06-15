using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using GameDevUtils.CurrencyManager;

public enum UIFeedbackType
{

	Selection = 0,
	ImpactLight,
	ImpactMedium,
	ImpactHeavy,
	Success,
	Warning,
	Error

}

public class HapticFeedback
{

	public static void Generate(UIFeedbackType type)
	{
		if (GameSettings.Instance.IsHapticEnable)
		{
			//if (SoundManager.instance)
			//    if (!SoundManager.instance.canHaptic)
			//        return;
			#if UNITY_IPHONE && !UNITY_EDITOR
		GenerateFeedback((int)type);
			#endif
		}
	}

	#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern void GenerateFeedback(int type);
	#endif

}