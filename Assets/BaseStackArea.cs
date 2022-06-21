using System;
using UnityEngine;

public class BaseStackArea : MonoBehaviour
{

	[SerializeField] int       maxCapacity;
	public           Transform targetPoint;

	public event Action OnStackValueRemove;
	public event Action OnStackValueFull;

	int _currentStack;

	int CurrentStack
	{
		get => -_currentStack;
		set
		{
			_currentStack = value;
			if (_currentStack != maxCapacity)
			{
				OnStackValueRemove?.Invoke();
			}
			else if (_currentStack == maxCapacity)
			{
				OnStackValueFull?.Invoke();
			}
		}
	}

	public bool IsAreaFUll => CurrentStack == maxCapacity;

	public void StackValueUp()
	{
		CurrentStack++;
	}

}