
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
public partial class TbEntity
{
    private readonly System.Collections.Generic.Dictionary<int, Entity> _dataMap;
    private readonly System.Collections.Generic.List<Entity> _dataList;
    
    public TbEntity(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, Entity>();
        _dataList = new System.Collections.Generic.List<Entity>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            Entity _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = Entity.DeserializeEntity(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, Entity> DataMap => _dataMap;
    public System.Collections.Generic.List<Entity> DataList => _dataList;

    public Entity GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Entity Get(int key) => _dataMap[key];
    public Entity this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}

