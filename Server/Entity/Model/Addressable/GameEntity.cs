using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class GameEntity:Entity, ISupportedMultiEntity
{
    public long networkObjectID=>Id;
    public long prefabID;
    List<long> scritpsIDList;
    public TransformData transformData;
    public void MaskSure(List<long> scritpsID)
    {

    }

    public InitData ToData()
    {
        InitData data = new InitData();
        data.NetworkObjectID = networkObjectID;
        data.PrefabID= prefabID;
        data.NetworkScriptsID = scritpsIDList;
        data.Transform= transformData;
        return data;
    }
}