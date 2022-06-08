using UnityEngine;

namespace GameAssets.GameSet.GameDevUtils.Managers
{


	public class SoundManager : MonoBehaviour
	{

		public static    SoundManager Instance { get; private set; }
		[SerializeField] AudioSource  bgSoundSource;
		[SerializeField] AudioSource  sFXSoundSource;

		public AudioClip   bgClip;
		public AudioClip   buttonClip;
		public AudioClip   winClip;
		public AudioClip   failClip;
		public AudioClip   stackAddClip;
		public AudioClip   stackRemoveClip;
		public AudioClip   makeUpToolClip;
		public AudioClip[] reactionClips;


		void Awake()
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

		void Start()
		{
			if (bgSoundSource.isPlaying)
				return;
			
			bgSoundSource.clip = bgClip;
			bgSoundSource.loop = true;
			bgSoundSource.Play();
		}


		public void SetBgSoundSetting(bool toggle)
		{
			bgSoundSource.mute         = !toggle;
		}

		public void SetSfxSoundSetting(bool toggle)
		{
			sFXSoundSource.mute    = !toggle;
		}

		public void PlayOneShot(AudioClip clip, float volume)
		{
			sFXSoundSource.PlayOneShot(clip, volume);
		}

		public void PlayButtonSound() => sFXSoundSource.PlayOneShot(buttonClip, 1);
	}


}