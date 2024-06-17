
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace cfg
{
public sealed partial class Effect : Luban.BeanBase
{
    public Effect(JSONNode _buf) 
    {
        { if(!_buf["Id"].IsNumber) { throw new SerializationException(); }  Id = _buf["Id"]; }
        { if(!_buf["EntityId"].IsNumber) { throw new SerializationException(); }  EntityId = _buf["EntityId"]; }
        { if(!_buf["ActiveSeconds"].IsNumber) { throw new SerializationException(); }  ActiveSeconds = _buf["ActiveSeconds"]; }
        { if(!_buf["AnimName"].IsString) { throw new SerializationException(); }  AnimName = _buf["AnimName"]; }
    }

    public static Effect DeserializeEffect(JSONNode _buf)
    {
        return new Effect(_buf);
    }

    /// <summary>
    /// 特效编号
    /// </summary>
    public readonly int Id;
    /// <summary>
    /// 实体编号
    /// </summary>
    public readonly int EntityId;
    /// <summary>
    /// 活跃时间
    /// </summary>
    public readonly float ActiveSeconds;
    /// <summary>
    /// 动画名称
    /// </summary>
    public readonly string AnimName;
   
    public const int __ID__ = 2072749489;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        
        
        
        
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "EntityId:" + EntityId + ","
        + "ActiveSeconds:" + ActiveSeconds + ","
        + "AnimName:" + AnimName + ","
        + "}";
    }
}

}
