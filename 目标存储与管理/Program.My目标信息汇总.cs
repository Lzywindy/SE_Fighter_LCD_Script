using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage.Game;


namespace IngameScript
{
    public partial class Program
    {
        public class My目标信息汇总
        {
            public readonly My实体字典 雷达目标_人物 = new My实体字典();
            public readonly My实体字典 雷达目标_小网格 = new My实体字典();
            public readonly My实体字典 雷达目标_大网格 = new My实体字典();
            public readonly HashSet<long> 锁定列表 = new HashSet<long>();
            public readonly My实体字典 雷达目标_小行星 = new My实体字典();
            public readonly My实体字典 雷达目标_星球 = new My实体字典();
            public readonly My实体字典 雷达目标_漂浮物 = new My实体字典();

            public void 获取实体(My传感器阵列 阵列) { if (阵列 == null || 阵列.Count < 1) return; foreach (var 方块 in 阵列) { if (方块 == null || 方块.Closed) continue; 实体列表.Clear(); 实体列表.从传感器获取(方块); foreach (var 实体 in 实体列表) { if (实体.IsEmpty()) continue; 添加实体分类(实体); } 实体列表.Clear(); } }
            public void 获取实体(My炮塔阵列 阵列) { if (阵列 == null || 阵列.Count < 1) return; foreach (var 方块 in 阵列) { if (方块 == null || 方块.Closed) continue; var 实体 = 方块.GetTargetedEntity(); if (实体.IsEmpty()) continue; 放入实体(实体, true); } }
            public void 获取实体(My炮塔块阵列 阵列) { if (阵列 == null || 阵列.Count < 1) return; foreach (var 方块 in 阵列) { if (方块 == null || 方块.Closed) continue; var 实体 = 方块.GetTargetedEntity(); if (实体.IsEmpty()) continue; 放入实体(实体, true); } }
            public void 放入实体(MyDetectedEntityInfo 实体, bool 是否锁定) { 添加实体分类(实体); if (是否锁定) 实体锁定(实体); }
            public void 清理() { 雷达目标_人物.移除过时信息(锁定列表, 时刻, 5); 雷达目标_小网格.移除过时信息(锁定列表, 时刻, 5); 雷达目标_大网格.移除过时信息(锁定列表, 时刻, 5); 雷达目标_小行星.移除过时信息(锁定列表, 时刻, 1e6f); 雷达目标_星球.移除过时信息(锁定列表, 时刻, 1e6f); 雷达目标_漂浮物.移除过时信息(锁定列表, 时刻, 1e6f); 实体列表.Clear(); }
            public void 清空更新步() => 更新步_目标 = 0;
            public void 更新步() => 更新步_目标++;
            void 添加实体分类(MyDetectedEntityInfo 实体) { switch (实体.Type) { case MyDetectedEntityType.SmallGrid: 雷达目标_小网格.添加或更新(实体, 时刻, 更新步_目标); break; case MyDetectedEntityType.LargeGrid: 雷达目标_大网格.添加或更新(实体, 时刻, 更新步_目标); break; case MyDetectedEntityType.CharacterOther: case MyDetectedEntityType.CharacterHuman: 雷达目标_人物.添加或更新(实体, 时刻, 更新步_目标); break; case MyDetectedEntityType.FloatingObject: 雷达目标_漂浮物.添加或更新(实体, 时刻, 更新步_目标); break; case MyDetectedEntityType.Asteroid: 雷达目标_小行星.添加或更新(实体, 时刻, 更新步_目标); break; case MyDetectedEntityType.Planet: 雷达目标_星球.添加或更新(实体, 时刻, 更新步_目标); break; default: break; } }
            void 实体锁定(MyDetectedEntityInfo 实体) { switch (实体.Relationship) { case MyRelationsBetweenPlayerAndBlock.Enemies: case MyRelationsBetweenPlayerAndBlock.NoOwnership: case MyRelationsBetweenPlayerAndBlock.Neutral: switch (实体.Type) { case MyDetectedEntityType.SmallGrid: case MyDetectedEntityType.LargeGrid: case MyDetectedEntityType.CharacterOther: case MyDetectedEntityType.CharacterHuman: 锁定列表.Add(实体.EntityId); break; default: break; } break; } }

            readonly My实体列表_信息 实体列表 = new My实体列表_信息();

            long 更新步_目标 = 0;
            public long 时刻 => DateTime.Now.Ticks;
        }









    }
}
