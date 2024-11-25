
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
    [SerializeField] private List<CardSO> listCard;
    [SerializeField] private CardBehaviour prefabCard;
    [SerializeField] private GameObject listCardGameObject;

    [SerializeField] private List<Item> listPrefabItem;


    [SerializeField] private Image introImg;
    [SerializeField] private Sprite introSpriteEnemy;
    [SerializeField] private Sprite introSpritePlayer;

    [SerializeField] private List<LevelData> levelData;
    [SerializeField] private int indexCurrentLevel = 0;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject particalWin;

    private void DrawCard()
    {
        // CheckDropCard
        if (listCardGameObject.transform.childCount >= 5)
        {
            listCardGameObject.transform.GetChild(0).GetComponent<CardBehaviour>().EffectDropCard();
        }


        // Draw Card
        CardSO thisCardSO = listCard[Random.Range(0, listCard.Count)];
        CardBehaviour cardBehaviour = Instantiate(prefabCard, listCardGameObject.transform);
        cardBehaviour.SetCard(thisCardSO);

        
    }
    public void EventCharacterDie(Character character)
    {

        listCharacter.Remove(character);
        DOVirtual.DelayedCall(1f, () =>
        {
            Item thisItem = Instantiate(listPrefabItem[Random.Range(0, listPrefabItem.Count)], nodesMapGO.transform);
            Node thatNode = character.GetCurrentNode();
            thisItem.SetCurrentNode(thatNode);
            CheckWin();
            thatNode.GetCollider().enabled = true;
            character.Die();
        });
     

    }

    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {



    }
    public void Lose()
    {
        mainMenu.SetActive(true);
        if (this.nodesMapGO != null)
        {
            Destroy(this.nodesMapGO);
        }

    }
    public void Win()
    {
        mainMenu.SetActive(true);
        if (this.nodesMapGO != null)
        {
            Destroy(this.nodesMapGO);
        }
        if(indexCurrentLevel < levelData.Count - 1)
        {
            indexCurrentLevel++;

        }
        else
        {
            indexCurrentLevel = 0;
        }    

    }
    public void CheckWin()
    {
        if (listCharacter.Count == 1 && listCharacter[0] is Player)
        {
            particalWin.SetActive(true);
            DOVirtual.DelayedCall(1f,() =>
            {
                particalWin.SetActive(false);

                Win();

            });
        }    
    }    

    public void EventButtonPlayGame()
    {
        mainMenu.SetActive(false);
        if (this.nodesMapGO != null)
        {
            Destroy(this.nodesMapGO);
        }    

        GameObject nodesMapGO = new GameObject();
        nodesMapGO.transform.localPosition = new Vector3(-6.6f, 5.25f);
        this.nodesMapGO = nodesMapGO;


        CreateMap();
        levelData[indexCurrentLevel].CreateMapLevel(ref listCharacter, nodesMapGO);
        StartCoroutine(StartTurn(0));
    }

    IEnumerator StartTurn(int i)
    {
        
        Character oldCurrentTurn = currentTurn;

        // thằng character cũ trả về layer id = -1
        if(oldCurrentTurn != null)
        {
            oldCurrentTurn.GetSpriteRenderer().sortingOrder = -2;

        }
        // thằng character mới set layer id = 0
        Character newCurrentTurn = listCharacter[i];
        newCurrentTurn.ResetMana();
        newCurrentTurn.GetSpriteRenderer().sortingOrder = -1;
        currentTurn = newCurrentTurn;
        if (newCurrentTurn is Enemy enemy)
        {
            if(oldCurrentTurn == null || (oldCurrentTurn is Player && oldCurrentTurn != null))
            {
                SetIntroAnim(introSpriteEnemy);
                yield return new WaitForSeconds(4f);
            }
          
            enemy.EnemyBaseAction();
        }
        else
        {
            SetIntroAnim(introSpritePlayer);
            DrawCard();
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
            if (nextTurnIndex > listCharacter.Count - 1) // index đang bị quá tràn k tồn tại tức nghĩa là turn cuối rồi reset
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
