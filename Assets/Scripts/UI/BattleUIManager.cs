using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    private static Text announceText;
    [SerializeField] private Text announceTextObject;

    [SerializeField] private List<Button> enemyTargetButton = new List<Button>();
    [SerializeField] private List<Button> allyTargetButton = new List<Button>();

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

    public void SetInit(int allyNum, int enemyNum)
    {
        for(int i = 3; i >= allyNum; i--)
        {
            this.allyTargetButton.RemoveAt(i);
        }
        for(int i = 3; i >= enemyNum; i--)
        {
            this.enemyTargetButton.RemoveAt(i);
        }
    }

    /// <summary>
    /// ターゲット選択ボタンの表示を切り替える
    /// </summary>
    /// <param name="targetIsAlly">ターゲットが味方かどうか</param>
    public void SetActiveAllyTargetButtons(bool targetIsAlly)
    {
        foreach(Button button in this.allyTargetButton)
        {
            button.gameObject.SetActive(targetIsAlly);
        }
        foreach(Button button in this.enemyTargetButton)
        {
            button.gameObject.SetActive(!targetIsAlly);
        }
        if (targetIsAlly) this.allyTargetButton[0].Select();
        else this.enemyTargetButton[0].Select();
    }
}
