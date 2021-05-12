using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game_UI_Player : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Player ID")]
    public TextMeshProUGUI m_txtID;

    [Header("Player Portrait")]
    public Image m_imgPortrait;
    public Image m_imgDeathIndicator;

    [Header("Player Life Count")]
    public TextMeshProUGUI m_txtLifeCount;



    //--- Methods ---//
    public void Init(int _startingLives, Bumper_Configuration _player)
    {
        UpdateLifeCount(_startingLives);

        m_txtID.text = (_player.GetIsAI()) ? "AI" : "P" + (_player.GetID() + 1).ToString();
        m_txtID.color = _player.GetColor();

        m_imgPortrait.color = _player.GetColor();
        m_imgDeathIndicator.enabled = false;
    }

    public void UpdateLifeCount(int _livesRemaining)
    {
        m_txtLifeCount.text = _livesRemaining.ToString();
        m_imgDeathIndicator.enabled = (_livesRemaining <= 0);
    }
}
