using System;
using mapKnight.Core;
using mapKnight.Extended.Components;
using Newtonsoft.Json;

namespace mapKnight.Extended {

    public abstract class Component {
        protected Entity Owner;

        public Component (Entity owner) {
            this.Owner = owner;
        }

        ~Component ( ) {
            Destroy( );
        }

        public static SerializationBinder SerializationBinder { get; } = new ComponentSerializationBinder( );

        public virtual void Collision (Entity collidingEntity) {
        }

        public virtual void Destroy ( ) {
        }

        public virtual void PostUpdate ( ) {
        }

        public virtual void Prepare ( ) {
        }

        public virtual void Tick ( ) {
        }

        public override string ToString ( ) {
            return this.GetType( ).Name;
        }

        public virtual void Update (DeltaTime dt) {
        }

        public abstract class Configuration {
            public readonly ComponentEnum Component;

            public Configuration ( ) {
                string shortedName = this.GetType( ).FullName.Substring(30).Replace("Component", "").Replace("+Configuration", "").Replace(".", "_").Replace("+", "_");
                if (!Enum.TryParse(shortedName, out Component)) {
                    Debug.Print(this, "Error parsing " + shortedName + "Component to an ComponentEnum Entry");
                    Component = ComponentEnum.None;
                }
            }

            public abstract Component Create (Entity owner);
        }

        private class ComponentSerializationBinder : SerializationBinder {

            public override void BindToName (Type serializedType, out string assemblyName, out string typeName) {
                assemblyName = null;
                typeName = serializedType.Name;
            }

            public override Type BindToType (string assemblyName, string typeName) {
                return Type.GetType($"mapKnight.Extended.Components.{ typeName }Component+Configuration");
            }
        }
    }
}