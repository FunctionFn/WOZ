using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum LEVEL_NAMES
{
	Room1,
	Room2

}

public class GameManager : MonoBehaviour 
{

	public LEVEL_NAMES NextLevelName;


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

    public static void GameOver()
    {
        SceneManager.LoadScene("arena");
    }

}