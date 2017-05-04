using UnityEngine;
using System.Collections;

public class IceWall : MonoBehaviour
{
    //public ParticleSystem hitPC;
    
    public float environmentDamage;
    //public float duration;
    public float maxHealth;
    public float currentHealth;
    public float healthDecay;
    public float bulletDamage;

    public Color startingColor;
    public float rper;
    public float gper;
    public float bper;

	public AudioClip IceShatter;
	public float volume;

    public float startingOffset;
    //AudioSource audio;

    public Vector3 pos;

    void Start()
    {
		//audio = GetComponent<AudioSource> ();

		currentHealth = maxHealth;

        startingColor = GetComponent<Renderer>().material.GetColor("_Color");

        rper = startingColor.r / 100;
        gper = startingColor.g / 100;
        bper = startingColor.b / 100;

        pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y + startingOffset, pos.z);
        iTween.MoveTo(gameObject, iTween.Hash("position", pos, "easeType", "easeInOutExpo", "time", Random.Range(1.0f, 1.5f)));
    }

    // Update is called once per frame
    void Update()
    {
        //duration -= Time.deltaTime;
        //if(duration <= 0)
        //{
        //    Destroy(gameObject);
        //}
        Decay(healthDecay * Time.deltaTime);

        if (currentHealth <= 0.0f)
        {
			//AudioSource.PlayClipAtPoint (IceShatter, new Vector3(0,18,0));
			Destroy (gameObject);


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
			//AudioSource.PlayClipAtPoint (IceShatter, new Vector3(0,18,0));
			//rare chance it will try play the sound at every frame and cause the game to slow down - Eddie
			Destroy(gameObject);
        }
        else if(other.GetComponent<Bullet>() && !other.GetComponent<EarthBullet>())
        {
            currentHealth -= bulletDamage;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<IceBullet>())
        {
			currentHealth -= bulletDamage;
        }
    }

    public void Decay(float dmg)
    {
        currentHealth -= dmg;
    }
		
}