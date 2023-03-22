using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 20f;
    private bool isFacingRight = true;
    private bool isHavingForce = false;
    private Vector2 velocityLeft;

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        MovementBind();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        Debug.Log(horizontal);
        /*velocityLeft = rb.velocity;
        if(Mathf.Round(velocityLeft.x)!= 0 && !isHavingForce)
        {
           // Debug.Log(velocityLeft);
            Vector2 tmpVelocity = rb.velocity;
            tmpVelocity.x = velocityLeft.x;
            rb.velocity = tmpVelocity;
           
            isHavingForce =true;
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(1500);
                isHavingForce = false;
                velocityLeft = Vector2.zero;
            }));
            thread.Start();
        }*/
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
     //   Debug.Log(rb.velocity.y);
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        

        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            

     /*       Vector3 velocity = rb.velocity;
            if (velocity.magnitude > 0.1f)
            {
                Vector3 oppositeForce = -velocity.normalized * velocity.magnitude * 1f;
                rb.AddForce(oppositeForce, ForceMode2D.Force);
            }

*/
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            // Debug.Log(localScale);


            /* localScale.y *= 5f;
             localScale.z *= 5f;
             rb.AddForce(localScale);*/

        }
    }

    private void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
    }

    private void UpdateAnimator()
    {
        if (horizontal != 0f && IsGrounded())
        {
            animator.SetBool("running", true);
        }
        else
        {
            animator.SetBool("running", false);
           
        }

        if (!IsGrounded())
        {
            if(rb.velocity.y > 0){
            animator.SetBool("jumping", true);
            }else{
                animator.SetBool("jumping", false);
            }

            if (rb.velocity.y < 0) {
                animator.SetBool("falling", true);
             
            }
            else
            {
                animator.SetBool("falling", false);
            }
        }
        else{
            animator.SetBool("falling", false);
        }
        
    }

    private void MovementBind()
    {
        
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {

            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            //      GetComponent<Rigidbody2D>().velocity = new Vector3(0,7,0);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower * -1);
        }
        if (Input.GetKey(KeyCode.R))
        {
            ResetVelocity();
            Vector2 tmpRespawnPoint = respawnPoint.position;
            tmpRespawnPoint.y += 1f;
            rb.position = tmpRespawnPoint;
        }
        if (Input.GetKey(KeyCode.E) && IsGrounded())
        {

            // Debug.Log(groundCheck.position);

            Vector2 tmp = groundCheck.position;
            tmp.x = groundCheck.position.x;
            respawnPoint.position = tmp;
        }

     //   Debug.Log(rb.velocity);
        Flip();
    }
}
