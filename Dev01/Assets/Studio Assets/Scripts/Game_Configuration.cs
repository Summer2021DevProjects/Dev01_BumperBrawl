using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Game_Configuration : MonoBehaviour
{
    //--- Public Constants ---//
    public const int MAX_PLAYER_COUNT = 4;



    //--- Public Variables ---//
    [Header("Player Configuration")]
    public GameObject m_playerPrefab;
    public GameObject m_aiPrefab;
    [Range(1, 4)] public int m_numRealPlayers;
    public Color[] m_playerColours;

    [Header("Rule Configuration")]
    public int m_gameDurationSec;
    public int m_bumperLives;



    //--- Private Variables ---//
    private static Game_Configuration m_instance;



    //--- Unity Methods ---//
    private void Awake()
    {
        // There should always only be one game configuration
        if (!m_instance)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Hook into the scene loading functionality so it can detect when we enter a game level
            // This way, we can try to spawn players when switching scenes
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Try to spawn players in the current scene as well, in case we are starting in a game level
            TrySpawnPlayers();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDisable()
    {
        if (m_instance != this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    //--- Methods ---//
    public void OnSceneLoaded(Scene _scene, LoadSceneMode _sceneMode)
    {
        TrySpawnPlayers();
    }

    public void TrySpawnPlayers()
    {
        var playerSpawner = FindObjectOfType<Game_Spawner>();

        if (playerSpawner)
        {
            List<GameObject> players = playerSpawner.SpawnPlayers(m_numRealPlayers, m_playerColours, m_playerPrefab, m_aiPrefab);
        }
    }
}
