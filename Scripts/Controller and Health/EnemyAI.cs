using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance;

    public Slider HealthSlider;

    [HideInInspector] public NavMeshAgent Enemy;
    
    public bool isBooked=false;
    
    public bool isDead = false;

    public Transform Target;

    public bool PoliceInRange = false;

    public bool BarrierInRange = false;

    public bool CheckPoliceFunc = false;

    public bool CheckBarrierFunc = false;

    private float XRotation;

    private float YRotation;






    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        HealthSlider.maxValue = LevelManager.Instance.EnemyPower;
        HealthSlider.value = LevelManager.Instance.EnemyPower;

        Enemy = GetComponent<NavMeshAgent>();
        Target = GameObject.FindGameObjectWithTag("Target").transform;
        this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", true);
        this.gameObject.GetComponent<Animator>().SetBool("isAttacking",false);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Rotation();

            if (Target && Target.gameObject.tag == "Police" && !PoliceInRange)
            {
                Enemy.SetDestination(Target.position);
            }

            if (Target && Target.gameObject.tag == "Target" && !CheckPoliceFunc)
            {
                Enemy.isStopped = false;
                CheckClosePolicmen();
                CheckCloseBarrier();
                Enemy.SetDestination(Target.position);
            }
            else if (Target && Target.gameObject.tag == "Police" && PoliceInRange)
            {
                CheckClosePolicmen();
                Enemy.SetDestination(Target.position);
            }
            else if (!Target && !CheckPoliceFunc)
            {
                Target = GameObject.FindGameObjectWithTag("Target").transform;
                CheckClosePolicmen();
                CheckCloseBarrier();
                Enemy.SetDestination(Target.position);
            }
            else if (Target && Target.tag == "Barrier" && BarrierInRange)
            {
                CheckCloseBarrier();
                Enemy.SetDestination(Target.position);
            }
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




    void CheckClosePolicmen()
    {
        CheckPoliceFunc = true; // update bar bar chlti hai, is liye isy true kia hai ta kay func aik bar pura chal jaye then update mey again call ho/
        //neechay false kr dia hai func ky end pe, tension nhi leni

        GameObject[] Policemen = GameObject.FindGameObjectsWithTag("Police");

        foreach (GameObject policemen in Policemen)
        {
            PoliceAI pulci = policemen.GetComponent<PoliceAI>();
            if (pulci)
            {
                float dist = Vector3.Distance(this.transform.position, pulci.transform.position);
                if (dist <= 4f)
                {
                    //Enemy.isStopped = true;
                    Target = policemen.transform;
                    PoliceInRange = true;
                    Rotation();
                    this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", false);
                    this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", true);
                }
                else
                {
                    PoliceInRange = false;
                    this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", true);
                    this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", false);
                    Target = GameObject.FindGameObjectWithTag("Target").transform;
                }
            }
            
        }

        CheckPoliceFunc = false;
    }


    void CheckCloseBarrier()
    {
        CheckBarrierFunc = true; // update bar bar chlti hai, is liye isy true kia hai ta kay func aik bar pura chal jaye then update mey again call ho/
        //neechay false kr dia hai func ky end pe, tension nhi leni

        GameObject[] Barriers = GameObject.FindGameObjectsWithTag("Barrier");

        foreach (GameObject barrier in Barriers)
        {
            BarrierScript barrial = barrier.GetComponent<BarrierScript>();
            if (barrial)
            {
                float dist = Vector3.Distance(this.transform.position, barrial.transform.position);
                
                if (dist <= 4f)
                {
                    //Enemy.isStopped = true;
                    Target = barrial.transform;
                    BarrierInRange = true;
                    Rotation();
                    this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", false);
                    this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", true);
                }
                else
                {
                    BarrierInRange = false;
                    this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", true);
                    this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", false);
                    Target = GameObject.FindGameObjectWithTag("Target").transform;
                }
            }

        }

        CheckBarrierFunc = false;
    }



}
