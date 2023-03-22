using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MobMovement : MonoBehaviour
{
    [Range(1f, 10f)]
    private float mobRange=9f;

    private float horizontal = 1f;
    private float speed = 3f;
  //  private float jumpingPower = 20f;
    private bool isFacingRight = true;
    private Vector2 spawnPos;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        spawnPos = groundCheck.position;
    }

    // Update is called once per frame
    void Update()
    {
      


    }

    private void FixedUpdate()
    {
        AutoMove();
        Flip();
        UpdateAnimator();
    }

    private void AutoMove()
    {
        if (isEdgeAhead() || isOutOfRange())
        {
            horizontal *= -1;
        }
       // horizontal = 1;
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }
    private bool isOutOfRange()
    {
      //  Debug.Log(groundCheck.position.x +" "+ spawnPos.x);
        if (isFacingRight)
        {
            if(groundCheck.position.x - spawnPos.x >= mobRange)
            {
               // Debug.Log(groundCheck.position.x - spawnPos.x +" R= "+ mobRange);
                return true;
            }
            return false;
        }
        else
        {
            if (spawnPos.x -groundCheck.position.x  >= mobRange)
            {
             //   Debug.Log(spawnPos.x - groundCheck.position.x + " L= " + mobRange);
                return true;
            }
            return false;
        }
    }
        private bool isEdgeAhead()
    {
        Vector2 tmpGround = groundCheck.position;
        if (isFacingRight)
        {
            tmpGround.x +=1f;
            return !IsGrounded(tmpGround);
        }
        else
        {
            tmpGround.x += -1f;
            return !IsGrounded(tmpGround);
        }

 

    }
    private void UpdateAnimator()
    {
        if (horizontal != 0f && IsGrounded(groundCheck.position))
        {
            animator.SetBool("running", true);
        }
        else
        {
            animator.SetBool("running", false);

        }

        if (!IsGrounded(groundCheck.position))
        {
            if (rb.velocity.y > 0)
            {
      //          animator.SetBool("jumping", true);
            }
            else
            {
   //             animator.SetBool("jumping", false);
            }

            if (rb.velocity.y < 0)
            {
    //            animator.SetBool("falling", true);

            }
            else
            {
            //    animator.SetBool("falling", false);
            }
        }
        else
        {
        //    animator.SetBool("falling", false);
        }

    }
    private bool IsGrounded(Vector2 pos)
    {
        //   Debug.Log(rb.velocity.y);
        return Physics2D.OverlapCircle(pos, 0.2f, groundLayer);
    }

    private void Flip()
    {


        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
