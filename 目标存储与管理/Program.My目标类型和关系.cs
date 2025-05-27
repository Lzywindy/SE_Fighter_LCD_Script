namespace IngameScript
{
    public partial class Program
    {
        public enum My目标类型和关系 : byte { 中立 = 0, 其他 = 0, 敌人 = 1, 朋友 = 2, 锁定的 = 4, 大网格 = 8, 小网格 = 16, 导弹 = 32, 小行星 = 64, 关系遮罩 = 中立 | 敌人 | 朋友, 类型遮罩 = 大网格 | 小网格 | 其他 | 导弹 | 小行星 }
    }
}
