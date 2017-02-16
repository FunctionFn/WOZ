using UnityEngine;
using System.Collections;

public class PlayerAbility : MonoBehaviour {

    public GameObject abilityPrefab;

    public float abilityTime;

    public Material playerColor;
    public Material indicatorColor;

    public string playerNumber;

    public Transform target;

    // Use this for initialization
    void Start () {
	
	}

    public void Initialize(Material col, Material icol, string pn, ref Transform t)
    {
        playerColor = col;
        indicatorColor = icol;
        playerNumber = pn;
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
