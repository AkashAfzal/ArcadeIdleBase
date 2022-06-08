using System.Collections;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolObject
{
    GameObject IPoolObject.GameObject => gameObject;
    [SerializeField]bool autoDestroyWithTime = false;
    [SerializeField] [Min(0)] float time = 1f;
    private void Awake()
    {
        if (autoDestroyWithTime)
            StartCoroutine(AutoDestroyPool());            
    }
    IEnumerator AutoDestroyPool()
    {
        yield return new WaitForSeconds(time);
        this.DestroyPool();
    }
    
}
