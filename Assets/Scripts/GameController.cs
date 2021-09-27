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

    public GameObject startInfo;

    public int moveCount;

    private void Awake()
    {
        SetGameControllerReference();
        playerSide = "X";
        gameOverPanel.SetActive(false);
        moveCount = 0;
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

    public void EndTurn()
    {
        moveCount++; 
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
        {
            GameOver(playerSide);
        }
        if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
        {
            GameOver(playerSide);
        }
        if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }
        if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
        {
            GameOver(playerSide);
        }
        if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }
        if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
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
        playerSide = (playerSide.Equals("X")) ? "O" : "X";

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
            SetPlayerColors(playerX, playerO);
        }
        else
        {
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

        for (int i =0;i<buttonList.Length;i++)
        {
            buttonList[i].text = "";
           
        }

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