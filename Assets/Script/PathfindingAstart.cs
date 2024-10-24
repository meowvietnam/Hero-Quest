using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PathfindingAstart : MonoBehaviour
{

    public static PathfindingAstart instance;


    private void Awake()
    {
        instance = this;


    }
    // Start is called before the first frame update
    void Start()
    {

        // Test

    }
 
    public List<Node> FindPath(Node startNode, Node endNode)
    {
        
        List<Node> closeNodes = new List<Node>();
        List<Node> resultNodes = new List<Node>();
        List<Node> openNodes = new List<Node>();

        Node currentNode;
        openNodes.Add(startNode);
        // currentNode = BestNodeCostInOpenList();
        while (openNodes.Count > 0)
        {
            currentNode = BestNodeCostInOpenList(openNodes);

            if (currentNode == endNode)
            {
                // Truy vấn ngược lại từ endNode về startNode
                Node temp = endNode;
                resultNodes.Add(temp);
                while (temp != null)
                {
                    
                   
                    
                    Node previousNode = temp.GetPreviousNode();
                    if (previousNode == startNode)
                    {
                        break;
                    }
                    resultNodes.Add(previousNode);
                    temp = previousNode;
                }
                resultNodes.Reverse(); // Đảo ngược danh sách
                break;
            }

            openNodes.Remove(currentNode);
            closeNodes.Add(currentNode);



            AddNeighborToOpenNodes(currentNode, endNode , ref openNodes, ref closeNodes);
        }
     
        Debug.Log(resultNodes.Count);


        return resultNodes;
    }
    void AddNeighborToOpenNodes(Node currentNode ,Node targetNode  , ref List<Node> openNodes, ref List<Node> closeNodes)
    {
        
           
        foreach (var neighbor in currentNode.GetNeighborNode())
        {
            InteractableNode interactiveObject = neighbor.GetCurrentInteractiveObject();
            if (closeNodes.Contains(neighbor) || neighbor.GetIsObstacle() || (interactiveObject != null && neighbor != targetNode))
            {
                continue; // Bỏ qua nếu neighbor đã trong closeNodes
            }

            if (!openNodes.Contains(neighbor))
            {
                neighbor.SetPreviousNode(currentNode);
                neighbor.gCost = currentNode.gCost + Vector3.Distance(currentNode.transform.position, neighbor.transform.position);
                neighbor.hCost = Vector3.Distance(neighbor.transform.position, targetNode.transform.position);
               // neighbor.fCost = neighbor.gCost + neighbor.hCost;
                openNodes.Add(neighbor);
            }
            else
            {
                // Kiểm tra nếu đường đi mới tốt hơn

                // Nếu neighbor đã nằm trong openNodes, kiểm tra nếu đường đi qua currentNode đến neighbor có gCost nhỏ hơn đường đi trước đó đến neighbor. Nếu có, cập nhật PreviousNode của neighbor là currentNode và cập nhật gCost, hCost, và fCost cho neighbor.
                float newGCost = currentNode.gCost + Vector3.Distance(currentNode.transform.position, neighbor.transform.position);
                if (newGCost < neighbor.gCost)
                {
                    neighbor.SetPreviousNode(currentNode);
                    neighbor.gCost = newGCost;
                    //neighbor.fCost = neighbor.gCost + neighbor.hCost;
                }
            }
        }
    }
    Node BestNodeCostInOpenList(List<Node> openNodes)
    {
        if(openNodes.Count == 1)
        {
            return openNodes.First();
        }

        // Tìm Node có giá trị f nhỏ nhất
        float minF = openNodes.Min(node => node.fCost); // tìm giá trị minf có trong danh sách các node
        List<Node> minFNodes = openNodes.Where(node => node.fCost == minF).ToList(); // tìm xem node này là node  
        if (minFNodes.Count == 1)
        {
            return minFNodes.First(); // nếu list này có 1 node tức là bé nhất
        }
        // nếu nhiều hơn 1 node tức là có nhiều f bé nhất giống nhau thì ta so sánh h cost

        // Nếu có nhiều Node có cùng giá trị f nhỏ nhất, trả về Node có giá trị h nhỏ nhất trong số đó
        float minH = minFNodes.Min(node => node.hCost);
        List<Node> minHNodes = minFNodes.Where(node => node.hCost == minH).ToList();  
        if (minHNodes.Count == 1)
        {
            return minHNodes.First(); // nếu list này có 1 node tức là bé nhất
        }

        float minG = minHNodes.Min(node => node.gCost);
        List<Node> minGNodes = minHNodes.Where(node => node.gCost == minG).ToList();
       
        return minGNodes.First(); // nếu list này có 1 node tức là bé nhất nếu có nhiều hơn 1 con đường giống nhau thì chọn cái nào cũng được nên k cần if nữa
      
    }   
    
   
    void Update()
    {
        
    }
}
