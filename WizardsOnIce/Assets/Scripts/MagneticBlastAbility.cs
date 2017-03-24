using UnityEngine;
using System.Collections;

public class MagneticBlastAbility : PlayerAbility {

    public float radius = 3.0F;
    public float power;
    public float onhitpower;
    public float indicatorTimer;
    // Use this for initialization
    void Start()
    {
        abilityPrefab = (GameObject)(Resources.Load("MeteorIndicator"));
        missilePrefab = (GameObject)(Resources.Load("MagnetBullet"));
        // CAN BE CHANGED FOR BALANCE
        abilityTime = 5.0f;
        FireTime = 0.6f;
        missileSpeed = 12.0f;
        power = 10.0f;
        onhitpower = 8.0f;
    // CAN BE CHANGED FOR BALANCE


    Physics.IgnoreLayerCollision(10, gameObject.layer);
    }

    // Update is called once per frame
    void Update()
    {
        FireTimer -= Time.deltaTime;

        
    }

    public override void TriggerAbility()
    {
        GameObject go = (GameObject)Instantiate(abilityPrefab, playerObject.transform.position, playerObject.transform.rotation);
        
        go.transform.GetChild(0).GetComponent<Renderer>().material = indicatorColor;
        go.GetComponent<Animator>().speed = 5.0f;

        go.GetComponent<MeteorIndicator>().countdown = true;

        Vector3 explosionPos = playerObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.GetComponent<PlayerController>() && hit != playerObject.GetComponent<Collider>())
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                Vector3 proj = Vector3.Project(rb.velocity, Quaternion.AngleAxis(-90, Vector3.up) * (rb.position - playerObject.GetComponent<Rigidbody>().position));
                rb.velocity = proj/* + rb.velocity) * .5f*/;

                Vector3 disNorm = (rb.position - playerObject.GetComponent<Rigidbody>().position);
                disNorm.Normalize();
                if (rb != null)
                {
                    hit.gameObject.GetComponent<Rigidbody>().AddForce(disNorm * power, ForceMode.Impulse);

                }
                hit.gameObject.GetComponent<PlayerController>().OnHit(onhitpower);
            }
        }

    }

    public override void Fire()
    {
        if (FireTimer <= 0)
        {
            GameObject go = (GameObject)Instantiate(missilePrefab, missileSpawnLocation.position, missileSpawnLocation.rotation);


            go.GetComponent<Rigidbody>().velocity = (missileSpawnLocation.transform.forward) * missileSpeed;
            go.GetComponent<Bullet>().shooter = playerNumber;
            Physics.IgnoreCollision(go.GetComponent<Collider>(), playerObject.GetComponent<Collider>());
            go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;
            FireTimer = FireTime;
        }
    }
}

