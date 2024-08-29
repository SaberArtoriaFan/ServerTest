using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class PaoPaoWaveController : MonoBehaviour
{
    public UISprite waveImage;
    PaoPaoGameManager gameManager => PaoPaoGameManager.Instance;
    public bool stopSpread;

    private bool isLast;
    public bool Authority=false;

    public void ResetImage(bool isLast)
    {
        this.isLast = isLast;
        waveImage.spriteName = isLast ? "_0007_7" : "_0003_11";
        waveImage.depth = 320 - (int)transform.localPosition.y;
    }


    public void Init()
    {
        if (Authority)
        {
            stopSpread = false;
            int layer = 1 << LayerMask.NameToLayer("Bomb") | 1 << LayerMask.NameToLayer("Box") | 1 << LayerMask.NameToLayer("Item") | 1 << LayerMask.NameToLayer("Player");
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 0.1f, layer);
            if (cols.Length > 0)
            {
                PaoPaoPaoContorller paopao = cols[0].GetComponent<PaoPaoPaoContorller>();
                if (paopao != null) paopao.Boom();
                PaoPaoBoxController box = cols[0].GetComponent<PaoPaoBoxController>();
                if (box != null) box.DestroyBox();
                PaoPaoItemController item = cols[0].GetComponent<PaoPaoItemController>();
                if (item != null) item.BeDestroy();
                PaoPaoRoleController role = cols[0].GetComponent<PaoPaoRoleController>();
                if (role != null) role.Hurt(false);
                stopSpread = true;
                return;
            }
        }
     
    }

    public void DisappearHandle()
    {
        StartCoroutine(DoDisappear());  
    }

    IEnumerator DoDisappear()
    {
        waveImage.spriteName = isLast ? "_0002_12" : "_0001_13";
        yield return 0;
        yield return 0;
        waveImage.spriteName = "_0000_14";
        yield return 0;
        yield return 0;
        waveImage.spriteName = "_0004_10";
        yield return 0;
        yield return 0;
        gameManager.PutIntoPool(gameObject, PaoPaoObjType.Wave);
    }
}
