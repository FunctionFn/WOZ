using UnityEngine;
using System.Collections;

public class LightningAbility : PlayerAbility
{

    // CAN BE CHANGED FOR BALANCE
    public float stunRockSpeed;
    // CAN BE CHANGED FOR BALANCE

    public GameObject lightningIndicator;
    public GameObject laserPrefab;


    public Transform meteorSpawn;

    public bool shooting;

    public float chargingTurnSpeed;

    public bool charging;
    public float currentCharge;
    public float chargeSpeed;

    float baseTurnSpeed;
    

    public GameObject AreaOfAffect;
    public GameObject chargingLaser;
    // Use this for initialization
    void Start()
    {
        abilityPrefab = (GameObject)(Resources.Load("Laserorigin"));
        missilePrefab = (GameObject)(Resources.Load("LightningAoEOrigin"));
        // CAN BE CHANGED FOR BALANCE
        abilityTime = 5.0f;
        FireTime = 0.5f;
        missileSpeed = 5.0f;
        stunRockSpeed = 12.0f;
        chargeSpeed = 0.65f;
        chargingTurnSpeed = 200.0f;
        // CAN BE CHANGED FOR BALANCE

        currentCharge = 0.0f;

        meteorSpawn = playerObject.transform.Find("PlayerCenter/MeteorSpawn");

        Physics.IgnoreLayerCollision(10, gameObject.layer);

        baseTurnSpeed = playerObject.GetComponent<PlayerController>().turnRate;
    }

    // Update is called once per frame
    void Update()
    {
        FireTimer -= Time.deltaTime;

        if (Input.GetAxis("Trigger" + playerNumber) < 0.5f && Input.GetAxis("Trigger" + playerNumber) > -0.5f)
        {
            ReleaseChargeShot();
        }

        if(shooting)
        {
            AreaOfAffect.transform.position = missileSpawnLocation.position;
            AreaOfAffect.transform.rotation = missileSpawnLocation.rotation;
        }

        if (Input.GetButtonUp("AbilityTrigger" + playerObject.GetComponent<PlayerController>().PlayerNumber) && playerObject.GetComponent<PlayerController>().AbilityTimer <= 0 && currentCharge >= 0)
        {
            LightningLaser();
            playerObject.GetComponent<PlayerController>().turnRate = baseTurnSpeed;
            //meteorReticle.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }

        if (Input.GetButton("AbilityTrigger" + playerObject.GetComponent<PlayerController>().PlayerNumber) && playerObject.GetComponent<PlayerController>().AbilityTimer <= 0)
        {
            //meteorReticle.GetComponent<SkinnedMeshRenderer>().enabled = true;
            playerObject.GetComponent<PlayerController>().turnRate = chargingTurnSpeed;
        }


        if (charging)
        {
            currentCharge += chargeSpeed * Time.deltaTime;

            float OldRange = 1.0f;
            float NewRange = (1.5f - .3f);
            float chargeper = (((currentCharge) * NewRange) / OldRange) + .3f;
            chargeper = Mathf.Clamp(chargeper, 0.1f, 1.5f);
            chargingLaser.transform.position = missileSpawnLocation.position;
            chargingLaser.transform.rotation = missileSpawnLocation.rotation;
            chargingLaser.transform.localScale = new Vector3(chargeper, chargeper, 1);
        }

    }

    public void LightningLaser()
    {
        //GameObject go = (GameObject)Instantiate(abilityPrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);


        //go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * stunRockSpeed;
        //go.GetComponent<Bullet>().shooter = playerNumber;
        //go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;
        //FireTimer = FireTime;

        //playerObject.GetComponent<PlayerController>().SetAbilityTimer(abilityTime);
    }

    public override void TriggerAbility()
    {
        if (!charging)
        {
            chargingLaser = (GameObject)Instantiate(abilityPrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);


            //go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * missileSpeed;
            //chargingLaser.GetComponent<Bullet>().shooter = playerNumber;
            chargingLaser.transform.GetChild(0).GetComponent<Renderer>().material = indicatorColor;
            //chargingBullet.GetComponent<Transform>().parent = playerObject.transform.GetChild(0);
            chargingLaser.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
            charging = true;


        }
        //else
        //{
        //    currentCharge += chargeSpeed * Time.deltaTime;
        //}
    }

    public override void Fire()
    {
        if (!shooting)
        {
            AreaOfAffect = (GameObject)Instantiate(missilePrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);

            shooting = true;
            
            AreaOfAffect.transform.GetChild(0).GetComponent<LightningAttack>().shooterPlayerObject = playerObject;
            AreaOfAffect.transform.GetChild(0).GetComponent<LightningAttack>().shooter = playerNumber;
        }
        else
        {
            
        }
    }

    public void ReleaseChargeShot()
    {
        if (shooting)
        {
            Destroy(AreaOfAffect);
            shooting = false;
        }
    }
}
