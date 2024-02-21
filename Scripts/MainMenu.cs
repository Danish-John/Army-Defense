using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{

    public DOTweenAnimation SettingsPanel;


    // Start is called before the first frame update
    void Start()
    {
        
    }




    public void SettingsButtonClicked()
    {
        SettingsPanel.DORestart();
    }
}
