namespace Fantasy;

public sealed class Unit : Entity
{
    public Dictionary<long,GameEntity> GameEntities { get; set; }=new Dictionary<long,GameEntity>();

    public long ClientID;



    public void AddGameEntity(GameEntity ge)
    {
        GameEntities.Add(ge.Id, ge);
        //return ge.Id;
    }
    public GameEntity[] GetAllGameEntites()
    {
        return GameEntities.Values.ToArray();
    }
    public GameEntity? GetGameEntity(long id)
    {
        if(GameEntities.TryGetValue(id, out GameEntity ge)) return ge;
        return null;
    }
}