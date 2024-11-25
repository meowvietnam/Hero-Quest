using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyBuffDame : EnemyBuff
{
    public override void ActionBuff(Character characterReader)
    {
        GameManager.instance.CallBuff(BuffState.BuffDame, characterReader, dame);
    }

    protected override List<Node> LogicTargetBaseAction()
    {

        List<Enemy> listEnemy = GetListEnemy();
        if(listEnemy.Count == 0)
        {
            return PathfindingAstart.instance.FindPath(currentNode, null);
        }    
        float minDame = listEnemy.Min(enemy => enemy.GetDame());
        List<Enemy> minDameEnemys = listEnemy.Where(enemy => enemy.GetDame() == minDame).ToList();

        return PathfindingAstart.instance.FindPath(currentNode, minDameEnemys.First().GetCurrentNode());
        
    }
}
