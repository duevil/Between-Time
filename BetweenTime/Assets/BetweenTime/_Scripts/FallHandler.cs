using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace BetweenTime._Scripts
{
    public class FallHandler : MonoBehaviour
    {
        private Vector3 _startPosition;
        private const float MaxDistance = 100f;

        [SerializeField] private Volume postProcessVolume;
        [SerializeField] private GameObject cameraBlocker;

        private Vignette _vignette;
        private Material _material;

        private void Start()
        {
            _startPosition = transform.position;
            _vignette = postProcessVolume.profile.TryGet<Vignette>(out var vignette) ? vignette : null;
            _material = cameraBlocker.GetComponent<Renderer>().material;
        }

        private void Update()
        {
            var dis = Mathf.Abs(transform.position.y - _startPosition.y);
            if (dis <= MaxDistance * 0.1f) return;

            var disExceeded = dis > MaxDistance * 1.1f;
            
            var intensity = disExceeded ? 0.0f : math.remap(MaxDistance * 0.1f, MaxDistance, 0.0f, 1.0f, dis);
            _vignette.intensity.value = intensity;
            var color = _material.color;
            _material.color = new Color(color.r, color.g, color.b, intensity);

            if (disExceeded) transform.position = _startPosition;
        }
    }
}