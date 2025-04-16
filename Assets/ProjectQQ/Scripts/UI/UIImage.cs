using UnityEngine;
using UnityEngine.UI;

namespace QQ
{
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("UI/UIImage")]
    [ExecuteInEditMode]
    public class UIImage : MonoBehaviour, IMeshModifier
    {
        private Graphic graphic;
        private Image image;

        [SerializeField] private bool flipX;
        [SerializeField] private bool flipY;

        private void Awake()
        {
            image = GetComponent<Image>();
            graphic = GetComponent<Graphic>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(graphic != null)
            {
                graphic.SetVerticesDirty();
            }
        }
#endif

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