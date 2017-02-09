using UnityEngine;
using System.Collections;

public class MeteorIndicator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Meteor>())
            Destroy(gameObject);
    }
}
