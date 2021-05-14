using UnityEngine;

public class Util_FXAutoDestroy : MonoBehaviour
{
    //--- Private Variables ---//
    private ParticleSystem[] m_particleSystems;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {
        // If any of the systems are still playing, just back out
        foreach(var system in m_particleSystems)
        {
            if (system.isPlaying)
                return;
        }

        // If we got here, none of the systems are playing and so we can safely destroy this
        Destroy(this.gameObject);
    }
}
