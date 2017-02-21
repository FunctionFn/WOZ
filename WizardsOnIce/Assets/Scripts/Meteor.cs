using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
    //public ParticleSystem hitPC;

    public string shooter;
    public float strength;
    public float environmentDamage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnBecameInvisible()
    {
        Destroy(gameObject, 2);
    }

    void OnDisable()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && other.gameObject.GetComponent<PlayerController>().PlayerNumber != shooter)
        {
            //other.gameObject.GetComponent<PlayerController>().Damage(1);

            Vector3 dir = other.GetComponent<Rigidbody>().position - GetComponent<Rigidbody>().position;

            other.GetComponent<Rigidbody>().AddForce(new Vector3(dir.x, 0, dir.z).normalized * strength);
            other.GetComponent<PlayerController>().OnHit();
        }

       
    }
}
