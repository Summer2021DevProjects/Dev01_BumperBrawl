using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Game_UI : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Timer")]
    public TextMeshProUGUI m_txtTimer;

    [Header("Players")]
    public Game_UI_Player[] m_playerUIs;



    //--- Private Variables ---//
    private Game_Manager m_gameManager;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_gameManager = FindObjectOfType<Game_Manager>();
        m_gameManager.OnRoundStart.AddListener(this.OnRoundStart);
        m_gameManager.OnPlayerLifeLost.AddListener(this.OnPlayerLifeLost);
        m_gameManager.OnTimerChanged.AddListener(this.OnTimerChanged);
    }

    private void OnDestroy()
    {
        m_gameManager.OnRoundStart.RemoveListener(this.OnRoundStart);
        m_gameManager.OnPlayerLifeLost.RemoveListener(this.OnPlayerLifeLost);
        m_gameManager.OnTimerChanged.RemoveListener(this.OnTimerChanged);
    }



    //--- Methods ---//
    public void OnRoundStart(int _startingLives, List<Bumper_Configuration> _players)
    {
        for (int i = 0; i < m_playerUIs.Length; i++)
            m_playerUIs[i].Init(_startingLives, _players[i]);
    }

    public void OnPlayerLifeLost(int _playerID, int _livesRemaining)
    {
        m_playerUIs[_playerID].UpdateLifeCount(_livesRemaining);
    }

    public void OnTimerChanged(float _newTime)
    {
        int newTimeSeconds = Mathf.CeilToInt(_newTime);
        m_txtTimer.text = newTimeSeconds.ToString();
    }
}
