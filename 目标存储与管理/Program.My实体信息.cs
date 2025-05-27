using Sandbox.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class My实体信息
        {
            public long 实体编号;
            public string 名称;
            public ushort 类型;
            public bool 有效的 { get; private set; }
            public MatrixD 世界矩阵 { get; private set; }
            public BoundingSphereD 体积 { get; private set; }
            public Vector3D 位置 { get; private set; }
            public Vector3D? 扫描点 { get; private set; }
            public Vector3 线速度 { get; private set; }
            public long 时间戳 { get; private set; }
            public ushort 所属关系 { get; private set; }
            public long 更新步;
            public static MyTuple<long, string, ushort, ushort, long, MyTuple<MatrixD, Vector3D, Vector3, BoundingSphereD>> 得到_自己实体_序列化信息(IMyCubeGrid 自己网格, long? 刻) => new MyTuple<long, string, ushort, ushort, long, MyTuple<MatrixD, Vector3D, Vector3, BoundingSphereD>>(自己网格.EntityId, 自己网格.CustomName, (ushort)(自己网格.GridSizeEnum == MyCubeSize.Large ? 3 : 2), 1, 刻 ?? 0, new MyTuple<MatrixD, Vector3D, Vector3, BoundingSphereD>(自己网格.WorldMatrix, 自己网格.WorldVolume.Center, 自己网格.LinearVelocity, 自己网格.WorldVolume));
            public My实体信息(IMyCubeGrid 自己, long? 刻)
            {
                实体编号 = 自己.EntityId;
                名称 = 自己.CustomName;
                类型 = 自己.GridSizeEnum == MyCubeSize.Large ? ((ushort)3) : ((ushort)2);
                有效的 = true;
                世界矩阵 = 自己.WorldMatrix;
                体积 = 自己.WorldVolume;
                位置 = 体积.Center;
                线速度 = 自己.LinearVelocity;
                时间戳 = 刻 ?? 0;
                所属关系 = 1;
                扫描点 = null;
            }
            public My实体信息(IMyEntity 实体, long? 刻)
            {
                实体编号 = 实体.EntityId;
                if (实体 is IMyCubeGrid)
                {
                    IMyCubeGrid 当前实体 = 实体 as IMyCubeGrid;
                    名称 = 当前实体.CustomName;
                    类型 = 当前实体.GridSizeEnum == MyCubeSize.Large ? ((ushort)3) : ((ushort)2);
                    线速度 = 当前实体.LinearVelocity;
                }
                else
                {
                    名称 = 实体.Name;
                    类型 = 0;
                    线速度 = Vector3.Zero;
                }
                有效的 = true;
                世界矩阵 = 实体.WorldMatrix;
                体积 = 实体.WorldVolume;
                位置 = 体积.Center;               
                时间戳 = 刻 ?? 0;
                所属关系 = 1;
                扫描点 = null;
            }
            public My实体信息(MyDetectedEntityInfo 信息, long? 刻)
            {
                实体编号 = 信息.EntityId;
                名称 = 信息.Name;
                类型 = (ushort)信息.Type;
                更新(信息, 刻);
            }
            public My实体信息(MyTuple<long, string, ushort, ushort, long, MyTuple<MatrixD, Vector3D, Vector3, BoundingSphereD>> Info)
            {
                实体编号 = Info.Item1;
                名称 = Info.Item2;
                类型 = Info.Item3;
                更新(Info);
            }
            public void 更新(MyDetectedEntityInfo 信息, long? 刻)
            {
                if (信息.EntityId != 实体编号) return;
                有效的 = !信息.IsEmpty();
                if (!有效的) return;
                世界矩阵 = 信息.Orientation;
                位置 = 信息.Position;
                体积 = BoundingSphereD.CreateFromBoundingBox(信息.BoundingBox);
                线速度 = 信息.Velocity;
                时间戳 = 刻 ?? 信息.TimeStamp;
                所属关系 = (ushort)信息.Relationship;
                扫描点 = 信息.HitPosition;
            }
            public void 更新(MyTuple<long, string, ushort, ushort, long, MyTuple<MatrixD, Vector3D, Vector3, BoundingSphereD>> 信息)
            {
                有效的 = true;
                所属关系 = 信息.Item4;
                时间戳 = 信息.Item5;
                世界矩阵 = 信息.Item6.Item1;
                位置 = 信息.Item6.Item2;
                体积 = 信息.Item6.Item4;
                线速度 = 信息.Item6.Item3;
            }
            public void 更新(My实体信息 实体信息)
            {
                if (实体编号 != 实体信息.实体编号) return;
                有效的 = 实体信息.有效的;
                世界矩阵 = 实体信息.世界矩阵;
                体积 = 实体信息.体积;
                位置 = 实体信息.位置;
                扫描点 = 实体信息.扫描点;
                线速度 = 实体信息.线速度;
                时间戳 = 实体信息.时间戳;
                所属关系 = 实体信息.所属关系;
                更新步 = 实体信息.更新步;
            }
            public static MyTuple<long, string, ushort, ushort, long, MyTuple<MatrixD, Vector3D, Vector3, BoundingSphereD>> 序列化(My实体信息 信息) => new MyTuple<long, string, ushort, ushort, long, MyTuple<MatrixD, Vector3D, Vector3, BoundingSphereD>>(信息.实体编号, 信息.名称, 信息.类型, 信息.所属关系, 信息.时间戳, new MyTuple<MatrixD, Vector3D, Vector3, BoundingSphereD>(信息.世界矩阵, 信息.扫描点 ?? 信息.位置, 信息.线速度, 信息.体积));
            public static My实体信息 反序列化(MyTuple<long, string, ushort, ushort, long, MyTuple<MatrixD, Vector3D, Vector3, BoundingSphereD>> 信息) => new My实体信息(信息);
            public static MyDetectedEntityType 转为实体类型(ushort 类型) => (MyDetectedEntityType)类型;
            public static MyRelationsBetweenPlayerAndBlock 转为与传感器所有者的关系(ushort 关系) => (MyRelationsBetweenPlayerAndBlock)关系;
            public static MyTuple<byte, long, Vector3D, double> 转为Whip141的雷达显示信息(My实体信息 信息)
            {
                My目标类型和关系 关系 = 0;
                switch (信息.所属关系)
                {
                    case 1:
                    case 2:
                    case 5:
                        关系 |= My目标类型和关系.朋友; break;
                    case 4: 关系 |= My目标类型和关系.敌人; break;
                    default: 关系 |= My目标类型和关系.中立; break;
                }
                if (信息.类型 == 3) 关系 |= My目标类型和关系.大网格;
                else if (信息.类型 == 2) 关系 |= My目标类型和关系.小网格;
                else if (信息.类型 == 8 || 信息.类型 == 9) 关系 |= My目标类型和关系.小行星;
                else { 关系 |= My目标类型和关系.其他; }
                return new MyTuple<byte, long, Vector3D, double>((byte)关系, 信息.实体编号, 信息.位置, 0);
            }

        }
    }
}
