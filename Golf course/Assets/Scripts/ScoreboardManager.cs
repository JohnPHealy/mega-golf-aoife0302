using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreboardManager : MonoBehaviour
{
    public TextMeshProUGUI names, putts;

    private PlayerRecord playerRecord;

    void Start()
    {
        playerRecord = GameObject.Find("Player Record").GetComponent<PlayerRecord>();
        names.text = "";
        putts.text = "";
        foreach (var player in playerRecord.GetScoreboardList())
        {
            names.text += player.name + "\n";
            putts.text += player.totalPutts + "\n";

        }
    }

    void Update()
    {
        putts.fontSize = names.fontSize;
    }

    public void ButtonReturnMenu()
    {
        Destroy(playerRecord.gameObject); //deletes previous player records otherwise would be kept
        SceneManager.LoadScene("Menu");
    }
}
