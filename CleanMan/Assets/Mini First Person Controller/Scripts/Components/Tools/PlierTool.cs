using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlierTool : MonoBehaviour
{
    public Camera camera;
    public GameObject CameraPoint;
    public float Speed;
    public Animator animator;
    public Animation animation;
    public GameObject Player;
    public bool Ifclean = false;
    public bool IfcleanPlier = false;
    public LayerMask GoodsPlier;
    public GameObject hitobjctPlier;
    public bool Ifhit = false;
    // Update is called once per frame
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        transform.parent = camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Speed = Player.GetComponent<Breath>().Cleanspeed;
        animator.speed = Speed;
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
    public void OnAnimationEvent()
    {
        Ifclean = false;
    }
}
