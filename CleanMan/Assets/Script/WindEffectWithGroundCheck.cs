using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffectWithGroundCheck : MonoBehaviour
{
    public WindZone windZone; 
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (windZone == null)
        {
            Debug.LogError("WindZone is not assigned!");
        }
    }

    void FixedUpdate()
    {
        if (windZone != null)
        {
            Vector3 windDirection = windZone.transform.forward; // 风向
            float windStrength = windZone.windMain;             // 主风力
            float turbulence = windZone.windTurbulence;         // 风的随机性

         
            Vector3 randomTurbulence = new Vector3(
                Random.Range(-turbulence, turbulence),
                Random.Range(0, turbulence), 
                Random.Range(-turbulence, turbulence)
            );

            Vector3 windForce = (windDirection + randomTurbulence) * windStrength;

           
            if (IsGrounded())
            {
           
                windForce.y = Mathf.Max(0.5f, windForce.y);
            }

          
            rb.AddForce(windForce, ForceMode.Force);
        }
    }

  
    private bool IsGrounded()
    {
        RaycastHit hit;
      
        return Physics.Raycast(transform.position, Vector3.down, out hit, 0.6f);
    }

    
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
           
            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            }
        }
    }
}
