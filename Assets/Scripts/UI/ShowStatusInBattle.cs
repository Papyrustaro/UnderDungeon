using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShowStatusInBattle : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private bool isEnemy;
    private BattleCharacter bc;
    private Text nameText;
    private Text hp;
    private Text atk;
    private Text spd;

    // Start is called before the first frame update
    void Start()
    {
        if (!SetCharacter()) return;
        nameText = this.transform.Find("Name").GetComponent<Text>();
        hp = this.transform.Find("HP").GetComponent<Text>();
        atk = this.transform.Find("ATK").GetComponent<Text>();
        spd = this.transform.Find("SPD").GetComponent<Text>();
        //if (!bc.StatusChange) bc.Start();
        nameText.text = bc.CharaClass.CharaName;

        hp.text = "HP: " + ((int)(bc.Hp)).ToString();
        atk.text = "ATK: " + ((int)(bc.Atk)).ToString();
        spd.text = "SPD: " + ((int)(bc.Spd)).ToString();
        bc.StatusChange = false;
    }

    private void Update()
    {
        if (bc.StatusChange)
        {
            hp.text = "HP: " + ((int)(bc.Hp)).ToString();
            atk.text = "ATK: " + ((int)(bc.Atk)).ToString();
            spd.text = "SPD: " + ((int)(bc.Spd)).ToString();
            bc.StatusChange = false;
        }
    }

    private bool SetCharacter()
    {
        DungeonBattleManager dbm = DungeonBattleManager.Instance;
        try
        {
            if (isEnemy)
            {
                this.bc = dbm.Enemys[this.index];
            }
            else
            {
                this.bc = dbm.Allys[this.index];
            }
        }catch(Exception)
        {
            Destroy(this.gameObject);
            return false;
        }
        return true;
    }

    public BattleCharacter Chara
    {
        get
        {
            if (this.bc == null) SetCharacter();
            return this.bc;
        }
    }
    
}
