using UnityEngine;
using System.Collections;

public class Meteor : PlayerAbility
{
    //public ParticleSystem hitPC;

    public string shooter;
    public float strength;
    public float environmentDamage;

    public float meteorSpeed = 10.0f;

    public GameObject meteorIndicator;

    void Start()
    {
        abilityPrefab = (GameObject)Instantiate(Resources.Load("Meteor"));
        meteorIndicator = (GameObject)Instantiate(Resources.Load("MeteorIndicator"));
        abilityTime = 5.0f;

        Physics.IgnoreLayerCollision(10, gameObject.layer);
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

    public override void TriggerAbility()
    {
        GameObject go = (GameObject)Instantiate(abilityPrefab, meteorSpawnLocation.position, meteorSpawnLocation.rotation);

        go.GetComponent<Rigidbody>().transform.LookAt(target);

        go.GetComponent<Rigidbody>().velocity = (go.GetComponent<Rigidbody>().transform.forward) * meteorSpeed;
        go.GetComponent<Meteor>().shooter = playerNumber;

        go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;

        GameObject go2 = (GameObject)Instantiate(meteorIndicator, target.position, playerCenter.rotation);

        go2.transform.GetChild(0).GetComponent<Renderer>().material = indicatorColor;
    }
}
