using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Enemy
{
    protected override IEnumerator CoroutineEnemyBaseAction()
    {
        List<Node> targetPath = LogicTargetBaseAction();

        Debug.Log("targer Path After config count " + targetPath.Count);

        if ((targetPath.Count < actionRange ||  currentNode.GetNeighborNode().Contains(Player.instance.GetCurrentNode())) && targetPath.Count != 0) 
        {
            
            // nếu là enemy range mõi khi quái hiện tại đứng sát với mục tiêu thì nó sẽ move ra xa so với player sao cho cái path nó = action range || hoặc là (quái là quái buff hoặc quái tầm xa) && player đứng quanh sát quái này thì sẽ move ra xa so với player
            SetupIfCountTargetPathGreaterThanActionRange(ref targetPath);
            yield return StartCoroutine(ActionMoving(PathfindingAstart.instance.FindPath(currentNode, targetPath[0])));
        }


        Action(targetPath);

    }


}
