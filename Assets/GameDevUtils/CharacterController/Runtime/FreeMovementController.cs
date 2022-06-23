using UnityEngine;
using GameDevUtils.CharacterController.InputSystem;
using GameDevUtils.HealthSystem;


namespace GameDevUtils.CharacterController
{


	public class FreeMovementController : BaseController, IDamageable
	{

		[SerializeField] HealthSystem.HealthSystem healthSystem;
		[SerializeField] float                     maxForwardSpeed;
		[SerializeField] float                     forwardSpeedAcceleration;
		[SerializeField] float                     forwardSpeedDeceleration;
		[SerializeField] float                     rotationSpeed;

		//Private Fields
		float         forwardSpeed;
		float         targetForwardSpeed;
		float         currentRotation;
		Vector3       currentVelocity;
		JoyStickInput joyStickInput;

		public bool IsPlayerInputApplied => joyStickInput?.JoystickDirection.magnitude > 0.1f;
		public bool IsDestroyed          { get; set; }


		protected override void Init()
		{
			joyStickInput = new JoyStickInput(120f, false, true);
		}

		protected override void DoUpdate()
		{
			joyStickInput.Calculate();
		}

		protected override void DoFixedUpdate()
		{
			base.DoFixedUpdate();
			AccelerateForwardSpeed();
			Movement();
			Rotation();
			UpdateAnimator(currentVelocity);
		}

		private void AccelerateForwardSpeed()
		{
			targetForwardSpeed = joyStickInput.JoystickDirection.magnitude * maxForwardSpeed;
			float verticalAcceleration = joyStickInput.JoystickDirection.magnitude > 0.1f ? forwardSpeedAcceleration : forwardSpeedDeceleration;
			forwardSpeed = Mathf.MoveTowards(forwardSpeed, targetForwardSpeed, verticalAcceleration * Time.deltaTime);
		}


		protected void Movement()
		{
			currentVelocity    = transform.forward * forwardSpeed;
			currentVelocity.y  = Rigidbody.velocity.y;
			Rigidbody.velocity = currentVelocity;
		}

		void Rotation()
		{
			if (joyStickInput.JoystickDirection.magnitude > 0)
			{
				currentRotation = Mathf.Atan2(joyStickInput.JoystickDirection.x, joyStickInput.JoystickDirection.y);
				currentRotation = Mathf.Rad2Deg * currentRotation;
				var targetRotation = Quaternion.Euler(Vector3.up * (currentRotation));
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
			}
		}

		void UpdateAnimator(Vector3 velocity)
		{
			float normVerticalSpeed = velocity.magnitude / (maxForwardSpeed / 3f);
			Animator.SetFloat("Value", normVerticalSpeed > 0.15f ? normVerticalSpeed : 0);
			Animator.speed = normVerticalSpeed > 0.15f ? maxForwardSpeed / 2 : 1;
		}

		public void Damage(float damageAmount, Vector3 hitPoint)
		{
			healthSystem.TakeDamage(damageAmount, hitPoint);
		}

		public void DestroyObject()
		{
			healthSystem.Death();
		}

	}


}