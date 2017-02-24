using UnityEngine;
using System.Collections;

public class IceWall : MonoBehaviour
{
    //public ParticleSystem hitPC;
    
    public float environmentDamage;
    //public float duration;
    public float maxHealth;
    public float currentHealth;
    public float bulletDamage;

    public Color startingColor;
    public float rper;
    public float gper;
    public float bper;

    void Start()
    {
        currentHealth = maxHealth;

        startingColor = GetComponent<Renderer>().material.GetColor("_Color");

        rper = startingColor.r / 100;
        gper = startingColor.g / 100;
        bper = startingColor.b / 100;


        
    }

    // Update is called once per frame
    void Update()
    {
        //duration -= Time.deltaTime;
        //if(duration <= 0)
        //{
        //    Destroy(gameObject);
        //}

        if(currentHealth <= 0.0f)
        {
            Destroy(gameObject);
        }

        GetComponent<Renderer>().material.SetColor("_Color", new Color(rper * currentHealth, gper * currentHealth, bper * currentHealth));
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
        else if(other.GetComponent<Bullet>())
        {
            currentHealth -= bulletDamage;
        }
    }

    public void Decay(float dmg)
    {
        currentHealth -= dmg;
    }
}
