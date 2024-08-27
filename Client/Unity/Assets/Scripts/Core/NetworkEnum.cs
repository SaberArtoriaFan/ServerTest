using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

 partial class NetWorkManager 
{
    public enum ConnectStatus
    {
        NoStart,
        Starting,
        Started
    }
    public enum ConnectResult
    {
        Succ,
        Fail,
        Break
    }
}