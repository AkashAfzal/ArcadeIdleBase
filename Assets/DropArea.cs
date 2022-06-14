using UnityEngine;

public class DropArea : MonoBehaviour
{
    [SerializeField] string       requiredID;
    [SerializeField] StackManager stackManager;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<StackManager>())
        {
            if(stackManager.IsStackQuantityFull) return;
            var playerStack = other.gameObject.GetComponent<StackManager>();
            playerStack.RemoveStack(requiredID);
            stackManager.AddStack(requiredID);
            

        }
    }

}
