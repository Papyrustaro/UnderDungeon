using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectButton : MonoBehaviour
{
    private BattleCharacter bc;
    [SerializeField] private DungeonBattleManager dbm;

    private void Start()
    {
        bc = transform.parent.GetComponent<ShowStatusInBattle>().Chara;
    }

    public void PressedCharacter()
    {
        dbm.SetInputTarget(this.bc, E_TargetType.OneEnemy);
    }
}
