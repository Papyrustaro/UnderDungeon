using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonUIManager : MonoBehaviour
{
    [SerializeField] private Text announceText;

    public void AnnounceByText(string announceText)
    {
        Debug.Log(announceText);
        this.announceText.text = announceText;
    }
}
