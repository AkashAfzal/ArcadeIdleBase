using System;
using GameDevUtils.StackSystem;
using UnityEngine;

public class BaseStackArea : MonoBehaviour
{
	public Transform    targetPoint;
	public StackManager stackManager;
	
	public event Action OnStackValueRemove;
	public event Action OnStackValueFull;

	int _currentStack;

	// int CurrentStack
	// {
	// 	get => _currentStack;
	// 	set
	// 	{
	// 		_currentStack = value;
	// 		if (stackManager.IsStackQuantityFull)
	// 		{
	// 			OnStackValueFull?.Invoke();
	// 		}
	// 	}
	// }

	// public bool IsAreaFUll => stackManager.IsStackQuantityFull;
	//
	// public void StackValueUp()
	// {
	// 	CurrentStack++;
	// }
	//
	// public void RemoveStackValue()
	// {
	// 	CurrentStack++;
	// 	OnStackValueRemove?.Invoke();
	// }

}