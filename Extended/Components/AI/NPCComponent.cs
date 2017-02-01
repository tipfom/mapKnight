﻿using mapKnight.Core;

namespace mapKnight.Extended.Components.AI {
    public class NPCComponent : Component {
        private bool _Available = true;
        public bool Available {
            get {
                if (!_Available) {
                    Owner.SetComponentInfo(ComponentData.SpriteAnimation, "deny", true);
                    Owner.SetComponentInfo(ComponentData.SpriteAnimation, "idle", false);
                }
                return _Available;
            }
        }

        private string[ ] messages;
        private int currentIndex = -1;

        public NPCComponent (Entity owner, string[ ] messages) : base(owner) {
            owner.Domain = EntityDomain.NPC;
            this.messages = messages;
        }

        public override void Collision (Entity collidingEntity) {
            if (collidingEntity.Domain == EntityDomain.Player) {
                Owner.SetComponentInfo(ComponentData.Color, Color.Fuchsia);
            }
        }

        public string NextMessage ( ) {
            currentIndex++;
            if (currentIndex == messages.Length) {
                currentIndex = -1;
                _Available = false;
                Owner.SetComponentInfo(ComponentData.SpriteAnimation, "idle", true);
                return null;
            } else {
                Owner.SetComponentInfo(ComponentData.SpriteAnimation, "talk", true);
                Owner.SetComponentInfo(ComponentData.SpriteAnimation, "idle", false);
                return messages[currentIndex];
            }
        }

        public new class Configuration : Component.Configuration {
            public string[ ] Messages;

            public override Component Create (Entity owner) {
                return new NPCComponent(owner, Messages);
            }
        }
    }
}