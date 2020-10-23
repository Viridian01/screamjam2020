using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimate : MonoBehaviour
{
    public GameObject handsCover;
    public GameObject handsOpen;

    public MonsterAI monster;

    // Start is called before the first frame update
    void Start()
    {
        SetCover(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!monster) return;

        if (monster.IsHunting())
        {
            SetCover(false);
        }
        else
        {
            SetCover(true);
        }
    }

    void SetCover(bool cover)
    {
        handsCover.SetActive(cover);

        handsOpen.SetActive(!cover);
    }
}
