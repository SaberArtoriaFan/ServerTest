using Fantasy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum PaoPaoObjType
{ 
    Bomb, Wave, Box, Item
}

public enum PaoPaoBoxesType
{
    Box1, Box2, Box3
}

public enum PaoPaoItemType 
{ 
    Bomb, Power, Speed   
}


public class PaoPaoGameManager : SingletonMono<PaoPaoGameManager>
{
    public GameObject bombPF;
    public GameObject wavePF;
    public Transform bombParent;
    public GameObject boxPF;
    public Transform[] boxPoints;
    public Transform boxParent;
    public GameObject itemPF;
    public Transform itemParent;
    public PaoPaoRoleController player1;
    //public PaoPaoRoleController player2;
    public Transform player1InitPos;
    public Transform player2InitPos;
    public UILabel lbPlayer1Hp;
    public UILabel lbPlayer2Hp;
    public UILabel lbPlayer1BombNum;
    public UILabel lbPlayer2BombNum;
    public UILabel lbPlayer1BombPower;
    public UILabel lbPlayer2BombPower;
    public UILabel lbPlayer1MoveSpeed;
    public UILabel lbPlayer2MoveSpeed;
    public GameObject gameoverPanel;
    public UISprite spWin;

    public bool gameOver = false;
    public Vector2 startPos = new Vector2(-560, -280);
    public float blockWidth = 80;
    public List<PaoPaoBoxController> boxes = new List<PaoPaoBoxController>();
    public List<PaoPaoItemController> items = new List<PaoPaoItemController>();

    public List<PaoPaoRoleController> players=new List<PaoPaoRoleController>();

    public PaoPaoRoleController GetControllerByClientID(long clientId)=>players.Find((u)=>u.netObj.ClientID==clientId);

    void OnEnable()
    {
        Application.targetFrameRate = 30;
        //ResetGame();
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Alpha0))
    //        ResetGame();
    //}
    private void Start()
    {
        NetWorkManager.Instance.NetworkParent = GameObject.FindObjectOfType<UIRoot>().transform;
        _ =NetWorkManager.Instance.InitRoom();
    }
    public async void ResetGame()
    {
        gameOver = false;
        gameoverPanel.SetActive(false);
        StopAllCoroutines();
        //foreach (var box in boxes)
        //{
        //    PutIntoPool(box.gameObject, PaoPaoObjType.Box);
        //}
        //foreach (var item in items)
        //{
        //    PutIntoPool(item.gameObject, PaoPaoObjType.Item);
        //}
        //boxes.Clear();
        //items.Clear();
        var pos = NetWorkManager.RoomSortId % 2 == 0 ? player1InitPos.localPosition : player2InitPos.localPosition;
        var res= await NetWorkManager.Instance.CreateGameObj((long)NetworkUtil.Prefab.Player,pos , Quaternion.identity, Vector3.one);
        player1 = res.GetComponent<PaoPaoRoleController>();

        //player1.transform.localPosition = player1InitPos.localPosition;
        //player2.transform.localPosition = player2InitPos.localPosition;
        //player1.Init();
        //player2.Init();
        //StartCoroutine(GenerateBox());
    }

    public Vector2 GetPosInMap(Transform tf)
    {
        Vector2 pos = Vector2.zero;
        int row = (int)((tf.localPosition.x - startPos.x + blockWidth / 2) / blockWidth);
        int col = (int)((tf.localPosition.y - startPos.y + blockWidth / 8) / blockWidth);
        pos = new Vector2(row * blockWidth + startPos.x, col * blockWidth + startPos.y);
        return pos;
    }

    public async FTask<PaoPaoPaoContorller> PutBomb(Vector2 pos,Action<PaoPaoPaoContorller> action)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(pos, 0.1f, 1 << LayerMask.NameToLayer("Bomb"));
        if (cols.Length > 0)
        {
            action(null);
            //PutIntoPool(bomb, PaoPaoObjType.Bomb);
            return null;
        }

        var res= await NetWorkManager.Instance.CreateGameObj((long)NetworkUtil.Prefab.Boom, new Vector3(pos.x, pos.y,0), Quaternion.identity, Vector3.one);
        var pao= res.GetComponent<PaoPaoPaoContorller>();
        action(pao);
        return pao;
       // GameObject bomb = GetObj(PaoPaoObjType.Bomb);
       // bomb.transform.SetParent(bombParent);
       // bomb.transform.localPosition = new Vector3(pos.x, pos.y, 0);
       // bomb.transform.localScale = Vector3.one;
       // bomb.transform.localRotation = Quaternion.identity;
     

       // var PaoContorller = bomb.GetComponent<PaoPaoPaoContorller>();
       //// PaoContorller.ResetImage();
       // bomb.SetActive(true);
       // PaoContorller.Init(role);
       // return PaoContorller;
    }

    public PaoPaoWaveController PutWave(Vector2 pos, MoveDirection direction, bool isLast,bool Authority)
    {
        GameObject wave = GetObj(PaoPaoObjType.Wave);
        wave.transform.SetParent(bombParent);
        wave.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        wave.transform.localScale = Vector3.one;
        switch(direction)
        {
            case MoveDirection.Left:
                wave.transform.localRotation = Quaternion.Euler(new Vector3(0,0, 90));
                break;
            case MoveDirection.Right:
                wave.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;
            case MoveDirection.Up:
                wave.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case MoveDirection.Down:
                wave.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
        }

        var waveController = wave.GetComponent<PaoPaoWaveController>();
        waveController.Authority = Authority;
        waveController.ResetImage(isLast);
        wave.SetActive(true);
        waveController.Init();
        return waveController;
    }

    public PaoPaoItemController PutItem(PaoPaoItemType paoPaoItemType, Vector2 pos)
    {
        GameObject item = GetObj(PaoPaoObjType.Item);
        item.transform.SetParent(itemParent);
        item.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        item.transform.localScale = Vector3.one;
        PaoPaoItemController itemController = item.GetComponent<PaoPaoItemController>();
        itemController.ResetImage(paoPaoItemType);
        item.SetActive(true);
        itemController.Init();
        items.Add(itemController);
        return itemController;
    }

    #region 箱子
    //IEnumerator GenerateBox()
    //{
    //    WaitForSeconds waitTime = new WaitForSeconds(3f);
    //    WaitForSeconds waitTime2 = new WaitForSeconds(15f);
    //    yield return waitTime;
        
    //    while (true)
    //    {
    //        while (boxes.Count > 5)
    //            yield return waitTime;
    //        PutBoxes((PaoPaoBoxesType)Random.Range(0, 3));
    //        yield return waitTime2;
    //    }
    //}

    public void PutBoxes(PaoPaoBoxesType paoPaoBoxesType)
    {
        Transform boxPoint = boxPoints[(int)paoPaoBoxesType];
        for(int i = 0; i < boxPoint.childCount; i++)
        {
            GameObject box = GetObj(PaoPaoObjType.Box);
            box.transform.SetParent(boxParent);
            Vector2 pos = GetPosInMap(boxPoint.GetChild(i));
            box.transform.localPosition = new Vector3(pos.x, pos.y, 0);
            box.transform.localScale = Vector3.one;
            box.transform.localRotation = Quaternion.identity;
            PaoPaoBoxController boxController = box.GetComponent<PaoPaoBoxController>();
            boxController.ResetImage();
            boxController.gameObject.SetActive(true);
            boxController.Init();
            boxes.Add(boxController);
        }
    }
    #endregion

    #region 对象池
    Dictionary<PaoPaoObjType, List<GameObject>> objPool = new Dictionary<PaoPaoObjType, List<GameObject>>();
    public GameObject GetObj(PaoPaoObjType objType)
    {
        if(objPool.ContainsKey(objType) == false) 
            objPool.Add(objType, new List<GameObject>());
        if (objPool[objType].Count == 0)
        {
            switch(objType)
            {
                case PaoPaoObjType.Bomb:
                    return GameObject.Instantiate(bombPF);
                case PaoPaoObjType.Wave:
                    return GameObject.Instantiate(wavePF);
                case PaoPaoObjType.Box:
                    return GameObject.Instantiate(boxPF);
                case PaoPaoObjType.Item:
                    return GameObject.Instantiate(itemPF);
                default: return null;
            }
        }
        else
        {
            GameObject obj = objPool[objType][objPool[objType].Count - 1];
            objPool[objType].Remove(obj);
            return obj;
        }
    }

    public void PutIntoPool(GameObject obj, PaoPaoObjType objType)
    {
        obj.SetActive(false);
        if (objPool.ContainsKey(objType) == false)
            objPool.Add(objType, new List<GameObject>());
        if(objPool[objType].Contains(obj)) return;
        objPool[objType].Add(obj);
    }
    #endregion

    #region 游戏结束
    private void OnDisable()
    {
        //ResetGame();
    }

    public void GameOver(PaoPaoPlayerType losePlayer)
    {
        if(gameOver) return;
        gameOver = true;
        gameoverPanel.SetActive(true);
        spWin.spriteName = losePlayer == PaoPaoPlayerType.Player2 ? "1" : "15";
    }
    #endregion

    public void UpdatePlayerInfo(PaoPaoPlayerType playerType)
    {
        //if (playerType == PaoPaoPlayerType.Player1)
        //{
        return;
            lbPlayer1Hp.text = $"Hp  {player1.hp}";
            lbPlayer1BombNum.text = player1.bombNum < player1.maxBombNum ? player1.bombNum.ToString() : $"{player1.bombNum} Max";
            lbPlayer1BombPower.text = player1.bombPower < player1.maxBombPower ? player1.bombPower.ToString() : $"{player1.bombPower} Max";
            lbPlayer1MoveSpeed.text = player1.moveSpeed < player1.maxMoveSpeed ? player1.moveSpeed.ToString() : $"{player1.moveSpeed} Max";
        //}
        //else if (playerType == PaoPaoPlayerType.Player2)
        //{
        //    lbPlayer2Hp.text = $"Hp  {player2.hp}";
        //    lbPlayer2BombNum.text = player2.bombNum < player2.maxBombNum ? player2.bombNum.ToString() : $"{player2.bombNum} Max";
        //    lbPlayer2BombPower.text = player2.bombPower < player2.maxBombPower ? player2.bombPower.ToString() : $"{player2.bombPower} Max";
        //    lbPlayer2MoveSpeed.text = player2.moveSpeed < player2.maxMoveSpeed ? player2.moveSpeed.ToString() : $"{player2.moveSpeed} Max";
        //}
    }
}
