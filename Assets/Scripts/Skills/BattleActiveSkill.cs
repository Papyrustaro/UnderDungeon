using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleActiveSkill : MonoBehaviour
{
    [SerializeField]
    private E_BattleActiveSkill id;
    [SerializeField]
    private string description; //スキルの説明
    [SerializeField]
    private int needTurn; //必要なターン
    [SerializeField]
    private E_Element element;
    [SerializeField]
    private bool needTarget; //プレイヤーにターゲットを訊くかどうか(特定の味方対象など)

    public E_BattleActiveSkill ID => id;
    public string SkillName => id.ToString();
    public string Description => description;
    public int NeedTurn => needTurn;
    public E_Element Element => element;
    public bool NeedTarget => needTarget;
}
