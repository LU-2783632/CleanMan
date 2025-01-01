using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class BroomTool : MonoBehaviour
{
    public Camera camera;
    public Transform PalmL;
    public Transform PalmR;
    public GameObject midPoint;
    public GameObject CameraPoint;
    public float Speed;
    //public Animator animatorbroom;
    public GameObject hand;
    Animator animatorhand;
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
        //transform.parent = Player.transform;
        transform.parent = midPoint.transform ;
        animatorhand = hand.GetComponent<Animator>();
        animatorhand.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        midPoint.transform .position = (PalmL.position + PalmR.position )/2;
        Speed = Player.GetComponent<Breath>().Cleanspeed;
        //animatorbroom.speed = Speed;
        animatorhand.speed = Speed;
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
            //animatorbroom.SetBool("Ifbroom", true);
            animatorhand.SetBool("IfBroom", true);


        }
        else
        {
            // animatorbroom.SetBool("Ifbroom", false);
            animatorhand.SetBool("IfBroom", false);
        }
        //
        if (!hand.GetComponent<Animationstop>().Ifclean)
        {
            Ifclean = false;
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

            // 调整射线方向为斜向下
            Vector3 downForward = Quaternion.Euler(5f, 0, 0) * Vector3.forward;  // 绕 Z 轴旋转 45°，然后指向下方
            Debug.Log($"downForward direction: {downForward}");
            Ifhit = Physics.Raycast(Point.position, downForward, out hit, 2.5f, GoodsBroom);

            

            Vector3 downForward = Quaternion.Euler(5f, 0, 0) * Vector3.forward;  // 绕 Z 轴旋转 45°，然后指向下方
            Debug.Log($"downForward direction: {downForward}");
            Ifhit = Physics.Raycast(Point.position, downForward, out hit, 2.5f, GoodsBroom);
          
            if (Ifhit)
            {
                Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");
                hitobjctBroom = hit.collider.gameObject;
                Debug.Log($"Raycast hit: {hitobjctBroom.name}");

                if (Input.GetMouseButton(0))
                {
                    IfcleanBroom = true;
                    Debug.Log("Cleaning state activated.");
                }
                if (Input.GetMouseButtonUp(0))
                {
                    IfcleanBroom = false;
                    Debug.Log("Cleaning state deactivated.");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any object.");
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
        Vector3 downForward = Quaternion.Euler(5f, 0, 0) * CameraPoint.transform.forward;
        Gizmos.DrawRay(CameraPoint.transform.position, downForward * 5f);
        Gizmos.DrawRay(Point.position, downForward * 5f);
    }  

}
