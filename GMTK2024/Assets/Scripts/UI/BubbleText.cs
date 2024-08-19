using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class BubbleText : MonoBehaviour
{
    public float DestroyAfterSeconds;
    public int minSize;
    public int maxSize;
    private TMP_Text tmpText;
    // Start is called before the first frame update
    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartTextAnimation()
    {
        StartCoroutine(TextAnimation());
    }

    private IEnumerator TextAnimation()
    {
        float elapsedTime = 0f;
        Color startColor = tmpText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // Target color with alpha = 0
        var randomTextSize = Random.Range(minSize, maxSize);

        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 startPosition = tmpText.transform.position;
        Vector3 endPosition = startPosition + randomDirection * 5f;

        tmpText.fontSize = randomTextSize;
        while (elapsedTime < DestroyAfterSeconds)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / DestroyAfterSeconds);
            tmpText.color = Color.Lerp(startColor, endColor, t);

            tmpText.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            yield return null; // Wait until the next frame
        }

        tmpText.color = endColor; // Ensure the final alpha is set to 0
        Destroy(gameObject);
    }
}
