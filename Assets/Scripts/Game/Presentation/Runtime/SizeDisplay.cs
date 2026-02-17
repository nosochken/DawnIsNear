using Game.Gameplay;
using UnityEngine;

[RequireComponent(typeof(Absorber))]
public class SizeDisplay : MonoBehaviour
{
    private const float ScalePerSize = 0.3f;

    private Absorber _absorber;

    private void Awake()
    {
        _absorber = GetComponent<Absorber>();
    }

    private void OnEnable()
    {
        _absorber.SizeChanged += UpdateVisuals;
    }

    private void OnDisable()
    {
        _absorber.SizeChanged -= UpdateVisuals;
    }

    private void UpdateVisuals(int size)
    {
        float scale = Mathf.Max(_absorber.MinSize + Mathf.Sqrt(size) * ScalePerSize, _absorber.MinSize);
        transform.localScale = Vector3.one * scale;
    }
}