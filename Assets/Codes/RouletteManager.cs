using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteManager : MonoBehaviour
{
    public GameObject RoulettePanel;
    public GameObject RoulettePlate;
    public GameObject GiveItemManager;
    public Transform needle;
    public Image[] slotNumber;
    public Button spinButton;
    public Button nextButton;

    List<int> ResultIndexList = new List<int>();
    int ItemCount = 4;

    public bool isSpinning = false;
    float rotateSpeed;
    float realDeltaTime;

    void OnEnable()
    {
        GameManager.instance.Stop();
        spinButton.interactable = true;
        nextButton.interactable = false;
        ResultIndexList.Clear();
    }

    void Start()
    {
        realDeltaTime = 0f;
    }

    void Update()
    {
        realDeltaTime += Time.unscaledDeltaTime;

        if (isSpinning)
        {
            if (rotateSpeed > 1f)
            {
                RoulettePlate.transform.Rotate(0, 0, -rotateSpeed * Time.unscaledDeltaTime);
                rotateSpeed = Mathf.Lerp(rotateSpeed, 0, Time.unscaledDeltaTime * 0.7f);
            }
            else
            {
                isSpinning = false;
                Result();
                nextButton.interactable = true;
            }
        }
    }

    public void StartRoulette()
    {
        float randomSpd = Random.Range(1.0f, 5.0f);
        rotateSpeed = 10000f * randomSpd;
        isSpinning = true;
        spinButton.interactable = false;
    }

    void Result()
    {
        int closetIndex = -1;
        float closeDis = float.MaxValue;
        float currentDis = 0f;

        for (int i = 0; i < ItemCount; i++)
        {
            currentDis = Vector2.Distance(slotNumber[i].transform.position, needle.position);
            if (closeDis > currentDis)
            {
                closeDis = currentDis;
                closetIndex = i;
            }
        }

        if (closetIndex != -1)
        {
            ResultIndexList.Add(closetIndex);
            Debug.Log("Closest Slot: " + closetIndex);
        }
    }

    public void NextPage()
    {
        // closetIndex를 인자로 전달
        GiveItemManager.GetComponent<GiveItemManager>().SetClosestIndex(ResultIndexList[0]);
        // 룰렛매니저를 비활성화
        gameObject.SetActive(false);
        // GiveItemManager를 활성화
        GiveItemManager.SetActive(true);
    }
}
