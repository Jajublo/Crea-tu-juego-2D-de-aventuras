using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelect : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
    public int itemIndex;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gameManager.ChangeWeaponText(itemIndex);
    }

    public void OnSelect(BaseEventData eventData)
    {
        gameManager.ChangeWeaponText(itemIndex);
    }
}
