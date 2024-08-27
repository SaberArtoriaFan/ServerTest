using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Fantasy
{
    
    public class NetworkTransform:NetworkBehavior
    {
        public bool syncPosition = true;
        public bool syncRotation = true;

        protected Vector3 position;
        protected Quaternion rotation;
        protected Vector3 localScale;
        protected Vector3 netPosition;
        protected Quaternion netRotation;
        protected Vector3 netLocalScale;
        public bool authority = false;
        //public bool syncPosition = true;
        //public bool syncRotation = true;
        public bool syncScale = false;
        //[HideInInspector] public SyncMode currMode = SyncMode.None;
        public float controlTime = 0.5f;
        public float lerpSpeed = 0.3f;
        public bool fixedSync = false;
        public float fixedSendTime = 1f;//固定发送时间
        internal float fixedTime;
        [HideInInspector] public float currControlTime;


        private float CanSentTime=>NetWorkManager.Instance.frame;

        protected override void Start()
        {
            base.Start();
            //netObj.CanDestroy = false;
            netPosition = position = transform.position;
            netRotation = rotation = transform.rotation;
            netLocalScale = localScale = transform.localScale;
        }

        public override void NetworkUpdate()
        {
            if (netObj.Identity == -1 )
                return;
            if (authority == false)
            {
                if(currControlTime > 0f)
                {
                    SyncTransform();
                    currControlTime -= CanSentTime;
                }
            }
            else
            {
                NetworkSyncCheck();

            }

            //if (currMode == SyncMode.Synchronized)
            //{
            //    SyncTransform();
            //}
            //else if (currControlTime > 0f & (currMode == SyncMode.Control | currMode == SyncMode.SynchronizedAll))
            //{
            //    currControlTime -= NetworkTime.I.CanSentTime;
            //    SyncTransform();
            //}
            //else
            //{
            //    NetworkSyncCheck();
            //}
        }

        public virtual void NetworkSyncCheck()
        {
            if (transform.position != position | transform.rotation != rotation | transform.localScale != localScale | (Time.time > fixedTime & fixedSync))
                SyncTransformState();
        }

        public virtual void SyncTransformState()
        {
            position = transform.position; //必须在这里处理，在上面处理会有点问题
            rotation = transform.rotation;
            localScale = transform.localScale;
            fixedTime = Time.time + fixedSendTime;
            NetWorkManager.Instance.Session.Send(new C2M_SyncTransform()
            {
                NetworkObjectID = netObj.Identity,
                Transform = new TransformData()
                {
                    position = this.position.ToMessage(),
                    quaternion = this.rotation.ToMessage()
                }
            });
            Debug.Log("请求同步位置");
        }

        public virtual void ForcedSynchronous()
        {
            SyncTransformState();
        }

        public virtual void SyncTransform()
        {
            if (syncPosition)
                transform.position = Vector3.Lerp(transform.position, netPosition, lerpSpeed);
            if (syncRotation)
                if (netRotation != Quaternion.identity)
                    transform.rotation = Quaternion.Lerp(transform.rotation, netRotation, lerpSpeed);
            if (syncScale)
                transform.localScale = netLocalScale;
        }

        public virtual void SyncControlTransform()
        {
            if (syncPosition)
            {
                position = netPosition;//位置要归位,要不然就会发送数据
                transform.position = netPosition;
            }
            if (syncRotation)
            {
                rotation = netRotation;
                transform.rotation = netRotation;
            }
            if (syncScale)
            {
                localScale = netLocalScale;
                transform.localScale = netLocalScale;
            }
        }

        public override void OnNetworkObjectInit()
        {
            base.OnNetworkObjectInit();
            authority = netObj.Authority;
            
            if (authority) //如果是公共网络物体则不能初始化的时候通知，因为可能前面有玩家了，如果发起同步会导致前面的玩家被拉回原来位置
                ForcedSynchronous(); //发起一次同步，让对方显示物体
            else
                currControlTime = controlTime; //如果是公共网络物体则开始时先处于被控制状态，这样就不会发送同步数据
        }

        //public override void OnNetworkObjectCreate(in Operation opt)
        //{
        //    if (opt.cmd == Command.Transform)
        //        SetNetworkSyncMode(opt);
        //    netPosition = opt.position;
        //    netRotation = opt.rotation;
        //    netLocalScale = opt.direction;
        //    SyncControlTransform();
        //}

        //public override void OnInitialSynchronization(in Operation opt)
        //{
        //    if (ClientBase.Instance.UID == opt.uid)
        //        return;
        //    SetNetworkSyncState(opt);
        //    SetNetworkSyncMode(opt);
        //    SyncControlTransform();
        //}

        public void HandleSyncTransform(TransformData data)
        {
            SetNetworkSyncState(data);
        }
        //public override void OnNetworkOperationHandler(in Operation opt)
        //{
        //    if (ClientBase.Instance.UID == opt.uid)
        //        return;
        //    SetNetworkSyncState(opt);
        //    if (currMode == SyncMode.SynchronizedAll | currMode == SyncMode.Control)
        //        SyncControlTransform();
        //    else if (currMode == SyncMode.None)
        //        SetNetworkSyncMode(opt);
        //}

        protected void SetNetworkSyncState(TransformData opt)
        {
            currControlTime = controlTime;
            netPosition = opt.position.ToUnity();
            netRotation = opt.quaternion.ToUnity() ;
            //netLocalScale = opt.direction;
            netLocalScale = transform.localScale;
        }
    }

}

