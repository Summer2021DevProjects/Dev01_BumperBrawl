using UnityEngine;

public class Bumper_VisualsCollision : MonoBehaviour
{
    //--- Public Variables ---//
    public GameObject m_fxPrefab;
    public string m_targetTag;



    //--- Unity Methods ---//
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == m_targetTag)
        {
            Instantiate(m_fxPrefab, collision.contacts[0].point, Quaternion.identity);
        }
    }
}
