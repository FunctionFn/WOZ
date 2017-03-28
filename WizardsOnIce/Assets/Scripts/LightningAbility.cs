using UnityEngine;
using System.Collections;

public class LightningAbility : PlayerAbility
{

    // CAN BE CHANGED FOR BALANCE
    public float stunRockSpeed;
    // CAN BE CHANGED FOR BALANCE

    public GameObject meteorIndicator;

    public Transform meteorSpawn;

    public bool charging;

    

    public GameObject AreaOfAffect;

    // Use this for initialization
    void Start()
    {
        abilityPrefab = (GameObject)(Resources.Load("StunRock"));
        missilePrefab = (GameObject)(Resources.Load("LightningAoEOrigin"));
        // CAN BE CHANGED FOR BALANCE
        abilityTime = 5.0f;
        FireTime = 0.5f;
        missileSpeed = 5.0f;
        stunRockSpeed = 12.0f;
        // CAN BE CHANGED FOR BALANCE

        meteorSpawn = playerObject.transform.Find("PlayerCenter/MeteorSpawn");

        Physics.IgnoreLayerCollision(10, gameObject.layer);
    }

    // Update is called once per frame
    void Update()
    {
        FireTimer -= Time.deltaTime;

        if (Input.GetAxis("Trigger" + playerNumber) < 0.5f && Input.GetAxis("Trigger" + playerNumber) > -0.5f)
        {
            ReleaseChargeShot();
        }

        if(charging)
        {
            AreaOfAffect.transform.position = missileSpawnLocation.position;
            AreaOfAffect.transform.rotation = missileSpawnLocation.rotation;
        }
        
    }

    public override void TriggerAbility()
    {
        GameObject go = (GameObject)Instantiate(abilityPrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);


        go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * stunRockSpeed;
        go.GetComponent<Bullet>().shooter = playerNumber;
        go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;
        FireTimer = FireTime;

        playerObject.GetComponent<PlayerController>().SetAbilityTimer(abilityTime);
    }

    public override void Fire()
    {
        if (!charging)
        {
            AreaOfAffect = (GameObject)Instantiate(missilePrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);

            charging = true;
            
            AreaOfAffect.transform.GetChild(0).GetComponent<LightningAttack>().shooterPlayerObject = playerObject;
            AreaOfAffect.transform.GetChild(0).GetComponent<LightningAttack>().shooter = playerNumber;
        }
        else
        {
            
        }
    }

    public void ReleaseChargeShot()
    {
        if (charging)
        {
            Destroy(AreaOfAffect);
            charging = false;
        }
    }
}
