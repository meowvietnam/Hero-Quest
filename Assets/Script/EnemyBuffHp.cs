using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBuffHp : EnemyBuff
{
    public override void ActionBuff(Character characterReader)
    {
        GameManager.instance.CallBuff(BuffState.BuffHp, characterReader, dame);
    }

    protected override List<Node> LogicTargetBaseAction()
    {

        List<Enemy> listEnemy = GetListEnemy();
        float minHp = listEnemy.Min(enemy => enemy.GetHp());
        List<Enemy> minHpEnemys = listEnemy.Where(enemy => enemy.GetHp() == minHp).ToList();

        return PathfindingAstart.instance.FindPath(currentNode, minHpEnemys.First().GetCurrentNode());

    }
}
