using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    private bool attacking = false;
    private HingeJoint2D hingeJoint;
    private Animator animator;
    [SerializeField] private float motorTorque = 10f;
    [SerializeField] private Rigidbody2D rb;

    public bool getAttacking()
    {
        return attacking;
    }
    public void setAttacking(bool newState)
    {
        animator.SetBool("attacking",newState);
        attacking = newState;
    }
    // Start is called before the first frame update
    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
        hingeJoint.useMotor = true;
        hingeJoint.motor = new JointMotor2D { motorSpeed = -motorTorque, maxMotorTorque = motorTorque };
        hingeJoint.useLimits = true;
        hingeJoint.limits = new JointAngleLimits2D { min = -45f, max = 45f };

        animator = GetComponent<Animator>();
        animator.SetBool("attacking", false);
    }
   


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //allow pass thru mob
        /*      if (collision.gameObject.name.Contains("mob"))
              {
                  Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), collision.collider);
              }*/
       
        if (collision.gameObject.name.Contains("player"))
        {
            Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), collision.collider);
        }
 /*       if ((collision.gameObject.name.Contains("mob")))
        {

           // Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), collision.collider);
            Debug.Log(collision.gameObject.name + " hihi");
        }*/



    }


    private void OnTriggerStay2D(Collider2D other)
    {
       
        if (other.gameObject.CompareTag("Mob")&& attacking)
        {
            GameEvents gameEvents = new GameEvents();
            gameEvents.addEvent(new EventHit(1, 1, 8,0.2f));
            other.gameObject.SendMessage("HandleEvent",gameEvents);
           // Debug.Log("hitting mob id"+ other.gameObject.GetInstanceID());
            /*          
                       Animator wpanimator = GetComponent<Animator>();

                       if (wpanimator.GetBool("attacking"))
                       {
                          // other.SendMessage
                           *//*Animator mobAnimation =other.GetComponent<Animator>();
                           mobAnimation.SetBool("hit",true);
                           Debug.Log("hitting mob");*//*
                       }*/
        } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        /*if (other.gameObject.CompareTag("Mob"))
        {
          //  Debug.Log("setting animation");
            Animator mobAnimation = other.GetComponent<Animator>();
            mobAnimation.SetBool("hit", false);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(collision.gameObject.name);
    }
 



    // Update is called once per frame
    void Update()
    {
/*        Vector3 vel = rb.velocity;
        if (vel.x !=0 || vel.y !=0)
        {
            vel.x = 0;
            vel.y = 0;
            rb.velocity = vel;
        }*/
       // Debug.Log("update");
    }
    private void LateUpdate()
    {
        
    }
   
}
