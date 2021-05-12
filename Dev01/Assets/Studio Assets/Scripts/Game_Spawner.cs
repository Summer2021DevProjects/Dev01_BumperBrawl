using UnityEngine;
using System.Collections.Generic;

public class Game_Spawner : MonoBehaviour
{
    //--- Public Variables ---//
    public Transform[] m_spawnLocations;



    //--- Methods ---//
    public List<GameObject> SpawnPlayers(int _numRealPlayers, Color[] _playerColors, GameObject _playerPrefab, GameObject _aiPrefab)
    {
        var spawnedBumpers = new List<GameObject>();

        for (int i = 0; i < Game_Configuration.MAX_PLAYER_COUNT; i++)
        {
            // Spawn the object
            var spawnLoc = m_spawnLocations[i];
            GameObject objectToSpawn = (i < _numRealPlayers) ? _playerPrefab : _aiPrefab;
            var objectInstance = Instantiate(objectToSpawn, spawnLoc.position, spawnLoc.rotation);
            spawnedBumpers.Add(objectInstance);

            // Tell the player (or AI) what Id they are
            var bumperConfig = objectInstance.GetComponent<Bumper_Configuration>();
            bumperConfig.Init(i, _playerColors[i]);
        }

        return spawnedBumpers;
    }
}
