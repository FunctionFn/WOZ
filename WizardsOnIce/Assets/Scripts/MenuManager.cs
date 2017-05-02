using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public Button[] player1Buttons;
    public Button[] player2Buttons;
    public Button[] player3Buttons;
    public Button[] player4Buttons;

    public Image[] charactersSelected;
    public Sprite[] charactersSelectedSprites;

    public Button startButton;

    public Image[] playerSkillHighlights;

    public int p1CurrentButton;
    public int p2CurrentButton;
    public int p3CurrentButton;
    public int p4CurrentButton;

    public bool p1AxisUsed;
    public bool p2AxisUsed;
    public bool p3AxisUsed;
    public bool p4AxisUsed;

    public Image[] winIndicators;
    public Text[] winCounters;
    // Use this for initialization
    void Start ()
    {
        p1CurrentButton = 0;
        p2CurrentButton = 0;
        p3CurrentButton = 0;
        p4CurrentButton = 0;

        p1AxisUsed = false;
        p2AxisUsed = false;
        p3AxisUsed = false;
        p4AxisUsed = false;

        if(GameManager.Inst.winner != -1)
        {
            winIndicators[GameManager.Inst.winner].enabled = true;
        }

        for(int i = 0; i < GameManager.Inst.playerWins.Length; i++)
        {
            if(GameManager.Inst.playerWins[i] > 0)
            {
                winCounters[i].enabled = true;
                winCounters[i].text = GameManager.Inst.playerWins[i].ToString();
            }
            else
            {
                winCounters[i].enabled = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

        for(int i = 0; i < 4; ++i)
        {
            charactersSelected[i].sprite = charactersSelectedSprites[(int)GameManager.Inst.PlayerSkills[i]];
        }

        // TODO: convert this mess into something readable:
        // make a function readinput that accepts a player number 0-3
        // includes one version of each of the following if's
        // call that function 4 times, once for each player

       

        // Button Selection
        if ((Input.GetAxis("Horizontal0") > 0.5 || Input.GetAxis("DPHorizontal0") > 0.5) && p1CurrentButton < player1Buttons.Length - 1 && !p1AxisUsed)
        {
            ExecuteEvents.Execute(player1Buttons[p1CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            p1CurrentButton++;
            p1AxisUsed = true;
        }
        else if ((Input.GetAxis("Horizontal0") < -0.5 || Input.GetAxis("DPHorizontal0") < -0.5) && p1CurrentButton > 0 && !p1AxisUsed)
        {
            ExecuteEvents.Execute(player1Buttons[p1CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            p1CurrentButton--;
            p1AxisUsed = true;
        }

        if (((Input.GetAxis("Horizontal0") < 0.5 && Input.GetAxis("DPHorizontal0") < 0.5) && (Input.GetAxis("Horizontal0") > -0.5 && Input.GetAxis("DPHorizontal0") > -0.5)) && p1AxisUsed)
        {
            p1AxisUsed = false;
        }

        if ((Input.GetAxis("Horizontal1") > 0.5 || Input.GetAxis("DPHorizontal1") > 0.5) && p2CurrentButton < player2Buttons.Length - 1 && !p2AxisUsed)
        {
            ExecuteEvents.Execute(player2Buttons[p2CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            p2CurrentButton++;
            p2AxisUsed = true;
        }
        else if ((Input.GetAxis("Horizontal1") < -0.5 || Input.GetAxis("DPHorizontal1") < -0.5) && p2CurrentButton > 0 && !p2AxisUsed)
        {
            ExecuteEvents.Execute(player2Buttons[p2CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            p2CurrentButton--;
            p2AxisUsed = true;
        }

        if (((Input.GetAxis("Horizontal1") < 0.5 && Input.GetAxis("DPHorizontal1") < 0.5) && (Input.GetAxis("Horizontal1") > -0.5 && Input.GetAxis("DPHorizontal1") > -0.5)) && p2AxisUsed)
        {
            p2AxisUsed = false;
        }

        if ((Input.GetAxis("Horizontal2") > 0.5 || Input.GetAxis("DPHorizontal2") > 0.5) && p3CurrentButton < player3Buttons.Length - 1 && !p3AxisUsed)
        {
            ExecuteEvents.Execute(player3Buttons[p3CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            p3CurrentButton++;
            p3AxisUsed = true;
        }
        else if ((Input.GetAxis("Horizontal2") < -0.5 || Input.GetAxis("DPHorizontal2") < -0.5) && p3CurrentButton > 0 && !p3AxisUsed)
        {
            ExecuteEvents.Execute(player3Buttons[p3CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            p3CurrentButton--;
            p3AxisUsed = true;
        }

        if (((Input.GetAxis("Horizontal2") < 0.5 && Input.GetAxis("DPHorizontal2") < 0.5) && (Input.GetAxis("Horizontal2") > -0.5 && Input.GetAxis("DPHorizontal2") > -0.5)) && p3AxisUsed)
        {
            p3AxisUsed = false;
        }

        if ((Input.GetAxis("Horizontal3") > 0.5 || Input.GetAxis("DPHorizontal3") > 0.5) && p4CurrentButton < player4Buttons.Length - 1 && !p4AxisUsed)
        {
            ExecuteEvents.Execute(player4Buttons[p4CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            p4CurrentButton++;
            p4AxisUsed = true;
        }
        else if ((Input.GetAxis("Horizontal3") < -0.5 || Input.GetAxis("DPHorizontal3") < -0.5) && p4CurrentButton > 0 && !p4AxisUsed)
        {
            ExecuteEvents.Execute(player4Buttons[p4CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);
            p4CurrentButton--;
            p4AxisUsed = true;
        }

        if (((Input.GetAxis("Horizontal3") < 0.5 && Input.GetAxis("DPHorizontal3") < 0.5) && (Input.GetAxis("Horizontal3") > -0.5 && Input.GetAxis("DPHorizontal3") > -0.5)) && p4AxisUsed)
        {
            p4AxisUsed = false;
        }


        // Button Highlighting
        ExecuteEvents.Execute(player1Buttons[p1CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
        ExecuteEvents.Execute(player2Buttons[p2CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
        ExecuteEvents.Execute(player3Buttons[p3CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
        ExecuteEvents.Execute(player4Buttons[p4CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);

        // Activate Buttons
        if (Input.GetButton("RollDash0"))
        {
            GameManager.Inst.SetPlayerSkill(0, p1CurrentButton);
            ExecuteEvents.Execute(player1Buttons[p1CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            playerSkillHighlights[0].rectTransform.localPosition = new Vector3(player1Buttons[p1CurrentButton].GetComponent<RectTransform>().localPosition.x, playerSkillHighlights[0].rectTransform.localPosition.y, playerSkillHighlights[0].rectTransform.localPosition.z);
        }
        else
        {
            ExecuteEvents.Execute(player1Buttons[p1CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.deselectHandler);
            ExecuteEvents.Execute(player1Buttons[p1CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        }
        if (Input.GetButton("RollDash1"))
        {
            GameManager.Inst.SetPlayerSkill(1, p2CurrentButton);
            ExecuteEvents.Execute(player2Buttons[p2CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            playerSkillHighlights[1].rectTransform.localPosition = new Vector3(player2Buttons[p2CurrentButton].GetComponent<RectTransform>().localPosition.x, playerSkillHighlights[1].rectTransform.localPosition.y, playerSkillHighlights[1].rectTransform.localPosition.z);
        }
        else
        {
            ExecuteEvents.Execute(player2Buttons[p2CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.deselectHandler);
            ExecuteEvents.Execute(player2Buttons[p2CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        }
        if (Input.GetButton("RollDash2"))
        {
            GameManager.Inst.SetPlayerSkill(2, p3CurrentButton);
            ExecuteEvents.Execute(player3Buttons[p3CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            playerSkillHighlights[2].rectTransform.localPosition = new Vector3(player3Buttons[p3CurrentButton].GetComponent<RectTransform>().localPosition.x, playerSkillHighlights[2].rectTransform.localPosition.y, playerSkillHighlights[2].rectTransform.localPosition.z);
        }
        else
        {
            ExecuteEvents.Execute(player3Buttons[p3CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.deselectHandler);
            ExecuteEvents.Execute(player3Buttons[p3CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        }
        if (Input.GetButton("RollDash3"))
        {
            GameManager.Inst.SetPlayerSkill(3, p4CurrentButton);
            ExecuteEvents.Execute(player4Buttons[p4CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
            playerSkillHighlights[3].rectTransform.localPosition = new Vector3(player4Buttons[p4CurrentButton].GetComponent<RectTransform>().localPosition.x, playerSkillHighlights[3].rectTransform.localPosition.y, playerSkillHighlights[3].rectTransform.localPosition.z);
        }
        else
        {
            ExecuteEvents.Execute(player4Buttons[p4CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.deselectHandler);
            ExecuteEvents.Execute(player4Buttons[p4CurrentButton].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        }

        if (Input.GetButtonDown("Start0") || Input.GetButtonDown("Start1") || Input.GetButtonDown("Start2") || Input.GetButtonDown("Start3"))
        {
            // A little janky
            // Load level 1
            if(GameManager.Inst.CheckNumPlayersSelected() >= 2)
            {
                ExecuteEvents.Execute(startButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
                GameManager.Inst.LoadNextScene(1);
            }
            
        }
    }
}
