using Sandbox.ModAPI.Ingame;
using VRageMath;


namespace IngameScript
{
    public partial class Program
    {
        public class My目标方位参数
        {
            public readonly My实体信息 目标;
            public Base6Directions.Direction 方向;
            public double 点积;
            public double 距离;
            public bool 是否有效;

            public My目标方位参数(IMyTerminalBlock 参考, My实体信息 目标)
            {
                this.目标 = 目标;
                更新(参考);
            }
            public void 更新(IMyTerminalBlock 参考)
            {
                Vector3D 方向向量 = 目标.位置 - 参考.WorldVolume.Center;
                是否有效 = !Vector3D.IsZero(方向向量);
                if (是否有效)
                {
                    距离 = 方向向量.Normalize();
                    方向 = 参考.WorldMatrix.GetClosestDirection(方向向量);
                    点积 = 方向向量.Dot(参考.WorldMatrix.GetDirectionVector(方向));
                }
                else
                {
                    距离 = default(double);
                    方向 = default(Base6Directions.Direction);
                    点积 = default(double);
                }
            }
            public static My目标方位参数 创建(IMyTerminalBlock 参考, My实体信息 目标) => new My目标方位参数(参考, 目标);
        }









    }
}
