using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class GoodswithPlier : MonoBehaviour
{
    public float speed;
    public float plierSpeed = 0.2f;
    public GameObject Player;
    public GameObject Plier;
    public bool Ifclean = false;
    private Vector3 originalScale;
    public Vector3 targetScale = new Vector3 (0.1f,0.1f,0.1f);
    public GameObject  PlierPoint;
    private GameObject Garbage;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        PlierPoint = GameObject.FindWithTag("PlierPoint");
        Garbage = GameObject.FindWithTag("Garbage");
        PlierPoint.transform .parent  = Player.transform;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Plier = GameObject.FindWithTag("Plier");
        Clean();
    }
    private void Clean()
    {
        if (Plier != null && Plier.GetComponent<PlierTool>().Ifhit)
        {
            if (this.gameObject == Plier.GetComponent<PlierTool>().hitobjctPlier)
            {
                gameObject.GetComponent<Outline>().enabled = true;
                Ifclean = Plier.GetComponent<PlierTool>().IfcleanPlier;
            }
        }
        else
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
        speed = Player.GetComponent<Breath>().Cleanspeed * plierSpeed;
        
        if (Ifclean)
        {
            transform.localScale -= Vector3.one * speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlierPoint.transform .position , speed * 3f * Time.deltaTime);
        }
        if (transform.localScale.x <= targetScale.x )
        {
            Garbage.GetComponent<Garbage>().Ifclean = true;
            Destroy(gameObject);
        }

    }
}
