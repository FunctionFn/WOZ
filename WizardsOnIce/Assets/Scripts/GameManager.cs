﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour 

{
    public List<PlayerController> PlayersAlive;

    private static GameManager _inst;
    public static GameManager Inst { get { return _inst; } }

    public int[] playerScores = new int[4];
    public string[] levelList;
    public int nextLevel;

    public OrderedDictionary levelsEnabled;

    public float winScore;

    public Dictionary<int, PlayerController.SkillID> PlayerSkills;

    public int winner;
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
        levelsEnabled = new OrderedDictionary();

    }

    void Start()
    {
        Inst.PlayerSkills.Add(0, PlayerController.SkillID.None);
        Inst.PlayerSkills.Add(1, PlayerController.SkillID.None);
        Inst.PlayerSkills.Add(2, PlayerController.SkillID.None);
        Inst.PlayerSkills.Add(3, PlayerController.SkillID.None);

        for (int i = 0; i < levelList.Length; i++)
        {
            Inst.levelsEnabled.Add(levelList[i], true);
        }

        winner = -1;

        
    }
    void Update()
    {
        if(Input.GetButtonUp("Submit") && SceneManager.GetActiveScene().name == "GameSettings")
        {
            ReconstructLevelList();
            if (levelList.Length > 1)
            {
                nextLevel = 0;
                LoadNextScene();
            }
        }

        if (Input.GetButtonUp("Submit") && SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            nextLevel = 0;
            LoadNextScene("GameSettings");
            ResetLevelsSelected();

        }



        //if (Input.GetButtonDown("Submit"))
        //{
        //    // Load level 1
        //    nextLevel = 1;
        //    LoadNextScene();
        //}
    }

	public void LoadNextScene()
	{
       SceneManager.LoadScene(levelList[nextLevel]);
	}
    public void LoadNextScene(int levelNum)
    {
        nextLevel = levelNum;
        SceneManager.LoadScene(levelList[nextLevel]);
    }

    public void LoadNextScene(string name)
    {
        SceneManager.LoadScene(name);
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
                winner = i;
                // Go to character select menu
                nextLevel = 0;

                for (int j = 0; j < playerScores.Length; j++)
                {
                    playerScores[j] = 0;
                }

                for (int j = 0; j < PlayerSkills.Count; j++)
                {
                    PlayerSkills[j] = PlayerController.SkillID.None;
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
        Inst.levelsEnabled[t.transform.GetChild(1).GetComponent<Text>().text] = t.isOn;
    }

    public void ReconstructLevelList()
    {

        int numLevels = 0;
        for (int i = 0; i < levelsEnabled.Count; i++)
        {
            if((bool)levelsEnabled[i])
            {
                numLevels++;
            }
        }


        levelList = new string[numLevels];
        string[] myKeys = new string[levelsEnabled.Count];
        levelsEnabled.Keys.CopyTo(myKeys, 0);

        int n = 0;
        for (int i = 0; i < levelsEnabled.Count; i++)
        {
            if ((bool)levelsEnabled[i])
            {
                levelList[n] = myKeys[i];
                n++;
            }
        }
        // TODO
        Debug.Log("here");
    }

    public void ResetLevelsSelected()
    {
        for (int i = 0; i < levelsEnabled.Count; i++)
        {
            levelsEnabled[i] = true;
        }
    }

    public int CheckNumPlayersSelected()
    {
        int count = 0;
        for (int i = 0; i < PlayerSkills.Count; i++)
        {
            if(PlayerSkills[i] != PlayerController.SkillID.None)
            {
                count++;
            }
        }
        return count;
    }
}