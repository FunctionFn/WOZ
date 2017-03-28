using UnityEngine;
using System.Collections;

public class LightningIndicator : MonoBehaviour {

    public int lifetimeFrames;
    int lifeCounter;
	// Use this for initialization
	void Start () {
        lifeCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        lifeCounter++;
        if(lifeCounter >= lifetimeFrames)
        {
            Destroy(gameObject);
        }
	}
}
