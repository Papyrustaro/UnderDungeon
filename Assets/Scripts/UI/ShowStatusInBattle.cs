using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStatusInBattle : MonoBehaviour
{
    [SerializeField]
    private BattleCharacter bc;
    private Text nameText;
    private Text hp;
    private Text atk;
    private Text spd;


    // Start is called before the first frame update
    void Start()
    {
        nameText = this.transform.Find("Name").GetComponent<Text>();
        hp = this.transform.Find("HP").GetComponent<Text>();
        atk = this.transform.Find("ATK").GetComponent<Text>();
        spd = this.transform.Find("SPD").GetComponent<Text>();
        nameText.text = bc.CharaClass.CharaName;
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

    public BattleCharacter Chara => this.bc;
    
}
