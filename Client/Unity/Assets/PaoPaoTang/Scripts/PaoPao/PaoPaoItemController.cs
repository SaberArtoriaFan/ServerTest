using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaoPaoItemController : MonoBehaviour
{
    public UISprite itemImage;
    public PaoPaoItemType paoPaoItemType;
    public PaoPaoGameManager gameManager;
    private bool canBeDestroy;
    public bool bePicked = false;

    public bool CanBeDestroy { get => canBeDestroy;private set => canBeDestroy = value; }

    public void ResetImage(PaoPaoItemType paoPaoItemType)
    {
        this.paoPaoItemType = paoPaoItemType;
        switch (paoPaoItemType)
        {
            case PaoPaoItemType.Bomb:
                itemImage.spriteName = "Tutorial_bubble";
                break;
            case PaoPaoItemType.Power:
                itemImage.spriteName = "power_up";
                break;
            case PaoPaoItemType.Speed:
                itemImage.spriteName = "kick";
                break;
        }
        itemImage.depth = 320 - (int)transform.localPosition.y;
    }

    public void Init()
    {
        bePicked = false;
        canBeDestroy = false;
        StartCoroutine(SetCanBeDestroy());
    }

    IEnumerator SetCanBeDestroy() 
    { 
        yield return new WaitForSeconds(0.5f);
        canBeDestroy = true; 
    }

    public void BeDestroy()
    {
        if (!canBeDestroy) return;
        if(gameManager.items.Contains(this))
            gameManager.items.Remove(this);
        gameManager.PutIntoPool(gameObject, PaoPaoObjType.Item);
    }
}
