using UnityEngine;
using System.Collections;

public class IceBlock : MonoBehaviour {

    public float maxhealth;
    public float currentHealth;

    public Color startingColor;
    public float rper;
    public float gper;
    public float bper;

    public float t;
    //public AudioSource audioS;

    public float startingOffset;
    Vector3 pos;
    
    // Use this for initialization
    void Start () {

       
        currentHealth = maxhealth;

        startingColor = GetComponent<Renderer>().material.GetColor("_Color");

        rper = startingColor.r / 100;
        gper = startingColor.g / 100;
        bper = startingColor.b / 100;

        pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y + startingOffset, pos.z);
        iTween.MoveTo(gameObject, iTween.Hash("position", pos, "easeType", "easeInOutExpo", "time", Random.Range(1.0f, 1.5f)));

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
        
        /*
        OldRange = (OldMax - OldMin)  
        NewRange = (NewMax - NewMin)  
        NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin
        */

        float OldRange = 100.0f;
        float NewRange = 100.0f - 35.0f;
        float r = (((currentHealth) * NewRange) / OldRange) + 35.0f;
        float g = r;
        float b = r;

        r *= rper;
        g *= gper;
        b *= bper;



        //float OldRange = 1.0f;
        //float NewRange = (1.0f - .5f);
        //float nrper = (((rper) * NewRange) / OldRange);
        //float ngper = (((gper) * NewRange) / OldRange);
        //float nbper = (((bper) * NewRange) / OldRange);
        GetComponent<Renderer>().material.SetColor("_Color", new Color(r, g, b));
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
