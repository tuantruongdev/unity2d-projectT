using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public class MobMovement : MonoBehaviour
{
    [Range(1f, 10f)]
    private float mobRange = 9f;
    [Range(1f, 100f)]


    public float totalTime = 0f; // total time for the countdown in seconds
    private float startTime; // time left in the countdown
    private  const float MAX_MOB_HP = 500f;
    private float mobHp = MAX_MOB_HP;
    private float horizontal = 1f;
    private float speed = 3f;
    //  private float jumpingPower = 20f;
    private bool isHit = false;
    private bool isDead = false;
    private bool isFacingRight = true;
    private Vector2 spawnPos;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform currentHealthBar;
    [SerializeField] private Transform baseHealthBar;
    [SerializeField] private SpriteRenderer currentHealthBarSprite;
    private Color baseHealthbarColor;
    private Thread animThread;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        spawnPos = groundCheck.position;
        UpdateHealthBarAnimator();
        baseHealthbarColor = currentHealthBarSprite.color;
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
        UpdateHitAnimator();
    }

    private void LateUpdate()
    {
        updateHealth();
    }

    void updateHealth()
    {
        if (this.mobHp < 0 && !isDead)
        {

            isDead = true;
            gameObject.SetActive(false);
            //   Debug.Log("mob is dead");
        }
        else
        {
            isDead = false;
        }
    }

    public void HandleEvent(GameEvents gameEvents)
    {
        foreach (Event e in gameEvents.getEvents())
        {
            if (e.eventId == 1 && e is EventHit)
            {
                EventHit eventHit = (EventHit)e;
                this.mobHp -= eventHit.damage;
                //updateHitAnimation();
                //Debug.Log("hitting"+Time.time);
                isHit = true;
                startTime = Time.time;
                totalTime = eventHit.freezeTime;
               
                Debug.Log(totalTime);
            }
        }
    }

    private void updateHitAnimation()
    {

        

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
            if (groundCheck.position.x - spawnPos.x >= mobRange)
            {
                // Debug.Log(groundCheck.position.x - spawnPos.x +" R= "+ mobRange);
                return true;
            }
            return false;
        }
        else
        {
            if (spawnPos.x - groundCheck.position.x >= mobRange)
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
            tmpGround.x += 1f;
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

        /*     if (!IsReady())
             {

                 animator.SetBool("hit", true);
                 // Debug.Log("stopping");
                 rb.velocity = Vector3.zero;
             }
             else
             {
                 animator.SetBool("hit", false);
             }*/

        if (isHit)
        {
            animator.SetBool("hit", true);
            UpdateHealthBarAnimator();
            rb.velocity = Vector2.zero;
        }
        else
        {
            animator.SetBool("hit", false);
        }


      

    }

    private void UpdateHitAnimator()
    {
        float elapsedTime = Time.time - startTime; // get the elapsed time
        float timeLeft = Mathf.Clamp(totalTime - elapsedTime, 0f, totalTime); // calculate the time left, clamped to zero
                                                                              //  Debug.Log(elapsedTime+"|" +timeLeft+"|"+ totalTime);
        if (timeLeft <= 0f)
        {
            // countdown is over, do something
            timeLeft = 0f; // clamp time left to zero
            isHit = false;
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

    private void UpdateHealthBarAnimator()
    {
        if(mobHp == MAX_MOB_HP)
        {
            currentHealthBar.gameObject.SetActive(false);
            baseHealthBar.gameObject.SetActive(false);
            return;
        }
       else {
            Color healthBarColor = baseHealthbarColor;
            currentHealthBar.gameObject.SetActive(true);
            baseHealthBar.gameObject.SetActive(true);
            Vector3 currentLocalScale = currentHealthBar.localScale;
            Vector3 maxLocalScale = baseHealthBar.localScale;
            float diff = mobHp / MAX_MOB_HP;
            Debug.Log(diff);
            healthBarColor.g *= diff;
            healthBarColor.b *= diff;
            currentHealthBarSprite.color = healthBarColor;
            if (currentLocalScale.x > 0f)
            {
                currentLocalScale.x = (maxLocalScale.x * diff);
            }
            currentHealthBar.localScale = new Vector3(currentLocalScale.x, currentLocalScale.y, currentHealthBar.localScale.z);
        }


       
       // currentHealthBar.wi
    }

}
