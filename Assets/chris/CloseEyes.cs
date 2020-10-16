using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseEyes : MonoBehaviour
{
    //Lid Objects 
    GameObject TopLid;
    GameObject BotLid;

    bool holdingSpace = false;

    // Start is called before the first frame update
    void Start()
    {
        TopLid = GameObject.Find("TopEyeL");
        BotLid = GameObject.Find("BotEyeL");
        
    }

    // Update is called once per frame
    void Update()
    {
        //Eyelid Close on Spacebar
        if (Input.GetKey(KeyCode.Space))
        {
            TopLid.GetComponent<Animator>().SetTrigger("CloseTop");
            BotLid.GetComponent<Animator>().SetTrigger("CloseBottom");
        }
        else
        {
            BotLid.GetComponent<Animator>().SetTrigger("OpenBottom");
            TopLid.GetComponent<Animator>().SetTrigger("OpenTop");
        }

        holdingSpace = Input.GetKey(KeyCode.Space);

        TopLid.GetComponent<Animator>().SetBool("Open", holdingSpace);
        BotLid.GetComponent<Animator>().SetBool("Open", holdingSpace);
    }

    public bool EyesOpen()
    {
        return holdingSpace;
    }
}
