using UnityEngine;
using UnityEngine.UI;

namespace QQ
{
    [AddComponentMenu("UI/UIImage")]
    public class UIImage : Image, IMeshModifier
    {
        private Graphic graphic;

        public bool flipX;
        public bool flipY;

        public void ModifyMesh(Mesh mesh) { }

        public void ModifyMesh(VertexHelper vh)
        {
            UIVertex vertex = default;

            // Flip
            for (var i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);

                vertex.position.x = flipX ? -vertex.position.x : vertex.position.x;
                vertex.position.y = flipY ? -vertex.position.y : vertex.position.y;

                vh.SetUIVertex(vertex, i);
            }
        }
    }
}