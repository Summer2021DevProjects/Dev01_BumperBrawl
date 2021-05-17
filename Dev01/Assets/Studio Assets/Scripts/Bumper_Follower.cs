using UnityEngine;
using System.Collections;

public class Bumper_Follower : MonoBehaviour
{
    //--- Private Variables ---//
    private Transform m_parent;
    private Vector3 m_parentOffset;



    //--- Unity Methods ---//
    private void Awake()
    {
        m_parent = this.transform.parent;
        m_parentOffset = this.transform.localPosition;

        this.transform.parent = null;
    }

    private void LateUpdate()
    {
        this.transform.position = m_parent.position + m_parentOffset;
    }
}
