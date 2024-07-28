using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static float Speed
    {
        get { return GameManager.instance.playerId == 0 ? 3f : 5f; }
    }

    public static float WeaponSpeed
    {
        get { return GameManager.instance.playerId == 0 ? 1f : 1.2f; }
    }

    public static float WeaponRate
    {
        get { return GameManager.instance.playerId == 0 ? 1f : 1.2f; }
    }

    public static float Damage
    {
        get { return GameManager.instance.playerId == 0 ? 1f : 1.2f; }
    }

    public static int Count
    {
        get { return GameManager.instance.playerId == 0 ? 0 : 0; }
    }
}
