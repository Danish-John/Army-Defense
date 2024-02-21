using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallProjectile : MonoBehaviour
{
    public GameObject ExplosionParticle;
    public float DamageRadius;
    public float Damage;

    


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Environment") || other.CompareTag("Obstacle"))
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

            if (other.gameObject.GetComponentInParent<EnemyHealth>().HealthBar.value <= 0 && CannonController.Instance)
            {
                CannonController.Instance.enemy = null;
                CannonController.Instance.FindNewTarget();
            }
            Destroy(this.gameObject);
        }
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



}
