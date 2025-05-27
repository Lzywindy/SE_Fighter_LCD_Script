using Sandbox.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class My其他物理量
        {
            public Vector3 线速度 { get; private set; }
            public Vector3 前_线速度 { get; private set; }
            public Vector3 线速度变化量 { get; private set; }
            public Vector3 角速度 { get; private set; }
            public Vector3 前_角速度 { get; private set; }
            public Vector3 角速度变化量 { get; private set; }
            public float 飞船质量 { get; private set; }
            public float 飞船速度 { get; private set; }


            public void 更新(IMyShipController 控制器)
            {
                if (实体不存在(控制器)) { 清空(); return; }
                var _速度 = 控制器.GetShipVelocities();
                前_角速度 = 角速度;
                角速度 = _速度.AngularVelocity;
                前_线速度 = 线速度;
                线速度 = _速度.LinearVelocity;
                线速度变化量 = 线速度 - 前_线速度;
                角速度变化量 = 角速度 - 前_角速度;
                飞船速度 = (float)控制器.GetShipSpeed();
                飞船质量 = 控制器.CalculateShipMass().PhysicalMass;
            }
            public void 清空()
            {
                线速度变化量 = 角速度变化量 = 前_角速度 = 角速度 = 前_线速度 = 线速度 = Vector3.Zero;
                飞船质量 = 飞船速度 = 0;
            }
        }

    }
}
