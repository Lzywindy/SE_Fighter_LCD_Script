using System;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public struct 绘图区域
        {
            public 绘图区域(string 区域名, Vector2 左上角, Vector2 右下角, Vector2 边界大小 = default(Vector2))
            {
                this.区域名 = 区域名; 总区域 = new 边界框(左上角, 右下角);
                绘图区 = new 边界框(总区域.A + 边界大小, 总区域.C - 边界大小);
                区域最小尺寸 = V最小值(绘图区.尺寸);
                字符最大缩放 = Math.Min(区域最小尺寸 / 32f, 1) * 0.9f;
                分辨率缩放 = 区域最小尺寸 / 参考尺寸;
            }
            public 绘图区域(string 区域名, float X最小, float X最大, float Y最小, float Y最大, Vector2 边界大小 = default(Vector2))
            {
                this.区域名 = 区域名;
                总区域 = new 边界框(向量(X最小, Y最小), 向量(X最大, Y最大));
                绘图区 = new 边界框(总区域.A + 边界大小, 总区域.C - 边界大小);
                区域最小尺寸 = V最小值(绘图区.尺寸);
                字符最大缩放 = Math.Min(区域最小尺寸 / 32f, 1) * 0.9f;
                分辨率缩放 = 区域最小尺寸 / 参考尺寸;
            }
            public 绘图区域(string 区域名, BoundingBox2 区域, Vector2 边界大小 = default(Vector2))
            {
                this.区域名 = 区域名;
                总区域 = new 边界框(区域);
                绘图区 = new 边界框(总区域.A + 边界大小, 总区域.C - 边界大小);
                区域最小尺寸 = V最小值(绘图区.尺寸);
                字符最大缩放 = Math.Min(区域最小尺寸 / 32f, 1) * 0.9f;
                分辨率缩放 = 区域最小尺寸 / 参考尺寸;
            }
            public readonly 边界框 总区域;
            public readonly 边界框 绘图区;
            public Vector2 中心 => 总区域.中心;
            public readonly string 区域名;
            public readonly float 字符最大缩放;
            public readonly float 区域最小尺寸;
            public readonly float 分辨率缩放;
            public float X0 => 总区域.X0;
            public float XM => 总区域.XM;
            public float Y0 => 总区域.Y0;
            public float YM => 总区域.YM;
            public Vector2 左上 => 总区域.A;
            public Vector2 左下 => 总区域.D;
            public Vector2 右上 => 总区域.B;
            public Vector2 右下 => 总区域.C;
        }

    }
}
