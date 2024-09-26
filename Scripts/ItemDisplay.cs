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
        manaBar.transform.parent.gameObject.SetActive(false);
    }

    public void ChangeItemIcon(int x, int ammo, int maxMana)
    {
        bool bomb = DataInstance.Instance.unlockBomb;
        bool bow = DataInstance.Instance.unlockBow;
        bool wand = DataInstance.Instance.unlockWand;

        if(wand)
        {
            manaBar.transform.parent.gameObject.SetActive(true);
        }

        if (bomb && x == 0 || bow && x == 1 || wand && x == 2)
        {
            image.sprite = itemsSprites[x];
            rectTransform.sizeDelta = itemsSpritesDimensions[x];
            rectTransform.rotation = Quaternion.Euler(0, 0, itemsSpritesRotation[x]);
            if (x == 2)
            {
                manaBar.fillAmount = ammo / (maxMana * 1f);
                ammoText.text = "";
            }
            else
            {
                ammoText.text = ammo.ToString();
            }
        }
        else
        {
            ammoText.text = "";
            rectTransform.sizeDelta = Vector2.zero;
        }
    }
}
