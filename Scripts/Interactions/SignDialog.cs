using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignDialog : BasicInteraction
{
    public string[] dialog;
    int dialogCounter;
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public override bool Interact(Vector2 playerFacing, Vector2 playerPos)
    {
        bool success = FacingObject(playerFacing);

        if (success) NextDialog();
        else EndDialog();

        return success;
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
