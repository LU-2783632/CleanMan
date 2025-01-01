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

    private BroomTool broomTool; // 声明 broomTool 变量
    void Start()
    {
        Initialize();

        Player = GameObject.FindWithTag("Player");
        Garbage = GameObject.FindWithTag("Garbage");

        if (Player == null)
        {
            Debug.LogWarning("Player not found.");
        }
        if (Garbage == null)
        {
            Debug.LogWarning("Garbage not found.");
        }
        if (Broom == null)
        {
            Debug.LogWarning("Broom not found.");
        }
        else
        {
            broomTool = Broom.GetComponent<BroomTool>();
            if (broomTool == null)
            {
                Debug.LogWarning("Broom does not have a BroomTool component.");
            }
        }
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

    public void Initialize()
    {
        // 初始化引用
        Player = GameObject.FindWithTag("Player");
        Garbage = GameObject.FindWithTag("Garbage");
        Broom = GameObject.FindWithTag("Broom");

        if (Player == null)
        {
            Debug.LogWarning("Player not found. Make sure there is a GameObject with the 'Player' tag.");
        }
        if (Garbage == null)
        {
            Debug.LogWarning("Garbage not found. Make sure there is a GameObject with the 'Garbage' tag.");
        }
        if (Broom == null)
        {
            Debug.LogWarning("Broom not found. Make sure there is a GameObject with the 'Broom' tag.");
        }

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            if (material.HasProperty("_Color"))
            {
                originalColor = material.color;
            }
            else
            {
                Debug.LogWarning("Material does not have a _Color property.");
            }
        }
        else
        {
            Debug.LogWarning("Renderer not found on this object.");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Broom == null)
        {
            Broom = GameObject.FindWithTag("Broom");
            if (Broom == null)
            {
                Debug.LogWarning("Broom is still not found.");
                return; // 等待下一帧再尝试
            }
            else
            {
                broomTool = Broom.GetComponent<BroomTool>();
                if (broomTool == null)
                {
                    Debug.LogWarning("Broom does not have a BroomTool component.");
                }
            }
        }

        if (Player == null || Garbage == null || material == null)
        {
            if (Player == null) Debug.LogWarning("Player is not initialized.");
            if (Garbage == null) Debug.LogWarning("Garbage is not initialized.");
            if (material == null) Debug.LogWarning("Material is not initialized.");
            return; // 等待下一帧再尝试
        }

        Clean(); ;
    }
    private void Clean()
    {
        if (broomTool == null)
        {
            Debug.LogWarning("BroomTool is null in Clean.");
            return;
        }

        if (broomTool.Ifhit && broomTool.hitobjctBroom != null)
        {
            if (this.gameObject == broomTool.hitobjctBroom)
            {
                Ifclean = broomTool.IfcleanBroom;
            }
        }

        if (Player == null)
        {
            Debug.LogWarning("Player is null in Clean.");
            return;
        }

        speed = Player.GetComponent<Breath>().Cleanspeed * broomSpeed;

        if (Ifclean)
        {
            Debug.Log($"Cleaning started for {gameObject.name}");

            alpha = Mathf.MoveTowards(alpha, 0.0f, speed * Time.deltaTime);
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            if (alpha <= 0.2f)
            {
                Debug.Log($"Leaf {gameObject.name} is fully cleaned.");
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log($"Leaf {gameObject.name} is not being cleaned.");
        }

        // 检查 Outline
        if (broomTool.hitobjctCamerahit != null && this.gameObject == broomTool.hitobjctCamerahit)
        {
            Outline outline = GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = (broomTool.hitobjctCamerahit == this.gameObject);
                Debug.Log($"Outline for {gameObject.name} is now: {outline.enabled}");
            }
            else
            {
                Debug.LogWarning($"Outline component is missing on {gameObject.name}");
            }
        }
        else
        {
            Outline outline = GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

}