using UnityEngine;

public interface IStackObject
{

    public string ID { get; }

    public GameObject gameObject { get; }

    public void SetPositionRotation(Vector3 position, Quaternion rotation);
    

}
