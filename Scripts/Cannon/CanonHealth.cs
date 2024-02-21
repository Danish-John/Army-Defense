using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CanonHealth : MonoBehaviour
{
    public static CanonHealth Instance;

    [HideInInspector] public bool isDead = false;

    public Slider HealthBar;

    public float ValToBeDec = 1f;

    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthBar.value <=0 && !isDead)
        {
            isDead = true;
            if (this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>())
            {
                this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            }
            else if (this.gameObject.GetComponentInChildren<MeshRenderer>())
            {
                MeshRenderer[] meshrenderer = this.gameObject.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer mr in meshrenderer)
                {
                    mr.enabled = false;
                }
            }
            this.gameObject.AddComponent<Rigidbody>();
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000, ForceMode.Acceleration);
            Invoke("DestroyGameobject", 0.5f);

        }
    }

    void DestroyGameobject()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        Destroy(this.gameObject);
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            HealthBar.value -= ValToBeDec;
        }
        else
        {
            return;
        }
    }


}
