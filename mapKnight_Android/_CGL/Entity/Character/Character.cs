﻿using System;
using System.Collections.Generic;

using mapKnight.Android;
using mapKnight.Values;
using mapKnight.Android.CGL.Entity;

namespace mapKnight.Entity
{
	public class Character : mapKnight.Android.CGL.Entity.CGLEntity
	{
		public ChangingProperty Energy;

		private int moveSpeed;
		private int jumpSpeed;

		public Character (int health, int energy, string name, int weight, Size bounds, List<CGLBoundedPoint> boundedpoints, List<CGLAnimation> animations, CGLSet set, int movespeed, int jumpspeed)
			: base (health, Content.Map.SpawnPoint, name, weight, bounds, boundedpoints, animations, set)
		{
			Energy = new ChangingProperty (energy);

			moveSpeed = movespeed;
			jumpSpeed = jumpspeed;
		}

		public void Move (Direction dir)
		{
			switch (dir) {
			case Direction.Left:
				this.Velocity.X = -moveSpeed;
				break;
			case Direction.Right:
				this.Velocity.X = moveSpeed;
				break;
			}
		}

		public void ResetMovement ()
		{
			this.Velocity.X = 0;
		}

		public void Jump ()
		{
			this.Velocity.Y = jumpSpeed;
		}
	}
}

