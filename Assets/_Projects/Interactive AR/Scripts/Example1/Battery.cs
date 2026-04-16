using UnityEngine;

namespace UniKL
{
    public class Battery : MonoBehaviour
    {
        [SerializeField] private float drainAmount = 1;
        [SerializeField] private float currentLevel = 50;
        private float maxLevel = 100;

        private MeshRenderer meshRenderer;

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void DrainBattery()
        {
            currentLevel -= drainAmount * Time.deltaTime;
            print("DRAIN BATTERY");
        }
    }
}
