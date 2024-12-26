using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodswithTowel : MonoBehaviour
{
    public float speed;
    public float towelSpeed = 0.2f;
    public GameObject Player;
    public GameObject Towel;
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
        Towel = GameObject.FindWithTag("Towel");
        Clean();
    }
    private void Clean()
    {
        if (Towel != null && Towel.GetComponent<TowelTool>().Ifhit)
        {
            if (this.gameObject == Towel.GetComponent<TowelTool>().hitobjctTowel)
            {
                gameObject.GetComponent<Outline>().enabled = true;
                Ifclean = Towel.GetComponent<TowelTool>().IfcleanTowel;
            }
        }
        else
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
        speed = Player.GetComponent<Breath>().Cleanspeed * towelSpeed;

        if (!Ifclean) return;
        alpha = Mathf.MoveTowards(alpha, 0.0f, speed * Time.deltaTime);
        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        if (alpha <= 0.1f)
        {
            Garbage.GetComponent<Garbage>().Ifclean = true;
            gameObject.SetActive(false);
        }
    }

}
