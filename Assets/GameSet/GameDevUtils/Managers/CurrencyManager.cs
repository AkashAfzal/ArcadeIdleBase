using System;
using System.Collections.Generic;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Managers
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
			SaveCurrency();
		}

		private void SaveCurrency()
		{
			PlayerPrefs.SetInt(CoinSavePrefs, TotalCurrency);
		}
	}


	public class CurrencyManager : MonoBehaviour
	{

		public static    CurrencyManager Instance { get; private set; }
		[SerializeField] List<Currency>  currencies;

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
			}
		}
	
		public void PlusCurrencyValue(string currencyName, int valueToSave)
		{
			foreach (Currency currency in currencies)
			{
				if (currency.CurrencyName == currencyName)
				{
					currency.AddCurrency(valueToSave);
					return;
				}
				else
				{
					Debug.LogError($"Currency Name of {currencyName} Not in List Please Add First");
				}
			}
		}
	
		public void SubtractCurrencyValue(string currencyName, int valueToSave)
		{
			foreach (Currency currency in currencies)
			{
				if (currency.CurrencyName == currencyName)
				{
					currency.RemoveCurrency(valueToSave);
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

		// public void AddNewCurrency(string currencyName, int initValueToSave)
		// {
		// 	var newCurrency = new Currency(currencyName, initValueToSave);
		// 	public
		//
		// }
	}


}