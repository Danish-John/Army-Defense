using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class PoliceAI : MonoBehaviour
{
    public static PoliceAI Instance;

    [HideInInspector] public NavMeshAgent Police;
    
    private EnemyAI Enemy;

    public Transform Target;
    
    [HideInInspector] public bool LastEnemyStanding = false;

    [HideInInspector] public bool isDead = false;

    public bool isClose = false;

    private float XRotation;

    private float YRotation;

    public GameObject Cap;

    public GameObject Stick;

    [HideInInspector] public bool CoinSetOnce = false;

    public AudioClip Fightclip;



    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        Police = GetComponent<NavMeshAgent>();

        AnimatorSetOnStart();

        FindNewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (!GameManager.Instance.OnceWela && !isClose)
            {
                // Check if the current target is still valid
                if (Target && !Enemy.isBooked)
                {
                    // Check if the distance to the current target is too far (you can adjust this threshold)
                    if (Vector3.Distance(transform.position, Target.position) > 10f)
                    {
                        // If the distance is too far, find a new target
                        FindNewTarget();
                    }
                }
                else
                {
                    // If no valid target or the current target is booked, find a new target
                    FindNewTarget();
                }

                // Move towards the target
                if (Target)
                {
                    Police.SetDestination(Target.position);
                    Police.isStopped = false;
                }
                
                if (isClose)
                {
                    Police.SetDestination(Target.position);
                }

                if (gameObject.GetComponentInChildren<CharacterHealth>().EngagedGameobject)
                {
                    Target = gameObject.GetComponentInChildren<CharacterHealth>().EngagedGameobject.transform;
                    Police.SetDestination(Target.position);
                }
            }
            else if (GameManager.Instance.OnceWela)
            {
                if (Target)
                {
                    Police.SetDestination(Target.position);
                    Police.isStopped = false;
                }
            }

            Rotation();
            StopNearEnemy();
            CheckForLastEnemyStanding();
        }
        
    }



    void Rotation()
    {
        if (Target)
        {
            XRotation = Target.transform.rotation.x;
            YRotation = Target.transform.rotation.y;

            Quaternion TargetRotation = Quaternion.Euler(transform.eulerAngles.x + YRotation, transform.eulerAngles.y + XRotation, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, 10);
        }
    }



    void StopNearEnemy()
    {
        if (Target)
        {
            float dist = Vector3.Distance(this.transform.position, Target.position);
            if (dist <= 4f)
            {
                //Debug.Log("Dist got");
                isClose = true;
                //Police.isStopped = true;
                this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", true);
                this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", false);
                this.gameObject.GetComponentInParent<Animator>().SetBool("isIdle", false);
                this.gameObject.GetComponentInParent<Animator>().SetBool("isCheering", false);

                this.gameObject.GetComponentInParent<CharacterHealth>().FightParticle.gameObject.SetActive(true);

                if (!LevelManager.Instance.FightSound.isPlaying)
                {
                    LevelManager.Instance.FightSound.PlayOneShot(Fightclip);
                }

                if (!this.gameObject.GetComponentInParent<CharacterHealth>().FightParticle.isPlaying)
                {
                    this.gameObject.GetComponentInParent<CharacterHealth>().FightParticle.Play();
                }
            }
            else if(dist > 4f)
            {
                isClose = false;
                Police.isStopped = false;
                this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", false);
                this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", true);
                this.gameObject.GetComponentInParent<Animator>().SetBool("isIdle", false);
                this.gameObject.GetComponentInParent<Animator>().SetBool("isCheering", false);
                //LevelManager.Instance.FightSound.Stop();
                this.gameObject.GetComponentInParent<CharacterHealth>().FightParticle.gameObject.SetActive(false);
            }
        }

    }




    public void CheckForLastEnemyStanding()
    {
        if (!GameObject.FindGameObjectWithTag("Enemy") && LevelManager.Instance.AllEnemiesSpawned)
        {
            this.gameObject.GetComponent<Animator>().SetBool("isCheering", true);
            this.gameObject.GetComponent<Animator>().SetBool("isAttacking", false);
            this.gameObject.GetComponent<Animator>().SetBool("isRuning", false);
            this.gameObject.GetComponentInParent<Animator>().Play("Cheering");
            this.gameObject.GetComponentInChildren<CharacterHealth>().FightParticle.gameObject.SetActive(false);
            LevelManager.Instance.FightSound.Stop();


            Police.isStopped = true;
            LastEnemyStanding = true;

            //GameManager.Instance.OnMissionComplete.Invoke();
        }
    }




    public void FindNewTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            Enemy = enemy.GetComponent<EnemyAI>();

            if (Enemy && !Enemy.isBooked)
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
            Target = closestTarget;
            Enemy.isBooked = true; // Mark the enemy as booked
            this.transform.GetChild(2).gameObject.SetActive(false); // Fight Particle
                 
        }
        else
        {
            // If no available target is found, set Target to null.
            Target = null;
        }
    }



    void AnimatorSetOnStart()
    {
        this.gameObject.GetComponent<Animator>().SetBool("isRuning", true);
        this.gameObject.GetComponent<Animator>().SetBool("isAttacking", false);
        
    }





}
