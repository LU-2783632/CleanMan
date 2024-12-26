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
    public LayerMask Goodscancatch;
    public GameObject hitobjctHand;
    private bool Ifhandmove = false;
    public bool Ifhit;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        transform.parent = camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
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
            }
            else if (Input.GetMouseButtonUp(0))
            {
                hitobjctHand.GetComponent<GoodsCancatch>().Ifmove = false;
            }
        }
        if (hitobjctHand != null && hitobjctHand.GetComponent<GoodsCancatch>().Hasmoved)
        {
            Ifhandmove = false;
            hitobjctHand = null;
        }
    }

}


