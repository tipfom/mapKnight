﻿using System;
using mapKnight.Core;
using mapKnight.Core.World;
using mapKnight.Extended.Components.Movement;
using mapKnight.Extended.Components.Player;
using mapKnight.Extended.Graphics;
using mapKnight.Extended.Graphics.UI;
using mapKnight.Extended.Graphics.UI.Layout;
using Map = mapKnight.Extended.Graphics.Map;
using mapKnight.Core.World.Components;
using System.Collections.Generic;
using mapKnight.Core.World.Serialization;
using mapKnight.Extended.Combat;

namespace mapKnight.Extended.Screens {

    public class GameplayScreen : Screen {
        private const int MAX_TIME_BETWEEN_UPDATES = 100;

        private UILabel debugLabel;
        private UIControlButton leftButton, rightButton;
        private UIGesturePanel controlPanel;
        private UIBar healthBar;

        private Map map;
        private Entity playerEntity;
        private PlayerComponent playerComponent;
        private string mapName;

        public GameplayScreen (string mapName) {
            this.mapName = mapName;
        }

        public override void Draw ( ) {
            map.Draw( );
            base.Draw( );
        }

        public override void Load ( ) {
            map = Assets.Load<Map>(mapName);
            for (int i = 0; i < map.Entities.Count; i++) {
                map.Entities[i].Prepare( );
            }
            map.EntityAdded += (Entity obj) => { obj.Prepare( ); };

            playerEntity = EntityCollection.Players.Diamond.Create(map.SpawnPoint, map);
            playerComponent = playerEntity.GetComponent<PlayerComponent>( );
            map.Focus(playerEntity.ID);

            debugLabel = new UILabel(this, new UILayout(new UIMargin(0.1f, 0.075f), UIMarginType.Absolute, UIPosition.Right | UIPosition.Top, UIPosition.Right | UIPosition.Top), 0.05f, "", UITextAlignment.Right);
            healthBar = new UIBar(this, new Color(255, 0, 0, 127), new Color(255, 255, 255, 63), playerComponent.Health, new UILayout(new UIMargin(0, 1, 0, 0.025f), UIMarginType.Relative, UIPosition.Left | UIPosition.Top), UIDepths.MIDDLE);

            SetupControls( );
            base.Load( );
        }

        private void SetupControls ( ) {
            controlPanel = new UIGesturePanel(this, new UILayout(new UIMargin(0, 6f / 5f * Window.Ratio - 0.15f, 0f, 2f), UIMarginType.Absolute), Assets.GetGestureStore("gestures"));
            controlPanel.OnGesturePerformed += (string gesture) => {
                playerEntity.SetComponentInfo(ComponentData.InputGesture, gesture);
#if DEBUG
                global::Android.Widget.Toast.MakeText(Assets.Context, gesture, global::Android.Widget.ToastLength.Short).Show( );
#endif
            };

            leftButton = new UIControlButton(this, "l", new UILayout(new UIMargin(Window.Ratio * 2f / 5f, Window.Ratio * 2f / 5f + .1f, Window.Ratio * 2f / 5f, .05f), UIMarginType.Absolute, UIPosition.Right | UIPosition.Bottom, UIPosition.Right | UIPosition.Bottom));
            leftButton.Click += ( ) => playerEntity.SetComponentInfo(ComponentData.InputInclude, ActionMask.Left);
            leftButton.Release += ( ) => playerEntity.SetComponentInfo(ComponentData.InputExclude, ActionMask.Left);
            leftButton.Leave += ( ) => playerEntity.SetComponentInfo(ComponentData.InputExclude, ActionMask.Left);

            rightButton = new UIControlButton(this, "r", new UILayout(new UIMargin(Window.Ratio * 2f / 5f, .05f, Window.Ratio * 2f / 5f, .05f), UIMarginType.Absolute, UIPosition.Right | UIPosition.Bottom, UIPosition.Right | UIPosition.Bottom));
            rightButton.Click += ( ) => playerEntity.SetComponentInfo(ComponentData.InputInclude, ActionMask.Right);
            rightButton.Release += ( ) => playerEntity.SetComponentInfo(ComponentData.InputExclude, ActionMask.Right);
            rightButton.Leave += ( ) => playerEntity.SetComponentInfo(ComponentData.InputExclude, ActionMask.Right);
        }

        public override void Update (DeltaTime dt) {
            if (Math.Abs(Manager.FrameTime.Milliseconds) < MAX_TIME_BETWEEN_UPDATES) {
                map.Update(dt);
                base.Update(dt);
                debugLabel.Color = Color.White;
            } else {
                debugLabel.Color = Color.Red;
            }
            debugLabel.Text = $"frame: {Manager.FrameTime.TotalMilliseconds:00.0}\n" +
                              $"update: {Manager.UpdateTime.TotalMilliseconds:00.0}\n" +
                            $"draw: {Manager.DrawTime.TotalMilliseconds:00.0}\n";
        }
    }
}