using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Game_Configuration : MonoBehaviour
{
    //--- Public Constants ---//
    public static Game_Configuration m_instance { get; private set; }
    public readonly int MAX_PLAYER_COUNT = 4;



    //--- Public Variables ---//
    [Header("Player Configuration")]
    public GameObject m_playerPrefab;
    public GameObject m_aiPrefab;
    [Range(1, 4)] public int m_numRealPlayers;
    public Color[] m_playerColours;

    [Header("Rule Configuration")]
    public int m_gameDurationSec;
    public int m_bumperLives;
    public float m_startCountdownDurationSec;



    //--- Unity Methods ---//
    private void Awake()
    {
        // There should always only be one game configuration
        if (!m_instance)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
