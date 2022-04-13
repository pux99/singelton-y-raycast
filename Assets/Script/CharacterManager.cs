using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instans;
    public float health, TimesBeenHit;
    private void Awake()
    {
        if(CharacterManager.instans == null)
        {
            CharacterManager.instans = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public float Hit()
    {
        TimesBeenHit++;
        return TimesBeenHit;
    }
    public float Changehealth(float change)
    {
        health += change;
        return health;
    }
}
