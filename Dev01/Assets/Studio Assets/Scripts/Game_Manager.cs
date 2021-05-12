using UnityEngine;
using System.Collections.Generic;

public class Game_Manager : MonoBehaviour
{
    //--- Player Data Struct - Keeps track of important game info for each player ---//
    private struct Game_PlayerData
    {
        int m_id;
        Color m_color;
        bool m_isAI;
        int m_numLives;
        int m_finalPlacing;
    }



    //--- Private Variables ---//
    private Game_Spawner m_playerSpawner;
    private Dictionary<Bumper_Configuration, Game_PlayerData> m_playerData;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_playerSpawner = FindObjectOfType<Game_Spawner>();
    }

    private void Start()
    {
        StartRound();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Detection With: " + other.name);
    }



    //--- Methods ---//
    public void StartRound()
    {
        var spawnedPlayers = m_playerSpawner.SpawnPlayers();
    }

    public void OnPlayerDeath(Bumper_Configuration _player)
    {
        Debug.Log(_player.name + " has died");
    }
}
