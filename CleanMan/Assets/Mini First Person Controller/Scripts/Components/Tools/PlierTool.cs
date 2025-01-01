using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlierTool : MonoBehaviour
{
    public Camera camera;
    public Transform Palm;
    public GameObject CameraPoint;
    public float Speed;
    public GameObject Player;
    public bool Ifclean = false;
    public bool IfcleanPlier = false;
    public LayerMask GoodsPlier;
    public GameObject hitobjctPlier;
    public bool Ifhit = false;
    public GameObject hand;
    Animator animatorhand;
    // Update is called once per frame
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        transform.parent = Palm.transform;
        animatorhand = hand.GetComponent<Animator>();
        animatorhand.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        Speed = Player.GetComponent<Breath>().Cleanspeed;
        animatorhand.speed = Speed;
        Move();
        Check();
    }
    private void Move()
    {
        if (!Player.GetComponent<ChangetTools>().IfChange)
        {
            if (Input.GetMouseButton(0))
            {
                Ifclean = true;
            }
        }
        if (Ifclean)
        {
            animatorhand.SetBool("IfTowel", true);
        }
        else
        {
            animatorhand.SetBool("IfTowel", false);
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
            IfcleanPlier = false;
            hitobjctPlier = null;
        }
        RaycastHit hit;
        Ifhit = Physics.Raycast(CameraPoint.transform.position, CameraPoint.transform.forward, out hit, 4f, GoodsPlier);
        if (Ifhit)
        {
            hitobjctPlier = hit.collider.gameObject;
            if (Input.GetMouseButton(0))
            {
                IfcleanPlier = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                IfcleanPlier = false;
            }
        }
        else
        {
            IfcleanPlier = false;
            hitobjctPlier = null;
        }
    }
}
