using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueVertexAnimator {
    public bool textAnimating = false;
    private bool stopAnimating = false;

    private readonly TMP_Text textBox;

    public DialogueVertexAnimator(TMP_Text _textBox) {
        textBox = _textBox;
    }

    private static readonly Color32 clear = new Color32(0, 0, 0, 0);
    private static readonly Vector3 vecZero = Vector3.zero;

    public IEnumerator AnimateTextIn(string processedMessage, Action onFinish) {
        textAnimating = true;
        float secondsPerCharacter = 1f / 150f;
        float timeOfLastCharacter = 0;

        TMP_TextInfo textInfo = textBox.textInfo;
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfer = textInfo.meshInfo[i];
            if (meshInfer.vertices != null) {
                for (int j = 0; j < meshInfer.vertices.Length; j++) {
                    meshInfer.vertices[j] = vecZero;
                }
            }
        }

        textBox.text = processedMessage;
        textBox.ForceMeshUpdate();

        TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
        Color32[][] originalColors = new Color32[textInfo.meshInfo.Length][];

        for (int i = 0; i < originalColors.Length; i++) {
            Color32[] theColors = textInfo.meshInfo[i].colors32;
            originalColors[i] = new Color32[theColors.Length];
            Array.Copy(theColors, originalColors[i], theColors.Length);
        }

        int charCount = textInfo.characterCount;
        float[] charAnimStartTimes = new float[charCount];

        for (int i = 0; i < charCount; i++) {
            charAnimStartTimes[i] = -1;
        }

        int visableCharacterIndex = 0;

        while (true) {
            if (stopAnimating) {
                for (int i = visableCharacterIndex; i < charCount; i++) {
                    charAnimStartTimes[i] = Time.unscaledTime;
                }
                visableCharacterIndex = charCount;
                FinishAnimating(onFinish);
            }
            if (ShouldShowNextCharacter(secondsPerCharacter, timeOfLastCharacter)) {
                if (visableCharacterIndex <= charCount) {
                    if (visableCharacterIndex < charCount && ShouldShowNextCharacter(secondsPerCharacter, timeOfLastCharacter)) {
                        charAnimStartTimes[visableCharacterIndex] = Time.unscaledTime;
                        visableCharacterIndex++;
                        timeOfLastCharacter = Time.unscaledTime;
                        if (visableCharacterIndex == charCount) {
                            FinishAnimating(onFinish);
                        }
                    }
                }
            }
            for (int j = 0; j < charCount; j++) {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[j];
                if (charInfo.isVisible) //Invisible characters have a vertexIndex of 0 because they have no vertices and so they should be ignored to avoid messing up the first character in the string whic also has a vertexIndex of 0
                {
                    int vertexIndex = charInfo.vertexIndex;
                    int materialIndex = charInfo.materialReferenceIndex;
                    Color32[] destinationColors = textInfo.meshInfo[materialIndex].colors32;
                    Color32 theColor = j < visableCharacterIndex ? originalColors[materialIndex][vertexIndex] : clear;
                    destinationColors[vertexIndex + 0] = theColor;
                    destinationColors[vertexIndex + 1] = theColor;
                    destinationColors[vertexIndex + 2] = theColor;
                    destinationColors[vertexIndex + 3] = theColor;
                }
            }
            textBox.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            for (int i = 0; i < textInfo.meshInfo.Length; i++) {
                TMP_MeshInfo theInfo = textInfo.meshInfo[i];
                theInfo.mesh.vertices = theInfo.vertices;
                textBox.UpdateGeometry(theInfo.mesh, i);
            }
            yield return null;
        }
    }

    private void FinishAnimating(Action onFinish) {
        textAnimating = false;
        stopAnimating = false;
        onFinish?.Invoke();
    }

    private static bool ShouldShowNextCharacter(float secondsPerCharacter, float timeOfLastCharacter) {
        return (Time.unscaledTime - timeOfLastCharacter) > secondsPerCharacter;
    }

    public void SkipToEndOfCurrentMessage() {
        if (textAnimating) {
            stopAnimating = true;
        }
    }
}
