using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Garbage : MonoBehaviour
{
    public int Garbagequantity = 0;
    public bool Ifclean = false;
    public TextMeshProUGUI Quantitytext;
    public int LevelGarbage; //本关所需要的垃圾数量
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quantity();

        //level
        if(Garbagequantity >= LevelGarbage)
        {
            LoadNextLevel();
        }
    }
    public void Quantity()
    {
        if (Ifclean)
        {
            Garbagequantity++;
            Ifclean = false;
        }
        Quantitytext.text  = Garbagequantity.ToString();
    }
    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // current level
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex); // next level
        }
        else
        {
            
        }
    }
}
