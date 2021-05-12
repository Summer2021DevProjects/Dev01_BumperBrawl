using UnityEngine;
using System.Collections.Generic;

public class Game_Spawner : MonoBehaviour
{
    //--- Public Variables ---//
    public Transform[] m_spawnLocations;



    //--- Methods ---//
    public List<Bumper_Configuration> SpawnPlayers()
    {
        var config = Game_Configuration.m_instance;
        var spawnedBumpers = new List<Bumper_Configuration>();

        for (int i = 0; i < config.MAX_PLAYER_COUNT; i++)
        {
            // Spawn the object
            var spawnLoc = m_spawnLocations[i];
            GameObject objectToSpawn = (i < config.m_numRealPlayers) ? config.m_playerPrefab : config.m_aiPrefab;
            var objectInstance = Instantiate(objectToSpawn, spawnLoc.position, spawnLoc.rotation);

            // Tell the player (or AI) what id they are
            var bumperConfig = objectInstance.GetComponent<Bumper_Configuration>();
            bumperConfig.Init(i, config.m_playerColours[i]);
            spawnedBumpers.Add(bumperConfig);
        }

        return spawnedBumpers;
    }

    //public List<Bumper_Configuration> SpawnPlayers(int _numRealPlayers, Color[] _playerColors, GameObject _playerPrefab, GameObject _aiPrefab)
    //{
    //    var spawnedBumpers = new List<Bumper_Configuration>();

    //    for (int i = 0; i < 4/*i < Game_Configuration.MAX_PLAYER_COUNT*/; i++)
    //    {
    //        // Spawn the object
    //        var spawnLoc = m_spawnLocations[i];
    //        GameObject objectToSpawn = (i < _numRealPlayers) ? _playerPrefab : _aiPrefab;
    //        var objectInstance = Instantiate(objectToSpawn, spawnLoc.position, spawnLoc.rotation);

    //        // Tell the player (or AI) what Id they are
    //        var bumperConfig = objectInstance.GetComponent<Bumper_Configuration>();
    //        bumperConfig.Init(i, _playerColors[i]);
    //        spawnedBumpers.Add(bumperConfig);
    //    }

    //    return spawnedBumpers;
    //}
}
