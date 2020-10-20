using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractiveTypes
{
    Default,
    Door,
    Item
}
public class InteractableObject : MonoBehaviour
{
    protected string objName;
    public string interactText;

    public virtual void Interact()
    {
        
    }
    public virtual void Interact(GameObject obj)
    {

    }
}
