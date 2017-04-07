using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    //public AudioSource bgmusic;
    private static AudioManager _inst;
    public static AudioManager Inst { get { return _inst; } }


    void Awake()
    {
        if (!_inst)
            _inst = this;
        else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);

        //bgmusic.loop = true;
        //bgmusic.Play();
        //bgmusic.loop = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
