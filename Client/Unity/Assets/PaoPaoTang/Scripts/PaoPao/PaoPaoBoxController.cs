using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaoPaoBoxController : MonoBehaviour
{
    public UISprite boxImage;
    public BoxCollider2D coll;
    public PaoPaoGameManager gameManager;
    private float waitToAppear = 2f;
    private bool destroy = false;
    private List<string> boxImageNameList = new List<string>() { "f_block_06", "f_block_08", "f_block_11", "f_block_13" };

    public void ResetImage()
    {
        boxImage.spriteName = "redBox";
        boxImage.alpha = 0f;
        boxImage.height = 80;
        boxImage.depth = 320 - (int)transform.localPosition.y + 80;
    }


    public void Init()
    {
        destroy = false;
        coll.enabled = false;
        waitToAppear = 2f;
        StartCoroutine(BoxLifeTime());
    }

    IEnumerator BoxLifeTime()
    {
        float alpha = 0f;
        float deltaAlphaSign = 1;
        while(waitToAppear > 0)
        {
            waitToAppear -= Time.deltaTime;
            alpha += deltaAlphaSign * Time.deltaTime;
            if (alpha >= 0.1f)
            {
                deltaAlphaSign *= -1;
                alpha = 0.099f;
            }
            else if(alpha <= 0)
            {
                deltaAlphaSign *= -1;
                alpha = 0.001f;
            }
            boxImage.alpha = alpha;
            yield return 0;
        }

        int layer = 1 << LayerMask.NameToLayer("Bomb") | 1 << LayerMask.NameToLayer("Box") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Item");
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 0.1f, layer);
        if (cols.Length > 0)
        {
            if (gameManager.boxes.Contains(this))
                gameManager.boxes.Remove(this);
            gameManager.PutIntoPool(gameObject, PaoPaoObjType.Box);
            yield break;
        }
        boxImage.alpha = 0.1f;
        boxImage.height = 100;
        boxImage.spriteName = boxImageNameList[Random.Range(0, boxImageNameList.Count)];
        boxImage.depth = 320 - (int)transform.localPosition.y;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * 3;
            boxImage.alpha = alpha;
            yield return 0;
        }
        boxImage.alpha = 1;
        coll.enabled = true;
    }

    public void DestroyBox()
    {
        if(destroy) return;
        destroy = true;
        if(gameManager.boxes.Contains(this))
            gameManager.boxes.Remove(this);
        gameManager.PutIntoPool(gameObject, PaoPaoObjType.Box);

        ////生成道具
        //if(Random.Range(0f,1f) < 0.5f)
        //{
        //    gameManager.PutItem((PaoPaoItemType)Random.Range(0, 3), gameManager.GetPosInMap(transform));
        //}
    }
}
