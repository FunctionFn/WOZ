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
    public float currentCharge;
    public float chargeSpeed;

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
        chargeSpeed = 0.75f;
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

        if (charging)
        {
            float OldRange = 1.0f;
            float NewRange = (1.5f - .3f);
            float chargeper = (((currentCharge) * NewRange) / OldRange) + .3f;
            chargeper = Mathf.Clamp(chargeper, 0.1f, 1.5f);
            AreaOfAffect.transform.position = missileSpawnLocation.position;
            AreaOfAffect.transform.rotation = missileSpawnLocation.rotation;
            //AreaOfAffect.transform.localScale = new Vector3(chargeper, chargeper, chargeper);
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


            //go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * missileSpeed;
            //AreaOfAffect.GetComponent<Bullet>().shooter = playerNumber;
            //AreaOfAffect.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;
            AreaOfAffect.GetComponent<Transform>().parent = playerObject.transform.GetChild(0);
            //AreaOfAffect.GetComponent<BoxCollider>().enabled = false;
            charging = true;

        }
        else
        {
            currentCharge += chargeSpeed * Time.deltaTime;
        }
    }

    public void ReleaseChargeShot()
    {
        if (charging)
        {
            AreaOfAffect.GetComponent<Transform>().parent = null;
            AreaOfAffect.GetComponent<BoxCollider>().enabled = true;
            AreaOfAffect.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * missileSpeed;
            //AreaOfAffect.GetComponent<EarthBullet>().SetChargeAmount(currentCharge);
            currentCharge = 0;
            charging = false;
        }
    }
}
