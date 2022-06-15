using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameDevUtils.StackSystem.UpgradeSystem
{


	public class UpgradePopup : MonoBehaviour
	{

		[SerializeField] Button          closeButton;
		[SerializeField] Button          upgradeButton;
		[SerializeField] TextMeshProUGUI upgradeCoinsText;
		UnityAction                      OnClick;

		void Start()
		{
			closeButton.onClick.AddListener(() => { gameObject.SetActive(false); });
			upgradeButton.onClick.AddListener(() => { OnClick?.Invoke(); });
		}

		public void OpenClosePopup(bool value, int coinsRequire = 0, bool isCurrencyEnough = false, UnityAction onClickAction = null)
		{
			upgradeCoinsText.text      = coinsRequire.ToString();
			upgradeButton.interactable = isCurrencyEnough;
			OnClick                    = onClickAction;
			gameObject.SetActive(value);
		}

	}


}