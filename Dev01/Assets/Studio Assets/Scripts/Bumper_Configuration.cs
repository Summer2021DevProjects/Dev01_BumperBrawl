using UnityEngine;

public class Bumper_Configuration : MonoBehaviour
{
    //--- Public Variables ---//
    public Renderer m_ring;
    public Transform m_characterParent;



    //--- Private Variables ---//
    private int m_id;
    private Color m_color;



    //--- Methods ---//
    public void Init(int _id, Color _color)
    {
        this.m_id = _id;
        this.m_color = _color;

        m_ring.material.color = _color;
        foreach (var charRend in m_characterParent.GetComponentsInChildren<Renderer>())
            charRend.material.color = _color;
    }
}
