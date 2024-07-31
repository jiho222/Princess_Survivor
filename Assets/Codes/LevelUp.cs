using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    List<Weapon> filteredWeapons = new List<Weapon>();
    public List<int> availableIndices = new List<int>();

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next(); // Show에서 Next를 호출하여 레벨업 아이템을 준비합니다.
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        // AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        // AudioManager.instance.EffectBgm(false);
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    public List<int> GetAvailableIndices()
    {
        return new List<int>(availableIndices); // availableIndices를 반환
    }

    void Next()
    {
        // 현재 Player 하위의 무기를 확인 및 필터링
        filteredWeapons.Clear(); // 필터링된 무기 리스트를 초기화

        // 모든 Weapon 컴포넌트를 가져온 후 필터링
        Weapon[] allWeapons = GameManager.instance.player.GetComponentsInChildren<Weapon>(true);

        // Weapon + 숫자 형식만 필터링
        foreach (Weapon weapon in allWeapons)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(weapon.name, @"^Weapon \d+$"))
            {
                filteredWeapons.Add(weapon);
            }
        }

        // Debug: 필터링된 Weapon의 이름과 활성화 상태 출력
        foreach (Weapon weapon in filteredWeapons)
        {
            Debug.Log("Filtered Weapon Name: " + weapon.name + ", Active: " + weapon.gameObject.activeSelf);
        }

        // 1. 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 현재 Player 하위의 무기를 확인
        bool hasWeapon1 = false;
        bool hasWeapon5 = false;

        foreach (Weapon weapon in filteredWeapons)
        {
            if (weapon.gameObject.activeSelf)
            {
                if (weapon.name == "Weapon 1")
                {
                    hasWeapon1 = true;
                }
                else if (weapon.name == "Weapon 5")
                {
                    hasWeapon5 = true;
                }
            }
        }

        // 3. 가능한 아이템 인덱스 리스트 생성
        availableIndices.Clear();

        for (int i = 0; i < items.Length; i++)
        {
            // Weapon 1이 있으면 Item 5를 제외
            if (hasWeapon1 && i == 5) continue;
            // Weapon 5가 있으면 Item 1을 제외
            if (hasWeapon5 && i == 1) continue;

            availableIndices.Add(i);
        }

        // 4. 랜덤 3개 아이템 선택
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = availableIndices[Random.Range(0, availableIndices.Count)];
            ran[1] = availableIndices[Random.Range(0, availableIndices.Count)];
            ran[2] = availableIndices[Random.Range(0, availableIndices.Count)];

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[2] != ran[0])
                break;
        }

        // 5. 선택된 아이템 활성화
        for (int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[index]];

            // 만렙 아이템의 경우는 소비아이템으로 대체
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
