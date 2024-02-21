using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    public GameObject ExplosionParticle;

    public Rigidbody rb;

    public float DamageRadius;
    public float Damage;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Environment"))
        {
            GameObject particle = Instantiate(ExplosionParticle, this.transform.position, this.transform.rotation);
            particle.GetComponent<ParticleSystem>().Play();

            // Damage in radius !!!
            RadiusFunc();
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            GameObject particle = Instantiate(ExplosionParticle, this.transform.position, this.transform.rotation);
            particle.GetComponent<ParticleSystem>().Play();

            other.gameObject.GetComponentInParent<EnemyHealth>().HealthBar.value -= 100f;
            RadiusFunc();

            if (other.gameObject.GetComponentInParent<EnemyHealth>().HealthBar.value <= 0 && TurretController.Instance)
            {
                TurretController.Instance.enemy = null;
                TurretController.Instance.FindNewTarget();
            }
            Destroy(this.gameObject);
        }

        //Destroy(this.gameObject);
    }



    void RadiusFunc()
    {

        // Damage in radius !!!
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in Enemies)
        {
            EnemyAI ene = enemy.GetComponent<EnemyAI>();
            if (ene)
            {
                if (Vector3.Distance(transform.position, ene.transform.position) <= DamageRadius)
                {
                    ene.HealthSlider.value -= Damage;
                }
            }
        }



    }

    void SelfDestroy()
    {
        GameObject particle = Instantiate(ExplosionParticle, this.transform.position, this.transform.rotation);
        particle.GetComponent<ParticleSystem>().Play();
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (TurretController.Instance)
        {
            if (TurretController.Instance.enemy && TurretController.Instance.enemy.GetComponent<EnemyAI>().isDead)
            {
                TurretController.Instance.FindNewTarget();
                return;
            }
            else if (TurretController.Instance.enemy && !TurretController.Instance.enemy.GetComponent<EnemyAI>().isDead)
            {
                Vector3 direction = (TurretController.Instance.enemy.transform.position - transform.position).normalized;
                rb.MovePosition(transform.position + direction * TurretController.Instance.ProjectileSpeed * Time.deltaTime);
                transform.LookAt(TurretController.Instance.enemy);
            }
            else if(!TurretController.Instance.enemy)
            {
                SelfDestroy();
            }
        }
        else
        {
            return;
        }


        //if (!TurretController.Instance.enemy)
        //{
        //    SelfDestroy();
        //}


    }
    


}
