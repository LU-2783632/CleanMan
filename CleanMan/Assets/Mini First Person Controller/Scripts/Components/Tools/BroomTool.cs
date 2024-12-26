using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomTool : MonoBehaviour
{
    public Camera camera;
    public GameObject CameraPoint;
    public float Speed;
    public Animator animator;
    public GameObject Player;
    public bool Ifclean = false;
    public bool IfcleanBroom = false;
    public LayerMask GoodsBroom;
    public GameObject hitobjctBroom;
    public GameObject hitobjctCamerahit;
    public bool Ifhit = false;
    public bool Camerahit = false;
    public Transform Point;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        transform.parent = Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Speed = Player.GetComponent<Breath>().Cleanspeed;
        animator.speed = Speed;
        Move();
        Check();
        CameraCheck();
    }
    private void Move()
    {
        if(!Player.GetComponent<ChangetTools>().IfChange)
        {
            if (Input.GetMouseButton(0))
            {
                Ifclean = true;
            }
        }
        if (Ifclean)
        {

            animator.enabled = true;
        }
        else
        {
            animator.enabled = false;
        }
    }
    
    public void Check()
    {
        //Check If Changetools
        if (Player.GetComponent<ChangetTools>().IfChange)
        {
            IfcleanBroom = false;
            hitobjctBroom = null;
        }
        else
        {
            RaycastHit hit;
            Ifhit = Physics.Raycast(Point.position, Point.forward, out hit, 1.5f, GoodsBroom);
            if (Ifhit)
            {
                hitobjctBroom = hit.collider.gameObject;
                if (Input.GetMouseButton(0))
                {
                    IfcleanBroom = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    IfcleanBroom = false;
                }
            }
            else
            {
                IfcleanBroom = false;
                hitobjctBroom = null;
            }
        }
    }
    public void CameraCheck()
    {
        if (Player.GetComponent<ChangetTools>().IfChange) return;
        RaycastHit hit;
        Camerahit = Physics.Raycast(CameraPoint.transform.position, CameraPoint.transform.forward, out hit, 5f, GoodsBroom);
        if (Camerahit)
        {
            hitobjctCamerahit = hit.collider.gameObject;
        }
        else
        {
            hitobjctCamerahit = null;
        }
    }
    public void OnAnimationEvent()
    {
        Ifclean = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(CameraPoint.transform.position, CameraPoint.transform.forward * 5f);
    }

}
