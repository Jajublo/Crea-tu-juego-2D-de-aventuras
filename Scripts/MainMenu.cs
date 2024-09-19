using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class MainMenu : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject saveSlots;

    public Sprite[] heartStatus;
    static int maxHearts = 14;

    [Header("Slot1")]
    public Image[] hearts0;
    public Image[] keys0;
    public TextMeshProUGUI coin0;
    public GameObject bow0;
    public GameObject bomb0;
    public GameObject wand0;

    [Header("Slot2")]
    public Image[] hearts1;
    public Image[] keys1;
    public TextMeshProUGUI coin1;
    public GameObject bow1;
    public GameObject bomb1;
    public GameObject wand1;

    [Header("Slot3")]
    public Image[] hearts2;
    public Image[] keys2;
    public TextMeshProUGUI coin2;
    public GameObject bow2;
    public GameObject bomb2;
    public GameObject wand2;

    public GameData gameData;

    private void Awake()
    {
        DataInstance.Instance.LoadData();
        SetSaveFiles();
    }

    public void SelectSaveData()
    {
        mainButtons.SetActive(false);
        saveSlots.SetActive(true);
    }

    public void Back()
    {
        mainButtons.SetActive(true);
        saveSlots.SetActive(false);
    }

    public void Play(int index)
    {
        DataInstance.Instance.SetSlotData(index);
        DataInstance.Instance.LoadDataSlot();
        SceneManager.LoadScene(DataInstance.Instance.sceneIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void SetSaveFiles()
    {
        gameData = DataInstance.Instance.gameData;
        UpdateHearts(gameData);
        UpdateKeys(gameData);
        UpdateCoins(gameData);
        UpdateItems(gameData);
    }

    private void UpdateHearts(GameData gameData)
    {
        int aux = gameData.saveData[0].hp;

        for (int i = 0; i < maxHearts; i++)
        {
            if (i < gameData.saveData[0].currentHearts)
            {
                hearts0[i].gameObject.SetActive(true);
                hearts0[i].sprite = GetHeartStatus(aux);
                aux -= 4;
            }
            else
            {
                hearts0[i].gameObject.SetActive(false);
            }
        }

        aux = gameData.saveData[1].hp;

        for (int i = 0; i < maxHearts; i++)
        {
            if (i < gameData.saveData[1].currentHearts)
            {
                hearts1[i].gameObject.SetActive(true);
                hearts1[i].sprite = GetHeartStatus(aux);
                aux -= 4;
            }
            else
            {
                hearts1[i].gameObject.SetActive(false);
            }
        }

        aux = gameData.saveData[2].hp;

        for (int i = 0; i < maxHearts; i++)
        {
            if (i < gameData.saveData[2].currentHearts)
            {
                hearts2[i].gameObject.SetActive(true);
                hearts2[i].sprite = GetHeartStatus(aux);
                aux -= 4;
            }
            else
            {
                hearts2[i].gameObject.SetActive(false);
            }
        }
    }

    private Sprite GetHeartStatus(int x)
    {
        switch (x)
        {
            case >= 4: return heartStatus[4];
            case 3: return heartStatus[3];
            case 2: return heartStatus[2];
            case 1: return heartStatus[1];
            default: return heartStatus[0];
        }
    }

    public void UpdateKeys(GameData gameData)
    {
        for (int i = 0; i < keys0.Length; i++)
        {
            keys0[i].gameObject.SetActive(gameData.saveData[0].currentKeys > i);
        }
        for (int i = 0; i < keys1.Length; i++)
        {
            keys1[i].gameObject.SetActive(gameData.saveData[1].currentKeys > i);
        }
        for (int i = 0; i < keys2.Length; i++)
        {
            keys2[i].gameObject.SetActive(gameData.saveData[2].currentKeys > i);
        }
    }

    public void UpdateCoins(GameData gameData)
    {
        coin0.text = gameData.saveData[0].coins.ToString();
        coin1.text = gameData.saveData[1].coins.ToString();
        coin2.text = gameData.saveData[2].coins.ToString();
    }

    public void UpdateItems(GameData gameData)
    {
        bomb0.SetActive(gameData.saveData[0].unlockBomb);
        bow0.SetActive(gameData.saveData[0].unlockBow);
        wand0.SetActive(gameData.saveData[0].unlockWand);

        bomb1.SetActive(gameData.saveData[1].unlockBomb);
        bow1.SetActive(gameData.saveData[1].unlockBow);
        wand1.SetActive(gameData.saveData[1].unlockWand);

        bomb2.SetActive(gameData.saveData[2].unlockBomb);
        bow2.SetActive(gameData.saveData[2].unlockBow);
        wand2.SetActive(gameData.saveData[2].unlockWand);
    }
}
