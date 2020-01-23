using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonActiveSkill : MonoBehaviour
{
    [SerializeField]
    private E_DungeonActiveSkill id;
    [SerializeField]
    private string description; //スキルの説明
    [SerializeField]
    private int needTurn; //必要なターン
    [SerializeField]
    private E_Element element;
    [SerializeField]
    private bool needTarget; //プレイヤーにターゲットを訊くかどうか(特定の味方対象など)

    public E_DungeonActiveSkill ID => id;
    public string SkillName => id.ToString();
    public string Description => description;
    public int NeedTurn => needTurn;
    public E_Element Element => element;
    public bool NeedTarget => needTarget;
}
