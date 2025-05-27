using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    public partial class Program : MyGridProgram
    {
        // This file contains your actual script.
        //
        // You can either keep all your code here, or you can create separate
        // code files to make your program easier to navigate while coding.
        //
        // Go to:
        // https://github.com/malware-dev/MDK-SE/wiki/Quick-Introduction-to-Space-Engineers-Ingame-Scripts
        //
        // to learn more about ingame scripts.
        //using 雪碧列表= 雪碧列表;
        const string
            雷达屏ID = "雷达屏",
            雷达传感器ID = "雷达",
            地面感知ID = "地面探测",
            字体 = "White";
        static readonly float
            线宽 = 1,
            雷达视场角度 = 14,
            锁定框比例 = 0.8f,
            雷达视场角度_弧度 = MathHelper.ToRadians(雷达视场角度),
            锁定视场 = 雷达视场角度_弧度 * 锁定框比例,
            立体雷达视角 = MathHelper.ToRadians(60);

        static readonly Color
            敌人 = new Color(237, 79, 36),
            友军 = new Color(169, 209, 142),
            自己 = new Color(68, 160, 255),
            未知 = new Color(255, 217, 102),
            水平仪上部分 = new Color(19, 69, 139),
            水平仪下部分 = new Color(139, 69, 19);
        static readonly Vector2 立体雷达视角_值 = 向量_从角度(立体雷达视角);
        public readonly My显示参数 显示参数 = new My显示参数()
        {
            显示范围 = 10000f,
            显示逻辑 = "自适应",
            锁定范围 = 10000f,
            锁定逻辑 = "自动",
            当前武器 = "LAAM",
            武器逻辑 = "直瞄",
            武器1名称 = "磁轨炮",
            武器1参数_百分比 = 1f,
            武器2名称 = "激光",
            武器2参数_百分比 = 0.6f,
            武器3名称 = "火箭弹",
            武器3参数_百分比 = 0.8f,
            武器4名称 = "机炮",
            武器4参数_百分比 = 0.4f,
            武器5类型 = "导弹",
            武器5数量 = 22,
            护盾储能百分比 = 1f,
            电力储能百分比 = 0.75f,
            曲速储能百分比 = 1f,
            氢气储量百分比 = 0.89f,
        };
        public Program()
        {
            // The constructor, called only once every session and
            // always before any other method is called. Use it to
            // initialize your script. 
            //     
            // The constructor is optional and can be removed if not
            // needed.
            // 
            // It's recommended to set Runtime.UpdateFrequency 
            // here, which will allow your script to run itself without a 
            // timer block.
            工作.Add(() => { });
            工作.Add(() => { });
            工作.Add(() => { });
            工作.Add(() => { });
            工作.Add(() => { });
            工作.Add(() => { });
            工作.Add(() => { });
            工作.Add(() => { });
            工作.Add(() => { });
            工作.Add(() => { });

            显示参数.目标信息汇总.清空更新步();
            Runtime.UpdateFrequency = UpdateFrequency.Update1 | UpdateFrequency.Update10 | UpdateFrequency.Update100 | UpdateFrequency.Once;
        }

        public void Save()
        {
            // Called when the program needs to save its state. Use
            // this method to save your state to the Storage field
            // or some other means. 
            // 
            // This method is optional and can be removed if not
            // needed.
        }

        public void Main(string argument, UpdateType updateSource)
        {
            // The main entry point of the script, invoked every time
            // one of the programmable block's Run actions are invoked,
            // or the script updates itself. The updateSource argument
            // describes where the update came from. Be aware that the
            // updateSource is a  bitfield  and might contain more than 
            // one update type.
            // 
            // The method itself is required, but the arguments above
            // can be removed if not needed.
            long 时刻 = DateTime.Now.Ticks;
            if (updateSource.HasFlag(UpdateType.Update100) || updateSource.HasFlag(UpdateType.Once))
            {
                //更新状态 = true;
                //所有LCD设备.Clear();
                显示参数.清理();
                List<IMyTerminalBlock> LCD显示器设备 = new List<IMyTerminalBlock>();
                GridTerminalSystem.GetBlockGroupWithName(雷达屏ID)?.GetBlocksOfType(LCD显示器设备, b => b != null && !b.Closed && b.IsSameConstructAs(Me) && (b is IMyTextSurfaceProvider || b is IMyTextPanel));
                foreach (var kv in 表面列表)
                    kv.Value.Clear();
                foreach (var LCD in LCD显示器设备)
                    分配LCD资源(表面列表, LCD);
                飞行控制器.获取方块(GridTerminalSystem, b => b != null && !b.Closed && b.IsSameConstructAs(Me));
                if (飞行控制器.Count > 0)
                    物理包.更新(飞行控制器[0]);
                传感器阵列.获取方块(GridTerminalSystem.GetBlockGroupWithName(雷达传感器ID), b => b != null && !b.Closed && b.IsSameConstructAs(Me));
                炮塔感知阵列.获取方块(GridTerminalSystem.GetBlockGroupWithName(雷达传感器ID), b => b != null && !b.Closed && b.IsSameConstructAs(Me));
                炮塔块感知阵列.获取方块(GridTerminalSystem.GetBlockGroupWithName(雷达传感器ID), b => b != null && !b.Closed && b.IsSameConstructAs(Me));
            }
            if (updateSource.HasFlag(UpdateType.Update1))
            {
                更新步_目标++;
                HashSet<long> 当前锁定的东西 = new HashSet<long>();

                //工作[计数]?.Invoke();
                //计数 = (计数 + 1) % 10;
            }
            if (updateSource.HasFlag(UpdateType.Update10))
            {
                显示参数.目标信息汇总.更新步();
                显示参数.目标信息汇总.锁定列表.Clear();
                显示参数.目标信息汇总.获取实体(传感器阵列);
                显示参数.目标信息汇总.获取实体(炮塔感知阵列);
                显示参数.目标信息汇总.获取实体(炮塔块感知阵列);
                //工作[计数]?.Invoke();
                //计数 = (计数 + 1) % 10;
                显示参数.显示范围 = Math.Max(显示参数.显示范围, 1000);
                敌对目标_ID_角度.Clear();
                显示参数.目标信息汇总.雷达目标_人物.得到目标夹角信息(敌对目标_ID_角度, 物理包);
                显示参数.目标信息汇总.雷达目标_小网格.得到目标夹角信息(敌对目标_ID_角度, 物理包);
                显示参数.目标信息汇总.雷达目标_大网格.得到目标夹角信息(敌对目标_ID_角度, 物理包);
                if (敌对目标_ID_角度.Count > 2)
                    敌对目标_ID_角度.SortNoAlloc((a, b) => Math.Sign(a.夹角 - b.夹角));
                foreach (var 敌对目标 in 敌对目标_ID_角度)
                    显示参数.目标信息汇总.锁定列表.Add(敌对目标.目标ID);
                显示器.清空缓存();
                物理包.清空();
                飞行控制器.RemoveAll(b => b == null || b.Closed);
                if (飞行控制器.Count > 0)
                    物理包.更新(飞行控制器[0]);
                显示参数.计算显示(物理包);
                foreach (var 种类 in 表面列表)
                {
                    var 种类名 = 种类.Key;
                    foreach (var 屏幕 in 种类.Value)
                    {
                        屏幕.背景颜色 = 背景;
                        string 额外参数 = 屏幕.额外参数;
                        switch (种类名)
                        {
                            case "战机53驾驶室":
                                LCD显示_战机53驾驶室(显示器, 物理包, 屏幕, 显示参数, 线宽, null);
                                break;
                            case "战机92驾驶室":
                                LCD显示_战机92驾驶室(显示器, 物理包, 屏幕, 显示参数, 线宽);
                                break;
                            case "战机正视HUD":
                                LCD布局_53正面HUD(显示器, (图, 绘图框, 视区_左, 视区_右, 隔) =>
                                {
                                    float 当前线宽 = Math.Max(线宽 * 绘图框.最大边长 / 两倍参考尺寸, 1), 附加附件尺寸 = 绘图框.最小边长 * 0.25f;
                                    绘制.雷达正视(图, 物理包, 边界框.创建_中心_尺寸(绘图框.中心, 向量(绘图框.最小边长)), 当前线宽, 显示器.前景色, 背景, 显示参数, 额外参数.Contains("线框"), 额外参数.Contains("裁剪"), 额外参数.Contains("战机座舱"));
                                    if (视区_左 != 边界框.默认)
                                        绘制.雷达平面(图, 视区_左, 当前线宽, 显示器.前景色, 背景, 显示参数, false, true);
                                    if (视区_右 != 边界框.默认)
                                        绘制.雷达告警(图, 显示参数, 视区_右, 显示器.前景色, 背景, 当前线宽, false, true);
                                    //绘制.雷达平面(图, 视区_右, 当前线宽, 显示器.前景色, 背景, 显示参数, true, true);
                                    //if (额外参数.Contains("水平仪") && !额外参数.Contains("战机座舱"))
                                    //    绘制.地平线(图, 物理包, 边界框.创建(绘图框.X0, 绘图框.X0 + 附加附件尺寸, 绘图框.YM - 附加附件尺寸, 绘图框.YM), 当前线宽, 显示器.前景色, 背景, true, true);
                                    //if (额外参数.Contains("平面雷达") && !额外参数.Contains("战机座舱"))
                                    //    绘制.雷达平面(图, 边界框.创建(绘图框.XM - 附加附件尺寸, 绘图框.XM, 绘图框.YM - 附加附件尺寸, 绘图框.YM), 当前线宽, 显示器.前景色, 背景, 显示参数, true, true);
                                    //if (额外参数.Contains("3D雷达") && !额外参数.Contains("战机座舱"))
                                    //    绘制.雷达3D平面(图, 边界框.创建(绘图框.XM - 附加附件尺寸, 绘图框.XM, 绘图框.Y0, 绘图框.Y0 + 附加附件尺寸), 当前线宽, 显示器.前景色, 背景, 显示参数, true, true, 额外参数.Contains("ED"), false);
                                }, 种类名, 屏幕, 0.975f, (分辨率) => { return !(分辨率 >= (3f / 5f) && 分辨率 <= (5f / 3f)); });
                                break;
                            case "战机平面雷达":
                                LCD布局1(显示器, (图, 绘图框, 隔) =>
                                {
                                    绘制.雷达平面(图, 边界框.创建_中心_尺寸(绘图框.中心, 向量(绘图框.最小边长)), Math.Max(线宽 * 绘图框.最大边长 / 两倍参考尺寸, 1), 显示器.前景色, 背景, 显示参数, 额外参数.Contains("线框"), 额外参数.Contains("裁剪"));
                                }, 种类名, 屏幕, 0.975f, (分辨率) => { return !(分辨率 >= (3f / 5f) && 分辨率 <= (5f / 3f)); });
                                break;
                            case "战机水平仪":
                                LCD布局1(显示器, (图, 绘图框, 隔) =>
                                {
                                    绘制.地平线(图, 物理包, 边界框.创建_中心_尺寸(绘图框.中心, 向量(绘图框.最小边长)), Math.Max(线宽 * 绘图框.最大边长 / 两倍参考尺寸, 1), 显示器.前景色, 背景, 额外参数.Contains("线框"), 额外参数.Contains("裁剪"));
                                }, 种类名, 屏幕, 0.975f, (分辨率) => { return !(分辨率 >= (3f / 5f) && 分辨率 <= (5f / 3f)); });
                                break;
                            case "绘制雷达3D":
                                LCD布局1(显示器, (图, 绘图框, 隔) =>
                                {
                                    float 当前线宽 = Math.Max(线宽 * 绘图框.最大边长 / 两倍参考尺寸, 1), 附加附件尺寸 = 绘图框.最小边长 * 0.25f;
                                    绘制.雷达3D平面(图, 边界框.创建_中心_尺寸(绘图框.中心, 向量(绘图框.最小边长)), Math.Max(线宽 * 绘图框.最大边长 / 两倍参考尺寸, 1), 显示器.前景色, 背景, 显示参数, 额外参数.Contains("线框"), 额外参数.Contains("裁剪"), 额外参数.Contains("ED"), 额外参数.Contains("战机座舱"));
                                    if (额外参数.Contains("水平仪") && !额外参数.Contains("战机座舱"))
                                        绘制.地平线(图, 物理包, 边界框.创建(绘图框.X0, 绘图框.X0 + 附加附件尺寸, 绘图框.YM - 附加附件尺寸, 绘图框.YM), 当前线宽, 显示器.前景色, 背景, true, true);
                                    if (额外参数.Contains("平面雷达") && !额外参数.Contains("战机座舱"))
                                        绘制.雷达平面(图, 边界框.创建(绘图框.XM - 附加附件尺寸, 绘图框.XM, 绘图框.YM - 附加附件尺寸, 绘图框.YM), 当前线宽, 显示器.前景色, 背景, 显示参数, true, true);
                                    if (额外参数.Contains("正视HUD") && !额外参数.Contains("战机座舱"))
                                        绘制.雷达正视(图, 物理包, 边界框.创建(绘图框.XM - 附加附件尺寸, 绘图框.XM, 绘图框.Y0, 绘图框.Y0 + 附加附件尺寸), 当前线宽, 显示器.前景色, 背景, 显示参数, 额外参数.Contains("线框"), 额外参数.Contains("裁剪"), 额外参数.Contains("战机座舱"));
                                }, 种类名, 屏幕, 0.975f, (分辨率) => { return !(分辨率 >= (3f / 5f) && 分辨率 <= (5f / 3f)); });
                                break;
                            case "雷达告警":
                                LCD布局1(显示器, (图, 绘图框, 隔) =>
                                {
                                    绘制.雷达告警(图, 显示参数, 边界框.创建_中心_尺寸(绘图框.中心, 向量(绘图框.最小边长)), 显示器.前景色, 背景, Math.Max(线宽 * 绘图框.最大边长 / 两倍参考尺寸, 1), 额外参数.Contains("线框"), 额外参数.Contains("裁剪"));
                                }, 种类名, 屏幕, 0.975f, (分辨率) => { return !(分辨率 >= (3f / 5f) && 分辨率 <= (5f / 3f)); });
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        int count = 0;
        public long 更新步_目标 = 0;
        readonly LCD显示器 显示器 = new LCD显示器();
        readonly My物理包 物理包 = new My物理包();
        readonly My控制器 飞行控制器 = new My控制器();
        readonly My传感器阵列 传感器阵列 = new My传感器阵列(); // 搜索用
        readonly My炮塔阵列 炮塔感知阵列 = new My炮塔阵列(); // 锁定用
        readonly My炮塔块阵列 炮塔块感知阵列 = new My炮塔块阵列(); // 锁定用

        readonly List<My目标夹角> 敌对目标_ID_角度 = new List<My目标夹角>();

      

        public static void LCD显示_战机53驾驶室(LCD显示器 显示器, My物理包 物理包, MyLCD参数 LCD设备, My显示参数 数据, float 线宽 = 1, Action<string> Debug = null) =>
            LCD布局1(显示器,
                (图, 视区, 隔) =>
                {

                    Color 基色 = LCD设备.前景颜色;//自己;
                    float 隔半 = 隔 * 0.5f, 隔四 = 隔半 * 0.5f, 系数 = (2 * 视区.a + 视区.b - 4 * 隔半) / (7 * 视区.a);
                    线宽 = Math.Max(线宽 * 视区.最大边长 / 参考尺寸 * 0.5f, 1);
                    全信息边界分区 分区 = default(全信息边界分区);
                    分区.框_正视 = 边界框.创建_中心_尺寸(视区.中心, 向量(视区.最大边长 * 系数));
                    分区.框_水平仪 = 边界框.创建(视区.X0, 分区.框_正视.X0 - 隔半, 分区.框_正视.Y0, 分区.框_正视.Y0 + (分区.框_正视.X0 - 隔半 - 视区.X0));
                    分区.框_平面 = 边界框.创建(分区.框_正视.XM + 隔半, 视区.XM, 分区.框_正视.Y0, 分区.框_水平仪.YM);
                    float 长度分三实际 = 分区.框_水平仪.尺寸.X, 速度长比例 = 0.45f, 速度长 = 长度分三实际 * 速度长比例, 其他二分 = 长度分三实际 * (1 - 速度长比例) * 0.5f, 高度一半 = (分区.框_正视.Y0 - 视区.Y0 - 隔) * 0.5f;
                    分区.框_速度 = 边界框.创建(视区.X0, 视区.X0 + 速度长 - 隔半, 分区.框_水平仪.YM + 隔半, 分区.框_正视.YM);
                    分区.框_滚转 = 边界框.创建(分区.框_速度.XM + 隔半, (分区.框_速度.XM + 分区.框_水平仪.XM) * 0.5f, 分区.框_速度.Y0, 分区.框_速度.YM);
                    分区.框_俯仰 = 边界框.创建((分区.框_速度.XM + 分区.框_水平仪.XM) * 0.5f + 隔半, 分区.框_水平仪.XM, 分区.框_速度.Y0, 分区.框_速度.YM);
                    分区.框_水平范围 = 边界框.创建(分区.框_平面.X0, 分区.框_平面.X0 + 分区.框_平面.半尺寸.X - 隔四, 分区.框_平面.YM + 隔半, 分区.框_正视.YM);
                    分区.框_水平模式 = 边界框.创建(分区.框_水平范围.XM + 隔半, 分区.框_平面.XM, 分区.框_平面.YM + 隔半, 分区.框_正视.YM);
                    分区.框_护盾 = 边界框.创建(分区.框_水平仪.X0, 分区.框_水平仪.XM, 分区.框_正视.YM + 隔半, 分区.框_正视.YM + 隔半 + 高度一半);
                    分区.框_曲速 = 边界框.创建(分区.框_水平仪.X0, 分区.框_水平仪.XM, 分区.框_护盾.YM + 隔半, 视区.YM);
                    分区.框_电能 = 边界框.创建(分区.框_正视.X0, 分区.框_正视.XM, 分区.框_正视.YM + 隔半, 分区.框_正视.YM + 隔半 + 高度一半);
                    分区.框_燃料 = 边界框.创建(分区.框_正视.X0, 分区.框_正视.XM, 分区.框_电能.YM + 隔半, 视区.YM);
                    分区.框_锁定参数 = 边界框.创建(分区.框_平面.X0, 分区.框_平面.X0 + 分区.框_平面.半尺寸.X - 隔四, 分区.框_电能.Y0, 分区.框_电能.YM);
                    分区.框_锁定模式 = 边界框.创建(分区.框_水平范围.XM + 隔半, 分区.框_平面.XM, 分区.框_电能.Y0, 分区.框_电能.YM);
                    分区.框_武器类型 = 边界框.创建(分区.框_平面.X0, 分区.框_平面.X0 + 分区.框_平面.半尺寸.X - 隔四, 分区.框_燃料.Y0, 分区.框_燃料.YM);
                    分区.框_武器模式 = 边界框.创建(分区.框_水平范围.XM + 隔半, 分区.框_平面.XM, 分区.框_燃料.Y0, 分区.框_燃料.YM);
                    分区.框_武器1 = 边界框.创建(分区.框_正视.X0, 分区.框_正视.XM, 视区.Y0, 视区.Y0 + 高度一半);
                    分区.框_武器2 = 边界框.创建(分区.框_正视.X0, 分区.框_正视.XM, 分区.框_武器1.YM + 隔半, 分区.框_正视.Y0 - 隔半);
                    分区.框_武器3 = 边界框.创建(分区.框_平面.X0, 分区.框_平面.XM, 分区.框_武器1.Y0, 分区.框_武器1.YM);
                    分区.框_武器4 = 边界框.创建(分区.框_平面.X0, 分区.框_平面.XM, 分区.框_武器2.Y0, 分区.框_武器2.YM);
                    分区.框_武器5 = 边界框.创建(分区.框_水平仪.X0, 分区.框_水平仪.XM, 分区.框_武器1.Y0, 分区.框_武器2.YM);
                    绘制.全信息屏幕(图, 数据, 物理包, 基色, 线宽, ref 分区, 视区.最大边长 / 视区.最小边长, Debug);
                },
                "战机53驾驶室", LCD设备, 0.975f,
                (分辨率) =>
                {
                    var 值 = Math.Round(分辨率, 2); return !(值 > 1.05f && 值 <= 2);
                });
        public static void LCD显示_战机92驾驶室(LCD显示器 显示器, My物理包 物理包, MyLCD参数 LCD设备, My显示参数 数据, float 线宽 = 1) =>
           LCD布局1(显示器,
               (图, 视区, 隔) =>
               {
                   Color 基色 = LCD设备.前景颜色;// 自己;
                   float 隔半 = 隔 * 0.5f, 隔四 = 隔半 * 0.5f;
                   线宽 = Math.Max(线宽 * 视区.最大边长 / 参考尺寸 * 0.5f, 1);
                   全信息边界分区 分区 = default(全信息边界分区);
                   分区.框_正视 = 边界框.创建_中心_尺寸(视区.中心, 向量(视区.最小边长));
                   分区.框_水平仪 = 边界框.创建(分区.框_正视.X0 - 隔半 - 视区.最小边长, 分区.框_正视.X0 - 隔半, 分区.框_正视.Y0, 分区.框_正视.YM);
                   分区.框_平面 = 边界框.创建(分区.框_正视.XM + 隔半, 分区.框_正视.XM + 隔半 + 视区.最小边长, 分区.框_正视.Y0, 分区.框_正视.YM);
                   int 行数 = 8;
                   float 剩余长度_半 = (分区.框_水平仪.X0 - 隔半 - 视区.X0), 高度分割 = (视区.最小边长 - (行数 - 1) * 隔半) / 行数, 末偏移 = 隔半 + 高度分割, 左头 = 视区.X0, 左尾 = 分区.框_水平仪.X0 - 隔半, 右头 = 分区.框_平面.XM + 隔半, 右尾 = 视区.XM;
                   分区.框_速度 = 边界框.创建(左头, 左尾, 视区.Y0, 视区.Y0 + 高度分割);
                   分区.框_滚转 = 边界框.创建(左头, 左头 + (剩余长度_半 - 隔半) * 0.5f, 分区.框_速度.YM + 隔半, 分区.框_速度.YM + 末偏移);
                   分区.框_俯仰 = 边界框.创建(左头 + (剩余长度_半 + 隔半) * 0.5f, 左尾, 分区.框_速度.YM + 隔半, 分区.框_速度.YM + 末偏移);
                   分区.框_护盾 = 边界框.创建(左头, 左尾, 分区.框_俯仰.YM + 隔半, 分区.框_俯仰.YM + 末偏移);
                   分区.框_曲速 = 边界框.创建(左头, 左尾, 分区.框_护盾.YM + 隔半, 分区.框_护盾.YM + 末偏移);
                   分区.框_电能 = 边界框.创建(左头, 左尾, 分区.框_曲速.YM + 隔半, 分区.框_曲速.YM + 末偏移);
                   分区.框_燃料 = 边界框.创建(左头, 左尾, 分区.框_电能.YM + 隔半, 分区.框_电能.YM + 末偏移);
                   分区.框_武器1 = 边界框.创建(左头, 左尾, 分区.框_燃料.YM + 隔半, 分区.框_燃料.YM + 末偏移);
                   分区.框_武器2 = 边界框.创建(左头, 左尾, 分区.框_武器1.YM + 隔半, 分区.框_武器1.YM + 末偏移);
                   分区.框_水平范围 = 边界框.创建(右头, 右头 + (剩余长度_半 - 隔半) * 0.5f, 视区.Y0, 视区.Y0 + 高度分割);
                   分区.框_水平模式 = 边界框.创建(右头 + (剩余长度_半 + 隔半) * 0.5f + 隔半, 右尾, 视区.Y0, 视区.Y0 + 高度分割);
                   分区.框_锁定参数 = 边界框.创建(右头, 右头 + (剩余长度_半 - 隔半) * 0.5f, 分区.框_水平范围.YM + 隔半, 分区.框_水平范围.YM + 末偏移);
                   分区.框_锁定模式 = 边界框.创建(右头 + (剩余长度_半 + 隔半) * 0.5f + 隔半, 右尾, 分区.框_水平范围.YM + 隔半, 分区.框_水平范围.YM + 末偏移);
                   分区.框_武器类型 = 边界框.创建(右头, 右头 + (剩余长度_半 - 隔半) * 0.5f, 分区.框_锁定参数.YM + 隔半, 分区.框_锁定参数.YM + 末偏移);
                   分区.框_武器模式 = 边界框.创建(右头 + (剩余长度_半 + 隔半) * 0.5f + 隔半, 右尾, 分区.框_锁定参数.YM + 隔半, 分区.框_锁定参数.YM + 末偏移);
                   分区.框_武器3 = 边界框.创建(右头, 右尾, 分区.框_武器类型.YM + 隔半, 分区.框_武器类型.YM + 末偏移);
                   分区.框_武器4 = 边界框.创建(右头, 右尾, 分区.框_武器3.YM + 隔半, 分区.框_武器3.YM + 末偏移);
                   分区.框_武器5 = 边界框.创建(右头, 右尾, 分区.框_武器4.YM + 隔半, 视区.YM);
                   float 长度一四 = (分区.框_水平仪.尺寸.X - 隔) * 0.25f, 高度一半 = (分区.框_正视.Y0 - 视区.Y0 - 隔) * 0.5f;
                   绘制.全信息屏幕(图, 数据, 物理包, 基色, 线宽, ref 分区, 9.0f / 2.0f, null);

               },
                "战机92驾驶室", LCD设备, 0.975f,
                (分辨率) =>
                {
                    var 值 = Math.Round(分辨率, 1); return 值 != 4.5f;
                });



        public static void LCD布局1(LCD显示器 显示器, Action<雪碧列表, 边界框, float> 绘图区, string 标题, MyLCD参数 LCD设备, float 可用区 = 0.975f, Func<float, bool> 分辨率筛选 = null)
        {
            显示器.更新(LCD设备);
            if (!显示器.分辨率检测(分辨率筛选)) return;
            雪碧列表 图 = new 雪碧列表();
            if (!显示器.有缓存(标题))
            {
                float 隔 = 显示器.可视区.最小边长 * (1 - 可用区);
                边界框 视区 = 边界框.缩进(显示器.可视区, 隔, 2); ;
                绘图区?.Invoke(图, 视区, 隔);
            }
            显示器.更新LCD(图, 标题);
        }
        public static void LCD布局_53正面HUD(LCD显示器 显示器, Action<雪碧列表, 边界框, 边界框, 边界框, float> 绘图区, string 标题, MyLCD参数 LCD设备, float 可用区 = 0.975f, Func<float, bool> 分辨率筛选 = null)
        {
            显示器.更新(LCD设备);
            if (!显示器.分辨率检测(分辨率筛选)) return;
            雪碧列表 图 = new 雪碧列表();
            if (!显示器.有缓存(标题))
            {
                float 隔 = 显示器.可视区.最小边长 * (1 - 可用区), 尺寸比 = 0.15f;
                边界框 视区 = 边界框.缩进(边界框.创建_方形(显示器.可视区, false), 隔, 2), 视区_左 = default(边界框), 视区_右 = default(边界框);
                if (显示器.比例 == 5f / 3f)
                {
                    视区_左 = 边界框.创建_中心_半尺寸(视区.中心 + 向量(-视区.a * 绘制.雷达显示占比 * 0.4f - 视区.b * 尺寸比 * 0.7f, 视区.b * 尺寸比 * 1.3f), 向量(视区.b * 尺寸比));
                    视区_右 = 边界框.创建_中心_半尺寸(视区.中心 + 向量(视区.a * 绘制.雷达显示占比 * 0.4f + 视区.b * 尺寸比 * 0.7f, 视区.b * 尺寸比 * 1.3f), 向量(视区.b * 尺寸比));
                }
                绘图区?.Invoke(图, 视区, 视区_左, 视区_右, 隔);

            }
            显示器.更新LCD(图, 标题);
        }
        readonly Dictionary<string, List<MyLCD参数>> 表面列表 = new Dictionary<string, List<MyLCD参数>>() {
            {"战机53驾驶室",new List<MyLCD参数>() },
            {"战机92驾驶室",new List<MyLCD参数>() },
            {"战机正视HUD",new List<MyLCD参数>() },
            {"战机平面雷达",new List<MyLCD参数>() },
            {"战机水平仪",new List<MyLCD参数>() },
            {"绘制雷达3D",new List<MyLCD参数>() },
            {"雷达告警",new List<MyLCD参数>() },
        };
        readonly List<Action> 工作 = new List<Action>();
        int 计数 = 0;
    }
}
