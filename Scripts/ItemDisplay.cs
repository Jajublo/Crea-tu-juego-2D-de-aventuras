using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public Sprite[] itemsSprites;
    public Vector2[] itemsSpritesDimensions;
    public float[] itemsSpritesRotation;

    private Image image;
    private RectTransform rectTransform;

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void ChangeItemIcon(int x)
    {
        image.sprite = itemsSprites[x];
        rectTransform.sizeDelta = itemsSpritesDimensions[x];
        rectTransform.rotation = Quaternion.Euler(0, 0, itemsSpritesRotation[x]);
    }
}
