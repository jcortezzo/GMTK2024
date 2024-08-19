using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSpawner : MonoBehaviour
{
    [SerializeField] private GameObject TextPrefab;
    [SerializeField] private List<string> TextList;
    [field:SerializeField] public int MinTextSize { get; set; }
    [field: SerializeField] public int MaxTextSize { get; set; }

    public void SpawnRandomText()
    {
        if (TextList.Count == 0) return;
        var randIndex = Random.Range(0, TextList.Count);
        var text = TextList[randIndex];

        var textObj = Instantiate(TextPrefab, transform.position, transform.rotation);
        var tmp = textObj.GetComponent<TMP_Text>();
        tmp.text = text;
        var textBubble = textObj.GetComponent<BubbleText>();
        textBubble.minSize = MinTextSize;
        textBubble.maxSize = MaxTextSize;
        textBubble?.StartTextAnimation();
    }
}
