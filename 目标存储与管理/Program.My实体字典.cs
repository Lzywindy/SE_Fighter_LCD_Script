using Sandbox.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VRage;
using VRage.Game;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class My实体字典 : IDictionary<long, My实体信息>
        {
            public void 得到目标夹角信息(List<My目标夹角> 目标夹角列表, My物理包 物理)
            {
                foreach (var 目标 in this)
                {
                    if (!目标.Value.有效的)
                        continue;
                    MyRelationsBetweenPlayerAndBlock 关系 = (MyRelationsBetweenPlayerAndBlock)目标.Value.所属关系;
                    if (关系 == MyRelationsBetweenPlayerAndBlock.NoOwnership || 关系 == MyRelationsBetweenPlayerAndBlock.Neutral || 关系 == MyRelationsBetweenPlayerAndBlock.Enemies)
                    {
                        double 角度 = AngleBetweenD(物理.控制器_世界矩阵.Forward, 目标.Value.位置 - 物理.飞船_位置);
                        if (角度 <= 锁定视场)
                            目标夹角列表.Add(new My目标夹角(目标.Value.实体编号, 角度));
                    }
                }
            }

            public void 添加或更新(MyDetectedEntityInfo 信息, long? 时刻, long 步) { if (字典.ContainsKey(信息.EntityId)) 字典[信息.EntityId].更新(信息, 时刻); else 字典.Add(信息.EntityId, new My实体信息(信息, 时刻)); 字典[信息.EntityId].更新步 = 步; }
            public void 添加或更新(My实体信息 信息) { if (!字典.ContainsKey(信息.实体编号)) 字典.Add(信息.实体编号, 信息); }
            public void 添加或更新(MyTuple<long, string, ushort, ushort, long, MyTuple<MatrixD, Vector3D, Vector3, BoundingSphereD>> 信息, long 步) { if (!字典.ContainsKey(信息.Item1)) 字典.Add(信息.Item1, new My实体信息(信息)); else 字典[信息.Item1].更新(信息); 字典[信息.Item1].更新步 = 步; }
            public void 移除过时信息(HashSet<long> 锁定列表, long 时刻, double 过时阈值) { List<long> 不存在的东西 = new List<long>(); foreach (var 东西 in 字典) { if (!东西.Value.有效的) 不存在的东西.Add(东西.Key); else if (TimeSpan.FromTicks(时刻 - 东西.Value.时间戳).TotalSeconds > 过时阈值) 不存在的东西.Add(东西.Key); } foreach (var 东西 in 不存在的东西) { 字典.Remove(东西); } 锁定列表.ExceptWith(不存在的东西); }
            public My实体信息 this[long 实体编号] { get { if (!字典.ContainsKey(实体编号)) return null; return 字典[实体编号]; } set { if (value == null) return; if (!字典.ContainsKey(实体编号)) 字典.Add(实体编号, value); 字典[实体编号].更新(value); } }
            public List<My实体信息> 所有实体() { List<My实体信息> 列表 = new List<My实体信息>(); foreach (var 东西 in 字典.Values) { 列表.Add(东西); } return 列表; }
            public bool ContainsKey(long key) => 字典.ContainsKey(key);
            public void Add(long key, My实体信息 value) { if (!字典.ContainsKey(key)) 字典.Add(key, value); 字典[key].更新(value); }
            public bool Remove(long key) { if (!字典.ContainsKey(key)) return false; return 字典.Remove(key); }
            public bool TryGetValue(long key, out My实体信息 value) { value = null; if (!字典.ContainsKey(key)) return false; value = 字典[key]; return true; }
            public void Add(KeyValuePair<long, My实体信息> item) { if (!字典.ContainsKey(item.Key)) 字典.Add(item.Key, item.Value); 字典[item.Key].更新(item.Value); }
            public void Clear() { 字典.Clear(); }
            public bool Contains(KeyValuePair<long, My实体信息> item) => 字典.ContainsKey(item.Key);
            public void CopyTo(KeyValuePair<long, My实体信息>[] array, int arrayIndex) { if (array == null || arrayIndex >= array.Length) return; var 列表 = 字典.ToArray(); for (int index = arrayIndex; index < array.Length; index++) { array[index] = 列表[index]; } }
            public bool Remove(KeyValuePair<long, My实体信息> item) { if (!字典.ContainsKey(item.Key)) return false; return 字典.Remove(item.Key); }
            public IEnumerator<KeyValuePair<long, My实体信息>> GetEnumerator() => 字典.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => 字典.GetEnumerator();
            public ICollection<long> Keys => 字典.Keys;
            public ICollection<My实体信息> Values => 字典.Values;
            public int Count => 字典.Count;
            public bool IsReadOnly => false;
            readonly Dictionary<long, My实体信息> 字典 = new Dictionary<long, My实体信息>();
        }
    }
}
