using UnityEngine;

public class Game_DeathTrigger : MonoBehaviour
{
    //--- Private Variables ---//
    private Game_Manager m_manager;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_manager = FindObjectOfType<Game_Manager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect when a player has hit the death trigger
        if (other.TryGetComponent<Bumper_Configuration>(out var bumperInfo))
            m_manager.OnPlayerDeath(bumperInfo);
    }
}
