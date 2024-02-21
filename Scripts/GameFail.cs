using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFail : MonoBehaviour
{

    bool once = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!once)
        {
            
            if (other.gameObject.tag == "Enemy")
            {
                once = true;
                GameManager.Instance.isGameFail = true;
                GameObject[] Police = GameObject.FindGameObjectsWithTag("Police");
                foreach (GameObject police in Police)
                {
                    if (police.GetComponent<PoliceAI>())
                    {
                        police.GetComponent<PoliceAI>().enabled = false;
                        police.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        Destroy(police);
                    }
                }

                //GameObject.FindGameObjectWithTag("Enemy").GetComponentInParent<EnemyAI>().enabled = false;

                GameObject[] Enemy = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in Enemy)
                {
                    if (enemy.GetComponent<EnemyAI>())
                    {
                        enemy.GetComponent<EnemyAI>().enabled = false;
                        //enemy.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        enemy.GetComponent<Animator>().enabled = false;

                    }
                }

                //Time.timeScale = 0;
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + LevelManager.Instance.LevelFailCoins);
                GameManager.Instance.ControlerImage.SetActive(false);
                UIHandlingScript.Instance.ShowMissionFailPanel();

                if (PlayerPrefs.GetInt("LevelFailOnce") == 0)
                {
                    PlayerPrefs.SetInt("LevelFailOnce", 1);
                }
            }






        }

        
    }
}
