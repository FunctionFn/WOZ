using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>())
        {
            GameManager.GameOver();
        }
    }
}
