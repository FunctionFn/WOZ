using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum LEVEL_NAMES
{
	Room1,
	Room2

}

public class GameManager : MonoBehaviour 
{

    public List<PlayerController> PlayersAlive;

	public LEVEL_NAMES NextLevelName;

    private void Awake()
    {

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
		if (NextLevelName == LEVEL_NAMES.Room1) 
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