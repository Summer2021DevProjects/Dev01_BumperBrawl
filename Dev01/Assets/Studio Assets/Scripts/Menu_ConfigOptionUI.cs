using UnityEngine;
using TMPro;

public class Menu_ConfigOptionUI : MonoBehaviour
{
    //--- Public Variables ---//
    public Menu_OptionTypes m_optionType;
    public TextMeshProUGUI m_txtValue;



    //--- Private Variables ---//
    private Menu_Controller m_menuController;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_menuController = FindObjectOfType<Menu_Controller>();
    }



    //--- Methods ---//
    public void SetValue(string _newText)
    {
        m_txtValue.text = _newText;
    }

    public void OnOptionChanged(bool _moveForward)
    {
        m_menuController.OnConfigOptionChanged(this, _moveForward);
    }
}
