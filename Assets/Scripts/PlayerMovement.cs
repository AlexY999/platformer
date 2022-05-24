using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float speed = 5f;
	[SerializeField] private float jumpForce = 2f;
	[SerializeField] private float damageFromAttack = 40f;

	private Animator _animator;
	private Rigidbody2D _rb;
	private BoxCollider2D _playerCollider;
	private Vector2 _move;
	private SpriteRenderer _sr;
	private Fight2D _fight2D;

	private static readonly int SpeedTag = Animator.StringToHash("Speed");
	private static readonly int AttackTag = Animator.StringToHash("Attack");
	private static readonly int JumpTag = Animator.StringToHash("Jump");
	
	private bool _onGround = true;

	void Start()
    {
	    _sr = GetComponent<SpriteRenderer>();
	    _animator = GetComponent<Animator>();
	    _rb = GetComponent<Rigidbody2D>();
	    _playerCollider = GetComponent<BoxCollider2D>();
	    _fight2D = GetComponent<Fight2D>();
    }

    void Update()
    {
		_move.x = Input.GetAxisRaw("Horizontal");

		_animator.SetFloat(SpeedTag, Mathf.Abs(_move.x));

		if (Input.GetButtonDown("Fire1"))
		{
			Fight2D.Action(transform.position, 0.2f, LayerMask.NameToLayer($"Enemy"), damageFromAttack, false);
			_animator.SetTrigger(AttackTag);
		}
		else if (Input.GetKey(KeyCode.Space))
		{
			if (!_onGround)
			{
				return;
			}    
			
			_animator.SetBool(JumpTag, true);
			Jump();
		}
    }
	
    private void Jump()
    {
	    _onGround = false;
	    _rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
    
	private void FixedUpdate()
	{
		_rb.AddForce(new Vector2(_move.x * speed, 0), ForceMode2D.Force);
		
		if (_move.x < 0f)
		{
			_sr.flipX = true;
		} else if (_move.x > 0f)
		{
			_sr.flipX = false;
		}
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.gameObject.TryGetComponent<CollisionZone>(out var zone)) return;

		if (zone.Type == CollisionZoneType.Ground)
		{
			_onGround = true;
			_animator.SetBool(JumpTag, false);
		}

		// foreach (var contact in collision.contacts)
		// {	
		// 	var playerBottomPoint =
		// 		gameObject.transform.position.y - _playerCollider.size.y;
		// 	
		// 	Debug.Log(contact.point.y - playerBottomPoint);
		//
		// 	if (zone.Type != CollisionZoneType.Ground || !(contact.point.y <= playerBottomPoint)) 
		// 		continue;
		// 	
		// 	_onGround = true;
		// 	_animator.SetBool(JumpTag, false);
		// 	break;
		// }
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.TryGetComponent<TriggerZone>(out var triggerZone))
		{
			if (triggerZone.Type == TriggerZoneType.Death)
			{
				Debug.Log("You are died!");
			}
			else if (triggerZone.Type == TriggerZoneType.Coins)
			{
				triggerZone.gameObject.SetActive(false);
				Debug.Log("You are collect the coin");
			}
			else if (triggerZone.Type == TriggerZoneType.End)
			{
				Debug.Log("You are complete the level");
			}
		}
	}
}
