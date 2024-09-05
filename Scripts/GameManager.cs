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

    public int coins;
    public TextMeshProUGUI coinsText;

    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;

    public GameObject npcDialogBox;
    public TextMeshProUGUI npcDialogText;
    public TextMeshProUGUI npcName;
    public Image npcImage;

    public GameObject choiceButtons;
    public Button yesButton;
    public Button noButton;

    private bool paused;
    public GameObject pauseMenu;

    public int selectedWeapon;
    public int[] selectedWeaponAmmo;
    public GameObject itemsMenu;
    private int maxMana = 5;

    private ItemDisplay itemDisplay;
    public TextMeshProUGUI weaponNameText;
    public string[] weaponName;
    public TextMeshProUGUI weaponDescriptionText;
    public string[] weaponDescription;

    public GameObject[] spawnItems;
    public float[] spawnItemsChance;

    private void Awake()
    {
        itemDisplay = FindObjectOfType<ItemDisplay>();
        DataInstance.Instance.LoadData();
        currentHearts = DataInstance.Instance.currentHearts;
        hp = DataInstance.Instance.hp;
        currentKeys = DataInstance.Instance.currentKeys;
        selectedWeapon = DataInstance.Instance.selectedWeapon;
        selectedWeaponAmmo = DataInstance.Instance.selectedWeaponAmmo;
        coins = DataInstance.Instance.coins;
        ResumeGame();
    }

    void Start()
    {
        currentHearts = Mathf.Clamp(currentHearts, minHearts, maxHearts);
        hp = Mathf.Clamp(hp, 1, currentHearts * 4);
        UpdateCurrentHearts();
        UpdateCurrentKeys(0);
        UpdateCoins(0);
        SelectWeapon(selectedWeapon);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(itemsMenu.activeSelf) OpenInvButton();
            else PauseButtonGame();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenInvButton();
        }
    }

    private void PauseGame()
    {
        if (paused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    private void OpenInv()
    {
        if (paused)
        {
            Time.timeScale = 0;
            itemsMenu.SetActive(true);
            itemsMenu.GetComponentsInChildren<Button>()[selectedWeapon].Select();
            ChangeWeaponText(selectedWeapon);
        }
        else
        {
            Time.timeScale = 1;
            itemsMenu.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        paused = false;
        PauseGame();
    }

    public void PauseButtonGame()
    {
        if (itemsMenu.activeSelf || dialogBox.activeSelf || npcDialogBox.activeSelf) return;
        paused = !paused;
        PauseGame();
    }

    public void OpenInvButton()
    {
        if (pauseMenu.activeSelf || dialogBox.activeSelf || npcDialogBox.activeSelf) return;
        paused = !paused;
        OpenInv();
    }

    public void SelectWeapon(int index)
    {
        selectedWeapon = index;
        itemDisplay.ChangeItemIcon(index, selectedWeaponAmmo[index], maxMana);
        paused = false;
        OpenInv();
    }

    public bool UpdateWeaponAmmo(int index, int amount)
    {
        if (amount > 0)
        {
            if (index == 2 && selectedWeaponAmmo[index] >= maxMana)
            {
                return false;
            }
            else if (selectedWeaponAmmo[index] >= 20)
            {
                return false;
            }
        }
        selectedWeaponAmmo[index] += amount;
        itemDisplay.ChangeItemIcon(index, selectedWeaponAmmo[index], maxMana);
        return true;
    }

    public void ChangeWeaponText(int index)
    {
        weaponNameText.text = weaponName[index];
        weaponDescriptionText.text = weaponDescription[index];
    }

    public void UpdateCoins(int amount)
    {
        coins = Mathf.Clamp(coins + amount, 0, 99);
        coinsText.text = coins.ToString();
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

    public void ShowChoiceButtons(int price)
    {
        if (coins >= price)
        {
            yesButton.interactable = true;
        }
        else
        {
            yesButton.interactable = false;
        }
        noButton.onClick.RemoveAllListeners();
        yesButton.onClick.RemoveAllListeners();
        choiceButtons.SetActive(true);
        noButton.Select();
    }

    public void HideChoiceButtons()
    {
        choiceButtons.SetActive(false);
    }

    public void SpawnItem(Vector2 enemyPos)
    {
        float x = Random.Range(0f, 100f);
        float sum = 0f;

        for (int i = 0; i < spawnItemsChance.Length; i++)
        {
            sum += spawnItemsChance[i];

            if(x < sum)
            {
                Instantiate(spawnItems[i], enemyPos, spawnItems[i].transform.rotation);
                break;
            }
        }
    }
}
