using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class HandTool : MonoBehaviour
{
    public Camera camera;
    public GameObject CameraPoint;
    private GameObject Player;
    public float Speed;
    public LayerMask Goodscancatch;
    public GameObject hitobjctHand;
    private bool Ifhandmove = false;
    public bool Ifhit;
    public GameObject hand;
    Animator animatorhand;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        transform.parent = camera.transform;
        animatorhand = hand.GetComponent<Animator>();
        animatorhand.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        Speed = Player.GetComponent<Breath>().Cleanspeed;
        animatorhand.speed = Speed;
        Hand();
    }

    private void Hand()
    {
        //Check If Changetools
        if (Player.GetComponent<ChangetTools>().IfChange)
        {
            Ifhandmove = false;
            hitobjctHand = null;
        }
        else
        {
            RaycastHit hit;
            Ifhit = Physics.Raycast(CameraPoint.transform.position, CameraPoint.transform.forward, out hit, 4f, Goodscancatch);
            if (Ifhit)
            {
                Ifhandmove = true;
                hitobjctHand = hit.collider.gameObject;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ifhandmove = false;
                    hitobjctHand = null;
                }
            }
        }
        if (Ifhandmove)
        {
            hitobjctHand.GetComponent<GoodsCancatch>().enabled = true;
            if (Input.GetMouseButtonDown(0))
            {
                hitobjctHand.GetComponent<GoodsCancatch>().Ifmove = true;
                animatorhand.SetBool("IfHand", true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                hitobjctHand.GetComponent<GoodsCancatch>().Ifmove = false;
                animatorhand.SetBool("IfHand", false);
            }
        }
        if (hitobjctHand != null && hitobjctHand.GetComponent<GoodsCancatch>().Hasmoved)
        {
            Ifhandmove = false;
            hitobjctHand = null;
            animatorhand.SetBool("IfHand", false);
        }
    }

}


