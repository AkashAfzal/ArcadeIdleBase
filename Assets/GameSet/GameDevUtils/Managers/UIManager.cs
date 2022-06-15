using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace GameDevUtils.CurrencyManager
{


	[System.Serializable]
	public class GameStateScreens
	{

		public GameState  stateType;
		public GameObject panel;

	}


	public class UIManager : MonoBehaviour
	{

		[SerializeField] Text gpLevelText;
		[SerializeField] Text mmLevelText;
		[SerializeField] Text gpExperimentLevelText;

		[SerializeField] private GameStateScreens[] gameStateScreens;

		public void EnableUIScreen(GameState gameState)
		{
			foreach (GameStateScreens stateScreen in gameStateScreens)
			{
				stateScreen.panel.SetActive(stateScreen.stateType == gameState);
			}

			UpdateLevel(GameManager.Instance.InfinityCurrentLevel);
		}

		void UpdateLevel(int levelNumber)
		{
			gpLevelText.text           = $"Level {levelNumber}";
			gpExperimentLevelText.text = $"Experiment Level {GameManager.Instance.NotInfinityCurrentLevel}";
			mmLevelText.text           = $"Level {levelNumber}";
		}

	}


}