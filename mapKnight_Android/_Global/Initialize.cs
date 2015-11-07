﻿using System;
using System.IO;
using System.Reflection;

using Android.Graphics;
using Android.Content;
using Android.Views;
using Android.Opengl;
using GL = Android.Opengl.GLES20;

using mapKnight_Android.CGL;

namespace mapKnight_Android
{
	public static partial class GlobalContent
	{
		public delegate void HandleInitCompleted (Context GameContext);

		public static event HandleInitCompleted OnInitCompleted;

		public delegate void HandleUpdate ();

		public static event HandleUpdate OnUpdate;

		public static void Init (XMLElemental configfile, Context GameContext)
		{
			Version = new Version (Assembly.GetExecutingAssembly ().GetName ().Version.ToString ());
			Log.All (typeof(GlobalContent), "Current Version : " + Version.ToString (), MessageType.Info);

			TileSize = Convert.ToInt32 (configfile ["images"].Find ("name", "tiles").Attributes ["tilesize"]);

			LoadImage (GameContext.Assets.Open (configfile ["images"].Find ("name", "tiles").Attributes ["src"]));
			InterfaceSprite = new CGLSprite<int> (GameContext.Assets.Open (configfile ["images"].Find ("name", "interface").Attributes ["src"]), configfile ["images"].Find ("name", "interface").GetAll ());

			LoadFonts (configfile, GameContext);

			TextureVertexWidth = TileSize / (float)ImageWidth;
			TextureVertexHeight = TileSize / (float)ImageHeight;

			TileTexCoordManager = LoadTileManager (configfile ["tiles"].GetAll ());
			OverlayTexCoordManager = LoadOverlayManager (configfile ["overlay"].GetAll ());

			ScreenSize = new Size (GameContext.Resources.DisplayMetrics.WidthPixels, GameContext.Resources.DisplayMetrics.HeightPixels);
			ScreenRatio = (float)ScreenSize.Width / (float)ScreenSize.Height;

			TouchManager = new ButtonManager ();

			LoadShader ();

			UpdateMatrix ();

			CGL.CGLText.CGLTextContainer.Init ();

			if (OnInitCompleted != null)
				OnInitCompleted (GameContext);
		}

		private static void UpdateMatrix ()
		{
			ProjectionMatrix = new float[16];
			ViewMatrix = new float[16];
			MVPMatrix = new float[16];

			Android.Opengl.Matrix.FrustumM (ProjectionMatrix, 0, -ScreenRatio, ScreenRatio, -1, 1, 3, 7);
			Android.Opengl.Matrix.SetLookAtM (ViewMatrix, 0, 0, 0, -3, 0f, 0f, 0f, 0f, 1f, 0f);
			Android.Opengl.Matrix.MultiplyMM (MVPMatrix, 0, ProjectionMatrix, 0, ViewMatrix, 0);
		}

		public static void Update (Size screensize)
		{
			ScreenSize = screensize;
			ScreenRatio = (float)ScreenSize.Width / (float)ScreenSize.Height;

			UpdateMatrix ();

			if (OnUpdate != null)
				OnUpdate ();
		}

		private static void LoadImage (Stream ImageStream)
		{
			int[] loadedtexture = new int[1];
			GL.GlGenTextures (1, loadedtexture, 0);

			BitmapFactory.Options bfoptions = new BitmapFactory.Options ();
			bfoptions.InScaled = false;
			Bitmap bitmap = BitmapFactory.DecodeStream (ImageStream, null, bfoptions);

			GL.GlBindTexture (GL.GlTexture2d, loadedtexture [0]);

			GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureMinFilter, GL.GlNearest);
			GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureMagFilter, GL.GlNearest);
			GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureWrapS, GL.GlClampToEdge);
			GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureWrapT, GL.GlClampToEdge);

			GLUtils.TexImage2D (GL.GlTexture2d, 0, bitmap, 0);

			ImageHeight = bitmap.Height;
			ImageWidth = bitmap.Width;

			bitmap.Recycle ();
			GL.GlBindTexture (GL.GlTexture2d, 0);

			// Error Check
			int error = GL.GlGetError ();
			if (error != 0) {
				Log.All (typeof(GlobalContent), "error while loading mainimage (errorcode => " + error.ToString () + ")", MessageType.Debug);
				throw new FileLoadException ("error while loading mainimage (errorcode => " + error.ToString () + ")");
			}
			if (loadedtexture [0] == 0) {
				Log.All (typeof(GlobalContent), "loaded mainimage is zero", MessageType.Debug);
				throw new FileLoadException ("loaded mainimage is zero");
			}

			// set MainTexture to the loaded texture
			MainTexture = loadedtexture [0];
		}

		private static void LoadFonts (XMLElemental configfile, Context GameContext)
		{
			Fonts = new System.Collections.Generic.Dictionary<Font, Typeface> ();
			Fonts.Add (Font.Tahoma, Typeface.CreateFromAsset (GameContext.Assets, configfile ["fonts"].Get (((XMLElemental element) => element.Attributes ["name"] == "Tahoma")).Attributes ["src"]));
			Fonts.Add (Font.ArcadeClassic, Typeface.CreateFromAsset (GameContext.Assets, configfile ["fonts"].Get (((XMLElemental element) => element.Attributes ["name"] == "ArcadeClassic")).Attributes ["src"]));
			Fonts.Add (Font.ArcadeDotted, Typeface.CreateFromAsset (GameContext.Assets, configfile ["fonts"].Get (((XMLElemental element) => element.Attributes ["name"] == "ArcadeDotted")).Attributes ["src"]));
		}
	}
}