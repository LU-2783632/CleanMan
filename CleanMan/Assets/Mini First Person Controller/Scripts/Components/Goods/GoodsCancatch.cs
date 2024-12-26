using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class GoodsCancatch : MonoBehaviour
{
    public bool Ifmove = false;
    public float speed;
    Vector3 Originposition;
    Quaternion Originrotation;
    public GameObject target;
    public bool Hasmoved = false;
    private GameObject Player;
    private GameObject Hand;
    private GameObject Garbage;
    private bool Ifhit;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Garbage= GameObject.FindWithTag("Garbage");
        Hand = GameObject.FindWithTag("Hand");
        Originposition = transform.position;
        Originrotation = transform.rotation ;
        target.transform .parent   = null;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move()
    {
        Ifhit = Hand.GetComponent<HandTool>().Ifhit;
        speed = Player.GetComponent<Breath>().Cleanspeed;
        //outline
        if(Ifhit && this.gameObject == Hand.GetComponent<HandTool>().hitobjctHand)
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
        else 
        {
            if(!Ifmove)
            {
                gameObject.GetComponent<Outline>().enabled = false;
            }
            else
            {
                gameObject.GetComponent<Outline>().enabled = true;
            }
        }
        //Move
        if (Hasmoved)
        {
            if(this.gameObject.layer == LayerMask.NameToLayer("GoodsCancatch"))
            {
                Garbage.GetComponent<Garbage>().Ifclean = true;
            }
            gameObject.GetComponent<Outline>().enabled = false;
            this.gameObject .layer = LayerMask.NameToLayer("GoodsNotcatch");
            target.SetActive(false);
        }
        else
        {
            if (Ifmove)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform .position, speed  * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, target.transform .rotation, speed  * Time.deltaTime);
                target.SetActive(true);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Originposition, 4f * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Originrotation, 3f * Time.deltaTime);
                target.SetActive(false);
            }
            if ( Vector3.Distance(transform.position, target.transform .position ) < 0.1f)
            {
                Hasmoved = true;
            }
        }
    }
}
