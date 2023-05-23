using System.IO;
using System.Net.Sockets;
using UnityEngine;
using System;


public class CS_TwitchConnect : MonoBehaviour
{
    #region F/P
    StreamReader reader;
    StreamWriter writer;
    TcpClient twitchClient;
    
    [Space, SerializeField]
    float pingCounter;
    const int PORT = 6667;
    const string URL = "irc.chat.twitch.tv";
    bool isConnect;
    public bool IsConnect => isConnect;
    
    bool canCheck;//kill

    #region Appel scripts

    [Space, Header("Appel scripts"), Space]
    [SerializeField]
    CS_AvatarBehavior avatarBehav;

    [SerializeField]
    CS_TwitchID twitchID;

    [SerializeField]
    CS_ChatterData chatterDatas;
    
    #endregion
    #endregion

    #region ProcessDatas
    void ProcessMessages()
    {
        if (!twitchClient.Connected) return;

        if (twitchClient.Available <= 0) return;
        
        string _message = reader.ReadLine();



        if (_message.Contains("PRIVMSG") && !_message.Contains("custom-reward-id="))
        {
            string[] _nameSeparator = new string[] { "display-name=" };
            string[] _nameResult = _message.Split(_nameSeparator, StringSplitOptions.None);
            int _splitName = _nameResult[1].IndexOf(";", 0);
            string _chatterName = _nameResult[1].Substring(0, _splitName);

            string[] _msgSeparator = new string[] { "PRIVMSG" };
            string[] _msgResult = _message.Split(_msgSeparator, StringSplitOptions.None);
            string[] _splitMsg = _msgResult[1].Split(':');
            string _finalMsg = _splitMsg[1];

            print("message final : " + _finalMsg);//kill
            print("chatter name : " + _chatterName);//kill
        }

        print("full message : " + _message);//kill

        /*if (_message.Contains("@badge-info=subscriber")|| _message.Contains("@badge-info=founder"))
        {
            print("this person is a subscriber");//kill
        }*/

        if (_message.Contains("custom-reward-id="))
        {
            string[] _rewardSeparator = new string[] { "custom-reward-id=" };
            string[] _rewardResult = _message.Split(_rewardSeparator, StringSplitOptions.None);
            string _rewardID = _rewardResult[1].Substring(0, _rewardResult[1].IndexOf(";", 0));

            ProcessRewards(_rewardID);
            print("Redeemed a custom reward ID : " + _rewardID);//kill
        } 
    }
    void ProcessRewards(string _rewardId)
    {
        Debug.Log("reward processing" + _rewardId);//kill
        if (_rewardId.Equals(avatarBehav.TwerkRewardId)) avatarBehav.MakeItTwerk();
    }
    #endregion

    #region TwitchConnection
    public void StartCheck()
    {
        canCheck = true;
    }
    void checkConnection()
    {
       
        if (twitchClient.Connected) isConnect = true;
        else isConnect = false;
           
    }
    public void ConnectToTwitch()
    {
        if (twitchID.Oauth == null || twitchID.InterfaceChannel == null || twitchID.ChannelToConnect == null) return;
        twitchClient = new TcpClient(URL, PORT);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + twitchID.Oauth);
        writer.WriteLine("Nick " + twitchID.InterfaceChannel.ToLower());
        writer.WriteLine("JOIN #" + twitchID.ChannelToConnect.ToLower());
        writer.WriteLine("CAP REQ :twitch.tv/tags");
        writer.WriteLine("CAP REQ :twitch.tv/commands");
        writer.WriteLine("CAP REQ :twitch.tv/membership");
        writer.Flush();
    }

    void CountPing()
    {
        if (isConnect)
        {
            pingCounter += Time.deltaTime;
            if (pingCounter > 60)
            {
                PingTwitch();
                pingCounter = 0;
            }
        }
    }

    public void DisconectToTwitch()
    {
        twitchClient.Close();
    }

    public void PingTwitch()
    {
        writer.WriteLine("PING " + URL);
        writer.Flush();
    }

    void Init()
    {
        isConnect = false;
        canCheck = false;
    }
    #endregion

    #region Unity methodes

    void Awake()
    {
        Init();
    }
    void Update()
    {
        if (canCheck)
        {
            checkConnection();
            ProcessMessages();
            CountPing();
        }
    }
    #endregion
}