using System;
using VRage.Game;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public struct My图标显示参数
        {
            /// <summary>
            /// 实体编号
            /// </summary>
            public readonly long 实体编号;
            /// <summary>
            /// 本地坐标
            /// </summary>
            public readonly Vector3 本地坐标;
            /// <summary>
            /// 正面坐标用的是俯仰角和偏航角（需要依据视场进行缩放）
            /// </summary>
            public readonly Vector2? 正面坐标;
            /// <summary>
            /// 顶视坐标用的是关于所给坐标系的相对位置（需要依据尺度进行缩放）
            /// </summary>
            public readonly Vector2 顶视坐标;
            /// <summary>
            /// 顶视朝向用的是关于所给坐标系的相对朝向
            /// </summary>
            public readonly float 顶视朝向;
            /// <summary>
            /// 1自己、2敌人、3友军、0未知
            /// </summary>
            public readonly int 派系;

            public My图标显示参数(My实体信息 实体信息, MatrixD 变换矩阵_逆)
            {
                实体编号 = 实体信息.实体编号;
                本地坐标 = Vector3D.Transform(实体信息.位置, 变换矩阵_逆);
                Vector3 方向 = Vector3D.TransformNormal(实体信息.世界矩阵.Forward, 变换矩阵_逆);
                顶视坐标 = 向量(本地坐标.X, 本地坐标.Z);
                正面坐标 = 获取正面视场角度(本地坐标);
                Vector2 顶视方向 = 向量(方向.X, 方向.Z);
                float 向量长 = 顶视方向.Length();
                if (向量长 != 0)
                    顶视朝向 = MathHelper.WrapAngle((float)Math.Acos(本地坐标.X / 向量长) - MathHelper.PiOver2);
                else
                    顶视朝向 = 0;
                MyRelationsBetweenPlayerAndBlock 关系 = (MyRelationsBetweenPlayerAndBlock)实体信息.所属关系;
                switch (关系) { case MyRelationsBetweenPlayerAndBlock.Owner: case MyRelationsBetweenPlayerAndBlock.FactionShare: 派系 = 1; break; case MyRelationsBetweenPlayerAndBlock.Enemies: 派系 = 2; break; case MyRelationsBetweenPlayerAndBlock.Friends: 派系 = 3; break; default: 派系 = 0; break; }
            }

            public static Vector2? 获取正面视场角度(Vector3 本地坐标)
            {
                if (本地坐标.Z >= 0)
                    return null;
                else
                    return 向量(Math.Atan(本地坐标.X / Math.Abs(本地坐标.Z)), Math.Atan(本地坐标.Y / Math.Abs(本地坐标.Z)));
            }
        }
    }
}
