﻿using UnityEngine;
using System.Collections;

public class MagneticBlastAbility : PlayerAbility {

    public float radius = 3.0F;
    public float power = 20.0F;

    // Use this for initialization
    void Start()
    {
        abilityPrefab = (GameObject)(Resources.Load("MeteorIndicator"));
        missilePrefab = (GameObject)(Resources.Load("MagnetBullet"));
        // CAN BE CHANGED FOR BALANCE
        abilityTime = 5.0f;
        FireTime = 0.5f;
        missileSpeed = 15.0f;
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

        Vector3 explosionPos = playerObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            if (hit.gameObject.GetComponent<PlayerController>() && hit != playerObject.GetComponent<Collider>())
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                Vector3 proj = Vector3.Project(rb.velocity, Quaternion.AngleAxis(-90, Vector3.up) * (rb.position - playerObject.GetComponent<Rigidbody>().position));
                rb.velocity = (proj + rb.velocity) * .5f;

                if (rb != null)
                {
                    rb.AddExplosionForce(power, explosionPos, radius, 0.0F, ForceMode.Impulse);
                    rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                }
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

