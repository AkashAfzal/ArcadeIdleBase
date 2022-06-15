using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapacityUpgrade : MonoBehaviour
{

	
	[SerializeField]              Image fillImage;
	[SerializeField, Range(0, 0.2f)] float fillAmount = 0.01f;
	bool                                IsStartFillAmount;


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && other.gameObject.GetComponent<StackManager>())
		{
			IsStartFillAmount = true;
			StartCoroutine(nameof(FillAmountCo));
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && other.gameObject.GetComponent<StackManager>())
		{
			IsStartFillAmount    = false;
			fillImage.fillAmount = 0;
			StopCoroutine(nameof(FillAmountCo));
		}
	}

	IEnumerator FillAmountCo()
	{
		while (IsStartFillAmount && fillImage.fillAmount != 1)
		{
			if (fillImage.fillAmount == 1)
			{
				IsStartFillAmount = false;
				StopCoroutine(nameof(FillAmountCo));
				break;
			}

			fillImage.fillAmount += fillAmount;
			yield return new WaitForSeconds(0.02f);
		}
	}

}