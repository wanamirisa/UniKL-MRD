using UnityEngine;

namespace UniKL
{
    public class Bulb : MonoBehaviour
    {
        [SerializeField] private float drainAmount = 1;

        [SerializeField] private Color onColor = Color.white;
        private Color offColor = Color.white;

        private MeshRenderer meshRenderer = null;

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        void Start()
        {
            offColor = meshRenderer.sharedMaterial.color;
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

        public void SendPower(ActionPayload payload)
        {
            // send power amount to battery
        }

        public void LightOff()
        {
            SetMaterialColor(offColor);
        }
    }
}
