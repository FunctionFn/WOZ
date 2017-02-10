﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    //public ParticleSystem hitPC;

    public string shooter;
    public float strength;
    public float environmentDamage;

    public Transform left;

    void Start()
    {
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
            other.GetComponent<Rigidbody>().velocity = Vector3.Project(other.GetComponent<Rigidbody>().velocity, left.position - GetComponent<Transform>().position);

            other.GetComponent<Rigidbody>().AddForce(this.GetComponent<Rigidbody>().velocity.normalized * strength);
            Destroy(gameObject);
        }

    }
}