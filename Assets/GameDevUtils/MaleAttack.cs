using System;
using System.Collections;
using System.Collections.Generic;
using GameDevUtils.HealthSystem;
using UnityEngine;

public class MaleAttack : AttackBase
{

	public override void Attack()
	{
		Debug.Log("AtCk");
		followerMovement.Animator.SetBool("Attack", true);
	}

	public override void Search()
	{
		
	}

	public override void MoveForAttack()
	{
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<IDamageable>() != null)
		{
			Attack();
		}
	}

}
