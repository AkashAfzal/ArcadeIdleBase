using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class AttackBase : MonoBehaviour, IAttack
{

	public abstract void Attack();


	public abstract void Search();


	public abstract void MoveForAttack();

}