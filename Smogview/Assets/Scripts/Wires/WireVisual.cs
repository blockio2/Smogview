using UnityEngine;

namespace Smogview.Wires
{
    [RequireComponent(typeof(LineRenderer))]
    public sealed class WireVisual : MonoBehaviour
    {
        public Wire wire;
        public LensManager lenses;
        private LineRenderer line;
        private void Awake()
        {
            line = GetComponent<LineRenderer>();
            if (wire == null) wire = GetComponent<Wire>();
        }
        private void Start()
        {
            var network = wire != null && wire.source != null ? wire.source.network : null;
            if (network != null && network.ui != null) lenses = network.ui.lenses;
        }
        private void LateUpdate()
        {
            if (wire == null || wire.source == null || lenses == null) return;
            var start = wire.transform.position;
            var end = wire.EndPosition;
            start.z = end.z = -2;
            // Draw a simple three-point wire without simulating rope collisions
            line.positionCount = 3;
            line.SetPosition(0, start);
            line.SetPosition(1, (start + end) * .5f + Vector3.down * .22f);
            line.SetPosition(2, end);
            // Show the wire color in Mental even when the wire has no power
            bool reveal = lenses.CurrentLens == LensManager.lens.Lmental;
            Color tint = reveal ? Wire.Tint(wire.color) : new Color(.66f, .71f, .73f);
            line.startColor = line.endColor = tint;
        }
    }
}
