using UnityEngine;
using System.Collections;

public class MeteorAbility : PlayerAbility {

    // CAN BE CHANGED FOR BALANCE
    public float meteorSpeed = 10.0f;
    // CAN BE CHANGED FOR BALANCE

    public GameObject meteorIndicator;
    public GameObject meteorReticle;
    public Transform meteorSpawn;


    // Use this for initialization
    void Start () {
        abilityPrefab = (GameObject)(Resources.Load("Meteor"));
        meteorIndicator = (GameObject)(Resources.Load("MeteorIndicator"));
        missilePrefab = (GameObject)(Resources.Load("Bullet"));
        // CAN BE CHANGED FOR BALANCE
        abilityTime = 5.0f;
        FireTime = 0.5f;
        missileSpeed = 15.0f;
        // CAN BE CHANGED FOR BALANCE

        meteorSpawn = playerObject.transform.Find("PlayerCenter/MeteorSpawn");
        meteorReticle = playerObject.transform.Find("PlayerCenter/TargetReticle/Shockwave_Export/ShockWave").gameObject;
        Physics.IgnoreLayerCollision(10, gameObject.layer);
    }
	
	// Update is called once per frame
	void Update () {
        FireTimer -= Time.deltaTime;

        if (Input.GetButtonUp("AbilityTrigger" + playerObject.GetComponent<PlayerController>().PlayerNumber) && playerObject.GetComponent<PlayerController>().AbilityTimer <= 0)
        {
            LaunchMeteor();
            meteorReticle.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }

        if(Input.GetButton("AbilityTrigger" + playerObject.GetComponent<PlayerController>().PlayerNumber) && playerObject.GetComponent<PlayerController>().AbilityTimer <= 0)
        {
            meteorReticle.GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
    }

    public void LaunchMeteor()
    {
        GameObject go = (GameObject)Instantiate(abilityPrefab, meteorSpawn.position, meteorSpawn.rotation);

        go.GetComponent<Rigidbody>().transform.LookAt(target);

        go.GetComponent<Rigidbody>().velocity = (go.GetComponent<Rigidbody>().transform.forward) * meteorSpeed;
        go.GetComponent<Meteor>().shooter = playerNumber;

        go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;

        GameObject go2 = (GameObject)Instantiate(meteorIndicator, target.position, playerTransform.rotation);

        go2.transform.GetChild(0).GetComponent<Renderer>().material = indicatorColor;

        playerObject.GetComponent<PlayerController>().SetAbilityTimer(abilityTime);
    }

    public override void TriggerAbility()
    {
        meteorReticle.GetComponent<SkinnedMeshRenderer>().enabled = true;
    }

    public override void Fire()
    {
        if (FireTimer <= 0)
        {
            GameObject go = (GameObject)Instantiate(missilePrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);


            go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * missileSpeed;
            go.GetComponent<Bullet>().shooter = playerNumber;
            go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;
            FireTimer = FireTime;
        }
    }
}
