﻿using System;
using System.Collections.Generic;

using Android.Graphics;
using GL = Android.Opengl.GLES20;
using Android.Opengl;

using Java.Nio;

using mapKnight.Basic;

namespace mapKnight.Android.CGL
{
	public class CGLText : IDisposable
	{
		private Paint TextPaint;

		public event EventHandler OnRefresh;

		private string iText;

		public string Text {
			get { return iText; }
			set {
				iText = value;
				Refresh ();
			}
		}

		private int iFontsize;

		public int Fontsize {
			get{ return iFontsize; }
			set {
				iFontsize = value;
				Refresh ();
			}
		}

		private Font iFont;

		public Font Font {
			get { return iFont; }
			set {
				iFont = value;
				Refresh ();
			}
		}

		private Basic.Point iPositionCorrection;

		private Basic.Point iPosition;

		public Basic.Point Position {
			get { return iPosition; }
			set {
				iPosition = value + iPositionCorrection;
				Refresh ();
			}
		}

		private Basic.Color iColor;

		public Basic.Color Color {
			get { return iColor; }
			set {
				iColor = value;
				Refresh ();
			}
		}

		private FontStyle iFontStyle;

		public FontStyle FontStyle {
			get{ return iFontStyle; }
			set {
				iFontStyle = value;
				Refresh ();
			}
		}

		public int Width{ get; private set; }

		public int Height{ get; private set; }

		public CGLText (string text, int fontsize, Font font) : this (text, fontsize, font, new Basic.Point (Content.ScreenSize), Basic.Color.Black, FontStyle.Normal)
		{
		}

		public CGLText (string text, int fontsize, Font font, Basic.Point position) : this (text, fontsize, font, position, Basic.Color.Black, FontStyle.Normal)
		{
		}

		public CGLText (string text, int fontsize, Font font, Basic.Point position, Basic.Color color) : this (text, fontsize, font, position, color, FontStyle.Normal)
		{
		}

		public CGLText (string text, int fontsize, Font font, Basic.Point position, FontStyle fontstyle) : this (text, fontsize, font, position, Basic.Color.Black, fontstyle)
		{
		}

		public CGLText (string text, int fontsize, Font font, Basic. Point position, Basic.Color color, FontStyle fontstyle)
		{
			this.iText = text;
			this.iFontsize = fontsize;
			this.iFont = font;
			this.iPosition = position;
			this.iColor = color;
			this.iFontStyle = fontstyle;

			Refresh ();

			CGLTextContainer.Subscribe (this);
		}

		private void Refresh ()
		{
			if (Content.AntialiasText)
				TextPaint = new Paint (PaintFlags.AntiAlias);
			else
				TextPaint = new Paint ();
			TextPaint.TextSize = this.Fontsize;
			TextPaint.TextAlign = Paint.Align.Left;
			TextPaint.SetARGB (this.Color.AlphaByte, this.Color.RedByte, this.Color.GreenByte, this.Color.BlueByte);
			TextPaint.SetStyle (Paint.Style.Fill);
			switch (FontStyle) {
			case FontStyle.Normal:
				TextPaint.SetTypeface (Content.Fonts [this.Font]);
				break;
			case FontStyle.Bold:
				TextPaint.SetTypeface (Typeface.Create (Content.Fonts [this.Font], TypefaceStyle.Bold));
				break;
			case FontStyle.Italic:
				TextPaint.SetTypeface (Typeface.Create (Content.Fonts [this.Font], TypefaceStyle.Italic));
				break;
			case FontStyle.BoldItalic:
				TextPaint.SetTypeface (Typeface.Create (Content.Fonts [this.Font], TypefaceStyle.BoldItalic));
				break;
			default:
				TextPaint.SetTypeface (Content.Fonts [this.Font]);
				break;
			}
			TextPaint.TextLocale = new Java.Util.Locale ("de", "de");

			Rect textbounds = new Rect ();
			TextPaint.GetTextBounds (this.Text, 0, this.Text.Length, textbounds);
			this.Height = textbounds.Height ();
			this.Width = textbounds.Width ();

			this.iPositionCorrection = new Basic.Point (0, 0);
			this.iPosition += this.iPositionCorrection;

			if (OnRefresh != null)
				OnRefresh (this, EventArgs.Empty);
		}

		public void Dispose ()
		{
			CGLTextContainer.Unsubcribe (this);
		}

		public static class CGLTextContainer
		{
			private static List<CGLText> subscribedItems;

			private static FloatBuffer fVertexBuffer;
			private static FloatBuffer fTextureBuffer;
			private static ShortBuffer fIndexBuffer;

			private static int texture;
			private static int renderprogram;

			private static int CoordsPerVertex = 3;
			private static int VertexStride = CGLTextContainer.CoordsPerVertex * sizeof(float);

			private static Bitmap textbitmap;
			private static Canvas textcanvas;

			public static void Init ()
			{
				subscribedItems = new List<CGLText> ();
				
				textbitmap = Bitmap.CreateBitmap (Content.ScreenSize.Width, Content.ScreenSize.Height, Bitmap.Config.Argb8888);
				textcanvas = new Canvas (textbitmap);

				renderprogram = CGLTools.GetProgram (Shader.FragmentShader, Shader.VertexShaderM);
				initTexture ();
				updateTexture ();
			}

			public static void Draw (float[] mvpMatrix)
			{
				GL.GlUseProgram (renderprogram);

				// Set the active texture unit to texture unit 0.

				int PositionHandle = GL.GlGetAttribLocation (renderprogram, "vPosition");
				GL.GlEnableVertexAttribArray (PositionHandle);
				GL.GlVertexAttribPointer (PositionHandle, CoordsPerVertex, GL.GlFloat, false, VertexStride, fVertexBuffer);

				int MVPMatrixHandle = GL.GlGetUniformLocation (renderprogram, "uMVPMatrix");
				GL.GlUniformMatrix4fv (MVPMatrixHandle, 1, false, mvpMatrix, 0);

				GL.GlEnable (GL.GlBlend);
				GL.GlBlendFunc (GL.GlSrcAlpha, GL.GlOneMinusSrcAlpha);

				int mTextureUniformHandle = GL.GlGetUniformLocation (renderprogram, "u_Texture");
				int mTextureCoordinateHandle = GL.GlGetAttribLocation (renderprogram, "a_TexCoordinate");
				GL.GlVertexAttribPointer (mTextureCoordinateHandle, 2, GL.GlFloat, false, 0, fTextureBuffer);
				GL.GlEnableVertexAttribArray (mTextureCoordinateHandle);

				GL.GlActiveTexture (GL.GlTexture0);
				GL.GlBindTexture (GL.GlTexture2d, texture);
				GL.GlUniform1i (mTextureUniformHandle, 0);

				GL.GlDrawElements (GL.GlTriangles, 6, GL.GlUnsignedShort, fIndexBuffer);
				GL.GlDisableVertexAttribArray (PositionHandle);
				GL.GlDisableVertexAttribArray (mTextureCoordinateHandle);
			}

			public static void Subscribe (CGLText Item)
			{
				if (!subscribedItems.Contains (Item)) {
					Item.OnRefresh += (object sender, EventArgs e) => {
						updateTexture ();
					};
					subscribedItems.Add (Item);
					updateTexture ();
				}
			}

			public static void RequestForeground (CGLText item)
			{
				if (subscribedItems.Contains (item)) {
					subscribedItems.Remove (item);
					subscribedItems.Insert (subscribedItems.Count, item);
					updateTexture ();
				}
			}

			public static void RequestBackground (CGLText item)
			{
				if (subscribedItems.Contains (item)) {
					subscribedItems.Remove (item);
					subscribedItems.Insert (0, item);
					updateTexture ();
				}
			}

			public static void Unsubcribe (CGLText Item)
			{
				if (subscribedItems.Contains (Item)) {
					subscribedItems.Remove (Item);
					updateTexture ();
				}
			}

			private static void initTexture ()
			{
				int[] temp = new int[1];
				GL.GlGenTextures (1, temp, 0);
				texture = temp [0];

				GL.GlBindTexture (GL.GlTexture2d, texture);
				GL.GlTexImage2D (GL.GlTexture2d, 0, GL.GlRgba, Content.ScreenSize.Width, Content.ScreenSize.Height, 0, GL.GlRgba, GL.GlUnsignedByte, null);

				GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureMinFilter, GL.GlNearest);
				GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureMagFilter, GL.GlNearest);
				GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureWrapS, GL.GlClampToEdge);
				GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureWrapT, GL.GlClampToEdge);

				GL.GlBindTexture (GL.GlTexture2d, 0);

				// init render buffer
				float[] Verticies = new float[] {
					-Content.ScreenRatio,
					1f,
					0f,
					-Content.ScreenRatio,
					-1f,
					0f,
					Content.ScreenRatio,
					-1f,
					0f,
					Content.ScreenRatio,
					1f,
					0f
				};

				ByteBuffer bytebuffer = ByteBuffer.AllocateDirect (Verticies.Length * sizeof(float));
				bytebuffer.Order (ByteOrder.NativeOrder ());
				fVertexBuffer = bytebuffer.AsFloatBuffer ();
				fVertexBuffer.Put (Verticies);
				fVertexBuffer.Position (0);

				short[] Indicies = new short[]{ 0, 1, 2, 0, 2, 3 };
				bytebuffer = ByteBuffer.AllocateDirect (Indicies.Length * sizeof(short));
				bytebuffer.Order (ByteOrder.NativeOrder ());
				fIndexBuffer = bytebuffer.AsShortBuffer ();
				fIndexBuffer.Put (Indicies);
				fIndexBuffer.Position (0);

				float[] TextureCoords = new float[]{ 1, 0, 1, 1, 0, 1, 0, 0 };
				bytebuffer = ByteBuffer.AllocateDirect (TextureCoords.Length * sizeof(float));
				bytebuffer.Order (ByteOrder.NativeOrder ());
				fTextureBuffer = bytebuffer.AsFloatBuffer ();
				fTextureBuffer.Put (TextureCoords);
				fTextureBuffer.Position (0);

				Content.OnUpdate += () => {	
					float[] UpdatedVerticies = new float[] {
						-Content.ScreenRatio,
						1f,
						0f,
						-Content.ScreenRatio,
						-1f,
						0f,
						Content.ScreenRatio,
						-1f,
						0f,
						Content.ScreenRatio,
						1f,
						0f
					};

					bytebuffer = ByteBuffer.AllocateDirect (UpdatedVerticies.Length * sizeof(float));
					bytebuffer.Order (ByteOrder.NativeOrder ());
					fVertexBuffer = bytebuffer.AsFloatBuffer ();
					fVertexBuffer.Put (UpdatedVerticies);
					fVertexBuffer.Position (0);

					textbitmap = Bitmap.CreateBitmap (Content.ScreenSize.Width, Content.ScreenSize.Height, Bitmap.Config.Argb8888);
					textcanvas = new Canvas (textbitmap);

					updateTexture ();
				};
			}

			private static void updateTexture ()
			{
				textbitmap.EraseColor (0);
									
				foreach (CGLText item in subscribedItems) {
					string[] lines = item.Text.Split (new char[]{ '\n' }, StringSplitOptions.None);
					for (int i = 0; i < lines.Length; i++) {
						textcanvas.DrawText (lines [i], item.Position.X, item.Position.Y + i * item.Fontsize + 3, item.TextPaint);
					}
				}

				int[] generatedtexture = new int[1];
				GL.GlGenTextures (1, generatedtexture, 0);
					
				GL.GlBindTexture (GL.GlTexture2d, generatedtexture [0]);
					
				GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureMinFilter, GL.GlNearest);
				GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureMagFilter, GL.GlNearest);
				GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureWrapS, GL.GlClampToEdge);
				GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureWrapT, GL.GlClampToEdge);
					
				GLUtils.TexImage2D (GL.GlTexture2d, 0, textbitmap, 0);
				
				GL.GlBindTexture (GL.GlTexture2d, 0);

				GL.GlDeleteTextures (1, new int[]{ texture }, 0);
				texture = generatedtexture [0];
			}
		}
	}
}