using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinosaurScript : MonoBehaviour {

    public Transform player;
    public Transform walking;
    public Transform[] groundPoints;
    public Transform wallCheck;
    public LayerMask whenToJump;
    public LayerMask whenToWalk;
    public LayerMask whatIsWall;
    public float movementSpeed;
    public float playerCheckRadius;
    private bool jump;
    private bool hittingWall;
    private bool dinoWalk;
    private Rigidbody2D dinoRigidBody;
    private Animator dinoAnimator;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    private bool isGrounded;
    public float jumpForce;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        isGrounded = IsGrounded();

        jump = Physics2D.OverlapCircle(player.position, playerCheckRadius, whenToJump);

        hittingWall = Physics2D.OverlapCircle(wallCheck.position, playerCheckRadius, whatIsWall);

        dinoWalk = Physics2D.OverlapCircle(walking.position, playerCheckRadius, whenToWalk);

        if (dinoRigidBody.velocity.y < 0)
        {
            dinoAnimator.SetBool("isGround", false);
        }

        if (dinoWalk)
        {
            if (transform.localScale.x < 0)
            {
                transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
            }

            else
            {
                transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
            }

            if (hittingWall)
            {
                Flip();
            }
        }
        else
        {
            if (jump)
            {
                if (isGrounded && jump)
                {
                    isGrounded = false;
                    dinoRigidBody.AddForce(new Vector2(0, jumpForce));
                    dinoAnimator.SetTrigger("isJumping");
                }
            }
        }
    }

    private bool IsGrounded()
    {
        if (dinoRigidBody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        dinoAnimator.ResetTrigger("isJumping");
                        dinoAnimator.SetBool("isGround", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
