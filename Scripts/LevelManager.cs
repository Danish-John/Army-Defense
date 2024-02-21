using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int EnemyPower;

    public GameObject[] EnemyBunch;
    
     public float Time;
    
    int i= 0;
    
    [HideInInspector] public bool AllEnemiesSpawned = false;

    public int Power;

    public int LevelRewardCoins;

    public int LevelFailCoins;

    public AudioSource FightSound;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TurningOnEnemies());
    }


    IEnumerator TurningOnEnemies()
    {
        if (i>=EnemyBunch.Length)
        {
            StopCoroutine(TurningOnEnemies());
            AllEnemiesSpawned = true;
        }
        else if (i < EnemyBunch.Length)
        {
            EnemyBunch[i].SetActive(true);
            //Time = 10f;
            i++;
        }
        yield return new WaitForSeconds(Time);
        StartCoroutine(TurningOnEnemies());
    }

  
}
