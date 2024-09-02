using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplay : MonoBehaviour
{
    public Sprite[] itemsSprites;
    public Vector2[] itemsSpritesDimensions;
    public float[] itemsSpritesRotation;
    public Image manaBar;

    public TextMeshProUGUI ammoText;

    private Image image;
    private RectTransform rectTransform;

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void ChangeItemIcon(int x, int ammo, int maxMana)
    {
        image.sprite = itemsSprites[x];
        rectTransform.sizeDelta = itemsSpritesDimensions[x];
        rectTransform.rotation = Quaternion.Euler(0, 0, itemsSpritesRotation[x]);
        if(x == 2)
        {
            manaBar.fillAmount = ammo / (maxMana * 1f);
            ammoText.text = "";
        }
        else
        {
            ammoText.text = ammo.ToString();
        }
    }
}
