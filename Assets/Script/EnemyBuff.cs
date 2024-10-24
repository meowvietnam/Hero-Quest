using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBuff : EnemyRange
{

    protected abstract override List<Node> LogicTargetBaseAction();

    public abstract void ActionBuff(Character characterReader);


    protected List<Enemy> GetListEnemy()
    {
        List<Enemy> listEnemy = new List<Enemy>();
        List<Character> listCharacter = GameManager.instance.GetListCharacter();
        for (int i = 0; i < listCharacter.Count; i++)
        {
            if (listCharacter[i] == this) // k lấy bản thân tức là k buff cho chính mình đc
            {
                continue;
            }
            if (listCharacter[i] is Enemy enemy)
            {
                listEnemy.Add(enemy);
            }
        }
        return listEnemy;

    }


}
