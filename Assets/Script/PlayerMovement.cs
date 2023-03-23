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
    private bool isAttacking;
    private Vector3 baseWeaponPosition;
    [SerializeField] private SpriteRenderer weapon;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
      


    }

    void Update()
    {
      //  Debug.Log(weapon.rotation);
        MovementBind();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        // Debug.Log(horizontal);
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

        //if attacking then stop
        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            return;
        }
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
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
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
        if (isAttacking)
        {
            animator.SetBool("attacking", true);
            weaponAnimator.SetBool("attacking", true);
            animator.SetBool("running", false);
            /*Quaternion rotate = weapon.rotation;
            Vector2 position = weapon.position;

            if (isFacingRight)
            {
                position.x = -3f;
                rotate.z = -0.5f;
                weapon.rotation = rotate;
            }
            else
            {
                position.x = 1f;
                rotate.z = 0.5f;
                weapon.rotation = rotate;
            }
            // position.y = -1.3f;
            weapon.position = position;
             Debug.Log(position);*/
            
        }
        else
        {
            animator.SetBool("attacking", false);
            weaponAnimator.SetBool("attacking", false);
            /* Quaternion currentPos = weapon.rotation;
             currentPos.z = 0.0f;
             weapon.rotation = currentPos;
 */
            // weapon.position = basePosWeapon;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //allow pass thru mob
        if (collision.gameObject.name.Contains("mob"))
        {
            Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), collision.collider);
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
        if (Input.GetKey(KeyCode.Space) && IsGrounded() && !isAttacking )
        {
            Vector3 tmpPos = weaponTransform.position;
          
            if (isFacingRight)
            {
                tmpPos.x += 0.5f;
            }
            else
            {
                tmpPos.x -= 0.5f;
            }
            tmpPos.y -= 0.5f;
            weaponTransform.position = tmpPos;
            isAttacking = true;
            weapon.sortingLayerName = "Front";
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isAttacking = false;
            weapon.sortingLayerName = "Back";
            weaponTransform.position = rb.position;
        }
      //  Debug.Log(weaponTransform.position);
        Flip();
    }
}
