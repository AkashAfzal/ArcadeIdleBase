using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace GameDevUtils.StackSystem.UpgradeSystem
{


	public class CapacityUpgrade : MonoBehaviour
	{

		//Inspector Fields 
		[SerializeField]                 string       upgradeStackName;
		[SerializeField] private         UpgradePopup upgradePopup;
		[SerializeField] private         StackManager stackToUpgrade;
		[SerializeField] private         Image        fillImage;
		[SerializeField, Range(0, 0.2f)] float        fillAmount = 0.01f;


		//Private Fields
		private bool IsStartFillAmount;

		//Properties
		private int UpgradePrice => stackToUpgrade.CurrentUpgradePriceOfStack(upgradeStackName);


		void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				IsStartFillAmount = true;
				StartCoroutine(nameof(FillAmountCo));
			}
		}

		void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				IsStartFillAmount    = false;
				fillImage.fillAmount = 0;
				upgradePopup.OpenClosePopup(false);
				StopCoroutine(nameof(FillAmountCo));
			}
		}

		IEnumerator FillAmountCo()
		{
			while (!stackToUpgrade.IsStackFullyUpgraded(upgradeStackName) && IsStartFillAmount && fillImage.fillAmount != 1)
			{
				fillImage.fillAmount += fillAmount;
				if (fillImage.fillAmount == 1)
				{
					IsStartFillAmount = false;
					ShowUpgradePopup();
					StopCoroutine(nameof(FillAmountCo));
				}

				yield return new WaitForSeconds(0.02f);
			}
		}

		private void ShowUpgradePopup()
		{
			upgradePopup.OpenClosePopup(true, UpgradePrice, CurrencySystem.CurrencyManager.Instance.TotalCurrencyFor("Coins") >= UpgradePrice, ButtonAction);
		}


		private void ButtonAction()
		{
			if (CurrencySystem.CurrencyManager.Instance.TotalCurrencyFor("Coins") >= UpgradePrice)
			{
				CurrencySystem.CurrencyManager.Instance.RemoveCurrencyValue("Coins", UpgradePrice);
				stackToUpgrade.UpgradeStackCapacity(upgradeStackName);
				upgradePopup.OpenClosePopup(false);
			}
			else
			{
				Debug.LogError("Coin Not Enough");
			}
		}

	}


}