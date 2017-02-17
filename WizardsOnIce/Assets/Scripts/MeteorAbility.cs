using UnityEngine;
using System.Collections;

public class MeteorAbility : PlayerAbility {

    // CAN BE CHANGED FOR BALANCE
    public float meteorSpeed = 10.0f;
    // CAN BE CHANGED FOR BALANCE

    public GameObject meteorIndicator;

    public Transform meteorSpawn;

    // Use this for initialization
    void Start () {
        abilityPrefab = (GameObject)(Resources.Load("Meteor"));
        meteorIndicator = (GameObject)(Resources.Load("MeteorIndicator"));

        // CAN BE CHANGED FOR BALANCE
        abilityTime = 5.0f;
        // CAN BE CHANGED FOR BALANCE

        meteorSpawn = playerObject.transform.Find("PlayerCenter/MeteorSpawn");

        Physics.IgnoreLayerCollision(10, gameObject.layer);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void TriggerAbility()
    {
        GameObject go = (GameObject)Instantiate(abilityPrefab, meteorSpawn.position, meteorSpawn.rotation);

        go.GetComponent<Rigidbody>().transform.LookAt(target);

        go.GetComponent<Rigidbody>().velocity = (go.GetComponent<Rigidbody>().transform.forward) * meteorSpeed;
        go.GetComponent<Meteor>().shooter = playerNumber;

        go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;

        GameObject go2 = (GameObject)Instantiate(meteorIndicator, target.position, playerTransform.rotation);

        go2.transform.GetChild(0).GetComponent<Renderer>().material = indicatorColor;
    }
}
