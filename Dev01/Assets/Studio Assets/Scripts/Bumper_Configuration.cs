using UnityEngine;
using UnityEngine.UI;

public class Bumper_Configuration : MonoBehaviour
{
    //--- Public Variables ---//
    public Renderer m_ring;
    public Transform m_characterParent;
    public Renderer m_characterEyes;
    public Image m_characterIcon; 



    //--- Private Variables ---//
    private int m_id;
    private Color m_color;
    private bool m_isAI;
    private string m_name;
    private Vector3 m_iconOffset;
    private Transform m_iconParent;



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

        GetComponentInChildren<Bumper_DashFX>().SetRingColour(_color);

        m_characterIcon.color = _color;

        //m_iconParent = m_characterIcon.GetComponentInParent<Canvas>().transform.parent;
        //m_iconOffset = m_characterIcon.GetComponentInParent<Canvas>().transform.localPosition;
        //m_characterIcon.GetComponentInParent<Canvas>().transform.parent = null;
    }

    private void LateUpdate()
    {
        m_characterIcon.GetComponentInParent<Canvas>().transform.forward = Camera.main.transform.position - m_characterIcon.transform.position;
        //m_characterIcon.GetComponentInParent<Canvas>().transform.position = m_iconParent.transform.position + (2.0f * m_iconOffset);
    }



    //--- Setters and Getters ---//
    public int GetID() { return m_id; }
    public Color GetColor() { return m_color; }
    public bool GetIsAI() { return m_isAI; }
    public string GetName() { return m_name; }
}
