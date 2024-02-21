using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Text CoinText;

    public Text LevelNumber;


    public Text BarrierPriceText;

    public Text PolicePriceText;

    public Text CanonPriceText;


    int PoliceUpgradeNumber;

    int BarrierUpgradeNumber;

    int CanonUpgradeNumber;


    public Button PoliceUpgradeBtn;

    public Button BarrierUpgradeBtn;

    public Button CanonUpgradeBtn;


    public GameObject[] PolicePrefabs;

    public GameObject[] BarrierPrefabs;

    public GameObject[] CanonPrefabs;


    public int[] PolicePrice;

    public int[] BarrierPrice;

    public int[] CanonPrice;


    public GameObject[] StoreBarrierPrefabs;

    public GameObject[] StorePolicePrefabs;

    public GameObject[] StoreCanonPrefabs;


    public GameObject PolicePrefab;

    public GameObject BarrierPrefab;

    public GameObject CanonPrefab;


    public AudioSource PurchaseAudio;

    public AudioSource LowCoinsAudio;




    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        int currentLevel = PlayerPrefs.GetInt("LevelNumber") + 1;
        LevelNumber.text = "Level " + currentLevel.ToString();

        int coins = PlayerPrefs.GetInt("Coins", 500);
        PlayerPrefs.SetInt("Coins", coins);
        
        CoinText.text = coins.ToString();

        PoliceUpgradeNumber = PlayerPrefs.GetInt("PoliceUpgradeNumber",0);
        BarrierUpgradeNumber = PlayerPrefs.GetInt("BarrierUpgradeNumber", 0);
        CanonUpgradeNumber = PlayerPrefs.GetInt("CanonUpgradeNumber", 0);



        PolicePrefab = PolicePrefabs[PoliceUpgradeNumber];
        BarrierPrefab = BarrierPrefabs[BarrierUpgradeNumber];
        CanonPrefab = CanonPrefabs[CanonUpgradeNumber];


        StoreBarrierPrefabs[BarrierUpgradeNumber].SetActive(true);
        StorePolicePrefabs[PoliceUpgradeNumber].SetActive(true);
        StoreCanonPrefabs[CanonUpgradeNumber].SetActive(true);


        UpdatePrice();
        CheckUpgradation();

        SetPolicePrefab(PoliceUpgradeNumber);
        SetBarrierPrefab(BarrierUpgradeNumber);
        SetCanonPrefab(CanonUpgradeNumber);

    }

    void UpdatePrice()
    {

        BarrierPriceText.text = BarrierPrice[PlayerPrefs.GetInt("BarrierUpgradeNumber", 0)].ToString();
        PolicePriceText.text = PolicePrice[PlayerPrefs.GetInt("PoliceUpgradeNumber", 0)].ToString();
        CanonPriceText.text = CanonPrice[PlayerPrefs.GetInt("CanonUpgradeNumber", 0)].ToString();
    }


    void CheckUpgradation()
    {
        if (PoliceUpgradeNumber == 2)
        {
            PoliceUpgradeBtn.interactable = false;
            PoliceUpgradeBtn.transform.GetChild(0).GetComponent<Text>().text = "Max Level !";
            //PoliceUpgradeBtn.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (BarrierUpgradeNumber == 2)
        {
            BarrierUpgradeBtn.interactable = false;
            BarrierUpgradeBtn.transform.GetChild(0).GetComponent<Text>().text = "Max Level !";
            //BarrierUpgradeBtn.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (CanonUpgradeNumber == 2)
        {
            CanonUpgradeBtn.interactable = false;
            CanonUpgradeBtn.transform.GetChild(0).GetComponent<Text>().text = "Max Level !";
        }
    }


    
    
    public void UpgradePolice()
    {
        if (PlayerPrefs.GetInt("Coins") >= PolicePrice[PlayerPrefs.GetInt("PoliceUpgradeNumber")])
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - PolicePrice[PlayerPrefs.GetInt("PoliceUpgradeNumber")]);
            PurchaseAudio.Play();
            PlayerPrefs.SetInt("PoliceUpgradeNumber", PlayerPrefs.GetInt("PoliceUpgradeNumber") + 1);
            PoliceUpgradeNumber = PlayerPrefs.GetInt("PoliceUpgradeNumber");
            ResetCoinsText();
            SetPolicePrefab(PoliceUpgradeNumber);
            UpdatePrice();
            CheckUpgradation();
        }
        else
        {
            LowCoinsAudio.Play();
        }

    }


    public void UpgradeBarrier()
    {
        if (PlayerPrefs.GetInt("Coins") >= BarrierPrice[PlayerPrefs.GetInt("BarrierUpgradeNumber")])
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - BarrierPrice[PlayerPrefs.GetInt("BarrierUpgradeNumber")]);
            PurchaseAudio.Play();

            PlayerPrefs.SetInt("BarrierUpgradeNumber", PlayerPrefs.GetInt("BarrierUpgradeNumber", 0) + 1);
            BarrierUpgradeNumber = PlayerPrefs.GetInt("BarrierUpgradeNumber");
            ResetCoinsText();
            SetBarrierPrefab(BarrierUpgradeNumber);
            UpdatePrice();
            CheckUpgradation();

        }
        else
        {
            LowCoinsAudio.Play();
        }
    }


    public void UpgradeCanon()
    {
        if (PlayerPrefs.GetInt("Coins") >= CanonPrice[PlayerPrefs.GetInt("CanonUpgradeNumber")])
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - CanonPrice[PlayerPrefs.GetInt("CanonUpgradeNumber")]);
            PurchaseAudio.Play();

            PlayerPrefs.SetInt("CanonUpgradeNumber", PlayerPrefs.GetInt("CanonUpgradeNumber", 0) + 1);
            CanonUpgradeNumber = PlayerPrefs.GetInt("CanonUpgradeNumber");
            ResetCoinsText();
            SetCanonPrefab(CanonUpgradeNumber);
            UpdatePrice();
            CheckUpgradation();
        }
        else
        {
            LowCoinsAudio.Play();
        }
    }





    void SetPolicePrefab(int index)
    {
        switch (index)
        {
            case 1:
                PolicePrefab = PolicePrefabs[1];
                StorePolicePrefabs[0].SetActive(false);
                StorePolicePrefabs[1].SetActive(true);
                StorePolicePrefabs[2].SetActive(false);


                break;
            case 2:
                PolicePrefab = PolicePrefabs[2];
                StorePolicePrefabs[0].SetActive(false);
                StorePolicePrefabs[1].SetActive(false);
                StorePolicePrefabs[2].SetActive(true);

                break;

        }
    }



    void SetBarrierPrefab(int index)
    {
        switch (index)
        {
            case 1:
                BarrierPrefab = BarrierPrefabs[1];
                StoreBarrierPrefabs[0].SetActive(false);
                StoreBarrierPrefabs[1].SetActive(true);
                StoreBarrierPrefabs[2].SetActive(false);

                break;

            case 2:
                BarrierPrefab = BarrierPrefabs[2];
                StoreBarrierPrefabs[0].SetActive(false);
                StoreBarrierPrefabs[1].SetActive(false);
                StoreBarrierPrefabs[2].SetActive(true);
                break;

        }
    }



    void SetCanonPrefab(int index)
    {
        switch (index)
        {
            case 1:
                CanonPrefab = CanonPrefabs[1];
                StoreCanonPrefabs[0].SetActive(false);
                StoreCanonPrefabs[1].SetActive(true);
                StoreCanonPrefabs[2].SetActive(false);


                break;
            case 2:
                CanonPrefab = CanonPrefabs[2];
                StoreCanonPrefabs[0].SetActive(false);
                StoreCanonPrefabs[1].SetActive(false);
                StoreCanonPrefabs[2].SetActive(true);

                break;

        }
    }



    void ResetCoinsText()
    {
        CoinText.text = PlayerPrefs.GetInt("Coins").ToString();
    }

    public void Quit()
    {
        Application.Quit();
    }

}
