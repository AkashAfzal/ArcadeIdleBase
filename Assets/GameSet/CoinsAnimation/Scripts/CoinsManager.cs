using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;
using GameAssets.GameSet.GameDevUtils.Managers;

public class CoinsManager : MonoBehaviour
{

	public static CoinsManager Instance;
	//References
	[Header ("UI references")]
	[SerializeField] TMP_Text coinUIText;
	[SerializeField] GameObject    animatedCoinPrefab;
	[SerializeField] Transform     target;
	public           RectTransform canvasRect;

	public Camera mainCam;

	[Space]
	[Header ("Available coins : (coins to pool)")]
	[SerializeField] int maxCoins;
	Queue<GameObject> coinsQueue = new Queue<GameObject> ();


	[Space]
	[Header ("Animation settings")]
	[SerializeField] [Range (0.5f, 0.9f)] float minAnimDuration;
	[SerializeField] [Range (0.9f, 2f)] float maxAnimDuration;

	[SerializeField] Ease easeType;
	[SerializeField] float spread;

	Vector3 targetPosition;


	private int _c = 0;

	public int Coins {
		get{ return _c; }
		set {
			_c = value;
			//update UI text whenever "Coins" variable is changed
			coinUIText.text = Coins.ToString ();
		}
	}

	void Awake ()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		targetPosition = target.position;

		_c              = CurrencyManager.Instance.TotalCurrencyFor("Coins");
		coinUIText.text = Coins.ToString ();
		//prepare pool
		PrepareCoins ();
	}

	void PrepareCoins ()
	{
		GameObject coin;
		for (int i = 0; i < maxCoins; i++) {
			coin = Instantiate (animatedCoinPrefab);
			coin.transform.parent = canvasRect;
			coin.SetActive (false);
			coinsQueue.Enqueue (coin);
		}
	}

	void Animate (Vector3 collectedCoinPosition, int amount)
	{
		for (int i = 0; i < amount; i++) {
			//check if there's coins in the pool
			if (coinsQueue.Count > 0) {
				//extract a coin from the pool
				GameObject coin = coinsQueue.Dequeue ();
				coin.SetActive (true);

				//move coin to the collected coin pos
				var rectTransform = coin.GetComponent<RectTransform>();
				rectTransform.localPosition = collectedCoinPosition + new Vector3 (Random.Range (-spread, spread), 0f, 0f);

				//animate coin to target position
				float duration = Random.Range (minAnimDuration, maxAnimDuration);
				coin.transform.DOMove (targetPosition, duration)
				.SetEase (easeType)
				.OnComplete (() => {
					//executes whenever coin reach target position
					coin.SetActive (false);
					coinsQueue.Enqueue (coin);

					Coins++;
					CurrencyManager.Instance.PlusCurrencyValue("Coins", 1);
				});
			}
		}
	}

	public void AddCoins (Vector3 collectedCoinPosition, int amount)
	{
		Animate (collectedCoinPosition, amount);
	}

	public void AddCoins(int amount)
	{
		Coins += amount;
		CurrencyManager.Instance.PlusCurrencyValue("Coins", amount);
	}
}
