﻿using System;
using System.Collections.Generic;

using mapKnight.Values;
using mapKnight.Utils;

namespace mapKnight.Entity
{
	public class EntityPreset
	{
		protected string name;
		protected Dictionary<Attribute, int> defaultAttributes;
		protected Dictionary<Attribute,float> attributeIncrease;

		public EntityPreset (XMLElemental entityConfig)
		{
			name = entityConfig.Attributes ["name"];

			// default attributes
			defaultAttributes = new Dictionary<Attribute, int> ();
			foreach (XMLElemental attribute in entityConfig["level"]["default"].GetAll()) {
				defaultAttributes.Add ((Attribute)Enum.Parse (typeof(Attribute), attribute.Name), Convert.ToInt32 (attribute.Value));
			}

			// increase of each attribute per level
			attributeIncrease = new Dictionary<Attribute, float> ();
			foreach (XMLElemental attribute in entityConfig["level"]["increase"].GetAll()) {
				attributeIncrease.Add ((Attribute)Enum.Parse (typeof(Attribute), attribute.Name), Convert.ToInt32 (attribute.Value));
			}
		}

		public virtual Entity Instantiate (uint level)
		{
			return Instantiate (level, new Point (0, 0));			
		}

		public virtual Entity Instantiate (uint level, Point position)
		{
			return new Entity (defaultAttributes [Attribute.Health] + (int)((level - 1) * attributeIncrease [Attribute.Health]), position, name);
		}
	}
}
