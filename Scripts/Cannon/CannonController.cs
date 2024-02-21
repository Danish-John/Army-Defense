using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{

    public static CannonController Instance;

    //[SerializeField] private LayerMask ObjectsNotToShoot;

    public Transform enemy;

    EnemyAI Enemy;

    float dist;

    public float MinDist;

    //public Transform Body;

    public Transform Barrel;

    public Transform LaunchingPoint;

    public GameObject CannonBall;

    //public float ProjectileSpeed;

    public float FireRate;

    float NextFire;

    public bool InRange = false;

    [HideInInspector] public Vector3 velocity; //use for body setting position

    public AudioClip CanonFireSfx;

    public AudioSource CanonAudioSource;


    private void Start()
    {
        Instance = this;
        CanonAudioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!enemy)
        {
            FindNewTarget();
        }
        else if (enemy && !enemy.GetComponent<EnemyAI>().isDead && !GameManager.Instance.isGameFail)
        {
            dist = Vector3.Distance(transform.position, enemy.position);
            if (dist <= MinDist)
            {

                InRange = true;

                //Body.LookAt(enemy);

                Vector3 Vo = CalculateVelocity(enemy.position, LaunchingPoint.position, 1f);
                velocity = Vo;



                //Vo.y = 0;
                //Vo.z = 0;

                transform.rotation = Quaternion.LookRotation(Vo);
                
                //Vo.x = 0;
                //Vo.z = 0;
                //Body.rotation = Quaternion.LookRotation(Vo);


                if (Time.time >= NextFire)
                {
                    NextFire = Time.time + 1.5f / FireRate;

                    Shoot(enemy.gameObject);
                }
            }
            else
            {
                InRange = false;
                FindNewTarget();
            }
        }


    }



    void Shoot(GameObject Target)
    {
        CanonAudioSource.PlayOneShot(CanonFireSfx);
        GameObject rocket = Instantiate(CannonBall, LaunchingPoint.position, Quaternion.identity);

        Rigidbody rb =  rocket.GetComponent<Rigidbody>();
        rb.velocity = velocity;
        
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
            //Enemy.isBooked = true; // Mark the enemy as booked
            //this.transform.GetChild(2).gameObject.SetActive(false); // Fight Particle

        }
        else
        {
            // If no available target is found, set Target to null.
            enemy = null;
        }
    }



    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        //define the distance x and y first
        Vector3 distance = target - origin;
        Vector3 distance_x_z = distance;
        distance_x_z.Normalize();
        distance_x_z.y = 0;

        //creating a float that represents our distance 
        float sy = distance.y;
        float sxz = distance.magnitude;


        //calculating initial x velocity
        //Vx = x / t
        float Vxz = sxz / time;

        ////calculating initial y velocity
        //Vy0 = y/t + 1/2 * g * t
        float Vy = sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distance_x_z * Vxz;
        result.y = Vy;



        return result;
    }






}


