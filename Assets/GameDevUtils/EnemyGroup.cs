using GameDevUtils.CharacterController;
using GameDevUtils.StackSystem;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{

	[SerializeField] private float     detectRadius;
	[SerializeField] private LayerMask opponentLayerMask;
	[SerializeField] private Enemy[]   allEnemies;

	
	private bool       attacking;
	private Collider[] m_Opponents = new Collider[3];


	public delegate void          OnAttackDelegate(Transform target);
	public event OnAttackDelegate OnGroupAttackInvoke;


	public delegate void              OnAttackStopDelegate();
	public event OnAttackStopDelegate OnGroupStopAttackInvoke;


	StackManager _playerStack;
	StackManager PlayerStack
	{
		get
		{
			if (_playerStack == null)
			{
				_playerStack = FindObjectOfType<FreeMovementController>().GetComponent<StackManager>();
			}

			return _playerStack;
		}
	}

	void Start()
	{
		SetEnemyGroupFoAllEnemies();
	}
	
	void SetEnemyGroupFoAllEnemies()
	{
		foreach (var enemy in allEnemies)
		{
			enemy.SetEnemyGroup(this);
		}	
	}


	void Update()
	{
		if (!attacking && Physics.OverlapSphereNonAlloc(transform.position, detectRadius, m_Opponents, opponentLayerMask) > 0)
		{
			attacking = true;
			OnGroupAttackInvoke?.Invoke(m_Opponents[0].transform);
		}
		if (attacking && PlayerStack.IsStackEmpty("Followers") && Physics.OverlapSphereNonAlloc(transform.position, detectRadius, m_Opponents,opponentLayerMask) == 0)
		{
			attacking = false;
			OnGroupStopAttackInvoke?.Invoke();
		}
	}

}