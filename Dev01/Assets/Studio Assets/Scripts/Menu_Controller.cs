using UnityEngine;
using UnityEngine.SceneManagement;

public enum Menu_OptionTypes
{
    NumPlayers,
    Timer,
    NumLives,
    Level,

    Count
}

public class Menu_Controller : MonoBehaviour
{
    [System.Serializable]
    public class Menu_ConfigInfo
    {
        public string m_name;
        public Menu_OptionTypes m_optionType;
        public string[] m_options;

        private int m_currentOption;

        public Menu_ConfigInfo()
        {
            m_currentOption = 0;
        }

        public string GetNextOption(bool _moveForward)
        {
            m_currentOption += (_moveForward) ? 1 : -1;

            if (m_currentOption < 0)
                m_currentOption = m_options.Length - 1;
            else if (m_currentOption >= m_options.Length)
                m_currentOption = 0;

            return m_options[m_currentOption];
        }

        public string GetValue()
        {
            return m_options[m_currentOption];
        }

        public int GetValueAsInt()
        {
            return int.Parse(GetValue());
        }
    }



    //--- Public Variables ---//
    public Menu_ConfigInfo[] m_options;



    //--- Private Variables ---//
    private Menu_ConfigOptionUI[] m_configOptionControls;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_configOptionControls = GetComponentsInChildren<Menu_ConfigOptionUI>();
        for (int i = 0; i < m_configOptionControls.Length; i++)
            m_configOptionControls[i].SetValue(m_options[i].GetValue());
    }



    //--- UI Callbacks ---//
    public void OnConfigOptionChanged(Menu_ConfigOptionUI _optionControl, bool _moveForward)
    {
        int optionID = (int)_optionControl.m_optionType;
        string newValue = m_options[optionID].GetNextOption(_moveForward);

        _optionControl.SetValue(newValue);
    }

    public void OnPlay()
    {
        // Pass the set values to the game config so it can actually control the game logic
        var config = Game_Configuration.m_instance;
        config.m_numRealPlayers =   m_options[(int)Menu_OptionTypes.NumPlayers].GetValueAsInt();
        config.m_gameDurationSec =  m_options[(int)Menu_OptionTypes.Timer].GetValueAsInt();
        config.m_startingLives =    m_options[(int)Menu_OptionTypes.NumLives].GetValueAsInt();

        // Load the correct game scene
        SceneManager.LoadScene(m_options[(int)Menu_OptionTypes.Level].GetValue());
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
