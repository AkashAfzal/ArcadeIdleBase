using UnityEngine;


namespace GameDevUtils.Controller
{


	public abstract class BaseController : MonoBehaviour
	{

		public LayerMask groundLayerMask;
		public bool      isGrounded;
		public float     jumpForce;


		private float jumpTimeStamp   = 0;
		private float minJumpInterval = 0.25f;
		private bool  jumpInput       = false;
		private float groundDistance  = 0.5f;

		private Collider  collider;
		private Rigidbody rigidBody;
		private Animator  animator;


		public Collider Collider
		{
			get
			{
				if (collider == null)
				{
					var result = this.GetComponent<Collider>();
					if (result == null)
					{
						result = this.GetComponentInChildren<Collider>();
					}

					collider = result;
				}

				return collider;
			}
		}
		public Rigidbody Rigidbody
		{
			get
			{
				if (rigidBody == null)
				{
					var result = this.GetComponent<Rigidbody>();
					if (result == null)
					{
						result = this.GetComponentInChildren<Rigidbody>();
					}

					rigidBody = result;
				}

				return rigidBody;
			}
		}
		public Animator Animator
		{
			get
			{
				if (animator == null)
				{
					var result = this.GetComponent<Animator>();
					if (result == null)
					{
						result = this.GetComponentInChildren<Animator>();
					}

					animator = result;
				}

				return animator;
			}
		}

		public bool Jump { get; set; }

		void Start()
		{
			Init();
		}

		protected abstract void Init();

		private void Update()
		{
			DoUpdate();
		}

		protected virtual void DoUpdate()
		{
			if (!jumpInput && Jump)
			{
				jumpInput = true;
			}
		}

		private void FixedUpdate()
		{
			DoFixedUpdate();
		}

		protected virtual void DoFixedUpdate()
		{
			jumpInput = false;
			GroundCheck();
			JumpingAndLanding();
		}


		void GroundCheck()
		{
			isGrounded = Physics.CheckCapsule(Collider.bounds.center, new Vector3(Collider.bounds.center.x, Collider.bounds.min.y - groundDistance, Collider.bounds.center.z), 0.35f, groundLayerMask);
		}

		protected virtual void JumpingAndLanding()
		{
			bool jumpCooldownOver = (Time.time - jumpTimeStamp) >= minJumpInterval;
			if (jumpCooldownOver && isGrounded && jumpInput)
			{
				jumpTimeStamp = Time.time;
				Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
			}
		}

	}


}