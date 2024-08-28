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

    GameObject dialogAnimation;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        randomPatrol = GetComponent<NPCRandomPatrol>();
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
        randomPatrol.FacePlayer(playerPos);
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
