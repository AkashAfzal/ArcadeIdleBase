using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace GameDevUtils.CurrencySystem
{


	public class CoinsAnimationManager : MonoBehaviour
	{

		//References
		[Header("UI references")] [SerializeField] GameObject    animatedCoinPrefab;
		[SerializeField]                           Transform     target;
		[SerializeField]                           RectTransform canvasRect;

		[Header("Animation settings")] [SerializeField] [Range(0.5f, 0.9f)] float minAnimDuration;
		[SerializeField] [Range(                               0.9f, 2f)]   float maxAnimDuration;
		[SerializeField]                                                    Ease  easeType;
		[SerializeField]                                                    float spread;

		[Header("Available coins : (coins to pool)")] [SerializeField] int               maxCoins;
		private readonly                                               Queue<GameObject> CoinsQueue = new Queue<GameObject>();


		Vector3 TargetPosition;


		void Awake()
		{
			TargetPosition = target.position;

			//prepare pool
			PrepareCoins();
		}

		void PrepareCoins()
		{
			for (int i = 0; i < maxCoins; i++)
			{
				GameObject coin = Instantiate(animatedCoinPrefab);
				coin.transform.parent = canvasRect;
				coin.SetActive(false);
				CoinsQueue.Enqueue(coin);
			}
		}

		public void Animate(string currencyName, int amount, Vector3 collectedCoinPosition)
		{
			for (int i = 0; i < amount; i++)
			{
				//check if there's coins in the pool
				if (CoinsQueue.Count > 0)
				{
					//extract a coin from the pool
					GameObject coin = CoinsQueue.Dequeue();
					coin.SetActive(true);

					//move coin to the collected coin pos
					var rectTransform = coin.GetComponent<RectTransform>();
					rectTransform.localPosition = collectedCoinPosition + new Vector3(Random.Range(-spread, spread), 0f, 0f);

					//animate coin to target position
					float duration = Random.Range(minAnimDuration, maxAnimDuration);
					coin.transform.DOMove(TargetPosition, duration).SetEase(easeType).OnComplete(() =>
					{
						//executes whenever coin reach target position
						coin.SetActive(false);
						CoinsQueue.Enqueue(coin);
						
						CurrencyManager.Instance.PlusCurrencyValue(currencyName,1);
					});
				}
			}
		}

	}


}