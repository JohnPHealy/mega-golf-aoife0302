using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public BallController ball;
    public TextMeshProUGUI labelPlayerName; //to display player name in top Right corner

    private PlayerRecord playerRecord;
    private int playerIndex;

    void Start()
    {
        playerRecord = GameObject.Find("Player Record").GetComponent<PlayerRecord>();
        playerIndex = 0;
        SetupPlayer();
    }

    private void SetupPlayer()
    {
        ball.SetupBall(playerRecord.playerColours[playerIndex]);
        labelPlayerName.text = playerRecord.playerList[playerIndex].name;
    }

    public void NextPlayer(int previousPutts)
    {
        playerRecord.AddPutts(playerIndex, previousPutts);
        if (playerIndex < playerRecord.playerList.Count-1)
        {
            playerIndex++;
            SetupPlayer();
        }
        else
        {
            if(playerRecord.levelIndex == playerRecord.levels.Length-1)
            {
                SceneManager.LoadScene("Scoreboard"); //load the scoreboard scene
            }
            else
            {
                playerRecord.levelIndex++;
                SceneManager.LoadScene(playerRecord.levels[playerRecord.levelIndex]);
            }
        }
    }
}
