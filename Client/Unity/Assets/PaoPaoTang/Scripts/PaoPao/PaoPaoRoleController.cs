using Fantasy;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
public enum PaoPaoPlayerType
{
    Player1, Player2, 
}

public enum MoveDirection
{
    Down, Up, Left, Right
}

public class PaoPaoRoleController : NetworkBehavior
{
    public PaoPaoPlayerType paoPaoPlayerType = PaoPaoPlayerType.Player1;
    public float moveSpeed = 0.1f;
    public Vector3 lastPos = Vector3.zero;
    public UISprite roleImage;
    PaoPaoGameManager gameManager => PaoPaoGameManager.Instance;
    public int bombNum;
    public int bombPower;
    public int canUseBombNum;
    public int maxBombNum;
    public int maxBombPower;
    public int maxMoveSpeed;
    public int hp;
    public int maxHp = 5;
    public Transform leftColl;
    public Transform rightColl;
    public Transform topColl;
    public Transform bottomColl;
    public float colRadius;

    private Dictionary<PaoPaoPlayerType, Dictionary<MoveDirection, List<string>>> anims =
        new Dictionary<PaoPaoPlayerType, Dictionary<MoveDirection, List<string>>> {
            {PaoPaoPlayerType.Player1, new Dictionary<MoveDirection, List<string>>{ { MoveDirection.Down, new List<string> { "1", "2", "1", "3" } },
                   { MoveDirection.Up, new List<string> { "9", "10", "9", "8" } },{ MoveDirection.Left, new List<string> { "7", "6", "5", "4" } },
                   { MoveDirection.Right, new List<string> { "7", "6", "5", "4" } } }
            },
            {PaoPaoPlayerType.Player2, new Dictionary<MoveDirection, List<string>>{ { MoveDirection.Down, new List<string> { "15", "11", "12", "15", "13", "14" } },
                   { MoveDirection.Up, new List<string> { "20", "16", "17", "20", "18", "19" } },{ MoveDirection.Left, new List<string> {  "25", "21", "22", "25", "23", "24" } },
                   { MoveDirection.Right, new List<string> { "25", "21", "22", "25", "23", "24" } } }
            }
        };
    private Dictionary<MoveDirection, int> moveImageIndexList = new Dictionary<MoveDirection, int> { { MoveDirection.Down, 0 } , { MoveDirection.Up, 0 } , { MoveDirection.Left, 0 } , { MoveDirection.Right, 0 } };
    private MoveDirection curMoveDirection = MoveDirection.Down;
    private bool move;
    private List<MoveDirection> keyDownList = new List<MoveDirection>();
    private bool putBomb = false;
    private PaoPaoPaoContorller curBomb;
    private float clearCurBombTime;
    private float clearCurBombIntervalTime = 0.5f;
    private int bombInitNum = 2;
    private int bombInitPower = 1;
    private int initSpeed = 250;
    private float muteTime = 1;
    private float curMuteTime = 0;
    IEnumerator IEMuteFlash;
    IEnumerator IEPlayAnim;

    //public NetworkObject netObj;

   protected override void Awake()
    {
        base.Awake();
        //networkObject = GetComponent<NetworkObject>();
        PaoPaoGameManager.Instance.players.Add(this);
 

    }
    protected override void  OnDestroy()
    {
        PaoPaoGameManager.Instance.players.Remove(this);
        base.OnDestroy();
    }
    public override void OnNetworkObjectInit()
    {
        base.OnNetworkObjectInit();
        Init();
    }
    public void Init()
    {
        var random = NetWorkManager.RoomSortId % 2;
        paoPaoPlayerType = (PaoPaoPlayerType)random;
        if (random == 0)
            roleImage.spriteName = "1";
        else
            roleImage.spriteName = "10";

        move = false;
        putBomb = false;
        lastPos = transform.localPosition;
        roleImage.depth = 320 - (int)transform.localPosition.y;
        canUseBombNum = bombNum = bombInitNum;
        bombPower = bombInitPower;
        moveSpeed = initSpeed;
        hp = maxHp;
        curMuteTime = 0;
        IEMuteFlash = null;
        if (netObj.Authority)
        {

            gameManager.UpdatePlayerInfo(paoPaoPlayerType);
            keyDownList.Clear();
            if (IEPlayAnim != null)
                StopCoroutine(IEPlayAnim);
            IEPlayAnim = PlayAnim();
            StartCoroutine(IEPlayAnim);
        }
   
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        if (gameManager.gameOver) return;

        roleImage.depth = 320 - (int)transform.localPosition.y;


        if (curMuteTime > 0)
        {
            curMuteTime -= Time.deltaTime;
        }
        if (netObj.Authority == false) return;
        
        if(true)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (keyDownList.Contains(MoveDirection.Left) == false)
                    keyDownList.Add(MoveDirection.Left);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (keyDownList.Contains(MoveDirection.Right) == false)
                    keyDownList.Add(MoveDirection.Right);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (keyDownList.Contains(MoveDirection.Down) == false)
                    keyDownList.Add(MoveDirection.Down);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (keyDownList.Contains(MoveDirection.Up) == false)
                    keyDownList.Add(MoveDirection.Up);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                if (keyDownList.Contains(MoveDirection.Left))
                    keyDownList.Remove(MoveDirection.Left);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                if (keyDownList.Contains(MoveDirection.Right))
                    keyDownList.Remove(MoveDirection.Right);
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                if (keyDownList.Contains(MoveDirection.Down))
                    keyDownList.Remove(MoveDirection.Down);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                if (keyDownList.Contains(MoveDirection.Up))
                    keyDownList.Remove(MoveDirection.Up);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (putBomb == false && canUseBombNum > 0)
                {
                    putBomb = true;
                    canUseBombNum--;
                    _ = gameManager.PutBomb(gameManager.GetPosInMap(transform), (b) =>
                    {
                        if (b != null)
                        {
                            curBomb = b;
                            clearCurBombTime = clearCurBombIntervalTime;
                        }else
                            canUseBombNum++;
                    });
                   
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
                putBomb = false;
        }
        //else if (paoPaoPlayerType == PaoPaoPlayerType.Player2)
        //{
        //    if (Input.GetKeyDown(KeyCode.LeftArrow))
        //    {
        //        if (keyDownList.Contains(MoveDirection.Left) == false)
        //            keyDownList.Add(MoveDirection.Left);
        //    }
        //    if (Input.GetKeyDown(KeyCode.RightArrow))
        //    {
        //        if (keyDownList.Contains(MoveDirection.Right) == false)
        //            keyDownList.Add(MoveDirection.Right);
        //    }
        //    if (Input.GetKeyDown(KeyCode.DownArrow))
        //    {
        //        if (keyDownList.Contains(MoveDirection.Down) == false)
        //            keyDownList.Add(MoveDirection.Down);
        //    }
        //    if (Input.GetKeyDown(KeyCode.UpArrow))
        //    {
        //        if (keyDownList.Contains(MoveDirection.Up) == false)
        //            keyDownList.Add(MoveDirection.Up);
        //    }
        //    if (Input.GetKeyUp(KeyCode.LeftArrow))
        //    {
        //        if (keyDownList.Contains(MoveDirection.Left))
        //            keyDownList.Remove(MoveDirection.Left);
        //    }
        //    if (Input.GetKeyUp(KeyCode.RightArrow))
        //    {
        //        if (keyDownList.Contains(MoveDirection.Right))
        //            keyDownList.Remove(MoveDirection.Right);
        //    }
        //    if (Input.GetKeyUp(KeyCode.DownArrow))
        //    {
        //        if (keyDownList.Contains(MoveDirection.Down))
        //            keyDownList.Remove(MoveDirection.Down);
        //    }
        //    if (Input.GetKeyUp(KeyCode.UpArrow))
        //    {
        //        if (keyDownList.Contains(MoveDirection.Up))
        //            keyDownList.Remove(MoveDirection.Up);
        //    }
        //    if (Input.GetKeyDown(KeyCode.KeypadEnter))
        //    {
        //        if (putBomb == false && canUseBombNum > 0)
        //        {
        //            putBomb = true;
        //            var bomb = gameManager.PutBomb(gameManager.GetPosInMap(transform), this);
        //            if(bomb != null)
        //            {
        //                curBomb = bomb;
        //                clearCurBombTime = clearCurBombIntervalTime;
        //                canUseBombNum--;
        //            }
        //        }
        //    }
        //    if (Input.GetKeyUp(KeyCode.KeypadEnter))
        //        putBomb = false;
        //}
        if (clearCurBombTime > 0)
        {
            clearCurBombTime -= Time.deltaTime;
            if (clearCurBombTime <= 0)
                curBomb = null;
        }
        if (keyDownList.Count > 0)
        {
            int layer = 1 << LayerMask.NameToLayer("Bomb") | 1 << LayerMask.NameToLayer("Wall") | 1 << LayerMask.NameToLayer("Box");
            if (keyDownList[keyDownList.Count - 1] == MoveDirection.Left)
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(leftColl.position, colRadius, layer);
                Collider2D[] cols2 = Physics2D.OverlapCircleAll(rightColl.position, colRadius, layer);
                if(cols.Length == 0 || (cols2.Length != 0 && cols2.Contains(cols[0])) || (curBomb != null && cols[0].gameObject == curBomb.gameObject))
                    transform.localPosition += new Vector3(-moveSpeed * Time.deltaTime, 0, 0);
                curMoveDirection = MoveDirection.Left;
            }
            else if (keyDownList[keyDownList.Count - 1] == MoveDirection.Right)
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(rightColl.position, colRadius, layer);
                Collider2D[] cols2 = Physics2D.OverlapCircleAll(leftColl.position, colRadius, layer);
                if (cols.Length == 0 || (cols2.Length != 0 && cols2.Contains(cols[0])) || (curBomb != null && cols[0].gameObject == curBomb.gameObject))
                    transform.localPosition += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
                curMoveDirection = MoveDirection.Right;
            }
            else if (keyDownList[keyDownList.Count - 1] == MoveDirection.Down)
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(bottomColl.position, colRadius, layer);
                Collider2D[] cols2 = Physics2D.OverlapCircleAll(topColl.position, colRadius, layer);
                if (cols.Length == 0 || (cols2.Length != 0 && cols2.Contains(cols[0])) || (curBomb != null && cols[0].gameObject == curBomb.gameObject))
                    transform.localPosition += new Vector3(0, -moveSpeed * Time.deltaTime, 0);
                curMoveDirection = MoveDirection.Down;
            }
            else if(keyDownList[keyDownList.Count - 1] == MoveDirection.Up)
            {
                Collider2D[] cols = Physics2D.OverlapCircleAll(topColl.position, colRadius, layer);
                Collider2D[] cols2 = Physics2D.OverlapCircleAll(bottomColl.position, colRadius, layer);
                if (cols.Length == 0 || (cols2.Length != 0 && cols2.Contains(cols[0])) || (curBomb != null && cols[0].gameObject == curBomb.gameObject))
                    transform.localPosition += new Vector3(0, moveSpeed * Time.deltaTime, 0);
                curMoveDirection = MoveDirection.Up;
            }
            move = true;
        }
        else
            move = false;

    

        Collider2D[] cols3 = Physics2D.OverlapCircleAll(transform.position, colRadius, 1 << LayerMask.NameToLayer("Item"));
        if(cols3.Length > 0)
        {
            var item = cols3[0].GetComponent<PaoPaoItemController>();
            if (item.bePicked == false&&item.CanBeDestroy)
            {
                switch (item.paoPaoItemType)
                {
                    case PaoPaoItemType.Bomb:
                        AddBombNum();
                        break;
                    case PaoPaoItemType.Power:
                        AddPower();
                        break;
                    case PaoPaoItemType.Speed:
                        AddSpeed();
                        break;
                }
                item.bePicked = true;
                item.BeDestroy();
            }    
        }
    }

    IEnumerator PlayAnim()
    {
        while(true)
        {
            if (curMoveDirection == MoveDirection.Left)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (curMoveDirection == MoveDirection.Right)
            {
                transform.localScale = Vector3.one;
            }

            if (move)
            {
                roleImage.spriteName = anims[paoPaoPlayerType][curMoveDirection][moveImageIndexList[curMoveDirection]];
                moveImageIndexList[curMoveDirection] = (moveImageIndexList[curMoveDirection] + 1) % anims[paoPaoPlayerType][curMoveDirection].Count;
            }
            else
            {
                roleImage.spriteName = anims[paoPaoPlayerType][curMoveDirection][0];
                moveImageIndexList[curMoveDirection] = 0;
            }
            lastPos = transform.localPosition;
            for(int i = 0; i < 8; i++) yield return null;
        }
       
    }

    public void AddCanUseBomb()
    {
        canUseBombNum++;
        if(canUseBombNum > maxBombNum)
            canUseBombNum = maxBombNum;
        if (canUseBombNum > bombNum)
            canUseBombNum = bombNum;
    }

    public void AddBombNum()
    {
        if(bombNum < maxBombNum)
        {
            bombNum++;
            canUseBombNum++;
            gameManager.UpdatePlayerInfo(paoPaoPlayerType);
        }
    }

    public void AddPower()
    {
        if(bombPower < maxBombPower)
        {
            bombPower++;
            gameManager.UpdatePlayerInfo(paoPaoPlayerType);
        }
    }

    public void AddSpeed()
    {
        if (moveSpeed < maxMoveSpeed)
        {
            moveSpeed += 50;
            gameManager.UpdatePlayerInfo(paoPaoPlayerType);
        }
    }

    public void Hurt(bool isOnlyAnim)
    {
        Log.Debug($"受伤:只播放动画：{isOnlyAnim}");
        if (curMuteTime > 0)
            return;

        curMuteTime = muteTime;
        if (IEMuteFlash != null)
            StopCoroutine(IEMuteFlash);
        IEMuteFlash = MuteFlash();
        StartCoroutine(IEMuteFlash);
        bombNum = bombNum / 2;
        if (bombNum < bombInitNum) bombNum = bombInitNum;
        bombPower = bombPower / 2;
        if (bombPower < bombInitPower) bombPower = bombInitPower;
        moveSpeed = moveSpeed / 2;
        if (moveSpeed < initSpeed) moveSpeed = initSpeed;


        if (isOnlyAnim)
            return;


        hp--;
        if(hp > 0)
        {
  
        }
        else
        {
            gameManager.GameOver(paoPaoPlayerType);
        }
        gameManager.UpdatePlayerInfo(paoPaoPlayerType);

        NetWorkManager.SendC2M(new C2M_Hurt() { NetworkObjectID=netObj.Identity,Value=1});

    }

    IEnumerator MuteFlash()
    {
        float alpha = 0f;
        float deltaAlphaSign = 1;

        while (curMuteTime > 0)
        {
            alpha += deltaAlphaSign * Time.deltaTime * 5;
            if (alpha >= 0.8f)
            {
                deltaAlphaSign *= -1;
                alpha = 0.799f;
            }
            else if (alpha <= 0.5f)
            {
                deltaAlphaSign *= -1;
                alpha = 0.501f;
            }
            roleImage.alpha = alpha;
            yield return 0;
        }
        roleImage.alpha = 1;
        IEMuteFlash = null;
    }
}
