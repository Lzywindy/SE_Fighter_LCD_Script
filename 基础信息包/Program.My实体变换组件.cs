using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class My实体变换组件
        {
            public Vector3D 位置 => 大小.Center;
            public MatrixD 矩阵 { get; private set; }
            public MatrixD 逆矩阵 { get; private set; }
            public BoundingSphereD 大小 { get; private set; }
            public void 更新(IMyEntity 实体) { if ( 实体不存在(实体)) { 清空(); return; } 矩阵 = 实体.WorldMatrix; 逆矩阵 = MatrixD.Invert(矩阵); 大小 = 实体.WorldVolume; }
            public void 更新(MyDetectedEntityInfo 实体) { if (实体.IsEmpty()) { 清空(); return; } 矩阵 = 实体.Orientation; 逆矩阵 = MatrixD.Invert(矩阵); 大小 = BoundingSphereD.CreateFromBoundingBox(实体. BoundingBox); }
            public void 清空() { 矩阵 = 逆矩阵 = MatrixD.Identity; 大小 = default(BoundingSphereD); }
        }

    }
}
