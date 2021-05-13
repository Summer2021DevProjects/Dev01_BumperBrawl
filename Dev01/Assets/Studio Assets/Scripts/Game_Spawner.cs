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
            bool isAI = (i >= config.m_numRealPlayers);
            GameObject objectToSpawn = (isAI) ? config.m_aiPrefab : config.m_playerPrefab;
            var objectInstance = Instantiate(objectToSpawn, spawnLoc.position, spawnLoc.rotation);

            // Tell the player (or AI) what id they are
            var bumperConfig = objectInstance.GetComponent<Bumper_Configuration>();
            bumperConfig.Init(i, config.m_playerColours[i], isAI);
            spawnedBumpers.Add(bumperConfig);
        }

        return spawnedBumpers;
    }

    public void RespawnPlayer(Bumper_Configuration _player)
    {
        _player.GetComponent<Bumper_Controls>().ResetValues();

        var spawnLoc = m_spawnLocations[_player.GetID()];
        _player.transform.position = spawnLoc.position;
        _player.transform.rotation = spawnLoc.rotation;

    }
}
