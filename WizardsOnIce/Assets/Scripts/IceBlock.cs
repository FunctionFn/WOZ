using UnityEngine;
using System.Collections;

public class IceBlock : MonoBehaviour {

    public float maxhealth;
    public float currentHealth;

    public Color startingColor;
    public float rper;
    public float gper;
    public float bper;
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
	    if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }


        GetComponent<Renderer>().material.SetColor("_Color", new Color(rper * currentHealth, gper * currentHealth, bper * currentHealth));
	}

    public void Decay(float dmg)
    {
        currentHealth -= dmg;
    }


}
