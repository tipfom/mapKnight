using System.Collections.Generic;
using mapKnight.Core;
using mapKnight.Extended.Graphics.UI.Layout;

namespace mapKnight.Extended.Graphics.UI {
    public class UIImage : UIItem {
        private string textureIdle;
        private string textureClick;
        private Color _ModificationColor;
        public Color ModificationColor { get { return _ModificationColor; } set { _ModificationColor = value; IsDirty = true; } }

        public UIImage (Screen owner, UILayout layout, string idletexture, string clicktexture, int depth, Color modificationcolor) : base(owner, layout, depth) {
            textureIdle = idletexture;
            textureClick = clicktexture;
            _ModificationColor = modificationcolor;

            Click += ( ) => IsDirty = true; 
            Release += ( ) => IsDirty = true; 
            Leave += ( ) => IsDirty = true;
            IsDirty = true;
        }

        public override IEnumerable<DepthVertexData> ConstructVertexData ( ) {
            yield return new DepthVertexData(Layout, this.Clicked ? textureClick : textureIdle, Depth, _ModificationColor);
        }
    }
}