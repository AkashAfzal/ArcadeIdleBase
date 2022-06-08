using UnityEngine;

namespace GameAssets.GameSet.GameDevUtils.Utility
{


    public class EnableRandom : MonoBehaviour
    {

        public GameObject[] allObjects;

        void Start()
        {
            allObjects[Random.Range(0, allObjects.Length)].SetActive(true);
        
        }
    }


}
