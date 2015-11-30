﻿using System;
using System.Collections.Generic;

namespace mapKnight_Android
{
	public class CharacterInfo
	{
		private Dictionary<string,Set> loadedSets;
		private Dictionary<string,Animation> loadedAnimations;
		private List<DefinitionPoint> loadedDefinitions;

		public CharacterInfo (XMLElemental config)
		{
			loadedSets = new Dictionary<string, Set> ();
			loadedAnimations = new Dictionary<string, Animation> ();
			loadedDefinitions = new List<DefinitionPoint> ();

			foreach (XMLElemental element in config.GetAll()) {
				switch (element.Name) {
				case "def":
					foreach (XMLElemental point in element.GetAll("point")) {
						loadedDefinitions.Add (new DefinitionPoint (Convert.ToInt32 (point.Attributes ["id"]), point.Attributes ["name"], Convert.ToInt32 (point.Attributes ["texture"])));
					}
					break;
				case "set":
					loadedSets.Add (element.Attributes ["name"].ToUpper (), new Set (element));
					break;
				case "anim":
					loadedAnimations.Add (element.Attributes ["name"].ToUpper (), new Animation (element));
					break;
				}
			}
		}
	}
}
