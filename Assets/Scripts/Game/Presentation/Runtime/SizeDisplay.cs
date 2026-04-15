using System;
using Game.Gameplay;
using UnityEngine;

namespace Game.Presentation
{
    [RequireComponent(typeof(Body))]
    public class SizeDisplay : MonoBehaviour
    {
        private const float BaseScale = 1f;
        private const float ScalePerSize = 0.3f;
        private const float SmoothSpeed = 8f;

        private Body _body;
        private float _targetScale;

        private void Awake()
        {
            _body = GetComponent<Body>();
        }

        private void OnEnable()
        {
            _body.Size.Changed += UpdateVisuals;
            UpdateVisuals(_body.Size.Current);
            transform.localScale = Vector3.one * _targetScale;
        }

        private void Update()
        {
            float currentScale = transform.localScale.x;
            float nextScale = Mathf.Lerp(currentScale, _targetScale, SmoothSpeed * Time.deltaTime);
            transform.localScale = Vector3.one * nextScale;
        }

        private void OnDisable()
        {
            _body.Size.Changed -= UpdateVisuals;
        }

        private void UpdateVisuals(int size)
        {
            _targetScale = BaseScale + size * ScalePerSize;
        }
    }
}