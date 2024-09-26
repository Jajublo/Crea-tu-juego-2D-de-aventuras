using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public Sprite trapOn;
    public Sprite trapOff;

    public float trapTime = 1f;
    public bool trapActive;

    private void Awake()
    {
        InvokeRepeating("Activate", 0, trapTime);
    }

    private void Activate()
    {
        foreach (Transform trap in transform)
        {
            if (trapActive)
            {
                trap.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                trap.gameObject.GetComponent<SpriteRenderer>().sprite = trapOn;
            }
            else
            {
                trap.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                trap.gameObject.GetComponent<SpriteRenderer>().sprite = trapOff;
            }
        }

        trapActive = !trapActive;
    }
}
