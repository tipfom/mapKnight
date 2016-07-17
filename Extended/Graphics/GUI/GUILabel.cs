using System.Collections.Generic;
using mapKnight.Core;

namespace mapKnight.Extended.Graphics.GUI {
    public class GUILabel : GUIItem {
        public const int CHAR_WIDTH_PIXEL = 7;
        public const int CHAR_HEIGHT_PIXEL = 9;
        public const int CHAR_SPACING_PIXEL = 1;
        public const int DEFAULT_TEXT_DEPTH = -2;

        private string _Text;
        public string Text {
            get { return _Text; }
            set { _Text = value; RequestUpdate( ); }
        }
        private Color _Color;
        public Color Color {
            get { return _Color; }
            set { _Color = value; RequestUpdate( ); }
        }

        readonly float charSize;

        public GUILabel (Screen owner, Vector2 position, float size, string text) : this(owner, position, DEFAULT_TEXT_DEPTH, size, text) {

        }

        public GUILabel (Screen owner, Vector2 position, int depth, float size, string text) : this(owner, position, depth, size, Color.White, text) {

        }

        public GUILabel (Screen owner, Vector2 position, int depth, float size, Color color, string text) : base(owner, new Rectangle(position, new Vector2(0f, 0f)), depth) {
            // label needs no touch management
            this._Text = text;
            this._Color = color;
            this.charSize = size;
        }

        public override List<VertexData> GetVertexData ( ) {
            return GetVertexData(this.Text, this.Position, this.charSize, DepthOnScreen, this.Color);
        }

        public Vector2 MeasureText ( ) {
            return MeasureText(Text, this.charSize);
        }


        public static List<VertexData> GetVertexData (string text, Vector2 position, float charSize, int depth, Color color) {
            List<VertexData> vertexData = new List<VertexData>( );
            charSize *= 2; // scale to screen size

            Vector2 currentPoint = position;
            foreach (char character in text.ToUpper( )) {
                float characterWidth = GetCharScale(character) * charSize;
                if (character == '\n') {
                    currentPoint.X = position.X;
                    currentPoint.Y -= charSize;
                } else if (character == ' ') {
                    currentPoint.X += charSize;
                } else {
                    vertexData.Add(new VertexData(
                        new float[ ] {
                            currentPoint.X ,currentPoint.Y,
                            currentPoint.X,currentPoint.Y - charSize,
                            currentPoint.X+ characterWidth,currentPoint.Y - charSize,
                            currentPoint.X+ characterWidth,currentPoint.Y},
                        character.ToString( ),
                        depth,
                        color
                        ));
                    currentPoint.X += characterWidth;
                }
            }

            return vertexData;
        }

        public static Vector2 MeasureText (string text, float charSize) {
            charSize *= 2;

            float maxWidth = 0;
            float currentWidth = 0;
            int height = 1;
            foreach (char character in text.ToUpper( )) {
                if (character == '\n') {
                    height++;
                    if (currentWidth > maxWidth)
                        maxWidth = currentWidth;
                    currentWidth = 0;
                } else {
                    currentWidth += GetCharScale(character) * charSize;
                }
            }
            if (currentWidth > maxWidth)
                maxWidth = currentWidth;

            return new Vector2(maxWidth, charSize * height);
        }

        private static float GetCharScale (char character) {
            float[ ] verticies = GUIRenderer.Texture.Get(character.ToString( ));
            return ((verticies[4] - verticies[0]) * GUIRenderer.Texture.Width) / ((verticies[3] - verticies[1]) * GUIRenderer.Texture.Height);
        }
    }
}