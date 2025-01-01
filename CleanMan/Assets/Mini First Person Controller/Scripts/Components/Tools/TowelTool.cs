using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowelTool : MonoBehaviour
{
    public Camera camera;
    public GameObject CameraPoint;
    public Animator animator;
    public float Speed;
    public GameObject Player;
    public bool Ifclean = false;
    public bool IfcleanTowel = false;
    public LayerMask GoodsTowel;
    public GameObject hitobjctTowel;
    public bool Ifhit = false;
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
            IfcleanTowel = false;
            hitobjctTowel = null;
        }
        else
        {
            RaycastHit hit;
            Ifhit = Physics.Raycast(CameraPoint.transform.position, CameraPoint.transform.forward, out hit, 4f, GoodsTowel);
            if (Ifhit)
            {
                hitobjctTowel = hit.collider.gameObject;
                if (Input.GetMouseButton(0))
                {
                    IfcleanTowel = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    IfcleanTowel = false;
                }
            }
            else
            {
                IfcleanTowel = false;
                hitobjctTowel = null;
            }
        }
    }
    public void OnAnimationEvent()
    {
        Ifclean = false;
    }

}
    

