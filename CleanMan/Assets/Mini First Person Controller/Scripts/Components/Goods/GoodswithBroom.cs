using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodswithBroom : MonoBehaviour
{
    public float speed;
    public float broomSpeed = 0.2f;
    public GameObject Player;
    public GameObject Broom;
    public bool Ifclean = false;
    private Material material;
    private Color originalColor;
    private float alpha = 1f;
    private GameObject Garbage;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Garbage = GameObject.FindWithTag("Garbage");
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            if (material.HasProperty("_Color"))
            {
                originalColor = material.color;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        Broom = GameObject.FindWithTag("Broom");
        Clean();
    }
    private void Clean()
    {
        if (Broom !=null && Broom.GetComponent<BroomTool>().Ifhit)
        {
            if (this.gameObject == Broom.GetComponent<BroomTool>().hitobjctBroom)
            {
                Ifclean = Broom.GetComponent<BroomTool>().IfcleanBroom;
            }
        }
        speed = Player.GetComponent<Breath>().Cleanspeed * broomSpeed;

        if (Ifclean)
        {
            alpha = Mathf.MoveTowards(alpha, 0.0f, speed * Time.deltaTime);
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }
        if (alpha <= 0.2f)
        {
            Garbage.GetComponent<Garbage>().Ifclean = true;
            gameObject.SetActive(false);
        }
        //outline
        if (Broom != null && Broom.GetComponent<BroomTool>().hitobjctCamerahit != null 
            && this.gameObject == Broom.GetComponent<BroomTool>().hitobjctCamerahit)
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }

    }

}