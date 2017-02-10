﻿using System;

namespace mapKnight.Extended.Components.AI {
    public class ButtonComponent : Component {
        private Action<Entity> onTriggerAction;
        private bool isDown;

        public ButtonComponent (Entity owner, Action<Entity> onTriggerAction) : base(owner) {
            this.onTriggerAction = onTriggerAction;
        }

        public override void Collision (Entity collidingEntity) {
            if (!isDown && collidingEntity.Domain == EntityDomain.Player) {
                isDown = true;
                onTriggerAction.Invoke(Owner);
                Owner.SetComponentInfo(ComponentData.SpriteAnimation, "down", true);
            }
        }

        public new class Configuration : Component.Configuration {
            public Action<Entity> OnTrigger;

            public override Component Create (Entity owner) {
                return new ButtonComponent(owner, OnTrigger);
            }
        }
    }
}