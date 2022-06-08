using UnityEngine;
using Random = UnityEngine.Random;

namespace GameAssets.GameSet.GameDevUtils.Utility
{


    public class RandomAnimation : MonoBehaviour
    {

        Animator MAnimator;


        void Start()
        {
            MAnimator = GetComponent<Animator>();
            int random = Random.Range(1, 3);
            MAnimator.SetBool(random.ToString(), true);
        }

    }


}
