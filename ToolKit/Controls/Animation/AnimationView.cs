﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using mapKnight.ToolKit.Controls.Xna;
using Microsoft.Xna.Framework;
using Vector2 = mapKnight.Core.Vector2;
using Microsoft.Xna.Framework.Graphics;
using mapKnight.ToolKit.Data;

namespace mapKnight.ToolKit.Controls.Animation {
    public class AnimationView : XnaControl {
        private DispatcherTimer renderTimer = new DispatcherTimer( ) { Interval = new TimeSpan(0,0,0,0,15), IsEnabled = false };
        private VertexAnimation currentAnimation;
        private bool paused = false;
        private Dictionary<string, Texture2D> textures;
        private Dictionary<string, Microsoft.Xna.Framework.Vector2> transformOrigins;
        private Texture2D entityTexture;
        private Texture2D groundTexture;
        private float entityRatio;
        private int nextFrame;
        private int nextFrameTime;
        private int currentFrame;
        private int pauseBegin;

        public AnimationView ( ) {
            renderTimer.Tick += (sender, e) => Update( );
            base.DeviceInitialized += ( ) => CreateEmptyTexture( );
            this.IsVisibleChanged += AnimationView_IsVisibleChanged;
        }

        private void AnimationView_IsVisibleChanged (object sender, System.Windows.DependencyPropertyChangedEventArgs e) {
            Pause( );
        }

        private void CreateEmptyTexture ( ) {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            entityTexture = new Texture2D(GraphicsDevice, 1, 1);
            entityTexture.SetData(new Color[ ] { new Color(51, 153, 255, 255) });

            groundTexture = new Texture2D(GraphicsDevice, 1, 1);
            groundTexture.SetData(new Color[ ] { new Color(0, 153, 51, 255) });
        }

        public void Play (VertexAnimation animation, float entityratio, Dictionary<string, BoneImage.ImageData> imagedata) {
            paused = false;
            textures = imagedata.ToDictionary(entry => entry.Key, entry => entry.Value.Image.ToTexture2D(GraphicsDevice));
            transformOrigins = imagedata.ToDictionary(entry => entry.Key, entry => new Microsoft.Xna.Framework.Vector2((float)entry.Value.TransformOrigin.X, (float)entry.Value.TransformOrigin.Y));
            currentAnimation = animation;
            entityRatio = entityratio;
            Reset( );
            renderTimer.Start( );
        }

        public void Reset ( ) {
            if (currentAnimation == null) return;
            currentFrame = 0;
            nextFrame = Math.Min(1, currentAnimation.Frames.Count - 1);
            nextFrameTime = Environment.TickCount + currentAnimation.Frames[currentFrame].Time;
        }

        public void Stop ( ) {
            renderTimer.Stop( );
        }

        public void Pause ( ) {
            paused = !paused;
            if (paused) {
                renderTimer.Stop( );
                pauseBegin = Environment.TickCount;
            } else {
                renderTimer.Start( );
                nextFrameTime += Environment.TickCount - pauseBegin;
            }
        }

        protected override void Render (SpriteBatch spriteBatch) {
            renderTimer.Stop( );
            if (paused || currentAnimation == null) return;

            // draw player and ground
            Rectangle entityDrawRectangle = new Rectangle( );
            if (entityRatio > 1f) {
                // width > height
                entityDrawRectangle.Width = (int)(0.75 * RenderSize.Width);
                entityDrawRectangle.Height = (int)(entityDrawRectangle.Width / entityRatio);
            } else {
                // height > width
                entityDrawRectangle.Height = (int)(0.75 * RenderSize.Height);
                entityDrawRectangle.Width = (int)(entityRatio * entityDrawRectangle.Height);
            }
            entityDrawRectangle.Location = new Point((int)(RenderSize.Width / 2 - entityDrawRectangle.Width / 2), (int)(RenderSize.Height / 2 - entityDrawRectangle.Height / 2));

            spriteBatch.Draw(entityTexture, entityDrawRectangle, Color.White);
            spriteBatch.Draw(groundTexture, new Rectangle(0, entityDrawRectangle.Bottom, (int)RenderSize.Width, 10), Color.White);

            foreach (VertexBone entry in InterpolateAnimation( )) {
                Texture2D texture = textures[entry.Image];
                int width = (int)(entry.Scale * texture.Width * entityDrawRectangle.Width);
                int height = (int)(entry.Scale * texture.Height * entityDrawRectangle.Width);
                int x = (int)(entityDrawRectangle.Center.X + entry.Position.X * entityDrawRectangle.Width);// - (0.5f - transformOrigins[entry.Image].X) * width);
                int y = (int)(entityDrawRectangle.Center.Y - entry.Position.Y * entityDrawRectangle.Height);// - (0.5f - transformOrigins[entry.Image].Y) * height);

                spriteBatch.Draw(texture, new Rectangle(x, y, width, height), null, Color.White, (float)(entry.Rotation * Math.PI / 180f), texture.Bounds.Size.ToVector2( ) * transformOrigins[entry.Image], entry.Mirrored ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            renderTimer.Start( );
        }

        private IEnumerable<VertexBone> InterpolateAnimation ( ) {
            if (Environment.TickCount > nextFrameTime) {
                if (nextFrame + 1 < currentAnimation.Frames.Count) {
                    // if the next Frame isnt the last ont
                    currentFrame = nextFrame;
                    nextFrame++;
                    nextFrameTime += currentAnimation.Frames[currentFrame].Time;
                } else if (currentAnimation.CanRepeat) {
                    currentFrame = nextFrame;
                    nextFrame = 0;
                    nextFrameTime += currentAnimation.Frames[currentFrame].Time;
                }
            }
            float progress = Core.Mathf.Clamp01((nextFrameTime - Environment.TickCount) / (float)currentAnimation.Frames[currentFrame].Time);

            for (int i = currentAnimation.Frames[0].Bones.Count-1; i > -1; i--) {
                Vector2 interpolatedPosition = Interpolate(currentAnimation.Frames[nextFrame].Bones[i].Position, currentAnimation.Frames[currentFrame].Bones[i].Position, progress);
                float interpolatedRotation = Interpolate(currentAnimation.Frames[nextFrame].Bones[i].Rotation, currentAnimation.Frames[currentFrame].Bones[i].Rotation, progress);
                yield return new VertexBone( ) { Position = interpolatedPosition, Rotation = interpolatedRotation, Mirrored = currentAnimation.Frames[nextFrame].Bones[i].Mirrored, Image = currentAnimation.Frames[nextFrame].Bones[i].Image, Scale = currentAnimation.Frames[nextFrame].Bones[i].Scale };
            }
        }

        private Vector2 Interpolate (Vector2 v1, Vector2 v2, float progress) {
            return new Vector2(v1.X + (v2.X - v1.X) * progress, v1.Y + (v2.Y - v1.Y) * progress);
        }

        private float Interpolate (float v1, float v2, float progress) {
            return v1 + (v2 - v1) * progress;
        }
    }
}
