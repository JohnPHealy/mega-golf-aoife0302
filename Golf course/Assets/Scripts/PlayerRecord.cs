using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerRecord : MonoBehaviour
{
    public List<Player> playerList;
    public string[] levels;
    public Color[] playerColours;
    [HideInInspector] public int levelIndex;

    //to make sure list is instantiated 
    void OnEnable()
    {
        playerList = new List<Player>(); //only happens once
        DontDestroyOnLoad(gameObject); //brings player names to next sceene
    }

    //method that menu manager can call to pass user input in the menu text field over to the player record
    public void AddPlayer(string name)
    {
        playerList.Add(new Player(name, playerColours[playerList.Count], levels.Length));
    }

    public void AddPutts(int playerIndex, int PuttCount)
    {
        playerList[playerIndex].putts[levelIndex] = PuttCount;
    }

    //for the scoreboard list to name players and putt score from low to high
    public List<Player> GetScoreboardList()
    {
        foreach (var player in playerList)
        {
            foreach (var puttScore in player.putts)
            {
                player.totalPutts += puttScore;
            }
        }
        return (from p in playerList orderby p.totalPutts select p).ToList();
    }


    public class Player
    {
        public string name;
        public Color colour;
        public int[] putts;
        public int totalPutts;

        public Player (string newName, Color newColor, int levelCount)
        {
            name = newName;
            colour = newColor;
            putts = new int[levelCount];
        }
    }


}
