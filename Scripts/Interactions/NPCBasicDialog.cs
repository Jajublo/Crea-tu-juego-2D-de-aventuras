using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBasicDialog : BasicInteraction
{
    public string[] dialog;
    public string npcName;
    public Sprite image;
    int dialogCounter;
    GameManager gameManager;
    NPCRandomPatrol randomPatrol;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        randomPatrol = GetComponent<NPCRandomPatrol>();
    }

    public override bool Interact(Vector2 playerFacing, Vector2 playerPos)
    {
        bool success = FacingNPC(playerFacing, playerPos, transform.position);

        if (success)
        {
            randomPatrol.FacePlayer(playerPos);
            NextDialog();
        }
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
            gameManager.NPCShowText(dialog[dialogCounter], npcName, image);
            dialogCounter++;
        }
    }

    private void EndDialog()
    {
        gameManager.NPCHideText();
        dialogCounter = 0;
    }
}
