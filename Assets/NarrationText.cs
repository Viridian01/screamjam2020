using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NarrationText : MonoBehaviour
{

    TextMeshProUGUI associatedText;

    // Start is called before the first frame update
    void Start()
    {
        associatedText = GetComponent<TextMeshProUGUI>();
    }

    public void DisplayText(string textToShow, float timeToWait)
    {
        StartCoroutine(TimedText(textToShow, timeToWait));
    }

    IEnumerator TimedText(string text, float time)
    {
        associatedText.text = text;
        yield return new WaitForSeconds(time);
        associatedText.text = "";
        StopCoroutine(TimedText(text, time));
    }
}
