using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class EndScreen_Ranking
{
    public EndScreen_Ranking(Game_PlayerData _playerResult)
    {
        m_result = _playerResult;
        m_finalRanking = -1;
    }

    public override bool Equals(object obj)
    {
        var otherRanking = obj as EndScreen_Ranking;
        return (this.m_result.m_numLives == otherRanking.m_result.m_numLives &&
                this.m_result.m_survivalTime == otherRanking.m_result.m_survivalTime);
    }

    public Game_PlayerData m_result;
    public int m_finalRanking;
    public EndScreen_RankStyle m_rankStyling;
}

[System.Serializable]
public struct EndScreen_RankStyle
{
    public string m_rankStr;
    public Color m_rankColor;
}

public class EndScreen_Controller : MonoBehaviour
{
    //--- Public Variables ---//
    public string m_menuSceneName;
    public EndScreen_RankingUI[] m_rankingUIs;
    public EndScreen_RankStyle[] m_rankStyles;


    //--- Private Variables ---//
    private static List<Game_PlayerData> m_gameResults;



    //--- Unity Methods ---//
    private void Start()
    {
        // DEBUG: A bunch of made-up data for testing. Should be able to delete soon
        //List<Game_PlayerData> res = new List<Game_PlayerData>();

        //var bumperConfig = new Bumper_Configuration();
        //bumperConfig.Init(0, Color.red, false);
        //var playerData = new Game_PlayerData(3, bumperConfig);
        //playerData.m_numLives = 0;
        //playerData.m_isDead = true;
        //playerData.m_survivalTime = 15.6f;
        //res.Add(playerData);

        //bumperConfig = new Bumper_Configuration();
        //bumperConfig.Init(1, Color.blue, false);
        //playerData = new Game_PlayerData(3, bumperConfig);
        //playerData.m_numLives = 3;
        //playerData.m_survivalTime = -1.0f;
        //res.Add(playerData);

        //bumperConfig = new Bumper_Configuration();
        //bumperConfig.Init(2, Color.green, true);
        //playerData = new Game_PlayerData(3, bumperConfig);
        //playerData.m_numLives = 1;
        //playerData.m_survivalTime = -1.0f;
        //res.Add(playerData);

        //bumperConfig = new Bumper_Configuration();
        //bumperConfig.Init(3, Color.yellow, true);
        //playerData = new Game_PlayerData(3, bumperConfig);
        //playerData.m_numLives = 2;
        //playerData.m_survivalTime = -1.0f;
        //res.Add(playerData);

        //StoreGameResults(res);

        DetermineRankings();
    }



    //--- Methods ---//
    public static void StoreGameResults(List<Game_PlayerData> _gameResults)
    {
        m_gameResults = _gameResults;
    }

    public void DetermineRankings()
    {
        // Sort the results by the number of lives left and by the amount of time survived
        // Then, reverse the list so the best player is at the front
        var sortedResults = m_gameResults.OrderBy(x => x.m_numLives).ThenBy(x => x.m_survivalTime).ToList();
        sortedResults.Reverse();

        // Port the results over into the new structure so we can determine the final rankings from it
        EndScreen_Ranking[] finalRankings = new EndScreen_Ranking[m_gameResults.Count];
        for (int i = 0; i < sortedResults.Count; i++)
            finalRankings[i] = new EndScreen_Ranking(sortedResults[i]);

        // Check for any ties and then assign the final rankings (1st, 2nd, 3rd, 4th)
        int rankToAssign = 1;
        int nextPlayerToAssignRank = 0;

        while (nextPlayerToAssignRank < finalRankings.Length)
        {
            // Give the rank to this player since they are the first in this new tier
            var firstPlayerOfTier = finalRankings[nextPlayerToAssignRank];
            firstPlayerOfTier.m_finalRanking = rankToAssign;
            nextPlayerToAssignRank++;

            // Check if any other players tied and so they belong in this tier as well
            int numTiedPlayers = 0;
            for (int otherPlayerId = nextPlayerToAssignRank; otherPlayerId < finalRankings.Length; otherPlayerId++)
            {
                var otherPlayer = finalRankings[otherPlayerId];

                // If the other player had the exact same final stats, they tied and so belong in this same ranking tier
                if (firstPlayerOfTier.Equals(otherPlayer))
                {
                    otherPlayer.m_finalRanking = rankToAssign;
                    numTiedPlayers++;
                    nextPlayerToAssignRank++;
                }
            }

            // If there were any additional players in the tier, we have to skip one or more ranks for the next tier
            rankToAssign += (numTiedPlayers + 1);
        }

        // Assign the final rank styling based on all the players' rankings
        foreach (var finalRank in finalRankings)
            finalRank.m_rankStyling = m_rankStyles[finalRank.m_finalRanking - 1];

        foreach(var finalRanking in finalRankings)
        {
            Debug.Log(finalRanking.m_finalRanking + " - " + finalRanking.m_result.m_numLives + " lives - " + finalRanking.m_result.m_survivalTime + " (s) survived");
        }

        ShowRankings(finalRankings);
    }

    public void ShowRankings(EndScreen_Ranking[] _finalRankings)
    {
        for (int i = 0; i < m_rankingUIs.Length; i++)
            m_rankingUIs[i].Init(_finalRankings[i]);
    }



    //--- UI Callbacks ---//
    public void OnMenuPressed()
    {
        SceneManager.LoadScene(m_menuSceneName);
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
