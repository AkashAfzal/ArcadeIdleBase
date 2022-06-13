using UnityEngine;

public class Collectable : MonoBehaviour, IStackObject
{

   [SerializeField] string id;
   public           string ID => id;

   void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         StackManager.Instance.AddStack(ID);
         gameObject.SetActive(false);
      }
   }

 

}
