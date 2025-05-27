using Sandbox.ModAPI.Ingame;
using VRage;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class MyLCD参数
        {
            public readonly IMyTextSurface 显示屏;
            public readonly Vector2 缩放;
            public readonly string 额外参数;
            public MyLCD参数(IMyTextSurface 显示屏, Vector2 缩放, string 额外参数) { this.显示屏 = 显示屏; this.缩放 = 缩放; this.额外参数 = 额外参数; }
            public bool 有效的 => 显示屏 != null;
            public Color 背景颜色 { get { if (有效的) { if (显示屏.ContentType != VRage.Game.GUI.TextPanel.ContentType.SCRIPT) return 显示屏.BackgroundColor; else return 显示屏.ScriptBackgroundColor; } else return Color.Black; } set { if (有效的) { 显示屏.BackgroundColor = value; 显示屏.ScriptBackgroundColor = value; } } }
            public Color 前景颜色 { get { if (有效的) { if (显示屏.ContentType != VRage.Game.GUI.TextPanel.ContentType.SCRIPT) return 显示屏.FontColor; else return 显示屏.ScriptForegroundColor; } else return Color.Black; } set { if (有效的) { 显示屏.FontColor = value; 显示屏.ScriptForegroundColor = value; } } }
            public static MyLCD参数 创建(IMyTextSurface 显示屏, Vector2 缩放, string 额外参数) => new MyLCD参数(显示屏, 缩放, 额外参数);
            public static MyLCD参数 创建(MyTuple<IMyTextSurface, Vector2, string> LCD设备) => new MyLCD参数(LCD设备.Item1, LCD设备.Item2, LCD设备.Item3);
        }
    }
}
