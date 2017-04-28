﻿using mapKnight.Extended.Graphics.Buffer;
using mapKnight.Extended.Graphics.Handle;
using OpenTK.Graphics.ES20;
using System;

namespace mapKnight.Extended.Graphics.Programs {
    public class GaussianBlurProgram : TextureProgram {
        public static GaussianBlurProgram Program;

        public static readonly GPUBuffer VERTEX_BUFFER;
        public static readonly GPUBuffer TEXTURE_BUFFER;
        public static readonly IndexBuffer INDEX_BUFFER;

        static GaussianBlurProgram ( ) {
            VERTEX_BUFFER = new GPUBuffer(2, 4, PrimitiveType.Quad, new float[ ] { -1f, 1f, -1f, -1f, 1f, -1f, 1f, 1f }, BufferUsage.DynamicDraw);
            TEXTURE_BUFFER = new GPUBuffer(2, 4, PrimitiveType.Quad, new float[ ] { 0, 1, 0, 0, 1, 0, 1, 1 }, BufferUsage.DynamicDraw);
            INDEX_BUFFER = new IndexBuffer(1);
        }

        private UniformVec2Handle pixelOffsetHandle;

        public GaussianBlurProgram ( ) : base(Assets.GetVertexShader("normal"), Assets.GetFragmentShader("gauss")) {
            pixelOffsetHandle = new UniformVec2Handle(glProgram, "u_pixel_offset");
        }

        public void Draw (Framebuffer original, Framebuffer cache, bool alphaBlending) {
            GL.ClearColor(0f, 0f, 0f, 0f);

            cache.Bind( );
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Apply(original.Texture.ID, INDEX_BUFFER, VERTEX_BUFFER, TEXTURE_BUFFER, alphaBlending);
            pixelOffsetHandle.Set(new float[ ] { original.PixelSize.X, 0f });
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedShort, IntPtr.Zero);

            original.Bind( );
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Apply(cache.Texture.ID, INDEX_BUFFER, VERTEX_BUFFER, TEXTURE_BUFFER, alphaBlending);
            pixelOffsetHandle.Set(new float[ ] { 0f, original.PixelSize.Y });
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedShort, IntPtr.Zero);
            original.Unbind( );

            Window.UpdateBackgroundColor( );
        }
    }
}