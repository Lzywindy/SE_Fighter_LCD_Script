using System.Collections.Generic;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript
{
    public partial class Program
    {
        public class 雪碧列表 : List<MySprite>
        {

            public void 元素(SpriteType 类型 = 图像类, string 值 = null, Vector2? 位置 = null, Vector2? 尺寸 = null, Color? 颜色 = null, string 字体 = null, TextAlignment 对齐 = 默认对齐, float 旋转 = 0f) => Add(new MySprite(类型, 值, 位置, 尺寸, 颜色, 字体, 对齐, 旋转));
            public void 图案(string 值, Vector2? 位置, Vector2? 尺寸, Color? 颜色, float 旋转 = 0f) => Add(new MySprite(图像类, 值, 位置, 尺寸, 颜色, null, 默认对齐, 旋转));
            public void 裁剪(Vector2? 位置, Vector2? 尺寸) => Add(new MySprite(SpriteType.CLIP_RECT, 方图1, 位置, 尺寸, null, null, 默认对齐, 0));
            public void 文本(string 值, Vector2? 位置, Color? 颜色, string 字体 = "White", TextAlignment 对齐 = 默认对齐, float 字号 = 1) => Add(new MySprite(SpriteType.TEXT, 值, 位置, null, 颜色, 字体, 对齐, 字号));
            public void 图案(string 值, 边界框 框, Color? 颜色, float 旋转 = 0f) => Add(new MySprite(图像类, 值, 框.中心, 框.尺寸, 颜色, null, 默认对齐, 旋转));
            public void 裁剪(边界框 框) => Add(new MySprite(SpriteType.CLIP_RECT, 方图1, 框.中心 - 框.半尺寸, 框.尺寸, null, null, 默认对齐, 0));
            public void 方框(边界框 框, Color? 颜色, Color? 背景, float 线宽 = 1, float 旋转 = 0f)
            {
                Vector2 中心 = 框.中心, 轴1 = 向量_旋转(向量(框.半尺寸.X - 线宽 * 0.5f, 0), 旋转), 轴2 = 向量_旋转(向量(0, 框.半尺寸.Y - 线宽 * 0.5f), 旋转);
                if (背景.HasValue) { Add(new MySprite(图像类, 方图1, 框.中心, 框.尺寸, 背景, null, 默认对齐, 旋转)); }
                Add(new MySprite(图像类, 方图1, 中心 + 轴2, 向量(框.a, 线宽), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 - 轴2, 向量(框.a, 线宽), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 + 轴1, 向量(线宽, 框.b), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 - 轴1, 向量(线宽, 框.b), 颜色, null, 默认对齐, 旋转));
            }
            public void 边角框(边界框 框, Color? 颜色, Color? 背景, float 线宽 = 1, float 旋转 = 0f, float 边角 = 0)
            {
                边角 = MathHelper.Clamp(边角, 0.2f, 0.8f);
                float 线长 = (1 - 边角) * 0.5f, 偏移 = 线长 * 0.5f + 边角 * 0.5f, a_ = 框.a * 线长, b_ = 框.b * 线长;
                Vector2 中心 = 框.中心, 轴1 = 向量_旋转(向量(框.半尺寸.X, 0), 旋转), 轴2 = 向量_旋转(向量(0, 框.半尺寸.Y), 旋转), 轴1偏移 = 轴1 * 偏移 * 2, 轴2偏移 = 轴2 * 偏移 * 2;
                if (背景.HasValue) { Add(new MySprite(图像类, 方图1, 框.中心, 框.尺寸, 背景, null, 默认对齐, 旋转)); }
                Add(new MySprite(图像类, 方图1, 中心 + 轴2 - 轴1偏移, 向量(a_, 线宽), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 + 轴2 + 轴1偏移, 向量(a_, 线宽), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 - 轴2 - 轴1偏移, 向量(a_, 线宽), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 - 轴2 + 轴1偏移, 向量(a_, 线宽), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 + 轴1 - 轴2偏移, 向量(线宽, b_), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 + 轴1 + 轴2偏移, 向量(线宽, b_), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 - 轴1 - 轴2偏移, 向量(线宽, b_), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 - 轴1 + 轴2偏移, 向量(线宽, b_), 颜色, null, 默认对齐, 旋转));
            }
            public void 十字(边界框 框, Color? 颜色, float 线宽 = 1, float 旋转 = 0f, float 孔 = 0)
            {
                孔 = MathHelper.Clamp(孔, 0, 0.8f);
                float 线长 = (1 - 孔) * 0.5f, 偏移 = 线长 * 0.5f + 孔 * 0.5f, a_ = 框.a * 线长, b_ = 框.b * 线长;
                Vector2 中心 = 框.中心, 轴1 = 向量_旋转(向量(框.半尺寸.X, 0), 旋转), 轴2 = 向量_旋转(向量(0, 框.半尺寸.Y), 旋转), 轴1偏移 = 轴1 * 偏移 * 2, 轴2偏移 = 轴2 * 偏移 * 2;
                Add(new MySprite(图像类, 方图1, 中心 + 轴1偏移, 向量(a_, 线宽), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 - 轴1偏移, 向量(a_, 线宽), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 - 轴2偏移, 向量(线宽, b_), 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 方图1, 中心 + 轴2偏移, 向量(线宽, b_), 颜色, null, 默认对齐, 旋转));
            }
            public void 框框(string 值, Vector2? 位置, Vector2? 尺寸, Color? 颜色, Color? 背景, float 线宽 = 1, float 旋转 = 0f)
            {
                Add(new MySprite(图像类, 值, 位置, 尺寸, 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 值, 位置, (尺寸 ?? Vector2.One) - 线宽, 背景 ?? Color.Black, null, 默认对齐, 旋转));
            }
            public void 框框(string 值, 边界框 框, Color? 颜色, Color? 背景, float 线宽 = 1, float 旋转 = 0f)
            {
                Add(new MySprite(图像类, 值, 框.中心, 框.尺寸, 颜色, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 值, 框.中心, 框.尺寸 - 线宽, 背景 ?? Color.Black, null, 默认对齐, 旋转));
            }
            public void 框框_逆(string 值, Vector2? 位置, Vector2? 尺寸, Color? 颜色, Color? 背景, float 线宽 = 1, float 旋转 = 0f)
            {
                Add(new MySprite(图像类, 值, 位置, (尺寸 ?? Vector2.One) - 线宽, 背景 ?? Color.Black, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 值, 位置, 尺寸, 颜色, null, 默认对齐, 旋转));
            }
            public void 框框_逆(string 值, 边界框 框, Color? 颜色, Color? 背景, float 线宽 = 1, float 旋转 = 0f)
            {
                Add(new MySprite(图像类, 值, 框.中心, 框.尺寸 - 线宽, 背景 ?? Color.Black, null, 默认对齐, 旋转));
                Add(new MySprite(图像类, 值, 框.中心, 框.尺寸, 颜色, null, 默认对齐, 旋转));
            }
            public void 进度条(float 值, 边界框 框, Color 颜色, Color 背景, Color 填充, float 线宽 = 1)
            {
                值 = MathHelper.Clamp(值, 0, 1);
                Add(new MySprite(图像类, 方图1, 框.中心, 框.尺寸, 颜色, null, 默认对齐, 0));
                边界框 内框 = 边界框.缩进(框, 线宽);
                Add(new MySprite(图像类, 方图1, 内框.中心, 内框.尺寸, 背景, null, 默认对齐, 0));
                Add(new MySprite(图像类, 方图1, 框.中心 - 向量(内框.半尺寸.X * (1 - 值), 0), 内框.尺寸 * 向量(值, 1), 填充, null, 默认对齐, 0));
            }

            public const TextAlignment 默认对齐 = TextAlignment.CENTER;
            public const SpriteType 图像类 = SpriteType.TEXTURE;
        }
    }
}
