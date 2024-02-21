using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceUpgrade : MonoBehaviour
{
    public GameObject BlackBG;

    public GameObject Hand;

    //public Material BarrierMaterial;

    

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("LevelFailOnce") == 1 && PlayerPrefs.GetInt("LevelNumber") > 0)
        {
            BlackBG.SetActive(true);
            Hand.SetActive(true);
            //BarrierMaterial.color = Color.black;

        }
    }

    
    public void RemoveForceUpgradation()
    {
        if (PlayerPrefs.GetInt("LevelFailOnce") == 1)
        {
            Hand.SetActive(false);
            BlackBG.SetActive(false);
            //BarrierMaterial.color = new Color(221,221,221);
         
        }
        PlayerPrefs.SetInt("LevelFailOnce", 2);
    }


}
