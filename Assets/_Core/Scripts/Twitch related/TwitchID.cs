using UnityEngine;
using System.IO;
using System;

public class TwitchID : MonoBehaviour
{
    #region F/P   

    #region scripts

    [Space, Header("Appel scripts"), Space]
    [SerializeField]
    UIManager uiManager;
    [SerializeField]
    TwitchConnect twitchConnect;

    #endregion    
        
    bool needToConnectAtStart;

    string DatasPath;

    string startConnectPath;

    string channelToConnect;
    public string ChannelToConnect
    {
        get { return channelToConnect; }
    }

    string oauth;
    public string Oauth
    {
        get { return oauth; }
    }

    string interfaceChannel;
    public string InterfaceChannel
    {
        get { return interfaceChannel; }
    }
    #endregion

    #region connection data
    public void ConnectAtStartBehavior(bool _isOn)
    {
        Debug.Log("oui");
        needToConnectAtStart = _isOn;
        string _content = "ConnectAtStart=" + needToConnectAtStart + ";";

        if (File.Exists(startConnectPath))
        {
            File.WriteAllText(startConnectPath, _content);
        }
        else
        {
            File.Create(startConnectPath).Close();
            File.AppendAllText(startConnectPath, _content);
        }
    }

    public void GetDatasFromUI()
    {
        channelToConnect = uiManager.ChannelToConnectInput.text;
        oauth = uiManager.ChannelOauthInput.text;
        interfaceChannel = uiManager.ChannelInterfaceInput.text;

        CreateDatasFile();
    }
    void GetDatasFromFile()
    {
        if (File.Exists(DatasPath))
        {
            string _content = File.ReadAllText(DatasPath);

            //looking for main channel name
            string[] _channelSeparator = new string[] { "channelToConnect=" };
            string[] _channelSplit = _content.Split(_channelSeparator, StringSplitOptions.None);
            int _channelIndex = _channelSplit[1].IndexOf(";", 0);
            string _chanelName = _channelSplit[1].Substring(0, _channelIndex);
            channelToConnect = _chanelName;

            //looking for OAuth token
            string[] _oauthSeparator = new string[] { "oauth=" };
            string[] _oauthSplit = _content.Split(_oauthSeparator, StringSplitOptions.None);
            int _oauthIndex = _oauthSplit[1].IndexOf(";", 0);
            string _oauthToken = _oauthSplit[1].Substring(0, _oauthIndex);
            oauth = _oauthToken;

            //looking for interface channel
            string[] _interfaceSeparator = new string[] { "interfaceChannel=" };
            string[] _interfaceSplit = _content.Split(_interfaceSeparator, StringSplitOptions.None);
            int _interfaceIndex = _interfaceSplit[1].IndexOf(";", 0);
            string _interfaceName = _interfaceSplit[1].Substring(0, _interfaceIndex);
            interfaceChannel = _interfaceName;                  
        }
        if(File.Exists(startConnectPath))
        {
            string _connectContent = File.ReadAllText(startConnectPath);
            //looking if need to connect at start
            if (_connectContent.Contains("ConnectAtStart=True;")) needToConnectAtStart = true;
            else if (_connectContent.Contains("ConnectAtStart=False;")) needToConnectAtStart = false;


            if (needToConnectAtStart == true)
            {
                twitchConnect.ConnectToTwitch();
                twitchConnect.PingTwitch();
                twitchConnect.StartCheck();
                uiManager.ConnectAtStartToggle.isOn = true;
            }
        }
        
    }

    void CreateDatasFile()
    {
        string _content = "channelToConnect=" + channelToConnect + ";\n\n" + "oauth=" + oauth + ";\n\n" + "interfaceChannel=" + interfaceChannel + ";";

        if (!File.Exists(DatasPath))
        {
            File.WriteAllText(DatasPath, "Connection Datas \n\n" + _content);
        }

        else
        {
            File.Create(DatasPath).Close();
            File.AppendAllText(DatasPath, "Connection Datas \n\n" + _content);
        }
    }
    #endregion

    void Init()
    {
        DatasPath = Application.persistentDataPath + "Datas.txt";
        startConnectPath = Application.persistentDataPath + "ConnectStart.txt";
        needToConnectAtStart = false;
    }

    
    #region unity methodes
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        GetDatasFromFile();
    }

    #endregion
}