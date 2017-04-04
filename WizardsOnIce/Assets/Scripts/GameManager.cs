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

    public int[] playerScores = new int[4];
    public string[] levelList;
    public int nextLevel;

    public Dictionary<string, bool> levelsEnabled;

    public float winScore;

    public Dictionary<int, PlayerController.SkillID> PlayerSkills;
	//public AudioSource click;

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

        for (int i = 0; i < levelList.Length; i++)
        {
            levelsEnabled.Add(levelList[i], true);
        }
    }
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            // Load level 1
            nextLevel = 1;
            LoadNextScene();
        }
    }

	public void LoadNextScene()
	{
       SceneManager.LoadScene(levelList[nextLevel]);
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
            switch(PlayersAlive[0].PlayerNumber)
            {
                case "0":
                    playerScores[0] += 1;
                    break;

                case "1":
                    playerScores[1] += 1;
                    break;

                case "2":
                    playerScores[2] += 1;
                    break;

                case "3":
                    playerScores[3] += 1;
                    break;

                   default:
                    Debug.Log("Error: Can't find players alive");
                    break;
            }

            PlayersAlive.Clear();
            GameOver();

        }
    }

    public void GameOver()
    {
        if (nextLevel <= levelList.Length - 2)
        {
            nextLevel++;
        }
        else
        {
            // Go to level 1
            nextLevel = 1;
        }
        
        for(int i = 0; i < playerScores.Length; i++)
        {
            if (playerScores[i] >= winScore)
            {
                // Go to character select menu
                nextLevel = 0;

                for (int j = 0; j < playerScores.Length; j++)
                {
                    playerScores[j] = 0;
                }

                for (int j = 0; j < PlayerSkills.Count; j++)
                {
                    PlayerSkills[j] = 0;
                }
            }
        }
        LoadNextScene();
    }


    public void SetPlayerSkill(int playerNum, int skillNum)
    {
        Inst.PlayerSkills[playerNum] = (PlayerController.SkillID)skillNum;
    }

    public void LevelSelectToggle(Toggle t)
    {
        levelsEnabled[t.transform.GetChild(0).GetComponent<Text>().text] = t.isOn;
    }

    public void ReconstructLevelList()
    {
        // TODO
    }
}