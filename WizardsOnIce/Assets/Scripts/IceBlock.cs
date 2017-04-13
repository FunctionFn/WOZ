using UnityEngine;
using System.Collections;

public class IceBlock : MonoBehaviour {

    public float maxhealth;
    public float currentHealth;

    public Color startingColor;
    public float rper;
    public float gper;
    public float bper;
    //public AudioSource audioS;

    // Use this for initialization
    void Start () {

       
        currentHealth = maxhealth;

        startingColor = GetComponent<Renderer>().material.GetColor("_Color");

        rper = startingColor.r / 100;
        gper = startingColor.g / 100;
        bper = startingColor.b / 100;
	}
	
	// Update is called once per frame
	void Update () {

        if (currentHealth <= 0)
        {
            //Destroy(gameObject);
            GetComponent<Renderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }

        if(GetComponent<Renderer>().enabled == false && currentHealth >= 0)
        {
            GetComponent<Renderer>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
        }

        //float OldRange = 1.0f;
        //float NewRange = (1.0f - .5f);
        //float nrper = (((rper) * NewRange) / OldRange);
        //float ngper = (((gper) * NewRange) / OldRange);
        //float nbper = (((bper) * NewRange) / OldRange);
        GetComponent<Renderer>().material.SetColor("_Color", new Color(rper * currentHealth, gper * currentHealth, bper * currentHealth));
	}

    public void Decay(float dmg)
    {
        if ((currentHealth > 0 && dmg > 0) || (currentHealth < 100 && dmg < 0))
        {
            currentHealth -= dmg;
            if(currentHealth > 100)
            {
                currentHealth = 100;
            }
            else if (currentHealth < 0)
            {
                currentHealth = -1.0f;
            }

        }

    }


}
