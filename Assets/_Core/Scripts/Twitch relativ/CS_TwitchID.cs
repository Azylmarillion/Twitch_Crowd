using UnityEngine;
using System.IO;
using System;

public class CS_TwitchID : MonoBehaviour
{
    #region F/P
    bool needToConnectAtStart;
    #region Appel scripts

    [Space, Header("Appel scripts"), Space]
    [SerializeField]
    CS_UIManager uiManager;

    [SerializeField]
    CS_TwitchConnect twitchConnect;

    #endregion    

    string path;

    string connectPath;

    string channelToConnect;

    public string ChannelToConnect => channelToConnect;

    string oauth;

    public string Oauth => oauth;    

    string interfaceChannel;

    public string InterfaceChannel => interfaceChannel;     
    #endregion

    #region connection data
    public void ConnectAtStartBehavior(bool _isOn)
    {
        needToConnectAtStart = _isOn;
        string _content = "ConnectAtStart=" + needToConnectAtStart + ";";

        if (File.Exists(connectPath))
        {
            File.WriteAllText(connectPath, _content);                    
        }
        else
        {
            File.Create(connectPath).Close();
            File.AppendAllText(connectPath, _content);
        }
    }

    public void ShowFileDatas()
    {
        if (!File.Exists(path)) return;
               
        if (!uiManager.ShowDatas.isOn)
        {
            uiManager.PlaceHolderTextChannelInterface.text = "*******";
            uiManager.PlaceHolderTextChannelOauth.text = "*******";
            uiManager.PlaceHolderTextChannelToConnect.text = "*******";
        }
        else if(uiManager.ShowDatas.isOn)
        {
            string _content = File.ReadAllText(path);

            //looking for main channel name
            string[] _channelSeparator = new string[] { "channelToConnect=" };
            string[] _channelSplit = _content.Split(_channelSeparator, StringSplitOptions.None);
            int _channelIndex = _channelSplit[1].IndexOf(";", 0);
            string _chanelName = _channelSplit[1].Substring(0, _channelIndex);
            uiManager.PlaceHolderTextChannelToConnect.text = _chanelName;

            //looking for OAuth token
            string[] _oauthSeparator = new string[] { "oauth=" };
            string[] _oauthSplit = _content.Split(_oauthSeparator, StringSplitOptions.None);
            int _oauthIndex = _oauthSplit[1].IndexOf(";", 0);
            string _oauthToken = _oauthSplit[1].Substring(0, _oauthIndex);
            uiManager.PlaceHolderTextChannelOauth.text = _oauthToken;

            //looking for interface channel
            string[] _interfaceSeparator = new string[] { "interfaceChannel=" };
            string[] _interfaceSplit = _content.Split(_interfaceSeparator, StringSplitOptions.None);
            int _interfaceIndex = _interfaceSplit[1].IndexOf(";", 0);
            string _interfaceName = _interfaceSplit[1].Substring(0, _interfaceIndex);
            uiManager.PlaceHolderTextChannelInterface.text = _interfaceName;
        }        
    }

    void CreateDatasFile()
    {
        string _content = "channelToConnect=" + channelToConnect + ";\n\n" + "oauth=" + oauth + ";\n\n" + "interfaceChannel=" + interfaceChannel + ";";

        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Connection Datas \n\n" + _content);
        }

        else
        {
            File.Create(path).Close();
            File.AppendAllText(path, "Connection Datas \n\n" + _content);
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
        if (File.Exists(path))
        {
            string _content = File.ReadAllText(path);

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

            uiManager.PlaceHolderTextChannelInterface.text = "*******";
            uiManager.PlaceHolderTextChannelOauth.text = "*******";
            uiManager.PlaceHolderTextChannelToConnect.text = "*******";
        }

        if(File.Exists(connectPath))
        {
            string _connectContent = File.ReadAllText(connectPath);
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

    void HideNShowTipingData()
    {
        if(Input.anyKeyDown && uiManager.TwitchCanvaIsVisible)
        {
            uiManager.TextHideChannelToConnect.text = uiManager.TextHideChannelToConnect.text + "*";
        }
    }
    #endregion

    void Init()
    {
        path = Application.persistentDataPath + "Datas.txt";
        connectPath = Application.persistentDataPath + "ConnectStart.txt";
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