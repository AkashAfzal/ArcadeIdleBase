using GameDevUtils.HealthSystem;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Hitbox : MonoBehaviour
{

	private                  GameObject Target;
	[HideInInspector] public int        damage;


	public void AttackTarget(GameObject target)
	{
		Target = target;
	}

	void ApplyDamage(IDamageable iDamageable)
	{
		iDamageable.Damage(damage, transform.position);
	}


	void OnTriggerEnter(Collider other)
	{
		if (Target != null && Target.gameObject == other.gameObject)
		{
			var iDamageAble = other.GetComponent<IDamageable>();
			if (iDamageAble != null)
			{
				ApplyDamage(iDamageAble);
			}
		}
	}

}