using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    private string playerSide;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartButton;

    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayerColor;
    public PlayerColor inactivePlayerColor;

    private string computerSide;
    public float delay;
    int value; //box indx
    public bool playerMove; 

    public GameObject startInfo;

    public int moveCount;
    public LineRenderer lineRenderer;

    private void Awake()
    {
        SetGameControllerReference();
        playerSide = "X";
        gameOverPanel.SetActive(false);
        moveCount = 0;

        playerMove = true;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (playerMove == false)
        {
            delay += delay * Time.deltaTime;
            if (delay >= 100)
            {
                value = Random.Range(0, 8);
                if (buttonList[value].GetComponentInParent<Button>().interactable == true)
                {
                    buttonList[value].text = computerSide;
                    buttonList[value].GetComponentInParent<Button>().interactable = false;
                    EndTurn();
                    
                }
            }
        }
    }

    void DrawLine(int i, int k)
    {
        lineRenderer.SetPosition(0, new Vector3(buttonList[i].transform.position.x,
            buttonList[i].transform.position.y, -9));
        lineRenderer.SetPosition(1, new Vector3(buttonList[k].transform.position.x,
            buttonList[k].transform.position.y, -9));
        Color color = (playerSide.Equals("X")) ? Color.green : Color.yellow;
        color.a = .3f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = Color.white;

        lineRenderer.enabled = true;
    }

    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerButtons(false);
        startInfo.SetActive(false);
    }

    void SetGameControllerReference()
    {
        for (int i = 0;i<buttonList.Length;i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameController(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public string GetComputerSide()
    {
        return computerSide;
    }

    public void EndTurn()
    {
        moveCount++;
        delay = 10;
        if (CheckWin(playerSide))
        {
            GameOver(playerSide);
        }

        if (CheckWin(computerSide))
        {
            GameOver(computerSide);
        }

        if (moveCount >= 9 )
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = "END";
            GameOver("draw");
        }

        Debug.Log("END TURN");
        ChangeSides();
    }

    bool CheckWin(string turn)
    {
        return AreBoxMatch(0,1,2,turn) || AreBoxMatch(3,4,5,turn) || AreBoxMatch(6,7,8,turn) ||
            AreBoxMatch(0,3,6,turn) || AreBoxMatch(1,4,7,turn) || AreBoxMatch(2,5,8,turn) ||
            AreBoxMatch(0,4,8,turn) || AreBoxMatch(2,4,6,turn);
    }

    bool AreBoxMatch(int i, int j, int k, string turn)
    {
        bool matched = (buttonList[i].text==turn 
            && buttonList[j].text==turn 
            && buttonList[k].text==turn);
        if (matched)
        {
            DrawLine(i,k);
        }
        return matched;
    }

    void GameOver(string winPlayer)
    {
        SetBoardInteractable(false);
        if(winPlayer == "draw") { 
            SetPlayerColorsInactive();
            gameOverText.text = "END";
        }
        else
        {
            Debug.Log("Game Over");
            gameOverPanel.SetActive(true);
            gameOverText.text = playerSide + "    Win!";
            restartButton.SetActive(true);
        }
    }

    void SetPlayerColors(Player nPlayer, Player oPlayer)
    {
        nPlayer.panel.color = activePlayerColor.panelColor;
        oPlayer.panel.color = inactivePlayerColor.panelColor;

        nPlayer.text.color = activePlayerColor.textColor;
        oPlayer.text.color = inactivePlayerColor.textColor;
    }

    void SetPlayerColorsInactive()
    {
        playerX.panel.color = inactivePlayerColor.panelColor;
        playerX.panel.color = inactivePlayerColor.panelColor;

        playerO.text.color = inactivePlayerColor.textColor;
        playerO.text.color = inactivePlayerColor.textColor;
    }

    void SetPlayerButtons(bool toggle)
    {
        playerX.button.interactable = toggle;
        playerO.button.interactable = toggle;
    }

    void ChangeSides()
    {
        //playerSide = (playerSide.Equals("X")) ? "O" : "X";

        playerMove = (playerMove == true) ? false : true;
        
        if(playerSide == "X")
        {
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            SetPlayerColors(playerO, playerX);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if (playerSide == "X")
        {
            computerSide = "O";
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            computerSide = "X";
            SetPlayerColors(playerO, playerX);
        }

        StartGame();
    }

    public void Restart()
    {
        playerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);
        restartButton.SetActive(false);
        SetPlayerButtons(true);
        startInfo.SetActive(true);
        playerMove = true;
        delay = 10;

        for (int i =0;i<buttonList.Length;i++)
        {
            buttonList[i].text = "";
           
        }

        lineRenderer.enabled = false;

    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i =0;i<buttonList.Length;i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }
}

[System.Serializable]
public class Player
{
    public Image panel;
    public Text text;
    public Button button;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor;
    public Color textColor;
}