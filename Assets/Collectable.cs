using UnityEngine;

public class Collectable : MonoBehaviour
{

   [SerializeField] string id;

   void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         StackManager.Instance.AddStack(id);
         gameObject.SetActive(false);
      }
   }

}
