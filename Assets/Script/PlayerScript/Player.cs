using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    public float movementSpeed;
    private bool facingRight;
    private Animator myAnimator;
    private bool attack;

    [SerializeField]
    private Transform[] groundPoint;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool isGrounded;
    private bool jump;
    [SerializeField]
    private float jumpForce;
    public bool airControl;

    private float timeBtwAttack;
    private float startTimeBtwAttack;
    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;
    public int damage;

	// Use this for initialization
	void Start ()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
	}

    // Update is called once per frame

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();
        HandleMovement(horizontal);
        Flip(horizontal);
        HandleAttacks();
        HandleLayers();
        ResetValue();
	}

    private void HandleMovement(float horizontal)
    {
        if(myRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }
        if(!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")&&(isGrounded || airControl))
        {
            myRigidbody.velocity = new Vector2(horizontal * movementSpeed, myRigidbody.velocity.y);
        }
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));

        if(isGrounded && jump)
        {
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0, jumpForce));
            myAnimator.SetTrigger("jump");
        }
    }

    private void HandleAttacks()
    {
        if(attack && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myAnimator.SetTrigger("attack");
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private void HandleInput()
    {
        if(timeBtwAttack <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attack = true;
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<EnemyScript>().TakeDamage(damage);
                }
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void ResetValue()
    {
        attack = false;
        jump = false;
    }

    private bool IsGrounded()
    {
        if(myRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoint)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for(int i = 0; i < colliders.Length; i++)
                {
                    if(colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if(!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
}
