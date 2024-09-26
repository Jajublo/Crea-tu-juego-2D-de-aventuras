using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepButton : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public bool save;
    public bool activated;
    public bool attack;
    public Sprite activeButton;
    public GameObject blocks;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !activated && !attack)
        {
            Save();
        }

        if (collision.CompareTag("Weapon") 
            || collision.CompareTag("Fire")
            || collision.CompareTag("Explosion")
            && !activated)
        {
            Save();
        }
    }

    public void Save()
    {
        if (save)
        {
            DataInstance.Instance.SaveSceneData(name);
        }
        Activate();
    }

    public void Activate()
    {
        spriteRenderer.sprite = activeButton;
        blocks.SetActive(false);
        activated = true;
    }
}
