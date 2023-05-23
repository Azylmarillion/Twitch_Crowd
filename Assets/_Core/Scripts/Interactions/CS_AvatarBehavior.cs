using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_AvatarBehavior : MonoBehaviour
{
    [SerializeField]
    Animator avatarAnimator;
    float timer;
    bool playTimer = false;
    [SerializeField]
    string twerkRewardIrd = "6eb76484-6625-415b-9528-49823771020e";
    public string TwerkRewardId => twerkRewardIrd;

    public void MakeItTwerk()
    {
        avatarAnimator.SetBool("Twerk", true);
        playTimer = true;
    }

    void TimerResetAnimation()
    {
        if(playTimer == true)
        {
            timer += Time.deltaTime;
            if (timer > 5)
            {
                avatarAnimator.SetBool("Twerk", false);
                playTimer = false;
            }
        }        
    }

    private void Update()
    {
        TimerResetAnimation();
    }
}
 
