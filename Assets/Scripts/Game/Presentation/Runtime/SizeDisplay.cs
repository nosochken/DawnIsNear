using Game.Gameplay;
using UnityEngine;

namespace Game.Presentation
{
    [RequireComponent(typeof(Unit))]
    public class SizeDisplay : MonoBehaviour
    {
        private const float ScalePerSize = 0.3f;

        private Unit _unit;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        private void OnEnable()
        {
            _unit.Body.Size.Changed += UpdateVisuals;
        }

        private void OnDisable()
        {
            _unit.Body.Size.Changed -= UpdateVisuals;
        }

        private void UpdateVisuals(int size)
        {
            float scale = Mathf.Max(_unit.Body.Size.Min + Mathf.Sqrt(size) * ScalePerSize, _unit.Body.Size.Min);
            transform.localScale = Vector3.one * scale;
        }
    }
}