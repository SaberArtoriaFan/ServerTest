using Fantasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;


public class LogicMgr : Entity
{
    List<Unit> units = new List<Unit>();

    public Unit GetUnit(long id) => units.Find((u) => u.Id == id);
    public Unit GetUnitByClientId(long clientId) => units.Find((u) => u.ClientID == clientId);

    public void AddUnit(Unit unit)
    {
        if (units.Contains(unit) == false) units.Add(unit);
    }
    public void RemoveUnit(long clientId)
    {
        var unit = units.Find(u => u.ClientID == clientId);
        if (unit!=null)
        {
            units.Remove(unit);
            unit.Dispose();
        }
    }
    public List<long> AllClientID()
    {
        return units.Select(u => u.ClientID).ToList();
    }
    public async FTask<long> AddGameEntity(Unit unit,long prefabId,List<long> scriptsID, TransformData transform)
    {
        var ge = GameEntity.Create<GameEntity>(this.Scene, false, false);
        ge.prefabID = prefabId;
        ge.transformData = transform;
        ge.MaskSure(scriptsID);
        await ge.AddComponent<AddressableMessageComponent>().Register();
        unit.AddGameEntity(ge);
        var sceneConfig = SceneConfigData.Instance.GetSceneBySceneType(SceneType.Gate)[0];
        for (int i = units.Count-1; i >= 0; i--)
        {
            if (units[i]!=unit)
            {
                this.Scene.NetworkMessagingComponent.SendInnerRoute(sceneConfig.RouteId, new M2G_CreateNetworkObjectId()
                {
                    ClientID = units[i].ClientID,
                    data = ge.ToData()
                });
            }

        }
        //unit.Scene.GetSession()
        return ge.Id;
    }
    public List<InitData> GetAllNeedInit()
    {
        var list=new List<InitData>();  
        for (int i = 0; i < units.Count; i++)
        {
           list.AddRange(units[i].GetAllGameEntites().Select(u => u.ToData()));
        }
        return list;
    }
}

