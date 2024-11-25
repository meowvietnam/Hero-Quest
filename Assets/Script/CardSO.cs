using System;
using UnityEngine;


public abstract class CardSO : ScriptableObject
{
   [SerializeField] protected string description;
   [SerializeField] protected int manaCost;
   [SerializeField] protected Sprite spriteCard;



    public Sprite GetSpriteCard()
    {
        return spriteCard;
    }

    public abstract void Play();


    public string GetDescription()
    {
        return description;

    }   
    public int GetManaCost()
    {
        return manaCost;
    }
   
}



