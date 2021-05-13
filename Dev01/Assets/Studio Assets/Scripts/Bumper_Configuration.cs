using UnityEngine;

public class Bumper_Configuration : MonoBehaviour
{
    //--- Public Variables ---//
    public Renderer m_ring;
    public Transform m_characterParent;
    public Renderer m_characterEyes;



    //--- Private Variables ---//
    private int m_id;
    private Color m_color;
    private bool m_isAI;
    private string m_name;



    //--- Methods ---//
    public void Init(int _id, Color _color, bool _isAI)
    {
        this.m_id = _id;
        this.m_color = _color;
        this.m_isAI = _isAI;
        this.m_name = (_isAI) ? "AI" : "P" + (this.m_id + 1).ToString();

        m_ring.material.color = _color;
        m_characterEyes.material.SetColor("_EmissionColor", _color);
        foreach (var charRend in m_characterParent.GetComponentsInChildren<Renderer>())
            charRend.material.color = _color;
    }



    //--- Setters and Getters ---//
    public int GetID() { return m_id; }
    public Color GetColor() { return m_color; }
    public bool GetIsAI() { return m_isAI; }
    public string GetName() { return m_name; }
}
