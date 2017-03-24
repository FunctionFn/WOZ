﻿using UnityEngine;
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

    public float winScore;

    public Dictionary<int, PlayerController.SkillID> PlayerSkills;
	public AudioSource click;

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
            }
        }
        LoadNextScene();
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