using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    public GameObject playerObject;
    private PlayerEngine player;
    private NarrationText narrationText;

    public string textToShow;
    public float timeToShow;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObject.GetComponent<PlayerEngine>();
        narrationText = FindObjectOfType<NarrationText>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerObject)
        {
            narrationText.DisplayText(textToShow, timeToShow);
        }
        Destroy(gameObject);
    }
}
