using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField inputPlayerName;
    public PlayerRecord playerRecord;
    public Button buttonStart;
    public Button buttonAddPlayer;

    public void ButtonAddPlayer()
    {
        playerRecord.AddPlayer(inputPlayerName.text);
        //make the play button interactable only once the user has entered at least one player name because we don't want the player to be able to start the game without any players being added
        buttonStart.interactable = true;
        inputPlayerName.text = "";
        //stop people from adding more players than what is the limit
        if(playerRecord.playerList.Count == playerRecord.playerColours.Length)
        {
            buttonAddPlayer.interactable = false;
        }
    }
    public void ButtonStart()
    {
        SceneManager.LoadScene(playerRecord.levels[0]);
    }

}
