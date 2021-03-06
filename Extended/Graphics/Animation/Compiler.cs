﻿using System;
using System.Collections.Generic;
using System.Linq;
using mapKnight.Core;
using mapKnight.Core.Graphics;
using mapKnight.Core.World;

namespace mapKnight.Extended.Graphics.Animation {
    public static class Compiler {
        /*
         steps : 
            boneverticies generieren
            texture erstellen
         */

        public static void Compile (string[ ] defaultTextures, float[ ] scales, Vector2[ ] offsets, IEnumerable<string> sprites, Entity entity, out float[ ][ ] boneverticies, out Spritebatch2D sprite) {
            sprite = Spritebatch2D.Combine(false, new List<Spritebatch2D>(sprites.Select(item => Assets.Load<Spritebatch2D>(item))));
#if DEBUG
            Debug.CheckGL(typeof(Compiler));
#endif
            boneverticies = new float[scales.Length][ ];
            float entityratio = entity.Transform.Width / entity.Transform.Height;
            for (int i = 0; i < boneverticies.Length; i++) {
                float[ ] texture = sprite[defaultTextures[i]];
                Vector2 textureSize = new Vector2(Math.Abs(texture[4] - texture[0]) * sprite.Width, Math.Abs(texture[1] - texture[3]) * sprite.Height);
                Vector2 vertexSize = new Vector2(textureSize.X * scales[i] / 2f, textureSize.Y * scales[i] / 2f * entityratio);
                Vector2 offset = (offsets[i] - textureSize / 2f) / (textureSize / 2f) * vertexSize;

                boneverticies[i] = new[ ] {
                    -vertexSize.X - offset.X,  vertexSize.Y + offset.Y,
                    -vertexSize.X - offset.X, -vertexSize.Y + offset.Y,
                     vertexSize.X - offset.X, -vertexSize.Y + offset.Y,
                     vertexSize.X - offset.X,  vertexSize.Y + offset.Y
                };
            }
        }
    }
}
