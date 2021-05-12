using UnityEngine;

public class Bumper_Configuration : MonoBehaviour
{
    //--- Public Variables ---//
    public Renderer m_ring;
    public Transform m_characterParent;



    //--- Private Variables ---//
    private int m_id;
    private Color m_color;
    private bool m_isAI;



    //--- Methods ---//
    public void Init(int _id, Color _color, bool _isAI)
    {
        this.m_id = _id;
        this.m_color = _color;
        this.m_isAI = _isAI;

        m_ring.material.color = _color;
        foreach (var charRend in m_characterParent.GetComponentsInChildren<Renderer>())
            charRend.material.color = _color;
    }



    //--- Setters and Getters ---//
    public int GetID() { return m_id; }
}
