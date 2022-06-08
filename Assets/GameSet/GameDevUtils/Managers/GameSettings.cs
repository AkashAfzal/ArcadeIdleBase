using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace GameAssets.GameSet.GameDevUtils.Managers
{


	[System.Serializable]
	public class Toggle
	{
		public Button button    = null;
		public Sprite OnSprite  = null;
		public Sprite OffSprite = null;

		private Image SpriteImage = null;

		public void AddListener(UnityAction call)
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(call);
			SpriteImage = button.GetComponent<Image>();
		}

		public void SetState(bool toggle) => SpriteImage.sprite = toggle ? OnSprite : OffSprite;

	}


	public class GameSettings : MonoBehaviour
	{

		public static GameSettings Instance { get; private set; }

		[SerializeField]         RectTransform SettingPanel  = null;
		[SerializeField]         Button        SettingButton = null;
		[Space] [SerializeField] Toggle        MusicToggle   = null;
		[SerializeField]         Toggle        SFXToggle     = null;
		[SerializeField]         Toggle        HapticToggle  = null;

		private bool  CanToggle = true;
		private bool  Toggle    = false;
		private float PosY      = 0f;

		bool ToggleStatusBg
		{
			set
			{
				PlayerPrefs.SetInt("toggleStatusBg", value ? 1 : 0); 
				SoundManager.Instance.SetBgSoundSetting(value);
			}
			get => PlayerPrefs.GetInt("toggleStatusBg") != 0;
		}
		bool ToggleStatusSfx
		{
			set
			{
				PlayerPrefs.SetInt("toggleStatusSFX", value ? 1 : 0);
				SoundManager.Instance.SetSfxSoundSetting(value);
			}
			get => PlayerPrefs.GetInt("toggleStatusSFX") != 0;
		}
		bool ToggleStatusHaptic
		{ 
			set => PlayerPrefs.SetInt("toggleStatusHaptic", value ? 1 : 0);
			get => PlayerPrefs.GetInt("toggleStatusHaptic") != 0;
		}

		public bool IsHapticEnable   => ToggleStatusHaptic;

		private void Awake()
		{
			if (!Instance)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				DestroyImmediate(gameObject);
			}
		}

		private void Start()
		{
			if (!PlayerPrefs.HasKey("toggleStatusHaptic"))
			{
				ToggleStatusBg     = true;
				ToggleStatusSfx    = true;
				ToggleStatusHaptic = true;
			}

			var anchoredPosition = SettingPanel.anchoredPosition;
			anchoredPosition              = new Vector2(anchoredPosition.x, -anchoredPosition.y);
			SettingPanel.anchoredPosition = anchoredPosition;
			PosY                          = -anchoredPosition.y;
			SettingButton.onClick.RemoveAllListeners();
			SettingButton.onClick.AddListener(TogglePanel);
			MusicToggle.AddListener(ToggleMusic);
			SFXToggle.AddListener(ToggleSfx);
			HapticToggle.AddListener(ToggleHaptic);
			LoadSettings();
		}

		public void InitializeSettings() => SettingButton.gameObject.SetActive(true);

		private void TogglePanel()
		{
			if (!CanToggle)
				return;
			CanToggle = false;
			Toggle    = !Toggle;
			if (Toggle)
			{
				SettingPanel.DOAnchorPos3DY(PosY, 0.25f, false).SetUpdate(true).OnComplete(() => CanToggle = true);
				if (SoundManager.Instance)
					SoundManager.Instance.PlayButtonSound();
				Invoke(nameof(ClosePanel), 3.0f);
			}
			else
				ClosePanel();
		}

		public void ClosePanel()
		{
			CancelInvoke();
			Toggle = false;
			SettingPanel.DOAnchorPos3DY(-PosY, 0.25f, false).SetUpdate(true).OnComplete(() => CanToggle = true);
		}

		private void LoadSettings()
		{
			MusicToggle.SetState(ToggleStatusBg);
			SFXToggle.SetState(ToggleStatusSfx);
			HapticToggle.SetState(ToggleStatusHaptic);
			SoundManager.Instance.SetBgSoundSetting(ToggleStatusBg);
			SoundManager.Instance.SetSfxSoundSetting(ToggleStatusSfx);
		}

		private void ToggleMusic()
		{
			ToggleStatusBg = !ToggleStatusBg;
			MusicToggle.SetState(ToggleStatusBg);
		}

		private void ToggleSfx()
		{
			ToggleStatusSfx = !ToggleStatusSfx;
			SFXToggle.SetState(ToggleStatusSfx);
		}

		private void ToggleHaptic()
		{
			ToggleStatusHaptic = !ToggleStatusHaptic;
			HapticToggle.SetState(ToggleStatusHaptic);
		}

	}


}