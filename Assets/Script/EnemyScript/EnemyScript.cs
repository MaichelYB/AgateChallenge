using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    private float movementSpeed;
    public float speed;
    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;
    private bool hittingWall;
    private float dazedTime;
    public float startDazedTime;

    private bool notAtEdge;
    public Transform edgeCheck;
    public LayerMask whatIsEdge;
    public int health;
	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);

        notAtEdge = Physics2D.OverlapCircle(edgeCheck.position, wallCheckRadius, whatIsEdge);

        if (hittingWall||notAtEdge)
        {
            Flip();
        }
        if(dazedTime <= 0)
        {
            movementSpeed = 6 * speed;
        }
        else
        {
            movementSpeed = 0;
            dazedTime -= Time.deltaTime;
        }
        if(transform.localScale.x < 0)
        {
            transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }
    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void TakeDamage(int damage)
    {
        dazedTime = startDazedTime; 
        health -= damage;
        Debug.Log("Damage taken!");
    }
}
