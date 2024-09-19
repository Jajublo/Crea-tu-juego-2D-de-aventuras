using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataInstance : MonoBehaviour
{
    private static DataInstance instance;

    public int slotIndex;

    public Vector2 playerPosition;
    public int sceneIndex;

    public int currentHearts;
    public int hp;
    public int currentKeys;
    public int coins;
    public bool unlockBow;
    public bool unlockBomb;
    public bool unlockWand;
    public int selectedWeapon;
    public int[] selectedWeaponAmmo;

    static string SaveDataKey = "SaveDataKey";
    public GameData gameData;
    public SaveData saveData;
    public SceneData sceneData;

    public static DataInstance Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = new GameObject("DataInstance");
                instance = go.AddComponent<DataInstance>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadData()
    {
        if(!PlayerPrefs.HasKey(SaveDataKey)) CreateGameData();

        string json = PlayerPrefs.GetString(SaveDataKey);
        gameData = JsonUtility.FromJson<GameData>(json);
    }

    public void SetSlotData(int index)
    {
        slotIndex = index;
        sceneIndex = gameData.saveData[index].sceneIndex;
    }

    public void SetPlayerPosition(Vector2 playerPos, int sceneIndex)
    {
        playerPosition = playerPos;
        this.sceneIndex = sceneIndex;

        GameManager gm = FindObjectOfType<GameManager>();

        currentHearts = gm.currentHearts;
        hp = gm.hp;
        currentKeys = gm.currentKeys;
        coins = gm.coins;
        selectedWeapon = gm.selectedWeapon;
        selectedWeaponAmmo = gm.selectedWeaponAmmo;

        SavePlayerData();
    }

    private void SavePlayerData()
    {
        saveData.playerPosition = playerPosition;
        saveData.sceneIndex = sceneIndex;

        saveData.currentHearts = currentHearts;
        saveData.hp = hp;
        saveData.currentKeys = currentKeys;
        saveData.coins = coins;
        saveData.selectedWeapon = selectedWeapon;
        saveData.selectedWeaponAmmo = selectedWeaponAmmo;
        saveData.unlockWand = unlockWand;
        saveData.unlockBow = unlockBow;
        saveData.unlockBomb = unlockBomb;

        gameData.saveData[slotIndex] = saveData;

        string json = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString(SaveDataKey, json);
        PlayerPrefs.Save();
    }

    public void SaveSceneData(string name)
    {
        if(sceneData == null || sceneData.sceneName != SceneManager.GetActiveScene().name)
        {
            sceneData = new SceneData();
            sceneData.sceneName = SceneManager.GetActiveScene().name;
            sceneData.objectsName = new List<string>();
        }

        if (saveData.sceneData.Contains(sceneData)) saveData.sceneData.Remove(sceneData);

        sceneData.objectsName.Add(name);

        saveData.sceneData.Add(sceneData);

        SavePlayerData();
    }

    public void LoadDataSlot()
    {
        saveData = gameData.saveData[slotIndex];

        playerPosition = saveData.playerPosition;
        sceneIndex = saveData.sceneIndex;

        currentHearts = saveData.currentHearts;
        currentKeys = saveData.currentKeys;
        hp = saveData.hp;
        coins = saveData.coins;
        selectedWeapon = saveData.selectedWeapon;
        selectedWeaponAmmo = saveData.selectedWeaponAmmo;
        unlockBomb = saveData.unlockBomb;
        unlockBow = saveData.unlockBow;
        unlockWand = saveData.unlockWand;

        foreach (SceneData sceneData in saveData.sceneData)
        {
            if (sceneData.sceneName == SceneManager.GetActiveScene().name)
            {
                this.sceneData = sceneData;

                foreach (string name in sceneData.objectsName)
                {
                    GameObject gameObject = GameObject.Find(name);

                    if(gameObject != null)
                    {
                        if (gameObject.GetComponent<LockedDoor>()) gameObject.GetComponent<LockedDoor>().OpenedDoor();
                        else if (gameObject.GetComponent<Chest>()) gameObject.GetComponent<Chest>().OpenedChest();
                        else gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void CreateGameData()
    {
        GameData gameData = new GameData();
        gameData.saveData = new List<SaveData>();
        gameData.saveData.Add(CreateSaveData(0));
        gameData.saveData.Add(CreateSaveData(1));
        gameData.saveData.Add(CreateSaveData(2));

        gameData.musicVolume = 1;
        gameData.sfxVolume = 1;

        string json = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString(SaveDataKey, json);
        PlayerPrefs.Save();
    }

    private SaveData CreateSaveData(int index)
    {
        SaveData saveData = new SaveData();
        saveData.saveDataSlot = index;
        saveData.playerPosition = new Vector2(0, -1);
        saveData.sceneIndex = 1;
        saveData.currentHearts = 3;
        saveData.hp = 12;
        saveData.currentKeys = 0;
        saveData.coins = 0;
        saveData.selectedWeaponAmmo = new int[] { 0, 0, 0 };
        saveData.unlockBomb = false;
        saveData.unlockBow = false;
        saveData.unlockWand = false;
        saveData.sceneData = new List<SceneData>();

        return saveData;
    }

    public void DeleteSaveData(int index)
    {
        gameData.saveData[index] = CreateSaveData(index);
    }

    private void Delete()
    {
        PlayerPrefs.DeleteAll();
    }
}

[System.Serializable]
public class GameData
{
    public List<SaveData> saveData;
    public float musicVolume;
    public float sfxVolume;
}

[System.Serializable]
public class SaveData
{
    public int saveDataSlot;

    public Vector2 playerPosition;
    public int sceneIndex;

    public int currentHearts;
    public int hp;
    public int currentKeys;
    public int coins;

    public bool unlockBow;
    public bool unlockBomb;
    public bool unlockWand;

    public int selectedWeapon;
    public int[] selectedWeaponAmmo;
    public List<SceneData> sceneData;
}

[System.Serializable]
public class SceneData
{
    public string sceneName;
    public List<string> objectsName;
}
