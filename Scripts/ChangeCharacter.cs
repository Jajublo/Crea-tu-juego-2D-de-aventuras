using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharacter : MonoBehaviour
{
    public GameObject characterSelector;

    public Image characterSelectButton;
    public SpriteRenderer characterScreen;

    public Sprite[] characterButtonSprites;
    public Sprite[] characterScreenSprites;

    bool oppened;

    private void Awake()
    {
        SelectCharacter(PlayerPrefs.GetInt("CharacterSelection", 0));
    }

    public void OpenSelector()
    {
        if (oppened)
        {
            characterSelector.SetActive(false);
        }
        else
        {
            characterSelector.SetActive(true);
        }

        oppened = !oppened;
    }

    public void SelectCharacter(int i)
    {
        PlayerPrefs.SetInt("CharacterSelection",i);
        PlayerPrefs.Save();

        characterSelectButton.sprite = characterButtonSprites[i];
        characterScreen.sprite = characterScreenSprites[i];
    }
}
