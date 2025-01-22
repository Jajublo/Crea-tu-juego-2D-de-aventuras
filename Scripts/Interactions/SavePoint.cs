using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : BasicInteraction
{
    public string[] dialog;
    int dialogCounter;
    GameManager gameManager;

    public GameObject dialogAnimation;

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
        DataInstance.Instance.SetPlayerPosition(transform.position - Vector3.up, SceneManager.GetActiveScene().buildIndex);

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
            if (dialogCounter == dialog.Length - 1)
            {
                gameManager.ShowChoiceButtons(0);
                gameManager.yesButton.onClick.AddListener(OpenTpMap);
                gameManager.noButton.onClick.AddListener(EndDialog);
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

    private void OpenTpMap()
    {
        EndDialog();
        gameManager.OpenInvButton();
        gameManager.SwitchTpMenu();
    }
}
