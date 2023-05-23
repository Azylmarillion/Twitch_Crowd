using UnityEngine;
using System;
using System.Collections.Generic;

public class CS_ChatterData : MonoBehaviour
{
    public bool IsSub;
    public bool IsModerator;
    public bool IsVIP;
    public string ChatterName;
    public string ChatterID;
    public string Message;

    public List<CS_ChatterData> MinizChatters = new List<CS_ChatterData>();

    #region Miniz

    void checkMiniz(int _chatterID)
    {
        _chatterID = Convert.ToInt32("");
    }   
    #endregion
}