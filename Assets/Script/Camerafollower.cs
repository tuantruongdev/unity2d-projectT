using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Camerafollower : MonoBehaviour
{
   
    [SerializeField] private Vector3 offset;
    [Range(0.1f,1f)]
    public float damping;
    [SerializeField] private Rigidbody2D rb;

    private Vector3 vel = Vector3.zero;
    private bool zoomIn = false;
    private float waitCounter = 0;
    [Range(0.01f, 0.1f)]
    public float zoomSpeed = 0.05f;  
    [Range(0, 1f)]
    public float aspectWithSpeed = 0.00f;
    [Range(1,3)]
    public float waitTime = 1;
    [Range(1,20)]
    public float baseCameraSize = 6f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {
        CameraWithSpeed();
        CameraFollowPlayer();
    }

    private void FixedUpdate()
    {
      
    }

    private void SmoothZoom(float form, float to, float speed)
    {
        Camera.main.orthographicSize = Mathf.Lerp(form, to, speed);
    }

    private void CameraWithSpeed()
    {
        if (Mathf.Abs(rb.velocity.magnitude) < 7)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter > waitTime)
            {
                zoomIn = true;
            }
        }
        else
        {
            zoomIn = false;
            waitCounter = 0;

        }
        if (zoomIn)
        {
            SmoothZoom(Camera.main.orthographicSize, baseCameraSize, zoomSpeed);
        }
        else
        {
            Vector2 vel = Vector2.zero;
            vel.x = Mathf.Abs(rb.velocity.x);
            vel.x *= aspectWithSpeed;
            SmoothZoom(Camera.main.orthographicSize, baseCameraSize + vel.x, zoomSpeed);
        }
    }
    private void CameraFollowPlayer()
    {
        Vector3 postion2d = rb.position;
        postion2d.z = 0f;
        Vector3 targetPosition = postion2d + offset;
        targetPosition.z = Camera.main.gameObject.transform.position.z;
      /*  UnityEngine.Debug.Log("current" + transform.position + " target:" + targetPosition + " vel:" + vel + " damp:" + damping);
        if(vel.x > 8f)
        {
            vel.x = 8f;
        } 
        if(vel.x < -8f)
        {
            vel.x = -8f;
        }
        if(vel.y > 8f)
        {
            vel.y = 8f;
        } 
        if(vel.y < -8f)
        {
            vel.y = -8f;
        }
*/
        Camera.main.gameObject.transform.position = Vector3.SmoothDamp(transform.position,  targetPosition, ref vel, damping);
    }


}