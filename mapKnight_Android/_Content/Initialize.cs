﻿using System;
using System.IO;
using System.Reflection;

using Android.Graphics;
using Android.Content;
using Android.Views;
using Android.Opengl;
using GL = Android.Opengl.GLES20;
using Matrix = Android.Opengl.Matrix;

using mapKnight.Android;
using mapKnight.Android.Net;
using mapKnight.Android.CGL;
using mapKnight.Utils;
using mapKnight.Values;

namespace mapKnight
{
	public static partial class Content
	{
		public delegate void HandleInitCompleted (Context GameContext);

		public static event HandleInitCompleted OnInit;

		public static event HandleInitCompleted OnPreInit;

		public static event HandleInitCompleted OnAfterInit;

		public delegate void HandleUpdate ();

		public static event HandleUpdate OnUpdate;

		public static void PreInit (XMLElemental configfile, Context context)
		{
			Context = context;
			Version = new Values.Version (Assembly.GetExecutingAssembly ().GetName ().Version.ToString ());

			CGLTools.LoadShader ();
			LoadFonts (configfile);

			ScreenSize = new Size (context.Resources.DisplayMetrics.WidthPixels, context.Resources.DisplayMetrics.HeightPixels);
			ScreenRatio = (float)ScreenSize.Width / (float)ScreenSize.Height;

			TileSize = Convert.ToInt32 (configfile ["images"].Find ("name", "tiles").Attributes ["tilesize"]);
			LoadImage (context.Assets.Open (configfile ["images"].Find ("name", "tiles").Attributes ["src"]));
			InterfaceSprite = new CGLSprite<int> (context.Assets.Open (configfile ["images"].Find ("name", "interface").Attributes ["src"]), configfile ["images"].Find ("name", "interface").GetAll ());
		
			TileTexCoordManager = LoadManager (configfile ["tiles"].GetAll ());
			OverlayTexCoordManager = LoadManager (configfile ["overlay"].GetAll ());

			Log.All (typeof(Content), "Current Version : " + Version.ToString (), MessageType.Info);

			if (OnPreInit != null)
				OnPreInit (context);
		}

		public static void Init (XMLElemental configfile)
		{

			TouchManager = new ButtonManager ();
			Terminal = new TerminalManager ();
			Map = new CGLMap (22, "physXTest2.devmap");
			Camera = new CGLCamera (-0.0f);
			Data = new SaveManager (System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "gamedata.db3"));
			LoadCharacter ();

			CGLText.CGLTextContainer.Init ();

			if (OnInit != null)
				OnInit (Content.Context);
		}

		public static void AfterInit ()
		{
			Map.AddEntity (Content.Character);
			Camera.Update ();

			if (OnAfterInit != null)
				OnAfterInit (Context);
		}

		public static void Update (Size screensize)
		{
			ScreenSize = screensize;
			ScreenRatio = (float)ScreenSize.Width / (float)ScreenSize.Height;

			if (OnUpdate != null)
				OnUpdate ();
		}

		private static void LoadImage (Stream ImageStream)
		{
			int[] loadedtexture = new int[1];
			GL.GlGenTextures (1, loadedtexture, 0);

			BitmapFactory.Options bfoptions = new BitmapFactory.Options ();
			bfoptions.InScaled = false;
			bfoptions.InPreferredConfig = Bitmap.Config.Argb8888;
			Bitmap bitmap = BitmapFactory.DecodeStream (ImageStream, null, bfoptions);

			GL.GlBindTexture (GL.GlTexture2d, loadedtexture [0]);

			GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureMinFilter, GL.GlNearest);
			GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureMagFilter, GL.GlNearest);
			GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureWrapS, GL.GlClampToEdge);
			GL.GlTexParameteri (GL.GlTexture2d, GL.GlTextureWrapT, GL.GlClampToEdge);

			GLUtils.TexImage2D (GL.GlTexture2d, 0, bitmap, 0);

			ImageHeight = bfoptions.OutHeight;
			ImageWidth = bfoptions.OutWidth;

			bitmap.Recycle ();
			GL.GlBindTexture (GL.GlTexture2d, 0);

			// Error Check
			int error = GL.GlGetError ();
			if (error != 0) {
				Log.All (typeof(Content), "error while loading mainimage (errorcode => " + error.ToString () + ")", MessageType.Debug);
				throw new FileLoadException ("error while loading mainimage (errorcode => " + error.ToString () + ")");
			}
			if (loadedtexture [0] == 0) {
				Log.All (typeof(Content), "loaded mainimage is zero", MessageType.Debug);
				throw new FileLoadException ("loaded mainimage is zero");
			}

			// set MainTexture to the loaded texture
			MainTexture = loadedtexture [0];

			TextureVertexWidth = TileSize / (float)ImageWidth;
			TextureVertexHeight = TileSize / (float)ImageHeight;
		}

		private static void LoadFonts (XMLElemental configfile)
		{
			Fonts = new System.Collections.Generic.Dictionary<Font, Typeface> ();
			Fonts.Add (Font.Tahoma, Typeface.CreateFromAsset (Context.Assets, configfile ["fonts"].Get (((XMLElemental element) => element.Attributes ["name"] == "Tahoma")).Attributes ["src"]));
			Fonts.Add (Font.ArcadeClassic, Typeface.CreateFromAsset (Context.Assets, configfile ["fonts"].Get (((XMLElemental element) => element.Attributes ["name"] == "ArcadeClassic")).Attributes ["src"]));
			Fonts.Add (Font.ArcadeDotted, Typeface.CreateFromAsset (Context.Assets, configfile ["fonts"].Get (((XMLElemental element) => element.Attributes ["name"] == "ArcadeDotted")).Attributes ["src"]));
			Fonts.Add (Font.BitOperator, Typeface.CreateFromAsset (Context.Assets, configfile ["fonts"].Find ("name", "Pixel").Attributes ["src"]));
		}

		private static void LoadCharacter ()
		{
			Entity.CharacterPreset preset = new Entity.CharacterPreset (Utils.XMLElemental.Load (Context.Assets.Open ("character/robot.character")), Context);
			Character = preset.Instantiate (10, "futuristic");
			Character.CollisionMask = mapKnight.Android.PhysX.PhysXFlag.Map;
		}
	}
}