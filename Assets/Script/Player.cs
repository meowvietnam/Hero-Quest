using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] private Text textHp;
    [SerializeField] private Text textMana;
    [SerializeField] private Text textDame;
    public static Player instance;
    
    protected override void Awake()
    {

        base.Awake();
        instance = this;
        textHp = GameObject.Find("TextHp").GetComponent<Text>();
       textMana = GameObject.Find("TextMana").GetComponent<Text>();
       textDame = GameObject.Find("TextDame").GetComponent<Text>();
       
    }
    public override void SetHp(float hp)
    {
        base.SetHp(hp);
        textHp.text = this.hp.ToString();
    }
    public override void SetMana(float mana)
    {
        base.SetMana(mana);
        textMana.text = this.mana.ToString();

    }
    public override void SetDame(float dame)
    {
        base.SetDame(dame);
        textDame.text = this.dame.ToString();

    }

    public override void NodeInteractionEvent(InteractableNode sender)
    {
        if(sender is Enemy enemy)
        {
            TakeDame(enemy.GetDame());
        }    
    }
}
