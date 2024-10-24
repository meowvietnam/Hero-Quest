using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.TextCore.Text;


public abstract class Character : InteractableNode
{
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] protected float dame;
    [SerializeField] protected float hp;
    [SerializeField] protected float manaOriginal;

    [SerializeField] protected float mana;
    [SerializeField] protected int actionRange = 1;
    [SerializeField] protected int manaCostAction = 1;

    [SerializeField] protected string nameCharacter = "";

    public enum AnimState
    {
        Idle = 0,
        Run = 1,
        Attack = 2,
        CastSkill = 3,
        Hit = 4,
        Death = 5,
    }
    protected void Start()
    {
        spriteRenderer.sortingOrder = -1;
        SetHp(hp);
        SetMana(manaOriginal);
        SetDame(dame);
    }
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected IEnumerator CoroutineMovingAndAction(List<Node> targetPath , InteractableNode interactableNode)
    {


        yield return StartCoroutine(ActionMoving(targetPath));
        yield return StartCoroutine(ActionAttack(interactableNode));

        GameManager.instance.SetStateGame(StateGame.None);
        SetAnim(AnimState.Idle);
        if(mana < manaCostAction )
        {
            GameManager.instance.SwapTurn();
        }
    }
    protected IEnumerator ActionMoving(List<Node> targetPath)
    {
        SetAnim(AnimState.Run);
        for (int i = 0; i < targetPath.Count; i++)
        {
            Node node = targetPath[i];
            if (mana == 0)
            {
                break;
            }

          //  Debug.Log(node.name);
            SetFlipCharacter(node);
            mana--;
            SetMana(mana);
            transform.DOMove(node.transform.position, 0.5f);
            SetNodeAfterMove(node);


            yield return new WaitForSeconds(0.5f);
        }
    }
    protected IEnumerator ActionAttack(InteractableNode interactableNode)
    {
        if (interactableNode != null && mana >= manaCostAction)
        {
          

            SetFlipCharacter(interactableNode.GetCurrentNode());
            mana -= manaCostAction;
            SetMana(mana);
            SetAnim(AnimState.Attack);
           
            yield return new WaitForSeconds(0.5f);


            interactableNode.NodeInteractionEvent(this);


        }
    }    
    void SetNodeAfterMove(Node node)
    {
        currentNode.SetCurrentInteractiveObject(null);
        currentNode = node;
        node.SetCurrentInteractiveObject(this);
    }    
    void SetFlipCharacter(Node node)
    {
        if (node.GetIndexX() < currentNode.GetIndexX())
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }    

    public void Action(List<Node> targetPath)
    {
        StartCoroutine(CoroutineAction(targetPath));
    }

  
    protected virtual IEnumerator CoroutineAction(List<Node> tartgetPath)
    {
        InteractableNode interactableNode = null;
        List<Node> tartgetPathAfterConfig = SetUpTargetWithActionRange(tartgetPath, ref interactableNode);
        GameManager.instance.SetStateGame(StateGame.Action);
        yield return StartCoroutine(CoroutineMovingAndAction(tartgetPathAfterConfig, interactableNode));
    }
    public void TakeDame(float dame)
    {
        hp -= dame;
        SetHp(hp);
        SetAnim(AnimState.Hit);
    }
    public List<Node> SetUpTargetWithActionRange(List<Node> targetPath, ref InteractableNode interactableNode)
    {
        Debug.Log(targetPath.Count);
        interactableNode = targetPath[targetPath.Count - 1].GetCurrentInteractiveObject();
        if(interactableNode != null)
        {
            for (int i = 0; i < actionRange; i++)
            {
                targetPath.RemoveAt(targetPath.Count - 1);
            }
        }    
      
        return targetPath;

    }
    public void ResetMana()
    {
        // start new turn của character này
        SetMana(manaOriginal);

    }    
    public void SetAnim(AnimState animType)
    {
        anim.SetInteger("Anim",(int)animType);
    }
    public virtual void SetManaOriginal(float manaOriginal)
    {
        this.manaOriginal = manaOriginal;
    }    
    public virtual void SetHp(float hp)
    {
        this.hp = hp;
    }
    public virtual void SetMana(float mana)
    {
        this.mana = mana;

    }
    public virtual void SetDame(float dane)
    {
        this.dame = dane;

    }
    public virtual void SetActionRange(int actionRange)
    {
        this.actionRange = actionRange;

    }
    public float GetManaOriginal()
    {
        return manaOriginal;
    }
    public float GetHp()
    {
        return hp;
    }
    public float GetMana()
    {
        return mana;
    }
    public float GetDame()
    {
        return dame;
    }
    public int GetActionRange()
    {
        return actionRange;
    }
    public string GetNameCharacter()
    {
        return nameCharacter;
    }
    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;
    }    

}
