using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    public Sprite spriteOn;
    public Sprite spriteOff;

    public GameObject blocks1;
    public GameObject blocks2;

    public bool active;

    private void Awake()
    {
        Activate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon")
            || collision.CompareTag("Fire")
            || collision.CompareTag("Explosion"))
        {
            active = !active;
            Activate();
        }
    }

    private void Activate()
    {
        blocks1.SetActive(active);
        blocks2.SetActive(!active);

        foreach (SwitchButton button in FindObjectsOfType<SwitchButton>())
        {
            if (active)
            {
                button.GetComponent<SpriteRenderer>().sprite = spriteOn;
            }
            else
            {
                button.GetComponent<SpriteRenderer>().sprite = spriteOff;
            }
            button.active = active;
        }
    }
}
