using Sandbox.ModAPI.Ingame;
using System;
using VRage.Game;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class My物理包
        {
            public MatrixD 控制器_世界矩阵 => 控制器变换组件.矩阵;
            public MatrixD 控制器_世界逆矩阵 => 控制器变换组件.逆矩阵;
            public Vector3D 控制器_位置 => 控制器变换组件.位置;
            public MatrixD 飞船_世界矩阵 => 飞船变换组件.矩阵;
            public MatrixD 飞船_世界逆矩阵 => 飞船变换组件.逆矩阵;
            public Vector3D 飞船_位置 => 飞船变换组件.位置;
            public BoundingSphereD 飞船尺寸 => 飞船变换组件.大小;
            public Vector3 重力 => 星球信息组.重力;
            public Vector3 人工重力 => 星球信息组.人工重力;
            public Vector3 混合重力 => 星球信息组.混合重力;
            public double? 地面高度 => 星球信息组.地面高度;
            public double? 海拔高度 => 星球信息组.海拔高度;
            public Vector3D? 星球位置 => 星球信息组.星球位置;
            public bool 有星球 => 星球信息组.是否有最近的星球;
            public Vector3 线速度 => 其他物理量.线速度;
            public Vector3 前_线速度 => 其他物理量.前_线速度;
            public Vector3 Δ线速度 => 其他物理量.线速度变化量;
            public Vector3 角速度 => 其他物理量.角速度;
            public Vector3 前_角速度 => 其他物理量.前_角速度;
            public Vector3 Δ角速度 => 其他物理量.角速度变化量;
            public float 飞船质量 => 其他物理量.飞船质量;
            public float 飞船速度 => 其他物理量.飞船速度;
            public Vector3 重力矢量 { get; private set; }
            public float 重力大小 { get; private set; }
            public double 滚转角 { get; private set; }
            public double 俯仰角 { get; private set; }
            public MatrixD 星球坐标 { get; private set; }

            public MyCubeSize 网格类型 { get; private set; }
            public My物理包() { 清空(); }
            public void 更新(IMyShipController 控制器)
            {
                if (实体不存在(控制器)) { 清空(); return; }
                控制器变换组件.更新(控制器);
                飞船变换组件.更新(控制器.CubeGrid);
                星球信息组.更新(控制器);
                其他物理量.更新(控制器);
                网格类型 = 控制器.CubeGrid.GridSizeEnum;

                if (有星球)
                {
                    重力矢量 = 重力;
                    重力大小 = 重力矢量.Normalize();
                    重力矢量 = 置零(重力矢量);
                    滚转角 = 0.0 - (Math.Acos(Vector3.Dot(Vector3D.Normalize(Vector3D.Reject(重力矢量, 控制器_世界矩阵.Forward)), 控制器_世界矩阵.Left)) - MathHelper.PiOver2);
                    if (重力矢量.Dot(控制器_世界矩阵.Up) >= 0f)
                    {
                        滚转角 = MathHelper.Pi - 滚转角;
                    }
                    滚转角 = MathHelper.WrapAngle((float)滚转角);

                    俯仰角 = Math.Acos(重力.Dot(控制器_世界矩阵.Forward) / (重力.Length() * 控制器_世界矩阵.Forward.Length())) - MathHelper.PiOver2;
                }
                else
                {
                    重力矢量 = Vector3.Zero;
                    重力大小 = 0;
                    俯仰角 = 滚转角 = 0;
                }
            }
            public void 清空()
            {
                网格类型 = MyCubeSize.Large;
                控制器变换组件.清空();
                飞船变换组件.清空();
                星球信息组.清空();
                其他物理量.清空();
                重力矢量 = Vector3.Zero;
                重力大小 = 0;
                俯仰角 = 滚转角 = 0;
                星球坐标 = MatrixD.Identity;
            }
            //public void 得到星球信息()

            readonly My实体变换组件 控制器变换组件 = new My实体变换组件();
            readonly My实体变换组件 飞船变换组件 = new My实体变换组件();
            readonly My星球信息组件 星球信息组 = new My星球信息组件();
            readonly My其他物理量 其他物理量 = new My其他物理量();

        }


        public class 速度组件
        {
            public Vector3 值 { get { return _值; } set { Δ = value - _值; _值 = value; } }
            public Vector3 Δ { get; private set; }
            public void 清零() { Δ = _值 = Vector3.Zero; }

            Vector3 _值;
        }

    }
}
