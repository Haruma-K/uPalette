using UnityEngine;
using uPalette.Runtime.Core.Synchronizer.Color;

[DisallowMultipleComponent]
[RequireComponent(typeof(TestImage))]
[ColorSynchronizer(typeof(TestImage), "Color")]
public sealed class TestSynchronizer : ColorSynchronizer<TestImage>
{
    [SerializeField] [Header("alphaを同期するか")]
    private bool _syncAlpha;

    public bool SyncAlpha
    {
        get => _syncAlpha;
        set => _syncAlpha = value;
    }

    protected override Color GetValue()
    {
        return Component.color;
    }

    protected override void SetValue(Color value)
    {
        Component.color = value;
    }
}