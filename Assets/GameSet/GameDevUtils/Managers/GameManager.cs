using System;
using System.Threading.Tasks;
using DG.Tweening;
// using ElephantSDK;
// using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


namespace GameAssets.GameSet.GameDevUtils.Managers
{

	public enum GameState
	{

		MainMenu      = 0,
		Gameplay      = 1,
		Pause         = 2,
		Win           = 3,
		Fail          = 4,
		FinalMomentum = 5

	}

	public class GameManager : MonoBehaviour
	{

		public static GameManager Instance { get; private set; }

		[SerializeField] GameState gameCurrentState = GameState.MainMenu;

		public GameState GameCurrentState
		{
			get => gameCurrentState;
			private set
			{
				gameCurrentState = value;
				onGameStateChangedEvent?.Invoke(gameCurrentState);
			}
		}


		static UnityEvent<GameState> onGameStateChangedEvent = new UnityEvent<GameState>();
		
		public LevelManager levelManager;
		public UIManager    uiManager;

		[SerializeField] float        beforeLevelCompleteDelay;
		[SerializeField] float        beforeLevelFailDelay;
		


		public int InfinityCurrentLevel    => levelManager.InfinityCurrentLevelNumber();
		public int NotInfinityCurrentLevel => levelManager.CurrentPlayLevelNumber();
		bool       IsLevelCompleteNotInvoke;


		public delegate void OnMainMenuEvent();

		public static event OnMainMenuEvent onMainMenuEvent;

		public delegate void OnGamePlayEvent();

		public static event OnGamePlayEvent onGamePlayEvent;

		public delegate void OnPauseEvent();

		public static event OnPauseEvent onPauseEvent;


		public delegate void OnFinalMomentumEvent();

		public static event OnFinalMomentumEvent onFinalMomentumEvent;

		public delegate void OnCompleteEvent();

		public static event OnCompleteEvent onCompleteEvent;

		public delegate void OnFailedEvent();

		public static event OnFailedEvent onFailedEvent;


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
			}
		}

		void OnEnable()
		{
			Application.targetFrameRate = 60;
			onGameStateChangedEvent.AddListener(OnGameStateChanged);
		}

		void OnDisable()
		{
			onGameStateChangedEvent.RemoveAllListeners();
		}

		void Start()
		{
			LoadLevelAtStart(); 
			ChangeGameState(gameCurrentState);
		}


		public void ChangeGameState(GameState state)
		{
			GameCurrentState = state;
		}

		async void OnGameStateChanged(GameState state)
		{
			switch (state)
			{
				case GameState.MainMenu:
					Time.timeScale = 1;
					uiManager.EnableUIScreen(GameState.MainMenu);
					onMainMenuEvent?.Invoke();
					break;

				case GameState.Gameplay:
					Time.timeScale = 1;
					uiManager.EnableUIScreen(GameState.Gameplay);
					onGamePlayEvent?.Invoke();
					break;

				case GameState.Pause:
					Time.timeScale = 0;
					uiManager.EnableUIScreen(GameState.Pause);
					onPauseEvent?.Invoke();
					break;

				case GameState.FinalMomentum:
					uiManager.EnableUIScreen(GameState.FinalMomentum);
					onFinalMomentumEvent?.Invoke();
					break;

				case GameState.Win:
					if (IsLevelCompleteNotInvoke) return;
					IsLevelCompleteNotInvoke = true;
					await Task.Delay(TimeSpan.FromSeconds(beforeLevelCompleteDelay));
					SoundManager.Instance.PlayOneShot(SoundManager.Instance.winClip, 1);
					uiManager.EnableUIScreen(GameState.Win);
					
					// Elephant.LevelCompleted(InfinityCurrentLevel);
					// GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "LevelComplete", InfinityCurrentLevel);
					HapticFeedback.Generate(UIFeedbackType.Success);
					NextUnlockLevel();
					onCompleteEvent?.Invoke();
					break;

				case GameState.Fail:
					Camera.main.DOShakePosition(0.5f, new Vector3(1, 1, 1));
					await Task.Delay(TimeSpan.FromSeconds(beforeLevelFailDelay));
					SoundManager.Instance.PlayOneShot(SoundManager.Instance.failClip, 1);
					uiManager.EnableUIScreen(GameState.Fail);

					// Elephant.LevelFailed(InfinityCurrentLevel);
					// GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "LevelFail", InfinityCurrentLevel);
					HapticFeedback.Generate(UIFeedbackType.Error);
					onFailedEvent?.Invoke();
					break;
			}
		}


		private void LoadLevelAtStart()
		{
			levelManager.LoadLevelAtStart();
			// Elephant.LevelStarted(InfinityCurrentLevel);
			// GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "LevelStart", InfinityCurrentLevel);
		}


		void NextUnlockLevel()
		{
			levelManager.NextUnlockLevel();
		}


		public void TabToContinue()
		{
			SoundManager.Instance.PlayButtonSound();
			ChangeGameState(GameState.Gameplay);
		}

		public void Restart()
		{
			SoundManager.Instance.PlayButtonSound();
			//followCameraAsset.ResetCameraValue();
			SceneManager.LoadScene(0);
		}


		public void ExperimentLevel()
		{
			levelManager.NextUnlockLevel();
			SoundManager.Instance.PlayButtonSound();
			//followCameraAsset.ResetCameraValue();
			SceneManager.LoadScene(0);
		}

	}


}