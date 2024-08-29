using Fantasy;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PaoPaoBombLifeType
{
    OriBomb, Boom
}
public class PaoPaoPaoContorller : NetworkBehavior
{
    public UISprite paoImage;
    public int power;
    public BoxCollider2D coll;

    [ShowInInspector]
    private PaoPaoBombLifeType paoPaoBombLifeType;
    private float lifeTime;
    private List<string> anim = new List<string> { "_0013_1", "_0013_1", "_0013_1", "_0012_2", "_0012_2", "_0012_2", "_0011_3", "_0011_3", "_0011_3", "_0012_2", "_0012_2", "_0012_2" };
    private List<string> boomImage = new List<string> { "_0010_4", "_0009_5", "_0008_6" };
    private int animIndex = 0;
    private int waitBoomTime = 3;
    private WaitForSeconds boomTime = new WaitForSeconds(0.2f);
    private PaoPaoRoleController role;

    PaoPaoGameManager gameManager => PaoPaoGameManager.Instance;
    void ResetImage()
    {
        paoImage.spriteName = "_0013_1";
        paoImage.depth = 320 - (int)transform.localPosition.y - (int)gameManager.blockWidth / 2;
    }

    public override void OnNetworkObjectInit()
    {
        base.OnNetworkObjectInit();
        Init(gameManager.GetControllerByClientID(netObj.ClientID));
    }

    void Init(PaoPaoRoleController role)
    {
        this.role = role;
        power = role.bombPower;
        paoPaoBombLifeType = PaoPaoBombLifeType.OriBomb;
        lifeTime = 0;
        animIndex = 0;
        ResetImage();
        StartCoroutine(BombLife());
    }

    void Update()
    {
        lifeTime += Time.deltaTime;
    }

    public void Boom()
    {
        paoPaoBombLifeType = PaoPaoBombLifeType.Boom;
    }


    IEnumerator BombLife()
    {
        while (paoPaoBombLifeType != PaoPaoBombLifeType.Boom)
        {
            if (paoPaoBombLifeType == PaoPaoBombLifeType.OriBomb)
            {
                paoImage.spriteName = anim[animIndex];
                animIndex = (animIndex + 1) % anim.Count;
                for(int i = 0; i < 8; ++i)
                    if(paoPaoBombLifeType == PaoPaoBombLifeType.OriBomb)
                        yield return 0;
                if (lifeTime >= waitBoomTime)
                    paoPaoBombLifeType = PaoPaoBombLifeType.Boom;
            }
        }
        if (paoPaoBombLifeType == PaoPaoBombLifeType.Boom)
        {
            if (role.netObj.Authority)
            {
                int layer = 1 << LayerMask.NameToLayer("Box") | 1 << LayerMask.NameToLayer("Item") | 1 << LayerMask.NameToLayer("Player");
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
                }
            }
               

            paoImage.spriteName = boomImage[Random.Range(0, boomImage.Count)];
       
            List<PaoPaoWaveController> leftWaveList = new List<PaoPaoWaveController>();
            List<PaoPaoWaveController> rightWaveList = new List<PaoPaoWaveController>();
            List<PaoPaoWaveController> downWaveList = new List<PaoPaoWaveController>();
            List<PaoPaoWaveController> upWaveList = new List<PaoPaoWaveController>();

            for (int i = 1; i <= power; i++)
            {
                if (leftWaveList.Count == 0 || leftWaveList[leftWaveList.Count - 1].stopSpread == false)
                {
                    Vector2 pos = new Vector2(transform.localPosition.x - gameManager.blockWidth * i, transform.localPosition.y);
                    leftWaveList.Add(gameManager.PutWave(pos, MoveDirection.Left, i == power, role.netObj.Authority));
                }
                if (rightWaveList.Count == 0 || rightWaveList[rightWaveList.Count - 1].stopSpread == false)
                {
                    Vector2 pos = new Vector2(transform.localPosition.x + gameManager.blockWidth * i, transform.localPosition.y);
                    rightWaveList.Add(gameManager.PutWave(pos, MoveDirection.Right, i == power, role.netObj.Authority));
                }
                if (downWaveList.Count == 0 || downWaveList[downWaveList.Count - 1].stopSpread == false)
                {
                    Vector2 pos = new Vector2(transform.localPosition.x, transform.localPosition.y - gameManager.blockWidth * i);
                    downWaveList.Add(gameManager.PutWave(pos, MoveDirection.Down, i == power, role.netObj.Authority));
                }
                if (upWaveList.Count == 0 || upWaveList[upWaveList.Count - 1].stopSpread == false)
                {
                    Vector2 pos = new Vector2(transform.localPosition.x, transform.localPosition.y + gameManager.blockWidth * i);
                    upWaveList.Add(gameManager.PutWave(pos, MoveDirection.Up, i == power, role.netObj.Authority));
                }

                yield return 0;
            }
            yield return boomTime;
            foreach (var wave in leftWaveList)
                wave.DisappearHandle();
            foreach (var wave in rightWaveList)
                wave.DisappearHandle();
            foreach (var wave in downWaveList)
                wave.DisappearHandle();
            foreach (var wave in upWaveList)
                wave.DisappearHandle();
            role.AddCanUseBomb();
            role = null;
            yield return 0;
            yield return 0;
            paoImage.spriteName = "_0005_9c";
            yield return 0;
            yield return 0;
            paoImage.spriteName = "_0004_10c";
            yield return 0;
            yield return 0;
            //gameManager.PutIntoPool(gameObject, PaoPaoObjType.Bomb);
            if (netObj.Authority)
                GameObject.Destroy(gameObject);
        }
    }
}
