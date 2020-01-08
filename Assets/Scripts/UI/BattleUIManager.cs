using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    private static Text announceText;
    [SerializeField] private Text announceTextObject;

    //[SerializeField] private GameObject enemyTargetButtonsObject;
    //[SerializeField] private GameObject allyTargetButtonsObject;
    [SerializeField] private Button[] enemyTargetButton = new Button[4];
    [SerializeField] private Button[] allyTargetButton = new Button[4];

    //public GameObject EnemyTargetButtons => this.enemyTargetButtonsObject;
    //public GameObject AllyTargetButtons => this.allyTargetButtonsObject;
    public Button[] EnemyTargetButton => this.enemyTargetButton;
    public Button[] AllyTargetButton => this.allyTargetButton;

    private void Awake()
    {
        announceText = announceTextObject;
    }
    public void PromptSelectTargetOneAlly()
    {
        ShowAnnounce("対象にする味方を選んでください");
        SetActiveAllyTargetButtons(true);
    }
    public static void ShowAnnounce(string text)
    {
        announceText.text = text;
        Debug.Log(text);
    }

    public void SetActiveAllyTargetButtons(bool flag)
    {
        foreach(Button button in this.allyTargetButton)
        {
            button.gameObject.SetActive(flag);
        }
        foreach(Button button in this.enemyTargetButton)
        {
            button.gameObject.SetActive(!flag);
        }
        if (flag) this.allyTargetButton[0].Select();
        else this.enemyTargetButton[0].Select();
    }
}
