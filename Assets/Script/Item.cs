using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : InteractableNode
{
    [SerializeField] private BuffState buffType;
    [SerializeField] private int buffValue = 2;
    public override void NodeInteractionEvent(InteractableNode sender)
    {
        GameManager.instance.CallBuff(buffType, (Character)sender , buffValue);
        Destroy(gameObject);
    }

     
   
}
