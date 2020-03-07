using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BattleActiveSkill : BattleActiveEffect
{
    [SerializeField]
    private E_BattleActiveSkill id;
    [SerializeField]
    private int needBattleSkillPoint; //必要なスキルポイント

    public E_BattleActiveSkill ID => this.id;
    public override string EffectName => this.id.ToString();
    public int NeedBattleSkillPoint => this.needBattleSkillPoint;

    public override void OtherFunc(BattleCharacter invoker, List<BattleCharacter> target)
    {
        GetComponent<OtherBattleActiveSkillsFunc>().SkillFunc(invoker, target, this);
    }
}
