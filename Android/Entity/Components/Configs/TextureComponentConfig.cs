namespace mapKnight.Android.Entity.Components.Configs {
    class TextureComponentConfig : Component.Config {
        public override int Priority { get { return 1; } }

        public string Texture;

        public override Component Create (Entity owner) {
            return new TextureComponent (owner, Texture);
        }
    }
}