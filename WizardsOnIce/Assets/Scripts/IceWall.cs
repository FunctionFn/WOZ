using UnityEngine;
using System.Collections;

public class IceWall : MonoBehaviour
{
    //public ParticleSystem hitPC;
    
    public float environmentDamage;
    public float duration;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        if(duration <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnDisable()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Meteor>())
        {
            Destroy(gameObject);
        }
    }
}
