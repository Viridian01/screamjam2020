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

    public AudioClip interactSound;

    public virtual void Interact()
    {
        // Play sound if it exists
        if (interactSound) AudioSource.PlayClipAtPoint(interactSound, transform.position);
    }
    public virtual void Interact(GameObject obj)
    {
        Interact();
    }
}
