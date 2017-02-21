using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour 
{

    public List<PlayerController> PlayersAlive;
    


    private static GameManager _inst;
    public static GameManager Inst { get { return _inst; } }

    void Awake()
    {

        if (!Inst)
        {
            _inst = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene("arena");
        }

        
    }

	public void LoadNextScene()
	{
		SceneManager.LoadScene ("arena");

        
	}
    public void QuitGame()
    {
        Application.Quit();
    }

    public void AddPlayer(PlayerController p)
    {
        PlayersAlive.Add(p);
    }

    public void SubPlayer(PlayerController p)
    {
        PlayersAlive.Remove(p);
        if (PlayersAlive.Count <= 1)
        {
            GameOver();
        }
    }

    public static void GameOver()
    {
        SceneManager.LoadScene("arena");
    }

}