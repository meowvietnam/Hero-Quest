using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

    }
    protected virtual List<Node> LogicTargetBaseAction()
    {
        return PathfindingAstart.instance.FindPath(currentNode, Player.instance.GetCurrentNode());
    }    
    public  void EnemyBaseAction()
    {
        StartCoroutine(CoroutineEnemyBaseAction());
    }

    protected virtual IEnumerator CoroutineEnemyBaseAction()
    {
        List<Node> targetPath = LogicTargetBaseAction();

        Debug.Log("targer Path After config count " + targetPath.Count);

        Action(targetPath);

        yield break;

    }

    protected override IEnumerator CoroutineAction(List<Node> tartgetPath)
    {
        yield return base.CoroutineAction(tartgetPath);
        if (mana >= manaCostAction)
        { 
            ActionContinueIfExcessMana();
        }

    }
   

    protected virtual void ActionContinueIfExcessMana()
    {
        // luôn target vào mục tiêu ban đầu cho tới khi hết mana nếu là assasin thì ghi đè vào đây cho nó chém 1 phát xong nó chạy
        EnemyBaseAction();

    }
   
    

    public override void NodeInteractionEvent(InteractableNode sender)
    {

        if (sender is Player player)
        {
            TakeDame(player.GetDame());
        }
        else if(sender is EnemyBuff enemyBuff)
        {
            enemyBuff.ActionBuff(this);
        }    
    }
    protected void SetupIfCountTargetPathGreaterThanActionRange(ref List<Node> targetPath)
    {
       
         
        int count = 0;
        
        Node targetNode = targetPath[targetPath.Count - 1];

        Node GetNodeWithActionRange(Node targetNode,ref int count)
        {
            count++;
            List<Node> neighborNodes = targetNode.GetNeighborNode();
            Node furthestNode = GetNodeMaxDistanceWithPlayer(neighborNodes);

            if (count == actionRange)
            {
                return furthestNode;
            }
            else
            {
                return GetNodeWithActionRange(furthestNode, ref count);
            }
        }

        Node GetNodeMaxDistanceWithPlayer(List<Node> neighborNodes)
        {
            float maxDistance = float.MinValue;
            Node furthestNode = null;

            foreach (Node neighbor in neighborNodes)
            {
                if (neighbor.GetCurrentInteractiveObject() != null ) continue;
                float distance = Vector3.Distance(neighbor.transform.position, Player.instance.transform.position);

                if (distance >= maxDistance)
                {
                    maxDistance = distance;
                    furthestNode = neighbor;
                }
            }

            return furthestNode;
        }

        Node nodeWithActionRange = GetNodeWithActionRange(targetNode, ref count);
        if (nodeWithActionRange != null)
        {
            targetPath.Insert(0,nodeWithActionRange);
        }
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }
}
