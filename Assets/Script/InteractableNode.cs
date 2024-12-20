using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableNode : MonoBehaviour
{
    [SerializeField] protected string nameInterActableNode = "";


    [SerializeField] protected Node currentNode;
    public void SetCurrentNode(Node currentNode)
    {
        this.currentNode = currentNode;
        this.currentNode.SetCurrentInteractiveObject(this);
        transform.position = currentNode.transform.position;
    }    
    public Node GetCurrentNode()
    {
        return currentNode;
    }
    public abstract void NodeInteractionEvent(InteractableNode sender);
    public string GetName()
    {
        return nameInterActableNode;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
