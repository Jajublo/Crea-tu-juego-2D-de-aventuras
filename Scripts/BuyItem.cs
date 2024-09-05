using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItem : BasicInteraction
{
    public string[] dialog;
    int dialogCounter;

    public int itemPrice;
    public int spawnAmmount;
    public GameObject item;

    GameManager gameManager;

    public GameObject dialogAnimation;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogAnimation = transform.GetChild(0).gameObject;
    }

    public override bool CanInteract(Vector2 playerFacing, Vector2 playerPos)
    {
        bool success = FacingNPC(playerFacing, playerPos, transform.position);

        dialogAnimation.SetActive(success);

        return success;
    }

    public override void Interact(Vector2 playerFacing, Vector2 playerPos)
    {
        NextDialog(playerPos);
    }

    private void NextDialog(Vector2 playerPos)
    {
        if (dialogCounter == dialog.Length)
        {
            EndDialog();
        }
        else
        {
            if(dialogCounter == dialog.Length - 1)
            {
                gameManager.ShowChoiceButtons(itemPrice);
                gameManager.noButton.onClick.AddListener(EndDialog);

                if (gameManager.coins >= itemPrice)
                {
                    gameManager.yesButton.onClick.AddListener(() => PurchaseItem(playerPos));
                    gameManager.yesButton.onClick.AddListener(EndDialog);
                }
            }
            gameManager.ShowText(dialog[dialogCounter]);
            dialogCounter++;
        }
    }

    private void EndDialog()
    {
        gameManager.HideText();
        gameManager.HideChoiceButtons();
        dialogCounter = 0;
        FindObjectOfType<PlayerMovement>().PausePlayer();
    }

    private void PurchaseItem(Vector2 playerPos)
    {
        gameManager.UpdateCoins(-itemPrice);
        for (int i = 0; i < spawnAmmount; i++)
        {
            Instantiate(item, playerPos, transform.rotation);
        }
    }
}
