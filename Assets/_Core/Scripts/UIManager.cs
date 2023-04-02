using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    #region F/P
    #region scripts

    [Space, Header("Appel scripts"), Space]
    [SerializeField]
    TwitchConnect twitchConnectScript;

    [SerializeField]
    TwitchID twitchIDScript;
    #endregion

    [SerializeField, Space]
    KeyCode menuKey= KeyCode.Escape;

    [SerializeField, Space]
    GameObject toggleHideNShowDatas;

    [SerializeField, Space]
    Canvas mainCanvas;
    bool mainCanvasIsActive;

    [SerializeField, Space]
    Canvas twitchIDCanvas;
    [Space]
    public TMP_InputField ChannelToConnectInput;
    [Space]
    public TMP_InputField ChannelInterfaceInput;
    [Space]
    public TMP_InputField ChannelOauthInput;
    [SerializeField]
    Image ConnectionIndicatorImage;
    [SerializeField]
    Button connectionButton;
    [SerializeField]
    Button disconnectionButton;
    public Toggle ConnectAtStartToggle;

    #endregion

    #region UI behavior
    void ConnectionIndicator()
    {
        if (mainCanvasIsActive == true)
        {
            if (twitchConnectScript.IsConnect == true)
            {
                ConnectionIndicatorImage.color = Color.green;
                connectionButton.gameObject.SetActive(false);
                disconnectionButton.gameObject.SetActive(true);
            }

            else if (twitchConnectScript.IsConnect == false)
            {
                ConnectionIndicatorImage.color = Color.red;
                connectionButton.gameObject.SetActive(true);
                disconnectionButton.gameObject.SetActive(false);
            }

            else return;
        }
    }
    void Init()
    {
        mainCanvasIsActive = false;
        HideMainUI();
        HideTwitchIDUI();
    }
    public void ShowFileExplorer()
    {
        Application.OpenURL(@"file://C:");
    }
    public void ShowOAuthTokenOnWeb()
    {
        Application.OpenURL("https://twitchapps.com/tmi/");
    }
    public void HideTwitchIDUI()
    {
        twitchIDCanvas.gameObject.SetActive(false);
    }
    public void ShowTwitchIDUI()
    {
        twitchIDCanvas.gameObject.SetActive(true);
    }
    void CheckMainUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && mainCanvasIsActive == false)
        {
            mainCanvasIsActive = true;
            ShowNHideMainUI(mainCanvasIsActive);
        }
        else if (Input.GetKeyDown(menuKey) && mainCanvasIsActive == true)
        {
            mainCanvasIsActive = false;
            ShowNHideMainUI(mainCanvasIsActive);
            HideTwitchIDUI();
        }
        else return;
    }
    void ShowNHideMainUI(bool _isActive)
    {
        mainCanvas.gameObject.SetActive(_isActive);
    }
    public void HideMainUI()
    {
        mainCanvas.gameObject.SetActive(false);
    }
    public void ToggleConnectBehavior()
    {
        bool _connectTmp = ConnectAtStartToggle.isOn;
        twitchIDScript.ConnectAtStartBehavior(_connectTmp);
    }
    #endregion

    #region unity methodes
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        CheckMainUI();        
    }
    private void FixedUpdate()
    {
        ConnectionIndicator();
    }

    #endregion
}