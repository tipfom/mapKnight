﻿using OpenTK.Graphics.ES20;

namespace mapKnight.Graphics.Handle {
    public class AttributeHandle {
        public int Location { get; private set; }

        public AttributeHandle (int program, string name) {
            Location = GL.GetAttribLocation(program, name);
        }

        public void Enable ( ) {
            GL.EnableVertexAttribArray(Location);
        }

        public void Disable ( ) {
            GL.DisableVertexAttribArray(Location);
        }

        public void Set (float[ ] data, int size) {
            GL.VertexAttribPointer(Location, size, VertexAttribPointerType.Float, false, 0, data);
        }
    }
}
