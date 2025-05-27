using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public void 分配LCD资源(Dictionary<string, List<MyLCD参数>> LCD_绘图脚本匹配, IMyTerminalBlock LCD设备)
        {
            if (LCD设备 == null || LCD设备.Closed)
                return;
            if ((LCD设备 is IMyFunctionalBlock) && !(LCD设备 as IMyFunctionalBlock).Enabled)
                return;
            if (!((LCD设备 is IMyTextSurfaceProvider) || (LCD设备 is IMyTextPanel)))
                return;
            MyIni LCD显示设置 = new MyIni();
            LCD显示设置.Clear();
            LCD显示设置.TryParse(LCD设备.CustomData);
            bool 需要写入样例 = false;
            if (!LCD显示设置.ContainsSection("数据大屏"))
            {
                LCD显示设置.AddSection("数据大屏");
                需要写入样例 = true;
            }
            IMyTextSurfaceProvider LCD设备类型 = LCD设备 as IMyTextSurfaceProvider;
            if (LCD设备类型 != null)
            {
                var _LCD设备 = LCD设备 as IMyTextSurfaceProvider;
                var LCD数目 = _LCD设备.SurfaceCount;
                for (int i = 0; i < LCD数目; i++)
                {
                    if (LCD显示设置.ContainsKey("数据大屏", $"LCD面{i}"))
                    {
                        var 类型 = LCD显示设置.Get("数据大屏", $"LCD面{i}").ToString("");
                        var 缩放X = LCD显示设置.Get("数据大屏", $"LCD面{i}_X缩放").ToSingle(1);
                        var 缩放Y = LCD显示设置.Get("数据大屏", $"LCD面{i}_Y缩放").ToSingle(1);
                        var 额外参数 = LCD显示设置.Get("数据大屏", $"LCD面{i}_额外参数").ToString("");
                        if (LCD_绘图脚本匹配.ContainsKey(类型))
                        {
                            var 当前屏幕 = _LCD设备.GetSurface(i);
                            LCD_绘图脚本匹配[类型].Add(MyLCD参数.创建(当前屏幕, 向量(缩放X, 缩放Y), 额外参数));
                        }
                    }
                    else
                    {
                        LCD显示设置.Set("数据大屏", $"LCD面{i}", "");
                        LCD显示设置.Set("数据大屏", $"LCD面{i}_X缩放", 1.0f);
                        LCD显示设置.Set("数据大屏", $"LCD面{i}_Y缩放", 1.0f);
                        LCD显示设置.Set("数据大屏", $"LCD面{i}_额外参数", "");
                    }
                }
            }
            else
            {
                var _LCD设备 = LCD设备 as IMyTextPanel;
                if (LCD显示设置.ContainsKey("数据大屏", "LCD面"))
                {
                    var 类型 = LCD显示设置.Get("数据大屏", "LCD面").ToString();
                    var 缩放X = LCD显示设置.Get("数据大屏", "LCD面_X缩放").ToSingle(1);
                    var 缩放Y = LCD显示设置.Get("数据大屏", "LCD面_Y缩放").ToSingle(1);
                    var 额外参数 = LCD显示设置.Get("数据大屏", $"LCD面_额外参数").ToString("");
                    if (LCD_绘图脚本匹配.ContainsKey(类型))
                        LCD_绘图脚本匹配[类型].Add(MyLCD参数.创建(_LCD设备, 向量(缩放X, 缩放Y), 额外参数));
                }
                else
                {
                    LCD显示设置.Set("数据大屏", $"LCD面", "");
                    LCD显示设置.Set("数据大屏", $"LCD面_X缩放", 1.0f);
                    LCD显示设置.Set("数据大屏", $"LCD面_Y缩放", 1.0f);
                    LCD显示设置.Set("数据大屏", $"LCD面_额外参数", "");
                }
            }
            if (需要写入样例)
            {
                LCD设备.CustomData = LCD显示设置.ToString();
            }
        }
    }
    public partial class Program
    {
        public static bool 实体不存在<T>(T 方块) where T : IMyEntity => 方块 == null || 方块.Closed;
        public static Vector2 向量(float X, float Y) => new Vector2(X, Y);
        public static Vector2 向量(double X, double Y) => new Vector2((float)X, (float)Y);
        public static Vector2 向量(float 尺寸) => new Vector2(尺寸, 尺寸);
        public static Vector2 向量_最小(Vector2 向) { var 值 = Math.Min(向.X, 向.Y); return 向量(值, 值); }
        public static Vector2 向量_从角度_快(float 角度) => new Vector2(Cos.Evaluate(角度), Sin.Evaluate(角度));
        public static Vector2 向量_从角度(float 角度) => new Vector2((float)Math.Cos(角度), (float)Math.Sin(角度));
        public static Vector2 向量_从角度(double 角度) => new Vector2((float)Math.Cos(角度), (float)Math.Sin(角度));
        public static Vector2 向量(Vector2 向, float SX, float SY) => new Vector2(SX, SY) * 向;
        public static Vector2 向量_旋转(Vector2 向, float 角度) { float S = (float)Math.Sin(角度), C = (float)Math.Cos(角度); return new Vector2(向.X * C - 向.Y * S, 向.X * S + 向.Y * C); }

        public static Color 缩放RGB(Color 颜色, float 缩放) => 颜色.ToVector3() * 缩放;
        public static Color 透明RGB(Color 颜色, float 透明) { Vector4 c = 颜色.ToVector4(); c.W *= 透明; return new Color(c); }
        public static float 计算百分比(float up, float down) { if (down == 0) return 0; return up / down; }
        public static float V最小值(Vector2 值) => Math.Min(值.X, 值.Y);
        public static float V最大值(Vector2 值) => Math.Max(值.X, 值.Y);
        public static float 字符行高(float 字体大小) => Math.Max(0, 字体大小) * 字体尺寸;
        public static float 环转360度(float 角度) { if (角度 < 0) 角度 = (360 - 角度) % 360; if (角度 >= 360) 角度 = 角度 % 360; return 角度; }
        public static float 字符宽度(char 字) { return char.IsLetter(字) ? 28 : 28; }
        public static readonly List<Base6Directions.Direction> 遍历方位 = new List<Base6Directions.Direction>() { Base6Directions.Direction.Forward, Base6Directions.Direction.Left, Base6Directions.Direction.Right, Base6Directions.Direction.Up, Base6Directions.Direction.Down, Base6Directions.Direction.Backward };

        static double SignedAngleD(Vector3D A, Vector3D B, Vector3D Axis) { if (Vector3.IsZero(A) || Vector3.IsZero(B)) return 1; return AngleBetweenD(A, B) * 符号非零(A.Cross(B).Dot(Axis)); }
        static double AngleBetweenD(Vector3D a, Vector3D b) { if (为零(a) || 为零(b)) return 0; a = Vector3D.Normalize(a); b = Vector3D.Normalize(b); var v = Math.Acos(a.Dot(b)); if (double.IsNaN(v)) return 0; return v; }
        static bool 为零(Vector3 向量, float 阈值 = 默认阈值) => Vector3.IsZero(向量, 阈值);
        static bool 为零(Vector3D 向量, double 阈值 = 默认阈值) => Vector3D.IsZero(向量, 阈值);
        static Vector3 方向(Vector3 向量, float 阈值 = 默认阈值) { if (为零(向量, 阈值)) return Vector3.Zero; return Vector3.Normalize(向量); }
        static Vector3D 方向(Vector3D 向量, double 阈值 = 默认阈值) { if (为零(向量, 阈值)) return Vector3D.Zero; return Vector3D.Normalize(向量); }
        static double 置零(double 值, double 阈值 = 默认阈值) => (double.IsNaN(值) || Math.Abs(值) < Math.Abs(阈值)) ? 0 : 值;
        static float 置零(float 值, float 阈值 = 默认阈值) => (float.IsNaN(值) || Math.Abs(值) < Math.Abs(阈值)) ? 0 : 值;
        static Vector3D 置零(Vector3D 向量, double 阈值 = 默认阈值) => new Vector3D(置零(向量.X, 阈值), 置零(向量.Y, 阈值), 置零(向量.Z, 阈值)); static
        Vector3 置零(Vector3 向量, float 阈值 = 默认阈值) => new Vector3(置零(向量.X, 阈值), 置零(向量.Y, 阈值), 置零(向量.Z, 阈值));
        static void 置零(ref Vector3D 值) { if (double.IsNaN(值.X)) 值.X = 0; if (double.IsNaN(值.Y)) 值.Y = 0; if (double.IsNaN(值.Z)) 值.Z = 0; }
        static Vector3D 置零(Vector3D 值) { if (double.IsNaN(值.X)) 值.X = 0; if (double.IsNaN(值.Y)) 值.Y = 0; if (double.IsNaN(值.Z)) 值.Z = 0; return 值; }
        static void 置零(ref Vector3 值) { if (float.IsNaN(值.X)) 值.X = 0; if (float.IsNaN(值.Y)) 值.Y = 0; if (float.IsNaN(值.Z)) 值.Z = 0; }
        static Vector3 置零(Vector3 值) { if (float.IsNaN(值.X)) 值.X = 0; if (float.IsNaN(值.Y)) 值.Y = 0; if (float.IsNaN(值.Z)) 值.Z = 0; return 值; }
        static bool 是否为零(double 值, double 阈值 = 默认阈值) => Math.Abs(值) < Math.Abs(阈值);
        static bool 是否为零(float 值, float 阈值 = 默认阈值) => Math.Abs(值) < Math.Abs(阈值);
        static Vector3D 求和(ICollection<Vector3D> 向量集) { if (集合为空(向量集)) return Vector3D.Zero; Vector3D 值 = Vector3D.Zero; foreach (var 向量 in 向量集) { 值 += 向量; } return 值; }
        static Vector3D 平均(ICollection<Vector3D> 向量集) { if (集合为空(向量集)) return Vector3D.Zero; Vector3D 值 = Vector3D.Zero; foreach (var 向量 in 向量集) { 值 += 向量; } return 值 /= 向量集.Count; }
        static Vector3 求和(ICollection<Vector3> 向量集) { if (集合为空(向量集)) return Vector3.Zero; Vector3 值 = Vector3.Zero; foreach (var 向量 in 向量集) { 值 += 向量; } return 值; }
        static bool 集合为空<T>(ICollection<T> 集合, bool 非空元素集合检查 = false) { if (集合 == null) return true; if (非空元素集合检查) return false; if (集合.Count < 1) return true; return false; }
        static bool 集合为空<T>(IEnumerable<T> 集合, bool 非空元素集合检查 = false) { if (集合 == null) return true; if (非空元素集合检查) return false; if ((集合?.ToList()?.Count ?? 0) < 1) return true; return false; }
        static double 角度计算(Vector3D 向量1, Vector3D 向量2) { if (为零(向量1) || 为零(向量2)) return 0; return Math.Acos(向量1.Dot(向量2) / (向量1.Length() * 向量2.Length())); }
        static int 符号非零(double 值) => (值 >= 0) ? 1 : -1;
        static Vector3D P世界转本地D(MatrixD 世界矩阵, Vector3D 点) => Vector3D.Transform(点, Matrix.Invert(世界矩阵));
        static Vector3D P本地转世界D(MatrixD 世界矩阵, Vector3D 点) => Vector3D.Transform(点, 世界矩阵);
        static Vector3 P世界转本地(Matrix 世界矩阵, Vector3 点) => Vector3.Transform(点, Matrix.Invert(世界矩阵));
        static Vector3 P本地转世界(Matrix 世界矩阵, Vector3 点) => Vector3.Transform(点, 世界矩阵);
        static Vector3D V世界转本地D(MatrixD 世界矩阵, Vector3D 向量) => Vector3D.TransformNormal(向量, Matrix.Invert(世界矩阵));
        static Vector3D V本地转世界D(MatrixD 世界矩阵, Vector3D 向量) => Vector3D.TransformNormal(向量, 世界矩阵);
        static Vector3 V世界转本地(Matrix 世界矩阵, Vector3 向量) => Vector3.TransformNormal(向量, Matrix.Invert(世界矩阵));
        static Vector3 V本地转世界(Matrix 世界矩阵, Vector3 向量) => Vector3.TransformNormal(向量, 世界矩阵);
        static MatrixD M世界转本地D(MatrixD 世界矩阵, MatrixD 矩阵) => MatrixD.Normalize(MatrixD.Multiply(世界矩阵, MatrixD.Invert(矩阵)));
        static MatrixD M本地转世界D(MatrixD 世界矩阵, MatrixD 矩阵) => MatrixD.Normalize(MatrixD.Multiply(世界矩阵, 矩阵));
        static Matrix M世界转本地(Matrix 世界矩阵, Matrix 矩阵) => Matrix.Normalize(世界矩阵 * Matrix.Invert(矩阵));
        static Matrix M本地转世界(Matrix 世界矩阵, Matrix 矩阵) => Matrix.Normalize(世界矩阵 * 矩阵);
        static Vector3D 过滤掉负方向向量(Vector3D 参考向量, Vector3D 向量) { var _参考向量 = 方向(参考向量); var 值 = _参考向量.Dot(向量); if (值 > 0) return 向量; return 向量 - 值 * _参考向量; }
        static Vector3 过滤掉负方向向量(Vector3 参考向量, Vector3 向量) { var _参考向量 = 方向(参考向量); var 值 = _参考向量.Dot(向量); if (值 > 0) return 向量; return 向量 - 值 * _参考向量; }
        static Vector3D 向量合成(Vector3D 主向量, Vector3D 次向量, double 长度限制) { if (Vector3D.IsZero(主向量)) return 次向量; var 主向量方向 = 方向(主向量); var 合成向量 = 主向量 + 次向量; var 主向量投影 = 主向量方向.Dot(合成向量); var 主投影向量 = 主向量投影 * 主向量方向; if (Math.Abs(主向量投影) >= 长度限制) return 主投影向量; var 次方向长度限制 = Math.Sqrt(长度限制 * 长度限制 - 主向量投影 * 主向量投影); var 次方向向量 = 合成向量 - 主投影向量; return 主投影向量 + Vector3D.ClampToSphere(主投影向量, 长度限制); }
        static string 速率转文本(float 值, int 位数 = 2) { 值 = Math.Abs(值); if (值 > 1000) 值 /= 1000; else return $"{Math.Round(值, 位数)}m/s"; if (值 > 10) 值 /= 10; else return $"{Math.Round(值, 位数)}km/s"; if (值 > 30000) 值 /= 30000; else return $"{Math.Round(值, 位数)}万m/s"; if (值 > 1000) 值 /= 1000; else return $"{Math.Round(值, 位数)}C"; if (值 > 1000) 值 /= 1000; else return $"{Math.Round(值, 位数)}kC"; return $"{Math.Round(值, 位数)}MC"; }
        static string 距离转文本(float 值, int 位数 = 2) { 值 = Math.Abs(值); if (值 > 1000) 值 /= 1000; else return $"{Math.Round(值, 位数)}m"; if (值 > 10) 值 /= 10; else return $"{Math.Round(值, 位数)}km"; if (值 > 30000) 值 /= 30000; else return $"{Math.Round(值, 位数)}万m"; if (值 > 60) 值 /= 60; else return $"{Math.Round(值, 位数)}ls"; if (值 > 60) 值 /= 60; else return $"{Math.Round(值, 位数)}lm"; if (值 > 24) 值 /= 24; else return $"{Math.Round(值, 位数)}lh"; if (值 > 365) 值 /= 365; else return $"{Math.Round(值, 位数)}lD"; return $"{Math.Round(值, 位数)}lY"; }
        static string 距离转文本(double 值, int 位数 = 2) { 值 = Math.Abs(值); if (值 > 1000) 值 /= 1000; else return $"{Math.Round(值, 位数)}m"; if (值 > 10) 值 /= 10; else return $"{Math.Round(值, 位数)}km"; if (值 > 30000) 值 /= 30000; else return $"{Math.Round(值, 位数)}万m"; if (值 > 60) 值 /= 60; else return $"{Math.Round(值, 位数)}ls"; if (值 > 60) 值 /= 60; else return $"{Math.Round(值, 位数)}lm"; if (值 > 24) 值 /= 24; else return $"{Math.Round(值, 位数)}lh"; if (值 > 365) 值 /= 365; else return $"{Math.Round(值, 位数)}lD"; return $"{Math.Round(值, 位数)}lY"; }
        public static float 角度循环转为360度(float 角度) { if (角度 < 0) 角度 = (360 - 角度) % 360; if (角度 >= 360) 角度 = 角度 % 360; return 角度; }
        public static void 线(雪碧列表 图, Vector2 开头, Vector2 结尾, Color 颜色, float 线宽) { var 值 = 结尾 - 开头; if (Vector2.IsZero(ref 值)) return; Vector2 大小 = 向量(Math.Max(1, 线宽), 值.Length()); float 数 = (值.X == 0) ? PIO2 : Atan.Evaluate(值.Y / 值.X); 图.图案(方图2, (开头 + 结尾) * 0.5f, 大小, 颜色, 数 - PIO2); }
        static Program()
        {
            float Δ = TPI / 200;
            for (int index = -200; index <= 200; index++)
            {
                var angle = index * Δ;
                var percent = index / 200f;
                Sin.Keys.Add(new CurveKey(angle, (float)Math.Sin(angle)));
                Cos.Keys.Add(new CurveKey(angle, (float)Math.Cos(angle)));
                Sin_Percent.Keys.Add(new CurveKey(percent, (float)Math.Sin(angle)));
                Cos_Percent.Keys.Add(new CurveKey(percent, (float)Math.Cos(angle)));
                Atan.Keys.Add(new CurveKey((float)Math.Tan(percent * PIO2), percent * PIO2));
            }
            Sin.PostLoop = CurveLoopType.Cycle;
            Sin.PreLoop = CurveLoopType.Cycle;
            Cos.PostLoop = CurveLoopType.Cycle;
            Cos.PreLoop = CurveLoopType.Cycle;
            Sin_Percent.PostLoop = CurveLoopType.Cycle;
            Sin_Percent.PreLoop = CurveLoopType.Cycle;
            Cos_Percent.PostLoop = CurveLoopType.Cycle;
            Cos_Percent.PreLoop = CurveLoopType.Cycle;
        }
    }
    public partial class Program
    {
        public static double R53 = Math.Round(5f / 3f, 1), RS2 = Math.Round(Math.Sqrt(2), 1);
        public static readonly Curve Sin_Percent = new Curve(), Cos_Percent = new Curve(), Sin = new Curve(), Cos = new Curve(), Atan = new Curve();
        public const float TPI = MathHelper.TwoPi, PIO2 = MathHelper.PiOver2, PIO4 = MathHelper.PiOver4, PI = MathHelper.Pi, 参考尺寸 = 512, 两倍参考尺寸 = 参考尺寸 * 2f, 半尺寸 = 参考尺寸 / 2, 字体大小 = 0.7f, 仪表外圈缩放 = 0.9f, 仪表内圈缩放 = 0.75f, 字体尺寸 = 32, 字体半尺寸 = 字体尺寸 / 2, 角度分 = 45;
        public static readonly float 偏移 = 90 + 角度分, 总角度 = MathHelper.ToRadians(360 - 角度分 * 2), 字体标题大小 = 字符行高(字体大小 * 1.2f), 字体内容大小 = 字符行高(字体大小);
        public readonly static Vector2 内方框线 = 向量(2), 尺寸 = 向量(参考尺寸, 参考尺寸), 中心偏移 = 向量(半尺寸, 半尺寸), 半尺寸2D = 中心偏移, 全屏尺寸 = 尺寸;
        public readonly static Color 底色 = 缩放RGB(Color.White, 0.1f), 线框 = Color.White, 背景 = Color.Black, 物品底色 = new Color(128, 128, 128, 80), 默认背景色 = new Color(0, 88, 151), 默认前景色 = new Color(179, 237, 255);
        const string 方图1 = "SquareSimple", 空心方图 = "SquareHollow", 方图2 = "SquareTapered", 圆图 = "Circle", 空心圆图 = "CircleHollow", 半圆图 = "SemiCircle", 直角三角图 = "RightTriangle", 三角图 = "Triangle";
        const float RPM2RADS = 1f / 60f * MathHelper.TwoPi, 大网格最大转速RPM = 30, 小网格最大转速RPM = 60, 默认阈值 = 1e-6f, 更新步 = 1f / 60f, 内部距离 = 0.2f;
    }
    public partial class Program
    {
        public class My方块列表<T> : List<T> where T : class, IMyTerminalBlock { public void 清空空物体() => RemoveAll(b => b == null || b.Closed); public void 获取方块(IMyGridTerminalSystem GridTerminalSystem, Func<IMyTerminalBlock, bool> collect = null) => GridTerminalSystem?.GetBlocksOfType(this, collect); public void 获取方块(IMyBlockGroup BlockGroup, Func<IMyTerminalBlock, bool> collect = null) => BlockGroup?.GetBlocksOfType(this, collect); }
        public class My驾驶室阵列 : My方块列表<IMyShipController> { }
        public class My摄像头阵列 : My方块列表<IMyCameraBlock> { public void 充能(bool 允许 = true) { foreach (var 方块 in this) { if (方块 == null || 方块.Closed) continue; 方块.EnableRaycast = 允许; } } }
        public class My传感器阵列 : My方块列表<IMySensorBlock> { }
        public class My炮塔阵列 : My方块列表<IMyLargeTurretBase> { }
        public class My炮塔块阵列 : My方块列表<IMyTurretControlBlock> { }
        public class My控制器 : My方块列表<IMyShipController> { }
        public class My实体列表_信息 : List<MyDetectedEntityInfo> { public void 从传感器获取(IMySensorBlock 方块) => 方块.DetectedEntities(this); }
    }
}
