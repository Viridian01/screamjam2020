using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopOffset : MonoBehaviour
{
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        if (!source || !source.clip) return;

        source.PlayDelayed(Random.Range(0f, source.clip.length/2f));
    }
}
