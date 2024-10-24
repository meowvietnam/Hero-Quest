using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardBehaviour : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CardSO cardSO;
    [SerializeField] private Text manaCostText;
    [SerializeField] private Image img;
    [SerializeField] private bool isBlock;


    public void OnPointerClick(PointerEventData eventData)
    {
        if(isBlock || GameManager.instance.GetStateGame() == StateGame.Action || Player.instance.GetMana() < cardSO.GetManaCost() || !(GameManager.instance.GetCurrentTurn() is Player))
        {
            return;
        }    
        Debug.Log("PlayCard");
        isBlock = true;
        img.DOFade(0.2f,0.5f);
        cardSO.Play();
        Player.instance.SetAnim(Character.AnimState.CastSkill);
        if(Player.instance.GetMana() <= 0)
        {
            GameManager.instance.SwapTurn();
        }    
        transform.DOMoveY(transform.position.y + 1, 0.5f).OnComplete(()=>{

         

            Destroy(gameObject);


        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isBlock)
        {
            return;
        }
        Debug.Log("PointerDown");
        string descriptionCard = $"use card to {cardSO.GetDescription()}";
        GameManager.instance.SetInforCard(cardSO.GetManaCost(), descriptionCard);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isBlock)
        {
            return;
        }
        GameManager.instance.ResetInforCard();
    }

    private void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        manaCostText.text = cardSO.GetManaCost().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
