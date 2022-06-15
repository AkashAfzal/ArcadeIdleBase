using UnityEngine;

namespace GameDevUtils.CurrencyManager
{


    public class LevelManager : MonoBehaviour
    {
        [SerializeField] GameObject[] allLevels;
        [SerializeField] int          currentLevelNumber;
        [SerializeField] bool         isTesting;
        readonly         string       Level_Pref     = "LevelNumber";
        readonly         string       PlayLevel_Pref = "PlayLevelNumber";

        public void LoadLevelAtStart()
        {
            if (allLevels.Length > 0)
            {
                Instantiate(allLevels[CurrentPlayLevelNumber() - 1], Vector3.zero, Quaternion.identity);
            }
        }

        public int InfinityCurrentLevelNumber()
        {
            if (!isTesting)
            {
                if (!PlayerPrefs.HasKey(Level_Pref))
                {
                    PlayerPrefs.SetInt(Level_Pref, 1);
                }

                return PlayerPrefs.GetInt(Level_Pref, 0);
            }
            else
                return currentLevelNumber;
        }
    
        public int CurrentPlayLevelNumber()
        {
            if (!isTesting)
            {
                if (!PlayerPrefs.HasKey(PlayLevel_Pref))
                {
                    PlayerPrefs.SetInt(PlayLevel_Pref, 1);
                }

                return PlayerPrefs.GetInt(PlayLevel_Pref, 0);
            }
            else
                return currentLevelNumber;
        }
    
        public void NextUnlockLevel()
        {
            int level     = PlayerPrefs.GetInt(Level_Pref,     0) + 1;
            int playLevel = PlayerPrefs.GetInt(PlayLevel_Pref, 0) + 1;
            if (playLevel > allLevels.Length)
            {
                playLevel = 1;
            }
            PlayerPrefs.SetInt(Level_Pref,     level);
            PlayerPrefs.SetInt(PlayLevel_Pref, playLevel);
            PlayerPrefs.Save();
        }

    }


}
