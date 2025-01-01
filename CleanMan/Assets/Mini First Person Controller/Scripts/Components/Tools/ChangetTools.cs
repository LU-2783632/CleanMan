using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ChangetTools : MonoBehaviour
{
    public GameObject tools;
    public GameObject Hand;
    public GameObject Broom;
    public GameObject Plier;
    public GameObject Towel;
    public bool IfChange = false;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        tools.SetActive(false);
        Handtool();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeTools();
    }
    private void ChangeTools()
    {
        if (Input.GetMouseButtonUp(1) && i == 0)
        {
            IfChange = true;
            tools.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        else if  (Input.GetMouseButtonDown(0))
        {
            Invoke("Changed", 0.2f);
            Invoke("Renew", 0.2f);
        }
        if(IfChange)
        {
            if (Input.GetMouseButtonDown(1))
            {
                i = 1;
            }
            if (i == 1)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    IfChange = false;
                    tools.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    i = 0;
                }
            }

            
        }
    }
    private void Changed()
    {
        IfChange = false;
    }
    private void Renew()
    {
        tools.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Handtool()
    {
        Invoke("IsHand", 0.1f);

    }
    private void IsHand()
    {
        Hand.SetActive(true);
        Towel.SetActive(false);
        Plier.SetActive(false);
        Broom.SetActive(false);
    }
    public void Toweltool()
    {
        Invoke("IsTowel", 0.1f);
    }
    private void IsTowel()
    {
        Towel.SetActive(true);
        Hand.SetActive(false);
        Plier.SetActive(false);
        Broom.SetActive(false);
    }
    public void Pliertool()
    {
        Invoke("IsPlier", 0.1f);
    }
    private void IsPlier()
    {
        Plier.SetActive(true);
        Hand.SetActive(false);
        Towel.SetActive(false);
        Broom.SetActive(false);

    }
    public void Broomtool()
    {
        Invoke("IsBroom", 0.1f);
    }
    private void IsBroom()
    {
        Broom.SetActive(true);
        Hand.SetActive(false);
        Towel.SetActive(false);
        Plier.SetActive(false);
    }

}
