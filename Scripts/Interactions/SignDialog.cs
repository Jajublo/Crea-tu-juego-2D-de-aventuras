using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignDialog : BasicInteraction
{
    public string[] dialog;
    int dialogCounter;
    GameManager gameManager;

    GameObject dialogAnimation;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogAnimation = transform.GetChild(0).gameObject;
    }

    public override bool CanInteract(Vector2 playerFacing, Vector2 playerPos)
    {
        bool success = FacingObject(playerFacing);

        dialogAnimation.SetActive(success);

        return success;
    }

    public override void Interact(Vector2 playerFacing, Vector2 playerPos)
    {
        NextDialog();
    }

    private void NextDialog()
    {
        if (dialogCounter == dialog.Length)
        {
            EndDialog();
        }
        else
        {
            gameManager.ShowText(dialog[dialogCounter]);
            dialogCounter++;
        }
    }

    private void EndDialog()
    {
        gameManager.HideText();
        dialogCounter = 0;
    }
}
