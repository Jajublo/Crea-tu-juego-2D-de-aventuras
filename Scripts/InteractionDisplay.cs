using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionDisplay : MonoBehaviour
{
    public Sprite[] itemsSprites;
    public Vector2[] itemsSpritesDimensions;

    private Image image;
    private RectTransform rectTransform;

    private GameManager gameManager;

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        gameManager = FindObjectOfType<GameManager>();
    }

    public void ChangeInteraction(BasicInteraction basicInteraction)
    {
        if (basicInteraction == null)
        {
            ChangeInteractionIcon(0); // Attack
        } 
        else if (basicInteraction is NPCBasicDialog)
        {
            ChangeInteractionIcon(1); // Read
        }
        else if (basicInteraction is SignDialog)
        {
            ChangeInteractionIcon(1); // Read
        }
        else if (basicInteraction is LockedDoor)
        {
            if(gameManager.currentKeys > 0)
            {
                ChangeInteractionIcon(2); // Unlock
            }
            else
            {
                ChangeInteractionIcon(3); // Locked
            }
        }
        else if (basicInteraction is Chest chest)
        {
            if (chest.locked)
            {
                if (gameManager.currentKeys > 0)
                {
                    ChangeInteractionIcon(2); // Unlock
                }
                else
                {
                    ChangeInteractionIcon(3); // Locked
                }
            }
            else
            {
                ChangeInteractionIcon(4); // Open
            }
        }
        else if (basicInteraction is BuyItem buy)
        {
            ChangeInteractionIcon(5);
        }
    }

    private void ChangeInteractionIcon(int x)
    {
        image.sprite = itemsSprites[x];
        rectTransform.sizeDelta = itemsSpritesDimensions[x];
    }
}
