using System.Collections.Generic;
using UnityEngine;


class PoolClass
{
	//Public Fields
	public List<GameObject> pool = new List<GameObject>();
	
	//Properties
	public string           TypePool { get; }
	public Transform        Parent   { get; }
	

	public PoolClass(string type, Transform parent)
	{
		TypePool = type;
		Parent   = parent;
	}
}


public static class PoolSystem
{
	
	static List<PoolClass> pools = new List<PoolClass>();

	static PoolSystem()
	{
		ResetPools();
	}

	public static void ResetPools()
	{
		pools = new List<PoolClass>();
	}

	public static GameObject InstantiatePool(this IPoolObject iPoolObject, int num)
	{
		PoolClass newPoolC = FindPoolClass(iPoolObject.GameObject.name);
		if (newPoolC.pool.Count > 0)
		{
			GameObject pg = newPoolC.pool[0];
			pg.SetActive(true);
			newPoolC.pool.RemoveAt(0);
			return pg;
		}

		GameObject go = GameObject.Instantiate(iPoolObject.GameObject, newPoolC.Parent, true);
		go.name             = iPoolObject.GameObject.name + num;
		return go;
	}

	public static void DestroyPool(this IPoolObject iPoolObject)
	{
		PoolClass newPoolC = FindPoolClass(iPoolObject.GameObject.name);
		newPoolC.pool.Add(iPoolObject.GameObject);
		iPoolObject.GameObject.SetActive(false);
	}

	public static List<GameObject> GetPoolOfType(this GameObject gameObject, string type)
	{
		return FindPoolClass(type).pool;
	}

	static PoolClass FindPoolClass(string type)
	{
		foreach (var item in pools)
		{
			if (type == item.TypePool)
				return item;
		}

		GameObject parent   = new GameObject { name = type + "s" };
		PoolClass  newPoolC = new PoolClass(type, parent.transform);
		pools.Add(newPoolC);
		return newPoolC;
	}

	public static string GetPoolType(this IPoolObject iPoolObject)
	{
		return FindPoolClass(iPoolObject.GameObject.name).TypePool;
	}

}

