
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.TextCore.Text;

public enum StateGame
{
    Action,
    None,
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int row;
    [SerializeField] private int col;
    [SerializeField] private Node[,] mapNodes;
    [SerializeField] private GameObject prefabNode;
    [SerializeField] private GameObject nodesMapGO;
    [SerializeField] private Sprite[] spriteMap;
    [SerializeField] private List<Character> listCharacter;
    [SerializeField] private Character currentTurn;

    [SerializeField] private StateGame stateGame;
    [SerializeField] private Text inforCardText;

    [SerializeField] private BuffSO buffConfig;

    [SerializeField] private Image introImg;
    [SerializeField] private Sprite introSpriteEnemy;
    [SerializeField] private Sprite introSpritePlayer;

    // Test

    public Enemy enemy;
    public Enemy enemyRange;

    public Enemy enemyBuff;
    public Enemy enemyBuffHp;
    public Enemy enemyBuffMana;
    public Enemy enemyAssasin;

    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        CreateMap();

        // test
        //44 54 55 34
        enemyBuff.SetCurrentNode(mapNodes[3, 3]);
        enemy.SetCurrentNode(mapNodes[4,4]);
        enemyBuffHp.SetCurrentNode(mapNodes[0, 0]);
        enemyBuffMana.SetCurrentNode(mapNodes[2, 1]);
        enemyRange.SetCurrentNode(mapNodes[5, 3]);
        enemyAssasin.SetCurrentNode(mapNodes[4, 2]);

        Player.instance.SetCurrentNode(mapNodes[5,5]);
        StartCoroutine(StartTurn(0));



    }
    IEnumerator StartTurn(int i)
    {
        Character oldCurrentTurn = currentTurn;
        // thằng character cũ trả về layer id = -1
        oldCurrentTurn.GetSpriteRenderer().sortingOrder = -2;
        // thằng character mới set layer id = 0
        Character newCurrentTurn = listCharacter[i];
        newCurrentTurn.ResetMana();
        newCurrentTurn.GetSpriteRenderer().sortingOrder = -1;
        currentTurn = newCurrentTurn;
        if (newCurrentTurn is Enemy enemy)
        {
            if (oldCurrentTurn is Player)
            {
                 SetIntroAnim(introSpriteEnemy);
                 yield return new WaitForSeconds(4f);

            }
            enemy.EnemyBaseAction();
        }
        else
        {
            SetIntroAnim(introSpritePlayer);

        }
    }
    void SetIntroAnim(Sprite introSprite)
    {
        introImg.sprite = introSprite;
        stateGame = StateGame.Action;
        introImg.DOFade(1, 2f).OnComplete(() => {


            introImg.DOFade(0, 2f).OnComplete(() => {


                stateGame = StateGame.None;



            });



        });
       
    }    
    public void SwapTurn()
    {
       
       int nextTurnIndex = listCharacter.IndexOf(currentTurn) + 1;
       if(nextTurnIndex > listCharacter.Count - 1) // index đang bị quá tràn k tồn tại tức nghĩa là turn cuối rồi reset
       {
            nextTurnIndex = 0;
       }    
       StartCoroutine(StartTurn(nextTurnIndex));  
    
       
        
    }
   
    void CreateMap()
    {
        mapNodes = new Node[row, col];
        Sprite spriteNode = null;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Node node = Instantiate(prefabNode,nodesMapGO.transform).GetComponent<Node>();
                node.gameObject.transform.localPosition = new Vector3(i,-j,0) * 1.5f;
                mapNodes[i,j] = node;
                if((i+j) % 2 == 0 )
                {
                    spriteNode = spriteMap[0];
                }    
                else
                {
                    spriteNode = spriteMap[1];
                }    
                node.SetIndex(i,j, spriteNode);
                

            }
        }

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {

                mapNodes[i, j].InitNode(mapNodes);
            }
        }

    }
    public void CallBuff(BuffState buffState ,Character characterReader, float valueBuff)
    {
        switch (buffState)
        {
            case BuffState.BuffDame:
                StartCoroutine(buffConfig.BuffDameAction(characterReader,valueBuff));

                break;
            case BuffState.BuffHp:
                StartCoroutine(buffConfig.BuffHpAction(characterReader, valueBuff));
                break;
            case BuffState.BuffMana:
                StartCoroutine(buffConfig.BuffManaAction(characterReader, valueBuff));
                break;
            default:
                break;
        }
    }    
    public List<Character> GetListCharacter()
    {
        return listCharacter;
    }    
    public Node[,] GetMapNodes()
    {
        return mapNodes;
    }    
    public Character GetCurrentTurn()
    {
        return currentTurn;
    }
    public StateGame GetStateGame()
    {
        return stateGame;
    }
    public void SetStateGame( StateGame stateGame)
    {
        this.stateGame = stateGame;
    }
    public void SetInforCard(float manaCost , string description)
    {
        RectTransform rectInforCard = inforCardText.transform.parent.GetComponent<RectTransform>();
        //Vector3 startAnchorPos = rectInforCard.anchoredPosition;
        //rectInforCard.localPosition = new Vector3(rectInforCard.localPosition.x, -rectInforCard.sizeDelta.y, 0);
        rectInforCard.DOAnchorPosY(rectInforCard.sizeDelta.y, 0.5f);
        inforCardText.text = $"Consumes <color=blue>{manaCost}</color> mana to {description}";
    }
    public void ResetInforCard()
    {
        RectTransform rectInforCard = inforCardText.transform.parent.GetComponent<RectTransform>();
        rectInforCard.DOAnchorPosY(-rectInforCard.sizeDelta.y, 0.5f);
    }
    // 
    // Update is called once per frame
    void Update()
    {
        
    }
}
