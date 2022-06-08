using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

	static Singleton<T> instance;

	public static T Instance => (T)instance;

	private void Awake()
	{
		instance = this;
	}

}