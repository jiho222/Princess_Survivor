using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotItems : MonoBehaviour
{
    public ItemData data;
    public Image icon;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1]; // 두번째 값으로 가져오기(첫 번쨰는 자기자신)
        icon.sprite = data.itemIcon;
    }

    // void OnEnable()
    // {
    //     textLevel.text = "Lv." + (level + 1);

    //     switch (data.itemType) {
    //         case ItemData.ItemType.Melee:
    //         case ItemData.ItemType.Range:
    //             textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
    //             break;
    //         case ItemData.ItemType.Glove:
    //         case ItemData.ItemType.Shoe:
    //             textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
    //             break;
    //         default:
    //             textDesc.text = string.Format(data.itemDesc);
    //             break;
    //     }
    // }
}
