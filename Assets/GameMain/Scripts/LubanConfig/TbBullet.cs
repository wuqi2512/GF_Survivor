
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
public partial class TbBullet
{
    private readonly System.Collections.Generic.Dictionary<int, Bullet> _dataMap;
    private readonly System.Collections.Generic.List<Bullet> _dataList;
    
    public TbBullet(JSONNode _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, Bullet>();
        _dataList = new System.Collections.Generic.List<Bullet>();
        
        foreach(JSONNode _ele in _buf.Children)
        {
            Bullet _v;
            { if(!_ele.IsObject) { throw new SerializationException(); }  _v = Bullet.DeserializeBullet(_ele);  }
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, Bullet> DataMap => _dataMap;
    public System.Collections.Generic.List<Bullet> DataList => _dataList;

    public Bullet GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Bullet Get(int key) => _dataMap[key];
    public Bullet this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}
