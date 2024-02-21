using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{

    public static TurretController Instance;

    [SerializeField] private LayerMask ObjectsNotToShoot;

    public Transform enemy;

    EnemyAI Enemy;

    float dist;
    
    public float MinDist;

    public Transform Head;

    public Transform Barrel;

    public GameObject Projectile;

    public float ProjectileSpeed;

    public float FireRate;

    float NextFire;

    public AudioClip RocketFireSfx;

    public AudioSource RocketAudioSource;


    private void Start()
    {
        Instance = this;
        RocketAudioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!enemy)
        {
            FindNewTarget();
        }
        else if(enemy && !enemy.GetComponent<EnemyAI>().isDead && !GameManager.Instance.isGameFail)
        {
            dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist <= MinDist)
            {
                //Head.LookAt(enemy);

                var lookPos = enemy.position - Head.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                rotation.z = 0;
                rotation.x = Mathf.Clamp(rotation.x, 0f, 10f);
                Head.rotation = Quaternion.Slerp(Head.rotation, rotation, Time.deltaTime * 10);


                if (Time.time >= NextFire)
                {
                    NextFire = Time.time + FireRate;

                    //RaycastHit hit;

                    //Vector3 direction = (enemy.position).normalized;

                    
                    //if (Physics.Raycast(Head.position, Head.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, ObjectsNotToShoot))
                    //{
                    //    Debug.Log("Props Hit");
                    //    Debug.DrawRay(Head.position, Head.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                    //    return;
                    //}
                    //else
                    //{
                    //    Debug.DrawRay(Head.position, Head.TransformDirection(Vector3.forward) * 1000, Color.blue);
                    //    Shoot(enemy.gameObject);
                    //}

                    Shoot(enemy.gameObject);


                }
            }
            else
            {
                FindNewTarget();
            }
        }


    }



    void Shoot(GameObject Target)
    {
        RocketAudioSource.PlayOneShot(RocketFireSfx);
        GameObject rocket = Instantiate(Projectile, Barrel.position, transform.rotation);
    }



    


    public void FindNewTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            Enemy = enemy.GetComponent<EnemyAI>();

            if (Enemy)
            {
                float distanceToTarget = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    closestTarget = enemy.transform;
                }
            }
        }

        // If a valid target is found, set it as the new target
        if (closestTarget != null)
        {
            enemy = closestTarget;

        }
        else
        {
            // If no available target is found, set Target to null.
            enemy = null;
        }
    }

}
