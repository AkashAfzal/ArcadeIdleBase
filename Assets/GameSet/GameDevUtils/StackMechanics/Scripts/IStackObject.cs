using UnityEngine;

public interface IStackObject
{

    public string ID { get; }

    public GameObject _GameObject { get; }

    public void SetPositionRotation(Vector3 position, Quaternion rotation);
    

}
