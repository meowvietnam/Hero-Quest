using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardBuffDame : CardBuff
{
    public override void Play()
    {
        base.Play();
        GameManager.instance.CallBuff(BuffState.BuffDame, Player.instance, valueBuff);

    }


}
