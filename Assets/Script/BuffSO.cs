using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffState
{

    BuffDame = 0,
    BuffHp = 1,
    BuffMana = 2,
}
[CreateAssetMenu]
public class BuffSO : ScriptableObject
{
  
    [SerializeField] private GameObject[] prefabEffectBuff;

    public  IEnumerator BuffDameAction(Character characterReader, float valueBuff)
    {
        characterReader.SetDame(characterReader.GetDame() + valueBuff);
        GameObject buffEffect = Instantiate(prefabEffectBuff[(int)BuffState.BuffDame],characterReader.transform);
        yield return new WaitForSeconds(5f);
        Destroy(buffEffect);
        

    }
    public IEnumerator BuffHpAction(Character characterReader, float valueBuff)
    {
        characterReader.SetHp(characterReader.GetHp() + valueBuff);
        GameObject buffEffect = Instantiate(prefabEffectBuff[(int)BuffState.BuffHp], characterReader.transform);
        yield return new WaitForSeconds(5f);
        Destroy(buffEffect);
    }
    public IEnumerator BuffManaAction(Character characterReader, float valueBuff)
    {
        characterReader.SetManaOriginal(characterReader.GetManaOriginal() + valueBuff);
        GameObject buffEffect = Instantiate(prefabEffectBuff[(int)BuffState.BuffMana], characterReader.transform);
        yield return new WaitForSeconds(5f);
        Destroy(buffEffect);
    }
}
