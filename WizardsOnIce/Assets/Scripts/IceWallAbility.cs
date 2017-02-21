using UnityEngine;
using System.Collections;

public class IceWallAbility : PlayerAbility
{

    // CAN BE CHANGED FOR BALANCE
    public float duration = 3.0f;
    // CAN BE CHANGED FOR BALANCE

    public Transform iceWallSpawn;

    // Use this for initialization
    void Start()
    {
        abilityPrefab = (GameObject)(Resources.Load("IceWall"));

        // CAN BE CHANGED FOR BALANCE
        abilityTime = 5.0f;
        // CAN BE CHANGED FOR BALANCE

        iceWallSpawn = playerObject.transform.Find("PlayerCenter/IceWallSpawn");

        Physics.IgnoreLayerCollision(10, gameObject.layer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TriggerAbility()
    {
        GameObject go = (GameObject)Instantiate(abilityPrefab, iceWallSpawn.position, iceWallSpawn.rotation);

        //go.transform.GetChild(0).GetComponent<Renderer>().material = playerColor;
    }
}
