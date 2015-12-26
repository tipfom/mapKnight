﻿using System;
using System.Collections.Generic;

using Android.Content;

using mapKnight.Utils;
using mapKnight.Entity;
using mapKnight.Values;

namespace mapKnight.Android.CGL.Entity
{
	public class CGLEntityPreset: EntityPreset
	{
		protected List<CGLSet> sets;
		protected List<CGLAnimation> animations;

		protected Dictionary<Slot, fRectangle> bpTextures;

		public CGLEntityPreset (XMLElemental entityConfig, Context context) : base (entityConfig)
		{
			foreach (XMLElemental set in entityConfig.GetAll("set")) {
				sets.Add (new CGLSet (set, context));
			}
			foreach (XMLElemental set in entityConfig.GetAll("animation")) {
				animations.Add (new CGLAnimation ());
			}

			foreach (XMLElemental def in entityConfig["def"].GetAll()) {
				switch (def.Name) {
				case "slot":
					for (int i = 0; i < Convert.ToInt32 (def.Attributes ["bpcount"]); i++) {
						bpTextures.Add ((Slot)Enum.Parse (typeof(Slot), def.Attributes ["name"]), CGLTools.ParseCoordinates (def ["texture"].Attributes, sets [0].TextureSize));
					}
					break;
				}
			}
		}

		public CGLEntity Instantiate (uint level, string set)
		{
			return Instantiate (level, set, new Point (0, 0));
		}

		public CGLEntity Instantiate (uint level, string set, Point position)
		{
			return new CGLEntity (defaultAttributes [mapKnight.Entity.Attribute.Health] + (int)((level - 1) * attributeIncrease [mapKnight.Entity.Attribute.Health]), position, name,
				bpTextures, animations, sets.FindAll ((CGLSet obj) => obj.Name == set));
		}
	}
}

