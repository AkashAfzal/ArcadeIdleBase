using GameDevUtils.HealthSystem;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Hitbox : MonoBehaviour
{

	private                  Transform Target;
	[HideInInspector] public int       damage;


	public void AttackTarget(Transform target)
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