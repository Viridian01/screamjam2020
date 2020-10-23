using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScary : MonoBehaviour
{
    public AudioSource asource;
    public PlayerSight sight;
    public float scaryRange = 20f;
    public float calmRate = 1f;

    float intenseCache = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MonsterAI monster = sight.ClosestMonster();

        if (!monster)
        {
            intenseCache = Mathf.Lerp(intenseCache, 0f, Time.deltaTime * calmRate);
            SetScaryIntensity(intenseCache);
            return;
        }

        if (monster.IsHunting() || monster.WasLookedAt())
        {
            float dist = Vector3.Distance(monster.transform.position, transform.position);

            float a = Mathf.Lerp(0f, 1f, 1f - dist / scaryRange);

            if (sight && !sight.CheckLoS(monster)) a /= 2f;

            SetScaryIntensity(a);
            intenseCache = a;
        }
        else
        {
            intenseCache = Mathf.Lerp(intenseCache, 0f, Time.deltaTime * calmRate);
            SetScaryIntensity(intenseCache);
        }
    }

    // 0 for safety, 1 for DANGER
    void SetScaryIntensity(float a)
    {
        if (!asource) return;

        if (a >= 0.5f)
        {
            asource.pitch = Mathf.Clamp(0.6f + (a * 0.8f), 1f, 1.5f);
        }
        else
        {
            asource.pitch = 1f;
        }

        asource.volume = a;
    }
}
