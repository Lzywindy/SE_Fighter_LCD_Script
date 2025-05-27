using System;
using System.Collections;
using System.Collections.Generic;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {

        /// <summary>
        /// 边界框是长方形
        /// 有顶点A、B、C、D
        /// 其中
        ///     AB、CD平行于X轴
        ///     BC、DA平行于Y轴
        /// </summary>
        public struct 边界框 : IEquatable<边界框>, IEqualityComparer<边界框>
        {
            /// <summary>
            /// 顶点A
            /// </summary>
            public Vector2 A { get; private set; }
            /// <summary>
            /// 顶点B
            /// </summary>
            public Vector2 B => new Vector2(XM, Y0);
            /// <summary>
            /// 顶点C
            /// </summary>
            public Vector2 C { get; private set; }
            /// <summary>
            /// 顶点D
            /// </summary>
            public Vector2 D => new Vector2(X0, YM);
            /// <summary>
            /// 宽
            /// </summary>
            public float a => C.X - A.X;
            /// <summary>
            /// 高
            /// </summary>
            public float b => C.Y - A.Y;
            /// <summary>
            /// 左边X坐标值
            /// </summary>
            public float X0 => A.X;
            /// <summary>
            /// 右边X坐标值
            /// </summary>
            public float XM => C.X;
            /// <summary>
            /// 上边Y坐标值
            /// </summary>
            public float Y0 => A.Y;
            /// <summary>
            /// 下边Y坐标值
            /// </summary>
            public float YM => C.Y;
            /// <summary>
            /// 长方形的尺寸
            ///     X：宽多少
            ///     Y：高多少
            /// </summary>
            public Vector2 尺寸 => new Vector2(a, b);
            /// <summary>
            /// 最小边长
            /// </summary>
            public float 最小边长 => Math.Min(a, b);
            /// <summary>
            /// 最大边长
            /// </summary>
            public float 最大边长 => Math.Max(a, b);
            /// <summary>
            /// 区域比例
            /// </summary>
            public float 比例 => a / b;
            /// <summary>
            /// 半宽半高
            /// </summary>
            public Vector2 半尺寸 => 尺寸 * 0.5f;
            /// <summary>
            /// 方形的中心坐标
            /// </summary>
            public Vector2 中心 => (C + A) * 0.5f;
            /// <summary>
            /// 从最大最小值开始创建
            /// </summary>
            /// <param name="最小">左上角顶点A</param>
            /// <param name="最大">右下角顶点C</param>
            public 边界框(Vector2 最小, Vector2 最大) { A = 最小; C = 最大; }
            /// <summary>
            /// 从坐标轴指开始创建边框
            /// </summary>
            /// <param name="X0">左边X坐标值</param>
            /// <param name="XM">右边X坐标值</param>
            /// <param name="Y0">上边Y坐标值</param>
            /// <param name="YM">下边Y坐标值</param>
            public 边界框(float X0, float XM, float Y0, float YM) { A = new Vector2(X0, Y0); C = new Vector2(XM, YM); }
            /// <summary>
            /// 从其他边界框创建
            /// </summary>
            /// <param name="区域">边界框</param>
            public 边界框(BoundingBox2 区域) { A = 区域.Min; C = 区域.Max; }
            /// <summary>
            /// 从中心、尺寸创建边界框
            /// </summary>
            /// <param name="中心">中心坐标</param>
            /// <param name="尺寸">宽高尺寸</param>
            /// <returns>创建好的边界框</returns>
            public static 边界框 创建_中心_尺寸(Vector2 中心, Vector2 尺寸) { Vector2 偏移 = 尺寸 * 0.5f, 小 = 中心 - 偏移, 大 = 中心 + 偏移; return new 边界框(小, 大); }
            /// <summary>
            /// 从中心、尺寸创建边界框（创建下来是正方形）
            /// </summary>
            /// <param name="中心">中心坐标</param>
            /// <param name="尺寸">边长</param>
            /// <returns>创建好的边界框</returns>
            public static 边界框 创建_中心_尺寸(Vector2 中心, float 尺寸) { Vector2 偏移 = new Vector2(尺寸 * 0.5f), 小 = 中心 - 偏移, 大 = 中心 + 偏移; return new 边界框(小, 大); }
            /// <summary>
            /// 从中心、半尺寸创建边界框
            /// </summary>
            /// <param name="中心">中心坐标</param>
            /// <param name="半尺寸">半宽半高尺寸</param>
            /// <returns>创建好的边界框</returns>
            public static 边界框 创建_中心_半尺寸(Vector2 中心, Vector2 半尺寸) { Vector2 小 = 中心 - 半尺寸, 大 = 中心 + 半尺寸; return new 边界框(小, 大); }
            /// <summary>
            /// 从中心、半尺寸创建边界框（创建下来是正方形）
            /// </summary>
            /// <param name="中心">中心坐标</param>
            /// <param name="半尺寸">边长</param>
            /// <returns>创建好的边界框</returns>
            public static 边界框 创建_中心_半尺寸(Vector2 中心, float 半尺寸) { Vector2 偏移 = new Vector2(半尺寸), 小 = 中心 - 偏移, 大 = 中心 + 偏移; return new 边界框(小, 大); }
            /// <summary>
            /// 创建正方形
            /// </summary>
            /// <param name="中心">方形中心</param>
            /// <param name="尺寸">方形高宽</param>
            /// <param name="内接_外接">正方形是在方形内部还是外部, 为真就是外部，为假就是内部</param>
            /// <returns>创建好的正方形边界框</returns>
            public static 边界框 创建_中心_方形(Vector2 中心, Vector2 尺寸, bool 内接_外接 = false) => 创建_中心_尺寸(中心, 内接_外接 ? Math.Max(尺寸.X, 尺寸.Y) : Math.Min(尺寸.X, 尺寸.Y));
            /// <summary>
            /// 创建正方形
            /// </summary>
            /// <param name="框">参考边界框</param>
            /// <param name="内接_外接">正方形是在方形内部还是外部, 为真就是外部，为假就是内部</param>
            /// <returns>创建好的正方形边界框</returns>
            public static 边界框 创建_方形(边界框 框, bool 内接_外接 = false) => 创建_中心_尺寸(框.中心, 内接_外接 ? Math.Max(框.尺寸.X, 框.尺寸.Y) : Math.Min(框.尺寸.X, 框.尺寸.Y));
            /// <summary>
            /// 从坐标轴指开始创建边框
            /// </summary>
            /// <param name="X0">左边X坐标值</param>
            /// <param name="XM">右边X坐标值</param>
            /// <param name="Y0">上边Y坐标值</param>
            /// <param name="YM">下边Y坐标值</param>
            /// <returns>创建好的边界框</returns>
            public static 边界框 创建(float X0, float XM, float Y0, float YM) => new 边界框(X0, XM, Y0, YM);
            /// <summary>
            /// 从最大最小值开始创建
            /// </summary>
            /// <param name="小">左上角顶点A</param>
            /// <param name="大">右下角顶点C</param>
            /// <returns>创建好的边界框</returns>
            public static 边界框 创建(Vector2 小, Vector2 大) => new 边界框(小, 大);
            /// <summary>
            /// 从其他边界框创建
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <returns>创建好的边界框</returns>
            public static 边界框 创建(BoundingBox2 区域) => new 边界框(区域);
            /// <summary>
            /// 平移边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="值">平移值</param>
            /// <returns>平移后的边界框</returns>
            public static 边界框 平移(边界框 区域, Vector2 值) => 创建_中心_尺寸(区域.中心 + 值, 区域.尺寸);
            /// <summary>
            /// 平移边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="值">平移值</param>
            /// <param name="轴">0沿X轴平移这个值，1沿Y轴平移这个值，其他X、Y均平移这个值</param>
            /// <returns>平移后的边界框</returns>
            public static 边界框 平移(边界框 区域, float 值, int 轴 = 2) => 创建_中心_尺寸(区域.中心 + new Vector2((轴 == 0 || 轴 > 1) ? 值 : 0, (轴 >= 1) ? 值 : 0), 区域.尺寸);
            /// <summary>
            /// 平移边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="值X">沿X轴平移</param>
            /// <param name="值Y">沿Y轴平移</param>
            /// <returns>平移后的边界框</returns>
            public static 边界框 平移(边界框 区域, float 值X, float 值Y) => 创建_中心_尺寸(区域.中心 + new Vector2(值X, 值Y), 区域.尺寸);
            /// <summary>
            /// 缩放边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="值">缩放值</param>
            /// <returns>缩放后的边界框</returns>
            public static 边界框 缩放(边界框 区域, Vector2 值) => 创建_中心_尺寸(区域.中心, 区域.尺寸 * 值);
            /// <summary>
            /// 缩放边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="值">缩放值</param>
            /// <param name="轴">0宽缩放这个值，1高缩放这个值，其他宽、高均缩放这个值</param>
            /// <returns>缩放后的边界框</returns>
            public static 边界框 缩放(边界框 区域, float 值, int 轴 = 2) => 创建_中心_尺寸(区域.中心, 区域.尺寸 * new Vector2((轴 == 0 || 轴 > 1) ? 值 : 1, (轴 >= 1) ? 值 : 1));
            /// <summary>
            /// 缩放边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="值X">宽缩放</param>
            /// <param name="值Y">高缩放</param>
            /// <returns>缩放后的边界框</returns>
            public static 边界框 缩放(边界框 区域, float 值X, float 值Y) => 创建_中心_尺寸(区域.中心, 区域.尺寸 * new Vector2(值X, 值Y));
            /// <summary>
            /// 缩进边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="值">缩进值(正值向内，负值向外)</param>
            /// <returns>缩进后的边界框</returns>
            public static 边界框 缩进(边界框 区域, Vector2 值) => 创建_中心_尺寸(区域.中心, 区域.尺寸 - 值);
            /// <summary>
            /// 缩进边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="值">缩进值(正值向内，负值向外)</param>
            /// <param name="轴">0宽缩进这个值，1高缩进这个值，其他宽、高均缩进这个值</param>
            /// <returns>缩进后的边界框</returns>
            public static 边界框 缩进(边界框 区域, float 值, int 轴 = 2) => 创建_中心_尺寸(区域.中心, 区域.尺寸 - new Vector2((轴 == 0 || 轴 > 1) ? 值 : 0, (轴 >= 1) ? 值 : 0));
            /// <summary>
            /// 缩进边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="值X">宽缩进(正值向内，负值向外)</param>
            /// <param name="值Y">高缩进(正值向内，负值向外)</param>
            /// <returns>缩进后的边界框</returns>
            public static 边界框 缩进(边界框 区域, float 值X, float 值Y) => 创建_中心_尺寸(区域.中心, 区域.尺寸 - new Vector2(值X, 值Y));
            /// <summary>
            /// 相等否
            /// </summary>
            /// <param name="other">其他边框</param>
            /// <returns>是否相等</returns>
            public bool Equals(边界框 other) => other.A == A && other.C == C;
            public bool Equals(边界框 x, 边界框 y) => x.A == y.A && x.C == y.C;
            public int GetHashCode(边界框 obj) => obj.GetHashCode();
            public override int GetHashCode() { int hashCode = -1834730338; hashCode = hashCode * -1521134295 + A.GetHashCode(); hashCode = hashCode * -1521134295 + C.GetHashCode(); return hashCode; }
            public override bool Equals(object obj) { if (obj == null || !(obj is 边界框)) return false; return A == ((边界框)obj).A && C == ((边界框)obj).C; }
            /// <summary>
            /// BoundingBox2转边界框
            /// </summary>
            /// <param name="区域">BoundingBox2类型的边界框</param>
            public static implicit operator 边界框(BoundingBox2 区域) => new 边界框(区域);
            /// <summary>
            /// 边界框转BoundingBox2
            /// </summary>
            /// <param name="区域">边界框</param>
            public static implicit operator BoundingBox2(边界框 区域) => new BoundingBox2(区域.A, 区域.C);
            /// <summary>
            /// 平移边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="位置">值</param>
            /// <returns>平移后的边界框</returns>
            public static 边界框 operator +(边界框 区域, Vector2 位置) => 创建_中心_尺寸(区域.中心 + 位置, 区域.尺寸);
            /// <summary>
            /// 平移边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="位置">值</param>
            /// <returns>平移后的边界框</returns>
            public static 边界框 operator -(边界框 区域, Vector2 位置) => 创建_中心_尺寸(区域.中心 - 位置, 区域.尺寸);
            /// <summary>
            /// 缩放边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="缩放">缩放值</param>
            /// <returns>缩放后的边界框</returns>
            public static 边界框 operator *(边界框 区域, Vector2 缩放) => 创建_中心_尺寸(区域.中心, 区域.尺寸 * 缩放);
            /// <summary>
            /// 缩放边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="缩放">缩放值</param>
            /// <returns>缩放后的边界框</returns>
            public static 边界框 operator /(边界框 区域, Vector2 缩放) => 创建_中心_尺寸(区域.中心, 区域.尺寸 / 缩放);
            /// <summary>
            /// 缩放边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="缩放">缩放值</param>
            /// <returns>缩放后的边界框</returns>
            public static 边界框 operator *(边界框 区域, float 缩放) => 创建_中心_尺寸(区域.中心, 区域.尺寸 * 缩放);
            /// <summary>
            /// 缩放边界框
            /// </summary>
            /// <param name="区域">边界框</param>
            /// <param name="缩放">缩放值</param>
            /// <returns>缩放后的边界框</returns>
            public static 边界框 operator /(边界框 区域, float 缩放) => 创建_中心_尺寸(区域.中心, 区域.尺寸 / 缩放);


            public static bool operator ==(边界框 x, 边界框 y) => x.A == y.A && x.C == y.C;
            public static bool operator !=(边界框 x, 边界框 y) => x.A != y.A || x.C != y.C;
            public static 边界框 默认 => default(边界框);
        }

    }
}
