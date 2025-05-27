using Sandbox.ModAPI.Ingame;
using System.Collections.Generic;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class My显示参数
        {
            public float 显示范围;
            public string 显示逻辑;
            public float 锁定范围;
            public string 锁定逻辑;
            public string 当前武器;
            public string 武器逻辑;
            public string 武器1名称;
            public float 武器1参数_百分比;
            public string 武器2名称;
            public float 武器2参数_百分比;
            public string 武器3名称;
            public float 武器3参数_百分比;
            public string 武器4名称;
            public float 武器4参数_百分比;
            public string 武器5类型;
            public int 武器5数量;
            public float 护盾储能百分比;
            public float 电力储能百分比;
            public float 曲速储能百分比;
            public float 氢气储量百分比;
            public readonly List<My图标显示参数> 目标绘制 = new List<My图标显示参数>();
            public readonly My目标信息汇总 目标信息汇总 = new My目标信息汇总();
            public HashSet<long> 锁定列表 => 目标信息汇总.锁定列表;
            public void 清理()
            {
                目标信息汇总.清理();
            }
            public void 计算显示(My物理包 物理包)
            {
                MatrixD 矩阵 = 物理包.控制器_世界矩阵;
                矩阵.Translation = 物理包.飞船_位置;
                MatrixD 逆矩阵 = MatrixD.Invert(矩阵);
                目标绘制.Clear();
                foreach (var 目标 in 目标信息汇总.雷达目标_人物)
                    目标绘制.Add(new My图标显示参数(目标.Value, 逆矩阵));
                foreach (var 目标 in 目标信息汇总.雷达目标_小网格)
                    目标绘制.Add(new My图标显示参数(目标.Value, 逆矩阵));
                foreach (var 目标 in 目标信息汇总.雷达目标_大网格)
                    目标绘制.Add(new My图标显示参数(目标.Value, 逆矩阵));
            }
        }
    }
}
