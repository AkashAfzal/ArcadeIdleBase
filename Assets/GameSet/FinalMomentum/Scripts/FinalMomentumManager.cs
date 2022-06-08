using System.Collections.Generic;
using UnityEngine;


namespace GameAssets.GameSet.FinalMomentum.Scripts
{


	public enum NumberType
	{

		Incremental,
		Double

	}

	public class FinalMomentumManager : MonoBehaviour
	{
		[Tooltip("If Enable Show Changes In Inspector View Else Don't Show Changes in Inspector")] 
		[SerializeField] bool isShowEffectInInspector;
		[Tooltip("Number To Show At Final Momentum")] 
		[SerializeField] int number;
		[Tooltip("Text To Show With Number")] 
		[SerializeField] string textShowWithNumber;
		[Tooltip("If Type is Incremental then Show 'Plus' of above 'Number' if type is Double then show 'Double' if above 'Number' ")] 
		[SerializeField] NumberType  numberType = NumberType.Incremental;
		
		[SerializeField] private Gradient                colorToApply;
		[SerializeField]         List<FinalMomentumCube> allCubes;

		[SerializeField] Vector3 positionOffsetToAdd;
		
		bool IsNotCompleted = false;

		void Start()
		{
			SetColorValues();
		}

		void OnValidate()
		{
			if (!isShowEffectInInspector) return;
			ShowEffect();
			SetCubesPosition();
		}


		private void SetColorValues()
		{
			double value = number;
			for (int i = 0; i < allCubes.Count; i++)
			{
				allCubes[i].SetColor(colorToApply.Evaluate((float) i / allCubes.Count));
				allCubes[i].SetText(value, textShowWithNumber);
				if (numberType == NumberType.Incremental)
				{
					value += number;
				}
				else if (numberType == NumberType.Double)
				{
					value *= number;
				}
			}
		}

		private void ShowEffect()
		{
			double value = number;
			for (int i = 0; i < allCubes.Count; i++)
			{
				allCubes[i].SetColor(colorToApply.Evaluate((float) i / allCubes.Count));
				allCubes[i].ShowColor();
				allCubes[i].SetText(value, textShowWithNumber);
				if (numberType == NumberType.Incremental)
				{
					value += number;
				}
				else if (numberType == NumberType.Double)
				{
					value *= number;
				}
			}
		}

		private void SetCubesPosition()
		{
			for (int i = 0; i < allCubes.Count; i++)
			{
				if (i != 0)
				{
					allCubes[i].transform.position = allCubes[i - 1].transform.position + new Vector3(positionOffsetToAdd.x, positionOffsetToAdd.y, positionOffsetToAdd.z);
				}
				
			}
		}
		
		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("PlayerDetector") && !IsNotCompleted)
			{
				IsNotCompleted = true;
				//MakeUpStack.Instance.HideTextCount();
				// other.transform.root.GetComponent<SplineSlideController>().MoveToFinalPosition();
				// Time.timeScale = 0.5f;
			}
		}

	}


}