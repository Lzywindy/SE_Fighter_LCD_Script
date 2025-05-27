using Sandbox.ModAPI.Ingame;
using System;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class My星球信息组件
        {
            public double? 地面高度 { get; private set; }
            public double? 海拔高度 { get; private set; }
            public Vector3D? 星球位置 { get; private set; }
            public bool 是否有最近的星球 { get; private set; }
            public Vector3 重力 { get; private set; }
            public Vector3 人工重力 { get; private set; }
            public Vector3 混合重力 { get; private set; }
            public Vector3 重力矢量 { get; private set; }
            public float 重力大小 { get; private set; }
            public void 更新(IMyShipController 控制器)
            {
                if (实体不存在(控制器)) { 清空(); return; }
                double distance;
                if (控制器.TryGetPlanetElevation(MyPlanetElevation.Surface, out distance)) { 地面高度 = distance; }
                else { 地面高度 = null; }
                if (控制器.TryGetPlanetElevation(MyPlanetElevation.Sealevel, out distance)) { 海拔高度 = distance; }
                else { 海拔高度 = null; }
                Vector3D p_pos;
                if (控制器.TryGetPlanetPosition(out p_pos)) { 星球位置 = p_pos; } else { 星球位置 = null; }
                是否有最近的星球 = (地面高度 != null || 海拔高度 != null) && 星球位置 != null; 重力 = 控制器.GetNaturalGravity(); 人工重力 = 控制器.GetArtificialGravity(); 混合重力 = 控制器.GetTotalGravity();
            }
            public void 清空()
            {
                是否有最近的星球 = false; 地面高度 = null; 海拔高度 = null; 星球位置 = null;
                重力 = Vector3.Zero;
                人工重力 = Vector3.Zero;
                混合重力 = Vector3.Zero;
            }
        }

    }
}
