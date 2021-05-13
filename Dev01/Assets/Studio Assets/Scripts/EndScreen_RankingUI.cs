using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreen_RankingUI : MonoBehaviour
{
    //--- Public Variables ---//
    public TextMeshProUGUI m_txtRanking;
    public Image m_imgPlayerPortrait;
    public TextMeshProUGUI m_txtPlayerName;
    public TextMeshProUGUI m_txtStats;



    //--- Methods ---//
    public void Init(EndScreen_Ranking _ranking)
    {
        // Style the ranking text
        m_txtRanking.text = _ranking.m_rankStyling.m_rankStr;
        m_txtRanking.color = _ranking.m_rankStyling.m_rankColor;

        // Style the player portrait
        m_imgPlayerPortrait.color = _ranking.m_result.m_bumperConfig.GetColor();

        // Style the player name
        m_txtPlayerName.text = _ranking.m_result.m_bumperConfig.GetName();
        m_txtPlayerName.color = _ranking.m_result.m_bumperConfig.GetColor();

        // If the player survived, show their final life count
        // If they died, show how long they survived instead
        if (_ranking.m_result.m_isDead)
            m_txtStats.text = "Time Survived (s): " + _ranking.m_result.m_survivalTime.ToString("F2");
        else
            m_txtStats.text = "Lives Left: " + _ranking.m_result.m_numLives.ToString();
    }
}
