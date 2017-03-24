using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {


    public Image[] images;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("RollDash0"))
        {
            images[0].enabled = true;
        }

        if (Input.GetButtonDown("RollDash1"))
        {
            images[1].enabled = true;
        }

        if (Input.GetButtonDown("RollDash2"))
        {
            images[2].enabled = true;
        }

        if (Input.GetButtonDown("RollDash3"))
        {
            images[3].enabled = true;
        }


        if (Input.GetButtonUp("RollDash0"))
        {
            images[0].enabled = false;
        }

        if (Input.GetButtonUp("RollDash1"))
        {
            images[1].enabled = false;
        }

        if (Input.GetButtonUp("RollDash2"))
        {
            images[2].enabled = false;
        }

        if (Input.GetButtonUp("RollDash3"))
        {
            images[3].enabled = false;
        }
    }
}
