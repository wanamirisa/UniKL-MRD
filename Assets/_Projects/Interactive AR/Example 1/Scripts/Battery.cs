using System;
using UnityEngine;
using UnityEngine.Events;

namespace UniKL
{
    public class Battery : MonoBehaviour
    {
        [SerializeField] private MeshRenderer batteryMesh = null;

        [SerializeField] Color maxColor = Color.green;
        [SerializeField] Color minColor = Color.red;

        [SerializeField] private float maxLevel = 100;
        [SerializeField] private float currentLevel = 0;

        [SerializeField] UnityEvent OnBatteryDepleted;
        [SerializeField] UnityEvent OnBatteryFull;

        void Start()
        {
            currentLevel = maxLevel;
            batteryMesh.material.color = maxColor;
        }

        public void DrainBattery(ActionPayload payload)
        {
            if (payload.Source.TryGetComponent(out Bulb bulb))
            {
                currentLevel -= bulb.drainAmount * Time.deltaTime;
                currentLevel = Mathf.Clamp(currentLevel, 0, maxLevel);

                if (currentLevel <= 0)
                {
                    OnBatteryDepleted?.Invoke();
                }

                ChangeMaterialColorOvertime(currentLevel);
            }
        }

        public void RechargeBattery(ActionPayload payload)
        {
            if (payload.Source.TryGetComponent(out Charger charger))
            {
                currentLevel += charger.chargeAmount * Time.deltaTime;
                currentLevel = Mathf.Clamp(currentLevel, 0, maxLevel);

                if (currentLevel >= maxLevel)
                {
                    OnBatteryFull?.Invoke();
                }

                ChangeMaterialColorOvertime(currentLevel);
            }
        }

        private void ChangeMaterialColorOvertime(float currentLevel)
        {
            // normalize currentLevel to 0 to 1
            float normalizedLevel = Mathf.Clamp01(currentLevel / maxLevel);
            batteryMesh.material.color = Color.Lerp(minColor, maxColor, normalizedLevel);
        }
    }
}
