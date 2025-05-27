using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class LCD显示器
        {
            public IMyTextSurface LCD面板 { get; private set; } = default(IMyTextSurface);
            public 边界框 可视区 { get; private set; } = default(边界框);
            public float 比例 => 可视区.a / 可视区.b;
            public float 缩放 => 可视区.最小边长 / 参考尺寸;
            public Vector2 中心 => 可视区.中心;
            public Vector2 尺寸 => 可视区.尺寸;
            public Vector2 中心偏移 => 可视区.中心 - 可视区.半尺寸;
            public Color 前景色 { get; private set; } = default(Color);
            public Color 背景色 { get; private set; } = default(Color);
            public bool 使用缓存 { get; set; } = true;
            public void 更新(MyLCD参数 LCD面板)
            {
                if (!LCD面板.有效的) return;
                可视区 = 边界框.创建_中心_尺寸(LCD面板.显示屏.TextureSize * 0.5f, LCD面板.显示屏.SurfaceSize);
                前景色 = LCD面板.前景颜色;
                背景色 = LCD面板.背景颜色;
                this.LCD面板 = LCD面板.显示屏;
                if (LCD面板.显示屏.ContentType != ContentType.SCRIPT) LCD面板.显示屏.ContentType = ContentType.SCRIPT;
            }
            public void 更新LCD(雪碧列表 元素, string 种类)
            {
                if (LCD面板 == null || 元素 == null) return;
                雪碧列表 缓存元素;
                if (使用缓存)
                {
                    string 分辨率 = $"{Math.Round(尺寸.X, 2)}x{Math.Round(尺寸.Y, 2)}"; ;
                    if (!图形缓存.ContainsKey(种类))
                        图形缓存.Add(种类, new Dictionary<string, 雪碧列表>());
                    var 当前绘图种类 = 图形缓存[种类];
                    if (!当前绘图种类.ContainsKey(分辨率))
                        当前绘图种类.Add(分辨率, 元素);
                    缓存元素 = 当前绘图种类[分辨率];
                }
                else { 缓存元素 = 元素; 图形缓存.Clear(); }
                if (缓存元素 == null || 缓存元素.Count < 1) return;
                using (var 画帧 = LCD面板.DrawFrame()) { 画帧.AddRange(缓存元素); }
            }
            public void 清空缓存() => 图形缓存.Clear();
            public bool 有缓存(string 种类)
            {
                string 分辨率 = $"{Math.Round(尺寸.X, 2)}x{Math.Round(尺寸.Y, 2)}";
                if (!使用缓存) return false;
                if (!图形缓存.ContainsKey(种类))
                    return false;
                var 当前绘图种类 = 图形缓存[种类];
                if (!当前绘图种类.ContainsKey(分辨率)) return false; return true;
            }
            public static Vector2 得到小框(Vector2 尺寸) { var 值 = Math.Min(尺寸.X, 尺寸.Y); return new Vector2(值, 值); }
            readonly Dictionary<string, Dictionary<string, 雪碧列表>> 图形缓存 = new Dictionary<string, Dictionary<string, 雪碧列表>>();
            public const float 标准屏幕像素 = 512;
            public static readonly Vector2 标准屏幕尺寸 = new Vector2(标准屏幕像素, 标准屏幕像素);
            public bool 分辨率检测(Func<float, bool> 分辨率筛选 = null)
            {
                if (分辨率筛选 == null) { 分辨率筛选 = (float 尺寸比例) => { var 值 = Math.Round(尺寸比例, 1); return 值 != 2 && 值 != 1 && 值 != R53 && 值 != RS2; }; }
                float 比例 = 尺寸.X / 尺寸.Y; 雪碧列表 元素 = new 雪碧列表();
                if (LCD面板.ContentType != ContentType.SCRIPT)
                {
                    LCD面板.ContentType = ContentType.SCRIPT;
                    LCD面板.Script = "";
                    LCD面板.ScriptBackgroundColor = 背景;
                    LCD面板.ScriptForegroundColor = new Color(0, 140, 240);
                }
                if (分辨率筛选?.Invoke(比例) ?? false)
                {
                    if (有缓存("非法分辨率"))
                    {
                        更新LCD(new 雪碧列表(), "非法分辨率");
                    }
                    else
                    {
                        var 最小尺寸 = Math.Min(尺寸.X, 尺寸.Y);
                        //绘图元素.Add(绘图("Cross", 向量(10 + 最小尺寸 * 0.5f, 屏幕中心.Y), 向量(最小尺寸, 最小尺寸)));
                        var 文本大小 = 最小尺寸 / 参考尺寸;
                        元素.文本($"分辨率错误: {Math.Round(尺寸.X)} : {Math.Round(尺寸.Y)}, {Math.Round(尺寸.X / 尺寸.Y, 1)}", 中心, Color.OrangeRed, "White", TextAlignment.CENTER, 文本大小);
                        更新LCD(元素, "非法分辨率");
                        LCD面板.WriteText($"分辨率错误: {尺寸.X} : {尺寸.Y}");
                    }
                    return false;
                }
                return true;
            }
            public static void 重置LCD背景颜色_默认(MyLCD参数 LCD面板)
            {
                LCD面板.背景颜色 = 背景;
            }



        }

    }
}
