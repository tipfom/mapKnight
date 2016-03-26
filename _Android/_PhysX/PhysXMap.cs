﻿using System;
using System.Collections.Generic;

using mapKnight.Basic;

namespace mapKnight.Android.PhysX {
    public class PhysXMap : Map {
        public const float TILE_BOX_SIZE = .5f;
        private List<PhysXEntity> addedEntitys = new List<PhysXEntity> ();
        private fVector2D Gravity = new fVector2D (0, -10);

        public PhysXMap (string name) : base (name) {

        }

        public void AddEntity (PhysXEntity entity) {
            addedEntitys.Add (entity);
        }

        public void RemoveEntity (PhysXEntity entity) {
            if (addedEntitys.Contains (entity)) {
                addedEntitys.Remove (entity);
            }
        }

        public void Step (float time) {
            time /= 1000f;

            foreach (PhysXEntity entity in addedEntitys) {
                if (entity.CollisionMask.HasFlag (PhysXFlag.Map)) {

                    // move on X
                    bool moved = false;

                    if (entity.Velocity.X > 0) {
                        // right
                        for (int x = (int)((entity.Position.X + entity.Bounds.Width) / TILE_BOX_SIZE); x <= (int)(entity.Position.X + entity.Bounds.Width + entity.Velocity.X * time) / TILE_BOX_SIZE; x++) {
                            for (int y = (int)(entity.Position.Y / TILE_BOX_SIZE); y <= (int)((entity.Position.Y + entity.Bounds.Height) / TILE_BOX_SIZE); y++) {
                                if (this.GetTile (x, y, 1).Mask.HasFlag (Tile.TileMask.COLLISION)) {
                                    entity.Position.X = (float)(x * TILE_BOX_SIZE) - (float)entity.Bounds.Width;
                                    entity.Velocity.X = 0;
                                    moved = true;
                                    break;
                                }
                            }
                        }
                    } else if (entity.Velocity.X < 0) {
                        // left
                        for (int x = (int)(entity.Position.X / TILE_BOX_SIZE); x >= (int)(entity.Position.X + entity.Velocity.X * time) / TILE_BOX_SIZE; x--) {
                            for (int y = (int)(entity.Position.Y / TILE_BOX_SIZE); y <= (int)((entity.Position.Y + entity.Bounds.Height) / TILE_BOX_SIZE); y++) {
                                if (this.GetTile (x, y, 1).Mask.HasFlag (Tile.TileMask.COLLISION)) {
                                    entity.Position.X = (float)((x + 1) * TILE_BOX_SIZE);
                                    entity.Velocity.X = 0;
                                    moved = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!moved) {
                        entity.Position.X += entity.Velocity.X * time;
                    }
                    entity.Velocity.X += (this.Gravity.X + entity.Acceleration.X) * time;

                    moved = false;
                    // move on Y
                    if (entity.Velocity.Y > 0) {
                        for (int y = (int)((entity.Position.Y + entity.Bounds.Height) / TILE_BOX_SIZE); y <= (entity.Position.Y + entity.Velocity.Y * time + entity.Bounds.Height) / TILE_BOX_SIZE; y++) {
                            if (moved)
                                break;
                            for (int x = (int)(entity.Position.X / TILE_BOX_SIZE); x < (int)((entity.Position.X + entity.Bounds.Width) / TILE_BOX_SIZE); x++) {
                                if (this.GetTile (x, y, 1).Mask.HasFlag (Tile.TileMask.COLLISION)) {
                                    entity.Position.Y = y * TILE_BOX_SIZE - entity.Bounds.Height;
                                    entity.Velocity.Y = 0;
                                    moved = true;
                                }
                            }

                            // to prevent gliching
                            if ((entity.Position.X + entity.Bounds.Width) % TILE_BOX_SIZE != 0 && this.GetTile ((int)((entity.Position.X + entity.Bounds.Width) / TILE_BOX_SIZE), y, 1).Mask.HasFlag (Tile.TileMask.COLLISION)) {
                                entity.Velocity.Y = 0;
                                moved = true;
                                break;
                            }
                        }
                    } else if (entity.Velocity.Y < 0) {
                        for (int y = (int)(entity.Position.Y / TILE_BOX_SIZE); y >= (int)(entity.Position.Y + entity.Velocity.Y * time) / TILE_BOX_SIZE; y--) {
                            if (moved)
                                break;
                            for (int x = (int)(entity.Position.X / TILE_BOX_SIZE); x < (int)((entity.Position.X + entity.Bounds.Width) / TILE_BOX_SIZE); x++) {
                                if (this.GetTile (x, y, 1).Mask.HasFlag (Tile.TileMask.COLLISION) || y == 0) {
                                    entity.Position.Y = (y + 1) * TILE_BOX_SIZE;
                                    entity.Velocity.Y = 0;
                                    moved = true;
                                    Log.Debug ("t", "collision");
                                    break;
                                }
                            }

                            // to prevent gliching
                            if ((entity.Position.X + entity.Bounds.Width) % TILE_BOX_SIZE != 0 && this.GetTile ((int)((entity.Position.X + entity.Bounds.Width) / TILE_BOX_SIZE), y, 1).Mask.HasFlag (Tile.TileMask.COLLISION)) {
                                entity.Position.Y = (y + 1) * TILE_BOX_SIZE;
                                entity.Velocity.Y = 0;
                                moved = true;
                                break;
                            }
                        }
                    }
                    if (!moved) {
                        entity.Position.Y += entity.Velocity.Y * time;
                    }
                    entity.Velocity.Y += (this.Gravity.Y + entity.Acceleration.Y) * time;
                }

                entity.Position.Y = Math.Min (Math.Max (0f, entity.Position.Y), Size.Height * TILE_BOX_SIZE - entity.Bounds.Height);
                entity.Position.X = Math.Min (Math.Max (0f, entity.Position.X), Size.Width * TILE_BOX_SIZE - entity.Bounds.Width);
            }
        }
    }
}
