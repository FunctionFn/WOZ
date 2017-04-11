using UnityEngine;
using System.Collections;

public class LightningLaser : MonoBehaviour {

    public float strengthModifier;
    public float fullChargeBonus;
    public float chargeAmt;
    public float minCharge;
    public float maxSpeedHitModifier;
    float chargeBonus;

    public float strength;
    public string shooter;

    public float environmentalDamage;

    public bool active;
    public float duration;

    public float wallDamage;

    public Transform left;
    // Use this for initialization
    void Start () {
        active = false;
	}

    public void SetChargeAmount(float charge)
    {
        chargeAmt = charge;

        if (chargeAmt < minCharge)
        {
            chargeAmt = minCharge;
        }
        else if (chargeAmt > 1.0f)
        {
            chargeAmt = 1.0f;
            chargeBonus += fullChargeBonus;
        }

        strengthModifier *= chargeAmt;

        environmentalDamage *= chargeAmt;
        environmentalDamage *= chargeAmt;
    }


    // Update is called once per frame
    void Update () {
	    if(active)
        {
            duration -= Time.deltaTime;

            if(duration <= 0)
            {
                Destroy(gameObject);
            }
        }

        
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && other.gameObject.GetComponent<PlayerController>().PlayerNumber != shooter)
        {
            Vector3 proj = Vector3.Project(other.gameObject.GetComponent<Rigidbody>().velocity, left.position - GetComponent<Transform>().parent.position);
            other.gameObject.GetComponent<Rigidbody>().velocity = (proj)/* + other.gameObject.GetComponent<Rigidbody>().velocity) * .5f*/;

            Vector3 dir = transform.forward;

            other.gameObject.GetComponent<Rigidbody>().AddForce(dir * (strength * strengthModifier + chargeBonus), ForceMode.Impulse);
            other.gameObject.GetComponent<PlayerController>().OnHit(maxSpeedHitModifier + (chargeBonus * 2));

            //fully charged shot disables dash
            if (chargeBonus > 0)
                other.gameObject.GetComponent<PlayerController>().DashTimer = other.gameObject.GetComponent<PlayerController>().DashTime + other.gameObject.GetComponent<PlayerController>().dashCooldown;
        }
        else if(other.GetComponent<IceBlockTriggerZone>())
        {
            other.GetComponent<IceBlockTriggerZone>().ib.Decay(wallDamage * chargeAmt * chargeAmt);
        }

        else if (other.GetComponent<IceWall>())
        {
            other.GetComponent<IceWall>().Decay(wallDamage * chargeAmt * chargeAmt);
        }

    }
}
