using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text textBox;

    private DialogueVertexAnimator dialogueVertexAnimator;
    void Awake() {
        dialogueVertexAnimator = new DialogueVertexAnimator(textBox);
    }

    public void PlayDialogue(string message) {
        StopCoroutine(dialogueVertexAnimator.AnimateTextIn(message, null));
        dialogueVertexAnimator.textAnimating = false;
        StartCoroutine(dialogueVertexAnimator.AnimateTextIn(message, null));
    }
}
