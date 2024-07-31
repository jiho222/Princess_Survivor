using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItemManager : MonoBehaviour
{
    int closetIndex;

    public void SetClosestIndex(int index)
    {
        closetIndex = index;
    }

    void OnEnable()
    {
        // GiveItemManager가 활성화될 때 실행할 추가 로직이 있다면 여기에 작성합니다.
    }



    public void OnDisable()
    {
        GameManager.instance.Resume();
        gameObject.SetActive(false);
    }
}

