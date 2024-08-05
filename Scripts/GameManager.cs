using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Image[] playerHearts;
    public Sprite[] heartStatus;
    public int currentHearts;
    public int hp;

    static int minHearts = 3;
    static int maxHearts = 14;

    public Image[] playerKeys;
    public int currentKeys;

    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;

    public GameObject npcDialogBox;
    public TextMeshProUGUI npcDialogText;
    public TextMeshProUGUI npcName;
    public Image npcImage;

    private bool paused;
    public GameObject pauseMenu;

    private void Awake()
    {
        DataInstance.Instance.LoadData();
        currentHearts = DataInstance.Instance.currentHearts;
        hp = DataInstance.Instance.hp;
        currentKeys = DataInstance.Instance.currentKeys;
        ResumeGame();
    }

    void Start()
    {
        currentHearts = Mathf.Clamp(currentHearts, minHearts, maxHearts);
        hp = Mathf.Clamp(hp, 1, currentHearts * 4);
        UpdateCurrentHearts();
        UpdateCurrentKeys(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if (paused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            pauseMenu.GetComponentInChildren<Button>().Select();
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        paused = false;
        PauseGame();
    }

    public void PauseButtonGame()
    {
        paused = !paused;
        PauseGame();
    }

    public bool CanHeal()
    {
        return hp < currentHearts * 4;
    }

    public void IncreaseMaxHP()
    {
        currentHearts++;
        currentHearts = Mathf.Clamp(currentHearts, minHearts, maxHearts);
        hp = currentHearts * 4;
        UpdateCurrentHearts();
    }

    public void UpdateCurrentHP(int x)
    {
        hp += x;
        hp = Mathf.Clamp(hp, 0, currentHearts * 4);
        UpdateCurrentHearts();
    }

    private void UpdateCurrentHearts()
    {
        int aux = hp;

        for (int i = 0; i < maxHearts; i++)
        {
            if (i < currentHearts)
            {
                playerHearts[i].enabled = true;
                playerHearts[i].sprite = GetHeartStatus(aux);
                aux -= 4;
            }
            else
            {
                playerHearts[i].enabled = false;
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

    public void UpdateCurrentKeys(int x)
    {
        currentKeys = Mathf.Clamp(currentKeys += x, 0, 99);

        for(int i = 0; i < playerKeys.Length; i++)
        {
            playerKeys[i].enabled = currentKeys > i;
        }
    }

    public void ShowText(string text)
    {
        dialogBox.SetActive(true);
        dialogText.text = text;
        Time.timeScale = 0;
    }

    public void HideText()
    {
        dialogBox.SetActive(false);
        dialogText.text = "";
        Time.timeScale = 1;
    }

    public void NPCShowText(string text, string name, Sprite image)
    {
        npcDialogBox.SetActive(true);
        npcDialogText.text = text;
        npcName.text = name;
        npcImage.sprite = image;
        Time.timeScale = 0;
    }

    public void NPCHideText()
    {
        npcDialogBox.SetActive(false);
        npcDialogText.text = "";
        npcName.text = "";
        npcImage.sprite = null;
        Time.timeScale = 1;
    }
}
