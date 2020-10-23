using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimate : MonoBehaviour
{
    public GameObject handsCover;
    public GameObject handsOpen;

    public MonsterAI monster;

    private bool kill = false;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        SetCover(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (kill)
        {
            transform.position = player.transform.position + player.transform.forward;
            return;
        }
        else
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
    }

    void SetCover(bool cover)
    {
        handsCover.SetActive(cover);

        handsOpen.SetActive(!cover);
    }

    public void OpenEyesDie()
    {
        SetCover(false);
        kill = true;
    }
}
