using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveItemManager : MonoBehaviour
{
    public LevelUp levelUp;
    public GameObject[] slotItems;
    private int closetIndex;

    public void SetClosestIndex(int index)
    {
        closetIndex = index + 1;
    }

    void OnEnable()
    {
        // LevelUp의 availableIndices를 가져온다
        List<int> availableIndices = levelUp.GetAvailableIndices();
        Debug.Log("availableIndices: " + availableIndices.Count);
        Debug.Log("closetIndex: " + closetIndex);

        // 랜덤으로 선택된 아이템들의 정보를 저장할 리스트
        List<int> selectedIndices = new List<int>();

        // availableIndices에서 closetIndex 개의 아이템 선택
        for (int i = 0; i < closetIndex; i++)
        {
            if (availableIndices.Count == 0) break; // 선택할 아이템이 없는 경우 종료

            int randomIndex = Random.Range(0, availableIndices.Count);
            int selectedIndex = availableIndices[randomIndex];
            // 선택된 인덱스를 기록
            selectedIndices.Add(selectedIndex);
            // 아이템 선택 및 활성화
            levelUp.Select(selectedIndex);
            availableIndices.RemoveAt(randomIndex); // 중복 방지

            Debug.Log("selectedIndices: " + selectedIndex);
        }

        // 선택된 아이템들에 대한 UI 업데이트
        UpdateItemPanel(selectedIndices);
    }

    void UpdateItemPanel(List<int> selectedIndices)
    {
        // 기존의 아이템 UI를 모두 비활성화
        foreach (GameObject slotItem in slotItems)
        {
            slotItem.SetActive(false);
        }

        // 선택된 아이템에 대한 SlotItem UI 활성화
        for (int i = 0; i < selectedIndices.Count; i++)
        {
            if (i >= slotItems.Length) break; // 인덱스가 slotItems 배열의 크기를 넘지 않도록 확인

            int index = selectedIndices[i];
            if (index >= levelUp.GetComponentsInChildren<Item>(true).Length) continue; // 유효한 인덱스인지 확인

            // 선택된 인덱스에 해당하는 slotItem 활성화
            GameObject slotItem = slotItems[i];
            Item item = levelUp.GetComponentsInChildren<Item>(true)[index];
            Image slotItemImage = slotItem.GetComponent<Image>();
            slotItemImage.sprite = item.data.itemIcon;
            slotItem.SetActive(true);
        }
    }

    public void OnDisable()
    {
        GameManager.instance.Resume();
        gameObject.SetActive(false);
    }
}
