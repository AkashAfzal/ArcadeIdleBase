using System;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevUtils.CurrencySystem
{


	[Serializable]
	public class Currency
	{

		[SerializeField] string currencyName;
		[SerializeField] int    totalCurrency;

		public string CurrencyName => currencyName;
		public int TotalCurrency
		{
			private set => totalCurrency = value;
			get => totalCurrency;
		}

		string CoinSavePrefs;

		public void SetInitialValues()
		{
			TotalCurrency = totalCurrency;
			CoinSavePrefs = currencyName;
			if (!PlayerPrefs.HasKey(CoinSavePrefs))
			{
				SaveCurrency();
			}
			else
			{
				totalCurrency = PlayerPrefs.GetInt(CoinSavePrefs);
			}
		}

		public void AddCurrency(int value)
		{
			TotalCurrency += value;
			SaveCurrency();
		}

		public void RemoveCurrency(int value)
		{
			TotalCurrency -= value;
			// if (TotalCurrency < 0) TotalCurrency = 0;
			SaveCurrency();
		}

		private void SaveCurrency()
		{
			PlayerPrefs.SetInt(CoinSavePrefs, TotalCurrency);
			PlayerPrefs.Save();
		}

	}


	public class CurrencyManager : MonoBehaviour
	{

		public static    CurrencyManager       Instance { get; private set; }
		[SerializeField] CoinsAnimationManager coinsAnimationManager;
		[SerializeField] List<Currency>        currencies;

		public delegate void OnCurrencyChangedDelegate(string changedCurrencyName, int currencyValur);

		public static event OnCurrencyChangedDelegate OnCurrencyChangedEvent;

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				DestroyImmediate(gameObject);
				return;
			}

			SetAllCurrenciesInitialValues();
			
		}


		void SetAllCurrenciesInitialValues()
		{
			foreach (Currency currency in currencies)
			{
				currency.SetInitialValues();
				OnCurrencyChangedEvent?.Invoke(currency.CurrencyName, currency.TotalCurrency);
			}
		}

		public void PlusCurrencyValue(string currencyName, int currencyValue)
		{
			foreach (Currency currency in currencies)
			{
				if (currency.CurrencyName == currencyName)
				{
					currency.AddCurrency(currencyValue);
					OnCurrencyChangedEvent?.Invoke(currencyName, currency.TotalCurrency);
					return;
				}
				else
				{
					Debug.LogError($"Currency Name of {currencyName} Not in List Please Add First");
				}
			}
		}
		
		public void PlusCurrencyValueWithAnimation(string currencyName, int currencyValue, Vector3 startPosition)
		{
			foreach (Currency currency in currencies)
			{
				if (currency.CurrencyName == currencyName)
				{
					coinsAnimationManager.Animate(currencyName, currencyValue, startPosition);
					return;
				}
				else
				{
					Debug.LogError($"Currency Name of {currencyName} Not in List Please Add First");
				}
			}
		}

		public void SubtractCurrencyValue(string currencyName, int currencyValue)
		{
			foreach (Currency currency in currencies)
			{
				if (currency.CurrencyName == currencyName)
				{
					currency.RemoveCurrency(currencyValue);
					OnCurrencyChangedEvent?.Invoke(currencyName, currency.TotalCurrency);
					return;
				}
				else
				{
					Debug.LogError($"Currency Name of {currencyName} Not in List Please Add First");
				}
			}
		}

		public int TotalCurrencyFor(string currencyName)
		{
			foreach (Currency currency in currencies)
			{
				if (currency.CurrencyName == currencyName)
				{
					return currency.TotalCurrency;
				}
			}

			Debug.LogError($"Currency Name of {currencyName} Not in List Please Add First");
			return 0;
		}

	}


}