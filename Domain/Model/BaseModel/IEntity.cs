namespace Domain.Model.BaseModel;

public interface IBaseEntity
{
    
}
public interface IEntity<TKey> : IBaseEntity
{
    public TKey Id { get; set; }
}