using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour 
{

    public List<PlayerController> PlayersAlive;
    


    private static GameManager _inst;
    public static GameManager Inst { get { return _inst; } }

    public Dictionary<int, PlayerController.SkillID> PlayerSkills;

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
        PlayerSkills = new Dictionary<int, PlayerController.SkillID>();
    }

    void Start()
    {
        

        Inst.PlayerSkills.Add(0, 0);
        Inst.PlayerSkills.Add(1, 0);
        Inst.PlayerSkills.Add(2, 0);
        Inst.PlayerSkills.Add(3, 0);
    }
    void Update()
    {
        
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
            PlayersAlive.Clear();
            GameOver();

        }
    }

    public static void GameOver()
    {
        SceneManager.LoadScene("arena");
    }


    public void SetPlayerSkill(Dropdown selector)
    {
        int playernum;

        switch (selector.name)
        {
            case "Player1Selector":
                playernum = 0;
                break;

            case "Player2Selector":
                playernum = 1;
                break;

            case "Player3Selector":
                playernum = 2;
                break;

            case "Player4Selector":
                playernum = 3;
                break;

            default:
                Debug.Log("Character select playernum error");
                playernum = 0;
                break;
        }


        Inst.PlayerSkills[playernum] = (PlayerController.SkillID)selector.value;

        
    }
}