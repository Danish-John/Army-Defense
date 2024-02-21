using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class EnemyHealth : MonoBehaviour
{
    public static EnemyHealth Instance;

    public Slider HealthBar;
    
    public ParticleSystem FightParticle;

    GameObject EngagedObject = null;

    private void Start()
    {
        Instance = this;
    }


    private void Update()
    {
        if (HealthBar.value <= 0)
        {
            if (!this.gameObject.GetComponentInParent<EnemyAI>().isDead)
            {
                GameController.Instance.PowerSlider.value++;
            }
            
            this.gameObject.GetComponentInParent<EnemyAI>().isDead = true;
            UIHandlingScript.Instance.UpdatePowerText(GameController.Instance.PowerSlider.value.ToString());

            this.gameObject.GetComponentInParent<NavMeshAgent>().enabled = false;
            this.gameObject.GetComponentInParent<EnemyAI>().gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            this.gameObject.GetComponentInParent<EnemyAI>().gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;

            this.gameObject.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * 1000, ForceMode.Impulse);
            this.gameObject.GetComponentInParent<CapsuleCollider>().enabled = false;
            Invoke("DestroyGameobject", 0.25f);
        }



        if (EngagedObject)
        {
            if (EngagedObject.GetComponent<PoliceAI>() && EngagedObject.GetComponent<PoliceAI>().isDead)
            {
                ExitIfPolice();
                EngagedObject = null;
                this.gameObject.GetComponentInParent<EnemyAI>().PoliceInRange = false;
                this.gameObject.GetComponentInParent<EnemyAI>().Target = GameObject.FindGameObjectWithTag("Target").transform;

            }
            else if (EngagedObject.GetComponent<BarrierScript>() && EngagedObject.GetComponent<BarrierScript>().isDead)
            {
                ExitIfPolice();
                EngagedObject = null;
            }
        }
        else if (!EngagedObject)
        {
            this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", true);
            this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", false);
        }
        
    }



    void DestroyGameobject()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.gameObject.GetComponentInChildren<Collider>().enabled = false;
        Destroy(this.gameObject);
    }







    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Police"))
        {
            StayIfPolice(other);
            HealthBar.value -= 0.3f;
            EngagedObject = other.gameObject.GetComponentInParent<PoliceAI>().gameObject;
            if (other.gameObject.GetComponentInParent<PoliceAI>().isDead)
            {
                ExitIfPolice();
                return;
            }

        }
        else if (other.CompareTag("Barrier") || other.CompareTag("Canon"))
        {
            StayIfBarrier(other);
            EngagedObject = other.gameObject;

            if (other.gameObject.GetComponentInParent<BarrierScript>() && other.gameObject.GetComponentInParent<BarrierScript>().isDead)
            {
                ExitIfPolice();
                return;
            }
            else if (other.gameObject.GetComponentInParent<CanonHealth>() && other.gameObject.GetComponentInParent<CanonHealth>().isDead)
            {
                ExitIfPolice();
                return;
            }
        }       
    }


    void StayIfPolice(Collider other)
    {
        this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", false);
        this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", true);
        
        this.gameObject.GetComponentInParent<EnemyAI>().Target = other.gameObject.transform;

        if (FightParticle.gameObject.activeInHierarchy == false)
        {
            FightParticle.gameObject.SetActive(true);
            FightParticle.Play();
        }
        if (this.gameObject.GetComponentInParent<EnemyAI>().enabled && this.gameObject.GetComponentInParent<EnemyAI>().isDead == false)
        {
            this.gameObject.GetComponentInParent<EnemyAI>().Enemy.isStopped = true;
        }
    }

    void StayIfBarrier(Collider other)
    {
        this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", false);
        this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", true);
        

        this.gameObject.GetComponentInParent<EnemyAI>().Target = other.gameObject.transform;
        if (!this.gameObject.GetComponentInParent<EnemyAI>().isDead && this.gameObject.GetComponentInParent<NavMeshAgent>().enabled)
        {
            this.gameObject.GetComponentInParent<EnemyAI>().Enemy.isStopped = true;
        }

        FightParticle.gameObject.SetActive(true);
        FightParticle.Play();
    }








    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Police") || other.CompareTag("Barrier") || other.CompareTag("Canon"))
        {
            ExitIfPolice();
        }
        else
        {
            ExitIfOther();
            return;
        }
            
    }






    void ExitIfPolice()
    {
        FightParticle.Stop();
        FightParticle.gameObject.SetActive(false);

        
        //Debug.Log("Runing Bool = " + this.gameObject.GetComponentInParent<Animator>().GetBool("isRuning"));


        if (this.gameObject.GetComponentInParent<EnemyAI>().enabled && this.gameObject.GetComponentInParent<EnemyAI>().isDead == false)
        {
            this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", false);
            //Debug.Log("Attcking Bool = " + this.gameObject.GetComponentInParent<Animator>().GetBool("isAttacking"));

            this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", true);
            //Debug.Log("Attcking Bool = " + this.gameObject.GetComponentInParent<Animator>().GetBool("isAttacking"));

            this.gameObject.GetComponentInParent<EnemyAI>().Target = GameObject.FindGameObjectWithTag("Target").transform;

            this.gameObject.GetComponentInParent<EnemyAI>().Enemy.isStopped = false;
            this.gameObject.GetComponentInParent<EnemyAI>().PoliceInRange = false ;

            this.gameObject.GetComponentInParent<EnemyAI>().isBooked = false;
        }
    } 

    void ExitIfOther()
    {
        if (this.gameObject.GetComponentInParent<EnemyAI>().enabled && this.gameObject.GetComponentInParent<EnemyAI>().isDead == false)
        {
            this.gameObject.GetComponentInParent<EnemyAI>().Enemy.isStopped = false;
            this.gameObject.GetComponentInParent<EnemyAI>().isBooked = false;
            this.gameObject.GetComponentInParent<Animator>().SetBool("isRuning", true);
            this.gameObject.GetComponentInParent<Animator>().SetBool("isAttacking", false);
        }
    }


}
