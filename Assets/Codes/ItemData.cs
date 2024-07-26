using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject의 기반이 되는 스크립트이다.
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount; // 근거리는 갯수, 원거리는 관통횟수
    public float[] damages;
    public int[] counts;
    
    [Header("# Weapon")]
    public GameObject projectile; // 투사체 프리펩
    public Sprite hand;
}
