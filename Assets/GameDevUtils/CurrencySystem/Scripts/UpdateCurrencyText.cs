using System;
using TMPro;
using UnityEngine;

namespace GameDevUtils.CurrencySystem
{


    public class UpdateCurrencyText : MonoBehaviour
    {

        [SerializeField] string          currencyName;
        [SerializeField] TextMeshProUGUI currencyText;

        void OnEnable()
        {
            CurrencyManager.OnCurrencyChangedEvent += OnCurrencyChanged;
        }

        void OnDisable()
        {
            CurrencyManager.OnCurrencyChangedEvent -= OnCurrencyChanged;
        }

        void OnDestroy()
        {
            CurrencyManager.OnCurrencyChangedEvent -= OnCurrencyChanged;
        }

        void OnCurrencyChanged(string changedCurrencyName, int currencyValue)
        {
            if (String.Equals(currencyName, changedCurrencyName))
            {
                currencyText.text = currencyValue.ToString();
            }
        }

    }


}
