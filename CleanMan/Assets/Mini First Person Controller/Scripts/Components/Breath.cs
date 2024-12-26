using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Breath : MonoBehaviour
{
    public Transform backgroundTransform;
    public Transform FillTransform;
    public Slider progressBar;
    public float FianlSpeed = 20f;
    public float FallSpeed = 20f;
    public float Cleanspeed;
    public float breathtime = 0.0f;
    bool Ifbreathin = false;
    private Image backgroundImage;
    private Image FillImage;
    private Color startColor;
    private float transitionSpeed = 2.0f;
    public AudioSource HeartAudio;
    private float progressSpeed;

    void Start()
    {
        Cleanspeed = 1f;
        breathtime = 5f;
        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = 100;
            progressBar.value = 50;
        }
        if (backgroundTransform != null)
        {
            backgroundImage = backgroundTransform.GetComponent<Image>();
            startColor = backgroundImage.color;
        }
        if (FillTransform != null)
        {
            FillImage = FillTransform.GetComponent<Image>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (FillTransform != null && backgroundTransform != null)
        {
            FillImage.color = backgroundImage.color;
        }
        Breathfunction();
        Concerntration();
    }
    private void Breathfunction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            breathtime = 2f;
            Ifbreathin = true;
            progressSpeed = FianlSpeed;

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            breathtime = 8f;
            Ifbreathin = false;
            progressSpeed = FianlSpeed;
        }
        if (Ifbreathin)
        {
            if (progressBar != null)
            {
                progressSpeed -= Time.deltaTime * FallSpeed;
                progressSpeed = Mathf.Clamp(progressSpeed, 10, FianlSpeed);
                progressBar.value += progressSpeed * Time.deltaTime;
                breathtime += Time.deltaTime * 0.8f;
                breathtime = Mathf.Clamp(breathtime, 0, 10);

                if (progressBar.value >= progressBar.maxValue)
                {
                    breathtime += Time.deltaTime * 3f;
                    breathtime = Mathf.Clamp(breathtime, 0, 10);
                }
            }
        }
        else
        {
            if (progressBar != null)
            {
                progressSpeed -= Time.deltaTime * FallSpeed;
                progressSpeed = Mathf.Clamp(progressSpeed, 10, FianlSpeed);
                progressBar.value -= progressSpeed * Time.deltaTime;
                breathtime -= Time.deltaTime * 0.8f;
                breathtime = Mathf.Clamp(breathtime, 0, 10);


                if (progressBar.value <= progressBar.minValue)
                {
                    breathtime -= Time.deltaTime * 3f;
                    breathtime = Mathf.Clamp(breathtime, 0, 10);
                }
            }
        }

    }
    public void Concerntration()
    {
        if(breathtime <=1 || breathtime > 9)
        {
            if (HeartAudio != null)
            {
                HeartAudio.pitch = 1.8f;
                HeartAudio.volume = 0.3f;
            }
            if (backgroundImage != null)
            {
                backgroundImage.color = Color.Lerp(backgroundImage.color,
                    Color.red, Time.deltaTime * transitionSpeed);
            }
            //Clean
            Cleanspeed -= Time.deltaTime;
            Cleanspeed = Mathf.Clamp(Cleanspeed, 0.2f, 2f);
        }
        else
        {
            if (HeartAudio != null)
            {
                HeartAudio.pitch = 0f;
                HeartAudio.volume = 0f;
            }
            if (backgroundImage != null)
            {
                backgroundImage.color = Color.Lerp(backgroundImage.color,
                    startColor, Time.deltaTime * transitionSpeed);
            }
            //Clean
            Cleanspeed += Time.deltaTime;
            Cleanspeed = Mathf.Clamp(Cleanspeed, 0.2f, 2f);
        }
    }

}
