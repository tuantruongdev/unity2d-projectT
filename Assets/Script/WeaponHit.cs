using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //allow pass thru mob
        /*      if (collision.gameObject.name.Contains("mob"))
              {
                  Physics2D.IgnoreCollision(rb.GetComponent<Collider2D>(), collision.collider);
              }*/
        Debug.Log(collision.gameObject.name);


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
