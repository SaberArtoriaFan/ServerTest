
using Fantasy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(UISprite))]
public class NetworkSprite : NetworkBehavior
{
    bool Authority=>netObj.Authority;

    string lastSpriteName;
    public UISprite uISprite;
    protected override void Start()
    {
        base.Start();
        uISprite= uISprite!=null? uISprite:  GetComponent<UISprite>();
        lastSpriteName = uISprite.spriteName;
    }

    public override void NetworkUpdate()
    {
        if (Authority)
        {
            if (lastSpriteName != uISprite.spriteName)
            {
                lastSpriteName = uISprite.spriteName;
                //同步图片
                NetWorkManager.SendC2M(new C2M_SyncSprite()
                {
                    NetworkObjectID = netObj.Identity,
                    SpriteName = uISprite.spriteName,
                });
            }
        }
    }
    public void HandleSprite(string spriteName)
    {
        this.uISprite.spriteName = spriteName;
    }
}
