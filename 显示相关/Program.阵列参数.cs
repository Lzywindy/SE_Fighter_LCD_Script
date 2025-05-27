using System;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public struct 阵列参数
        {
            public readonly 边界框 绘图区域;
            public readonly Vector2 物品区域;
            public readonly Vector2 行列;
            public readonly Vector2 阵列物品尺寸;
            public int 行 { get; private set; }
            public int 列 { get; private set; }
            public 阵列参数(边界框 绘图区域, Vector2 物品区域)
            {
                this.绘图区域 = 绘图区域;
                this.物品区域 = 物品区域;
                Vector2 高宽比例 = 绘图区域.尺寸 / 物品区域;
                float 当前缩放 = 1.0f / V最小值(高宽比例);
                阵列物品尺寸 = this.物品区域 * 当前缩放;
                行列 = 绘图区域.尺寸 / 阵列物品尺寸;
                行 = Math.Max((int)行列.Y, 1);
                列 = Math.Max((int)行列.X, 1);
            }
            public void 得到行列和大小(int 个数, Action<int, 边界框> 绘制函数)
            {
                int 测试倍率 = 1, _列, _行, 数 = 0;
                while ((行 * 列) < 个数) { 行 = Math.Max((int)行列.Y, 1) * 测试倍率; 列 = Math.Max((int)行列.X, 1) * 测试倍率; 测试倍率++; }
                Vector2I 行列值 = new Vector2I(列, 行);
                Vector2 物品尺寸 = 阵列物品尺寸 / Math.Max(测试倍率 - 1, 1), 起始位置 = 绘图区域.中心 - 物品尺寸 * 行列值 * 0.5f;
                边界框 起始框 = 边界框.创建(起始位置, 起始位置 + 物品尺寸);
                for (_行 = 0; _行 < 行列值.Y; _行++) { for (_列 = 0; _列 < 行列值.X; _列++) { 绘制函数?.Invoke(数, 起始框 + 向量(_列, _行) * 物品尺寸); 数++; if (数 >= 个数) break; } if (数 >= 个数) break; }
            }
        }

    }
}
