﻿using System;

namespace mapKnight_Android
{
	public struct Color
	{
		public string RGBCode{ get; private set; }

		public int RedByte{ get; private set; }

		public int GreenByte{ get; private set; }

		public int BlueByte{ get; private set; }

		public int AlphaByte{ get; private set; }

		public float Red{ get; private set; }

		public float Green{ get; private set; }

		public float Blue{ get; private set; }

		public float Alpha{ get; private set; }

		public Color (float red, float green, float blue, float alpha) : this ()
		{
			this.Red = red;
			this.Green = green;
			this.Blue = blue;
			this.Alpha = alpha;

			this.RedByte = (int)(red * 255);
			this.GreenByte = (int)(green * 255);
			this.BlueByte = (int)(blue * 255);
			this.AlphaByte = (int)(alpha * 255);

			HexValue redHex = new HexValue ((uint)RedByte);
			HexValue greenHex = new HexValue ((uint)GreenByte);
			HexValue blueHex = new HexValue ((uint)BlueByte);
			RGBCode = "#" + redHex.Code + greenHex.Code + blueHex.Code;
		}

		public Color (string RGB, float alpha) : this ()
		{
			RGBCode = RGB;

			char[] rgbchars = RGB.Replace ("#", "").ToCharArray ();
			HexValue red = new HexValue (new string (new char[] { rgbchars [0], rgbchars [1] }));
			HexValue green = new HexValue (new string (new char[] { rgbchars [2], rgbchars [3] }));
			HexValue blue = new HexValue (new string (new char[] { rgbchars [4], rgbchars [5] }));

			this.Red = (float)red.Value / 255f;
			this.Green = (float)green.Value / 255f;
			this.Blue = (float)blue.Value / 255f;
			this.Alpha = alpha;

			this.RedByte = (int)(red.Value);
			this.GreenByte = (int)(green.Value);
			this.BlueByte = (int)(blue.Value);
			this.AlphaByte = (int)(alpha * 255);
		}

		public Color (float red, float green, float blue) : this (red, green, blue, 1.0f)
		{
		}

		public Color (int red, int green, int blue, int alpha) : this ((float)red / 255, (float)green / 255, (float)blue / 255, (float)alpha / 255)
		{
		}

		public Color (HexValue RGB) : this (RGB.Code, 255)
		{
		}

		public Color (HexValue RGB, float alpha) : this (RGB.Code, alpha)
		{
		}

		public Color (HexValue RGB, int alpha) : this (RGB.Code, alpha)
		{
		}

		public Color (string RGB) : this (RGB, 255)
		{
		}

		public Color (string RGB, int alpha) : this (RGB, (float)alpha / 255)
		{
		}

		public static Color White { get { return new Color ("#FFFFFF", 255); } }

		public static Color Black { get { return new Color ("#000000", 255); } }
	}
}