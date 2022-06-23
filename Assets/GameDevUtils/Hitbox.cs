using GameDevUtils.HealthSystem;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Hitbox : MonoBehaviour
{

	[HideInInspector] public int damage;

	void ApplyDamage(IDamageable iDamageable)
	{
		iDamageable.Damage(damage, transform.position);
	}


	void OnTriggerEnter(Collider other)
	{
		// Debug.Log(other.name);
		var iDamageAble = other.GetComponent<IDamageable>();
		if (iDamageAble != null)
		{
			// Debug.Log("dAMAGE");
			ApplyDamage(iDamageAble);
		}
		
	}

}