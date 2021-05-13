using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

//--- Player Data Struct - Keeps track of important game info for each player ---//
public class Game_PlayerData
{
    public Game_PlayerData(int _startingLives, Bumper_Configuration _bumperConfig)
    {
        m_numLives = _startingLives;
        //m_finalPlacing = -1;
        m_isDead = false;
        m_bumperConfig = _bumperConfig;
        m_survivalTime = -1.0f;
    }

    public Bumper_Configuration m_bumperConfig;
    public int m_numLives;
    //public int m_finalPlacing;
    public bool m_isDead;
    public float m_survivalTime;
}

public class Game_Manager : MonoBehaviour
{
    //--- Public Events ---//
    public UnityEvent<int, List<Bumper_Configuration>> OnRoundStart { get; } = new UnityEvent<int, List<Bumper_Configuration>>();
    public UnityEvent<int, int> OnPlayerLifeLost { get; } = new UnityEvent<int, int>();
    public UnityEvent<float> OnTimerChanged { get; } = new UnityEvent<float>();



    //--- Private Variables ---//
    private Dictionary<Bumper_Configuration, Game_PlayerData> m_playerData;
    private Game_Configuration m_config;
    private Game_Spawner m_playerSpawner;
    private int m_playersLeft;
    private float m_countdownStart;
    private bool m_gameCountdownEnabled;
    private float m_gameTimeLeft;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_config = Game_Configuration.m_instance;
        m_playerSpawner = FindObjectOfType<Game_Spawner>();
        m_playersLeft = m_config.MAX_PLAYER_COUNT;
        m_gameCountdownEnabled = false;
        m_countdownStart = (float)m_config.m_gameDurationSec;
        m_gameTimeLeft = m_countdownStart;
    }

    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        // Continue the game clock countdown. If time is up, the game ends
        if (m_gameCountdownEnabled)
        {
            m_gameTimeLeft -= Time.deltaTime;
            OnTimerChanged.Invoke(m_gameTimeLeft);

            if (m_gameTimeLeft <= 0.0f)
                OnGameOver();
        }
    }



    //--- Methods ---//
    public void StartRound()
    {
        TriggerPlayerSpawning();
        m_gameCountdownEnabled = true;
        OnRoundStart.Invoke(m_config.m_startingLives, new List<Bumper_Configuration>(m_playerData.Keys));
    }

    public void OnPlayerDeath(Bumper_Configuration _player)
    {
        // Lower the player life count
        var playerData = m_playerData[_player];
        playerData.m_numLives--;
        OnPlayerLifeLost.Invoke(_player.GetID(), playerData.m_numLives);

        // If the player is out of lives, they are now officially dead
        // Otherwise, they should get respawned
        if (playerData.m_numLives <= 0)
        {
            playerData.m_isDead = true;
            playerData.m_survivalTime = m_countdownStart - m_gameTimeLeft;
            //playerData.m_finalPlacing = m_nextPlacing--;
            m_playersLeft--;

            // If there is only one player left, the game is now over
            if (m_playersLeft <= 1)
                OnGameOver();
        }
        else
        {
            m_playerSpawner.RespawnPlayer(_player);
        }
    }

    public void OnGameOver()
    {
        Debug.Log("GAME OVER");
    }



    //--- Utility Methods ---//
    private void TriggerPlayerSpawning()
    {
        var spawnedPlayers = m_playerSpawner.SpawnPlayers();

        m_playerData = new Dictionary<Bumper_Configuration, Game_PlayerData>();
        foreach (var player in spawnedPlayers)
            m_playerData.Add(player, new Game_PlayerData(m_config.m_startingLives, player));
    }
}
