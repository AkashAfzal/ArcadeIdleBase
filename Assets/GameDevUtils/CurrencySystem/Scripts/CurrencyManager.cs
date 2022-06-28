using System;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevUtils.CurrencySystem
{


	[Serializable]
	public class Currency
	{

		[Tooltip("Name of Currency")] [SerializeField]             string    currencyName;
		[Tooltip("Total value of Currency")] [SerializeField]      int       totalCurrency;
		[Tooltip("Sprite Icon For Animation")] [SerializeField]    Sprite    currencyIcon;
		[Tooltip("It must bhe Canvas Transform")] [SerializeField] Transform animationTarget;

		public string  CurrencyName   => currencyName;
		public Sprite  CurrencyIcon   => currencyIcon;
		public Vector3 TargetPosition => animationTarget.position;
		
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
			if (TotalCurrency < 0) TotalCurrency = 0;
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

		public void AddCurrencyValue(string currencyName, int currencyValue)
		{
			Currency currency = RequiredCurrency(currencyName);
			if (currency == null) return;
			currency.AddCurrency(currencyValue);
			OnCurrencyChangedEvent?.Invoke(currencyName, currency.TotalCurrency);
		}

		public void AddCurrencyValueWithAnimation(string currencyName, int currencyValue, Vector3 startPosition)
		{
			Currency currency = RequiredCurrency(currencyName);
			if (currency == null) return;
			coinsAnimationManager.Animate(currencyName, currencyValue, currency.CurrencyIcon, startPosition, currency.TargetPosition);
		}

		public void RemoveCurrencyValue(string currencyName, int currencyValue)
		{
			Currency currency = RequiredCurrency(currencyName);
			if (currency == null) return;
			currency.RemoveCurrency(currencyValue);
			OnCurrencyChangedEvent?.Invoke(currencyName, currency.TotalCurrency);
		}

		public int TotalCurrencyFor(string currencyName)
		{
			var currency = RequiredCurrency(currencyName);
			return currency?.TotalCurrency ?? 0;
		}

		private Currency RequiredCurrency(string currencyName)
		{
			Currency requiredCurrency = null;
			foreach (Currency currency in currencies)
			{
				if (currency.CurrencyName == currencyName)
				{
					requiredCurrency = currency;
					return requiredCurrency;
				}
			}

			Debug.LogError($"Currency Name of {currencyName} Not in List Please Add First");
			return requiredCurrency;
		}

	}


}