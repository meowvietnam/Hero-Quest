﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node : MonoBehaviour
{
    public float gCost; //
    public float hCost;
    public float fCost => gCost + hCost;
    [SerializeField] private int indexX;
    [SerializeField] private int indexY;
    [SerializeField] private bool isObstacle;
    [SerializeField] private Node previousNode;
    [SerializeField] private List<Node> neighborNodes = new List<Node>();
    private SpriteRenderer spriteRenderer;
    [SerializeField] private InteractableNode currentInteractiveObject;

    public void SetCurrentInteractiveObject(InteractableNode currentInteractiveObject)
    {
        this.currentInteractiveObject = currentInteractiveObject;
    }    
    // Start is called before the first frame update
    void Awake()
    {
       
        
       spriteRenderer = GetComponent<SpriteRenderer>();    
         

    }
    public List<Node> GetNeighborNode()
    {
        return neighborNodes;
    }    
    public void InitNode(Node[,] nodesMap )
    {
        
        SetNeighborNode(indexX+1, indexY, nodesMap);
        SetNeighborNode(indexX - 1, indexY, nodesMap);
        SetNeighborNode(indexX, indexY+1, nodesMap);
        SetNeighborNode(indexX, indexY - 1, nodesMap);
        /*
        SetNeighborNode(indexX+1, indexY - 1, nodesMap);
        SetNeighborNode(indexX-1, indexY + 1, nodesMap);
        SetNeighborNode(indexX + 1, indexY + 1, nodesMap);
        SetNeighborNode(indexX - 1, indexY - 1, nodesMap);
        */

    }
    public void SetObstacle()
    {
        isObstacle = true;
        spriteRenderer.color = Color.black;
    }
    public InteractableNode GetCurrentInteractiveObject()
    {
        return currentInteractiveObject;
    }
    public bool GetIsObstacle()
    {
        return isObstacle;
    }
    private void SetNeighborNode(int x , int y , Node[,] nodesMap)
    {
        
        
        if (x < 0 || y < 0 || x >= nodesMap.GetLength(0) || y >= nodesMap.GetLength(1))
        {
            return;
        }
     
        neighborNodes.Add(nodesMap[x,y]);
    }
    public void SetIndex(int indexX, int indexY , Sprite spriteNode)
    {
        this.indexX = indexX;
        this.indexY = indexY;
        spriteRenderer.sprite = spriteNode;
    }    
    public int GetIndexX() { return indexX; }
    public int GetIndexY() {  return indexY; }
    public void SetPreviousNode(Node previouseNode)
    {
        this.previousNode = previouseNode;
    }    
    public Node GetPreviousNode()
    {
        return previousNode;
    }
    // Update is called once per frame
    private void OnMouseUp()
    {
        if (GameManager.instance.GetStateGame() == StateGame.Action ||  currentInteractiveObject is Player)
        {
            return;
        }
        GameManager.instance.ResetInforCard();
        if(IsMouseOverNode())
        {

            if (GameManager.instance.GetCurrentTurn() is Player playerTurn && playerTurn.GetMana() > 0 )
            {
             
                   Debug.Log("Call Moving");
                   playerTurn.Action(PathfindingAstart.instance.FindPath(playerTurn.GetCurrentNode(), this));
                
            }
        }    


    }
    
    private void OnMouseDown()
    {
        if(GameManager.instance.GetStateGame() == StateGame.Action || currentInteractiveObject is Player)
        {
            return;
        }    
        if(GameManager.instance.GetCurrentTurn() is Player playerTurn)
        {
                InteractableNode interactableNode = null;
                int manaCost = playerTurn.SetUpTargetWithActionRange(PathfindingAstart.instance.FindPath(playerTurn.GetCurrentNode(),this), ref interactableNode).Count;
                if(interactableNode == null)
                {
                    GameManager.instance.SetInforCard(manaCost, "move to this point");

                }
                else
                {
                    if(interactableNode is Enemy enemy)
                    {
                        GameManager.instance.SetInforCard(manaCost + playerTurn.GetActionRange(), $"Move and eliminate <color=red>{enemy.GetNameCharacter()}</color>");

                    }
                }    


        }
    }
    
    // Phương thức để kiểm tra xem chuột có đang ở trên node hay không
    private bool IsMouseOverNode()
    {
        // Lấy vị trí chuột trong không gian thế giới
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // Đặt z = 0 để chỉ định nó trong không gian 2D

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero); // Raycast với hướng (0, 0)

        // Kiểm tra xem ray có va chạm với collider của node này không
        if (hit.collider != null && hit.collider.transform == transform)
        {
            return true; // Chuột đang ở trên node này
        }
        return false; // Chuột không ở trên node này
    }
}