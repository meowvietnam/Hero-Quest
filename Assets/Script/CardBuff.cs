using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardBuff : CardSO
{
    [SerializeField] private BuffState buffType;

    [SerializeField] private float valueBuff;
    public override void Play()
    {
        Player.instance.SetMana(Player.instance.GetMana() - manaCost);
        GameManager.instance.CallBuff(buffType, Player.instance, valueBuff);

    }


}
