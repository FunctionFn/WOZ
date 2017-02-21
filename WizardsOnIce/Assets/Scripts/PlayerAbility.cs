using UnityEngine;
using System.Collections;

public class PlayerAbility : MonoBehaviour {

    public GameObject abilityPrefab;

    public float abilityTime;

    public Material playerColor;
    public Material indicatorColor;

    public string playerNumber;

    public GameObject playerObject;

    public Transform target;
    public Transform playerTransform;

    // Use this for initialization
    void Start () {
	
	}

    public void Initialize(Material col, Material icol, string pn, GameObject play)
    {
        playerColor = col;
        indicatorColor = icol;
        playerNumber = pn;

        playerObject = play;

        target = play.transform.Find("PlayerCenter/TargetReticle");
        playerTransform = play.transform.Find("PlayerCenter");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void TriggerAbility()
    {

    }

    public float GetAbilityTime()
    {
        return abilityTime;
    }
}
