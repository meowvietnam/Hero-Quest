using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    public Vector2Int indexMap;
    public InteractableNode prefab;
    public bool isObstacle;
}
[CreateAssetMenu]
public class LevelData : ScriptableObject
{
  
    [SerializeField] private List<Map> map = new List<Map>();

    public void CreateMapLevel(ref List<Character> listCharacter , GameObject mapGameObject)
    {
        Node[,] mapNodes = GameManager.instance.GetMapNodes();
        listCharacter.Clear();
        int count = 0;
        foreach (Map element in map)
        {
            if(element.isObstacle)
            {
                mapNodes[element.indexMap.y, element.indexMap.x].SetObstacle();
            }    
            else
            {
                InteractableNode obj = Instantiate(element.prefab , mapGameObject.transform);
                if(obj is Character character)
                {
                    listCharacter.Add(character);   
                }    
                obj.SetCurrentNode(mapNodes[element.indexMap.y,element.indexMap.x]);
            }
            Debug.Log(count);

            count++;

        }



    }    
}
