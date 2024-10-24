using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyAssassin : Enemy
{
    protected override void ActionContinueIfExcessMana()
    {
        Action(LogicTargetIfExcessMana());
    }
    private List<Node> LogicTargetIfExcessMana()
    {
        Node[,] mapNodes = GameManager.instance.GetMapNodes();
        if (Player.instance.GetCurrentNode().GetIndexX() <= mapNodes.GetLength(0) / 2)
        {
            // Duyệt từ phải sang trái
            for (int i = mapNodes.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = mapNodes.GetLength(1) - 1; j >= 0; j--)
                {
                    if (mapNodes[i, j].GetIsObstacle() || mapNodes[i, j].GetCurrentInteractiveObject())
                    {
                        continue;
                    }
                    List<Node> targetPath = PathfindingAstart.instance.FindPath(currentNode, mapNodes[i, j]);
                    if (targetPath.Count >= mana)
                    {
                        return targetPath;
                    }
                }
            }
        }
        else
        {
            // Duyệt từ trái sang phải
            for (int i = 0; i < mapNodes.GetLength(0); i++)
            {
                for (int j = 0; j < mapNodes.GetLength(1); j++)
                {
                    if (mapNodes[i, j].GetIsObstacle() || mapNodes[i, j].GetCurrentInteractiveObject())
                    {
                        continue;
                    }
                    List<Node> targetPath = PathfindingAstart.instance.FindPath(currentNode, mapNodes[i, j]);
                    if (targetPath.Count >= mana)
                    {
                        return targetPath;
                    }
                }
            }
        }
        return null;
    }
}
