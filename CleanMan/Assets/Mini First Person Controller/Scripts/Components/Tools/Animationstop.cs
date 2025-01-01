using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animationstop : MonoBehaviour
{
    public bool Ifclean;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnAnimationEvent()
    {
        Ifclean = false;
    }
}
