using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CharacterHealth : MonoBehaviour
{
    public static CharacterHealth Instance;

    public Slider HealthBar;
   
    float ValToBeDec=1;

    public ParticleSystem FightParticle;

    public GameObject EngagedGameobject = null;

    //GameObject EngagedBarrier = null;

    //public AudioSource FightSound;




    private void Start()
    {
        Instance = this;

        if (this.gameObject.tag == "Barrier")
        {
            ValToBeDec = 0.1f;
        }
        else if (this.gameObject.tag == "Police")
        {
            ValToBeDec = 0.4f;
        }
    }




    private void Update()
    {
        if (HealthBar.value <= 0)
        {

            if (this.gameObject.tag == "Police")
            {
                //FightSound.Stop();
                this.gameObject.GetComponentInParent<PoliceAI>().isDead = true;
                this.gameObject.GetComponentInParent<NavMeshAgent>().enabled = false;
                this.gameObject.GetComponentInParent<PoliceAI>().gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                this.gameObject.GetComponentInParent<PoliceAI>().gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
                
                if (this.gameObject.GetComponentInParent<PoliceAI>().Cap)
                {
                    this.gameObject.GetComponentInParent<PoliceAI>().Cap.SetActive(false);
                }

                if (this.gameObject.GetComponentInParent<PoliceAI>().Stick)
                {
                    this.gameObject.GetComponentInParent<PoliceAI>().Stick.SetActive(false);
                }


                this.gameObject.GetComponentInParent<Rigidbody>().AddForce(Vector3.down*100, ForceMode.Impulse);
                Invoke("DestroyGameobject", 1);

            }
            else if (this.gameObject.tag == "Barrier" && !this.gameObject.GetComponent<BarrierScript>().isDead)
            {
                this.gameObject.GetComponent<AudioSource>().Play();
                this.gameObject.GetComponent<BarrierScript>().isDead = true;
                this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                this.gameObject.AddComponent<Rigidbody>();
                this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000, ForceMode.Impulse);
                Invoke("DestroyGameobject", 1);
            }
        }

        if (EngagedGameobject)
        {
            if(this.gameObject.tag == "Police")
            {
                if (EngagedGameobject.GetComponentInParent<EnemyAI>() && EngagedGameobject.GetComponentInParent<EnemyAI>().isDead)
                {
                    ExitPoliceIfEnemy();
                    EngagedGameobject = null;
                }
            }
        }

        //if (EngagedBarrier)
        //{
        //    if (this.gameObject.tag == "Police")
        //    {
        //        if (EngagedBarrier.GetComponent<BarrierScript>() && EngagedBarrier.GetComponent<BarrierScript>().isDead)
        //        {
        //            this.gameObject.GetComponentInParent<PoliceAI>().IsBarrierHitting = false;
        //            ExitPoliceIfBarrier();
        //            EngagedBarrier = null;
        //        }
        //    }
        //}


    }



    void DestroyGameobject()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        Destroy(this.gameObject);
    }








    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy" )
        {
            if (this.gameObject.tag == "Police" && !this.gameObject.GetComponentInParent<PoliceAI>().isDead)
            {
                EngagedGameobject = other.GetComponentInParent<EnemyAI>().gameObject;
                this.gameObject.GetComponentInParent<PoliceAI>().Target = other.GetComponentInParent<Transform>().transform;
                StayPoliceIfEnemy();

            }
            HealthBar.value -= ValToBeDec;
        }
        else if (other.CompareTag("Barrier"))
        {
            if (this.gameObject.tag == "Police" && !this.gameObject.GetComponentInParent<PoliceAI>().isDead)
            {
                StayPoliceIfBarrier();
            }
        }
        else
        {
            return;
        }

    }



    void StayPoliceIfEnemy()
    {
        this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", true);
        this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", false);
        this.gameObject.GetComponentInParent<Animator>().SetBool("isIdle", false);


        FightParticle.gameObject.SetActive(true);
        FightParticle.Play();

        //FightSound.Play();
    }



    void StayPoliceIfBarrier()
    {
        this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", false);
        this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", false);
        this.gameObject.GetComponentInParent<Animator>().SetBool("isIdle", true);
        

        if (PoliceAI.Instance.LastEnemyStanding)
        {
            this.gameObject.GetComponentInParent<Animator>().SetBool("isCheering", true);
            this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", false);
            this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", false);
            this.gameObject.GetComponentInParent<Animator>().Play("Cheering");
        }
    }











    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (this.gameObject.tag == "Police")
            {
                ExitPoliceIfEnemy();
                return;
            }
        }
        else if (other.tag == "Barrier")
        {
            if (this.gameObject.tag == "Police" && !this.gameObject.GetComponentInParent<PoliceAI>().isDead && this.gameObject.GetComponentInParent<NavMeshAgent>().enabled)
            {
                ExitPoliceIfBarrier();
                return;
            }
        }
        else
            return;
    }




    void ExitPoliceIfEnemy()
    {
        this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", false);
        this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", true);
        this.gameObject.GetComponentInParent<Animator>().SetBool("isIdle", false);

        FightParticle.gameObject.SetActive(false);
        FightParticle.Stop();

        //FightSound.Stop();



        if (this.gameObject.GetComponentInParent<PoliceAI>().enabled && this.gameObject.GetComponentInParent<PoliceAI>().isDead == false)
        {
            this.gameObject.GetComponentInParent<PoliceAI>().Police.isStopped = false;
        }
    }




    void ExitPoliceIfBarrier()
    {
        if (this.gameObject.GetComponentInParent<PoliceAI>() && !this.gameObject.GetComponentInParent<PoliceAI>().isDead)
        {
            this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", false);
            this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", true);
            this.gameObject.GetComponentInParent<Animator>().SetBool("isIdle", false);
            this.gameObject.GetComponentInParent<PoliceAI>().FindNewTarget();
            this.gameObject.GetComponentInParent<PoliceAI>().Police.isStopped = false;
        }
        
    }



}
