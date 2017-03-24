using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    public Text countdownText;

    public float countdownTime;
    float countdownTimer;

	// Use this for initialization
	void Start () {
        countdownTimer = countdownTime;
        countdownText.text = Mathf.Ceil(countdownTimer).ToString();
        
	}
	
	// Update is called once per frame
	void Update () {
        countdownTimer -= Time.deltaTime * 2;
        countdownText.text = Mathf.Ceil(countdownTimer).ToString();

        if(countdownTimer <= 0)
        {
            countdownText.enabled = false;
        }
    }
}
