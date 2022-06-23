using System;
using UnityEngine;


namespace GameDevUtils.HealthSystem
{


	[System.Serializable]
	public class HealthSystem : IHealthSystem
	{

		[SerializeField] float              m_MaxHealth = 100;
		public           float              maximumHealth => m_MaxHealth;
		public           float              currentHealth { get; protected set; }
		public           bool               isAlive       { get; set; }
		public event Action                 OnDeath;
		public event Action<float, Vector3> OnTakeDamage;
		public event Action<float>          OnHealDamage;
		public event Action                 OnRevive;

		public HealthSystem()
		{
			currentHealth = maximumHealth;
			isAlive       = true;
		}

		public void TakeDamage(float damageAmount, Vector3 hitPoint)
		{
			if (!isAlive) return;
			currentHealth -= damageAmount;
			if (currentHealth <= 0)
			{
				isAlive = false;
				OnDeath?.Invoke();
			}

			OnTakeDamage?.Invoke(damageAmount, hitPoint);
		}

		public void HealDamage(float amount)
		{
			if (!isAlive) return;
			OnHealDamage?.Invoke(amount);
			currentHealth += amount;
			if (currentHealth >= maximumHealth) currentHealth = maximumHealth;
		}

		public void Revive()
		{
			currentHealth = maximumHealth;
			isAlive       = true;
			OnRevive?.Invoke();
		}

		public void Death()
		{
			if (!isAlive) return;
			currentHealth = 0;
			isAlive       = false;
			OnDeath?.Invoke();
		}

	}

	public interface IHealthSystem
	{

		float                        maximumHealth { get; }
		float                        currentHealth { get; }
		bool                         isAlive       { get; set; }
		event Action                 OnDeath;
		event Action<float, Vector3> OnTakeDamage;
		event Action<float>          OnHealDamage;
		event Action                 OnRevive;

		void TakeDamage(float damageAmount, Vector3 hitPoint);

		void HealDamage(float amount);

		void Revive();

		void Death();

	}

	public interface IDamageable
	{

		bool IsDestroyed { get; set; }

		void Damage(float damageAmount, Vector3 hitPoint);

		void DestroyObject();

	}


}