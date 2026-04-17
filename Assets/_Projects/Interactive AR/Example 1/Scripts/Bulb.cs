using UnityEngine;

namespace UniKL
{
    public class Bulb : MonoBehaviour
    {
        public float drainAmount = 1;


        [SerializeField] private MeshRenderer meshRenderer = null;
        [SerializeField] private Color onColor = Color.yellow;
        [SerializeField] private Color offColor = Color.white;

        void Start()
        {
            LightOff();
        }

        private void SetMaterialColor(Color color)
        {
            if (!meshRenderer) return;

            meshRenderer.sharedMaterial.color = color;
        }

        public void LightOn()
        {
            SetMaterialColor(onColor);
        }

        public void LightOff()
        {
            SetMaterialColor(offColor);
        }
    }
}
