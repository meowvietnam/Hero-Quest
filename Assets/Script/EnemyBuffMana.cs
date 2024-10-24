using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyBuffMana : EnemyBuff
{
    public override void ActionBuff(Character characterReader)
    {
        GameManager.instance.CallBuff(BuffState.BuffMana, characterReader, dame);
    }

    protected override List<Node> LogicTargetBaseAction()
    {
        List<Enemy> listEnemy = GetListEnemy();
        float minManaOriginal = listEnemy.Min(enemy => enemy.GetManaOriginal());
        List<Enemy> minManaOriginalEnemys = listEnemy.Where(enemy => enemy.GetManaOriginal() == minManaOriginal).ToList();

        return PathfindingAstart.instance.FindPath(currentNode, minManaOriginalEnemys.First().GetCurrentNode());

    }
}
