using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using LightType = UnityEngine.LightType;

namespace DaftAppleGames.Common.Buildings
{
    public class BuildingLights : MonoBehaviour
    {
        private List<Light> _pointLights;
        private List<Light> _spotLights;
        private List<Light> _areaLights;
        public Light directionalLight;

        // Start is called before the first frame update
        private void Start()
        {
            GetAllLights();
        }

        private void GetAllLights()
        {
            Light[] allLights = GetComponentsInChildren<Light>(true);

            _pointLights = new List<Light>();
            _spotLights = new List<Light>();
            _areaLights = new List<Light>();

            foreach (Light currentLight in allLights)
            {
                switch (currentLight.type)
                {
                    case LightType.Point:
                        _pointLights.Add(currentLight);
                        break;
                    case LightType.Spot:
                        _spotLights.Add(currentLight);
                        break;
                    case LightType.Area:
                        _areaLights.Add(currentLight);
                        break;
                }
            }
        }

        [Button("Turn on Points")]
        public void TurnOnPointLights()
        {
            SetLightsState(_pointLights, true);
        }

        [Button("Turn off Points")]
        public void TurnOffPointLights()
        {
            SetLightsState(_pointLights, false);
        }

        [Button("Turn on Spots")]
        public void TurnOnSpotLights()
        {
            SetLightsState(_spotLights, true);
        }

        [Button("Turn off Spots")]
        public void TurnOffSpotLights()
        {
            SetLightsState(_spotLights, false);
        }

        [Button("Turn on Areas")]
        public void TurnOnAreaLights()
        {
            SetLightsState(_areaLights, true);
        }

        [Button("Turn off Areas")]
        public void TurnOffAreaLights()
        {
            SetLightsState(_areaLights, false);
        }

        [Button("Turn on All")]
        public void TurnOnAllLights()
        {
            TurnOnPointLights();
        }

        [Button("Turn off All")]
        public void TurnOffAllLights()
        {
            TurnOffPointLights();
        }

        [Button("Turn on Directional")]
        public void TurnOnDirectionalLight()
        {
            directionalLight.enabled = true;
        }

        [Button("Turn off Directional")]
        public void TurnOffDirectionalLight()
        {
            directionalLight.enabled = false;
        }

        private void SetLightsState(List<Light> lights, bool state)
        {
            foreach (Light currentLight in lights)
            {
                currentLight.enabled = state;
            }
        }
    }
}