using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : BasicInteraction
{
    GameManager gameManager;
    private bool opened;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public override bool CanInteract(Vector2 playerFacing, Vector2 playerPos)
    {
        if (opened) return false;

        bool success = FacingObject(playerFacing);

        return success;
    }

    public override void Interact(Vector2 playerFacing, Vector2 playerPos)
    {
        OpenDoor();
    }

    private bool OpenDoor()
    {
        if(gameManager.currentKeys > 0)
        {
            gameManager.UpdateCurrentKeys(-1);
            opened = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            DataInstance.Instance.SaveSceneData(name);
            return true;
        }
        return false;
    }

    public void OpenedDoor()
    {
        opened = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
