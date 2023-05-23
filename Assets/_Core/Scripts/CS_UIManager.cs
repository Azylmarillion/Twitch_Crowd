using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CS_UIManager : MonoBehaviour
{
    #region F/P
    #region Appel scripts

    [Space, Header("Appel scripts"), Space]
    [SerializeField]
    CS_TwitchConnect twitchConnectScript;
    [SerializeField]
    CS_TwitchID twitchIDScript;
    #endregion

    [SerializeField, Space]
    KeyCode menuKey = KeyCode.Escape;    

    [SerializeField, Space]
    Canvas mainCanvas;

    bool mainCanvasIsActive;

    [HideInInspector]
    public bool TwitchCanvaIsVisible;

    [SerializeField, Space]
    Canvas twitchIDCanvas;

    [Space]
    public TMP_InputField ChannelToConnectInput;

    [Space]
    public TMP_Text TextChannelToConnect;

    [Space]
    public TMP_Text TextHideChannelToConnect;

    [Space]
    public TMP_Text PlaceHolderTextChannelToConnect;

    [Space]
    public TMP_InputField ChannelInterfaceInput;

    [Space]
    public TMP_Text TextChannelInterface;

    [Space]
    public TMP_Text PlaceHolderTextChannelInterface;

    [Space]
    public TMP_InputField ChannelOauthInput;

    [Space]
    public TMP_Text TextOauth;

    [Space]
    public TMP_Text PlaceHolderTextChannelOauth;

    [SerializeField, Space]
    Image ConnectionIndicatorImage;

    [SerializeField]
    Button connectionButton;

    [SerializeField]
    Button disconnectionButton;

    public Toggle ConnectAtStartToggle;

    [SerializeField]
    Toggle showDatas;
    public Toggle ShowDatas => showDatas;
    #endregion

    #region UI behavior
    void ConnectionIndicator()
    {
        if (mainCanvasIsActive)
        {
            if (twitchConnectScript.IsConnect)
            {
                ConnectionIndicatorImage.color = Color.green;
                connectionButton.gameObject.SetActive(false);
                disconnectionButton.gameObject.SetActive(true);
            }

            else if (!twitchConnectScript.IsConnect)
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
        TwitchCanvaIsVisible = false;
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
        TwitchCanvaIsVisible = false;
    }
    public void ShowTwitchIDUI()
    {
        twitchIDCanvas.gameObject.SetActive(true);
        TwitchCanvaIsVisible = true;
    }
    void CheckMainUI()
    {
        if (Input.GetKeyDown(menuKey) && !mainCanvasIsActive)
        {
            mainCanvasIsActive = true;
            ShowNHideMainUI(mainCanvasIsActive);
        }
        else if (Input.GetKeyDown(menuKey) && mainCanvasIsActive)
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