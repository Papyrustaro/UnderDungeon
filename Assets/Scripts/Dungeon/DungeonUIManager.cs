using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonUIManager : MonoBehaviour
{
    private Text announceText;

    public static DungeonUIManager Instance { get; private set; }

    public Text AnnounceText => this.announceText;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        try
        {
            this.announceText = GameObject.Find("DungeonCanvas/AnnounceText").GetComponent<Text>();
        }
        catch
        {
            throw new System.Exception();
        }
    }

    public void AnnounceByText(string announceText)
    {
        Debug.Log(announceText);
        this.announceText.text = announceText;
    }

    public void LoadDungeonScene()
    {
        this.announceText = GameObject.Find("DungeonCanvas/AnnounceText").GetComponent<Text>();
    }
}
