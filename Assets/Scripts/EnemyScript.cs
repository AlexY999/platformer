using System.Collections;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Health")] [SerializeField] private float hp = 100;
    [SerializeField] private GameObject healthBar;

    [Header("Attack force")] [SerializeField]
    private float attackForce = 20;

    [Header("Move info")] [SerializeField] private float speedMove;
    [SerializeField] private float distance;

    private float _startHp = 100;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private static readonly int Property = Animator.StringToHash("Attack 3");

    public void DamageEnemy(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }

        var transformLocalScale = healthBar.transform.localScale;
        transformLocalScale.x = hp / _startHp;
        healthBar.transform.localScale = transformLocalScale;
    }

    private void Start()
    {
        _startHp = hp;
        StartCoroutine(MoveEnemy());
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator MoveEnemy()
    {
        Vector3 startPosition = transform.position;
        Vector3 velocity = Vector3.right * speedMove;

        while (true)
        {
            if (transform.position.x > startPosition.x + distance)
                velocity.x = -speedMove;
            else if (transform.position.x < startPosition.x - distance)
                velocity.x = speedMove;

            transform.position += velocity * Time.deltaTime;

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        float delta = other.contacts[0].point.x - transform.position.x;
        other.rigidbody.AddForce(new Vector2(delta * attackForce, 0), ForceMode2D.Impulse);

        if (delta < 0)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;

        _animator.SetTrigger(Property);
    }
}
