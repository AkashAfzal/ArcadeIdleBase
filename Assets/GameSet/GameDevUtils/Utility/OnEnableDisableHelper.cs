using UnityEngine;
using UnityEngine.Events;

public class OnEnableDisableHelper : MonoBehaviour
{

    public UnityEvent onEnableEvent;
    public UnityEvent onDisableEvent;


    void OnEnable()
    {
        onEnableEvent.Invoke();
    }

    void OnDisable()
    {
        onDisableEvent.Invoke();
    }

}
