using UnityEngine;

namespace Smogview.Wires
{
    [RequireComponent(typeof(LineRenderer))]
    public sealed class WireVisual : MonoBehaviour
    {
        public Wire wire;
        public LensSwitching lenses;
        public PuzzleOnePowerController controller;
        private LineRenderer line;
        private void Awake() { line = GetComponent<LineRenderer>(); }
        private void LateUpdate()
        {
            var start = wire.transform.position;
            var end = wire.EndPosition;
            start.z = end.z = -2;
            // A simple three-point cable preview; it does not simulate rope collisions or routed length.
            line.positionCount = 3;
            line.SetPosition(0, start);
            line.SetPosition(1, (start + end) * .5f + Vector3.down * .22f);
            line.SetPosition(2, end);
            // Mental reveals channel identity even on an unpowered lead; color is not a power indicator.
            bool reveal = controller.MentalUnlocked && lenses.CurrentLens == LensSwitching.lens.Lmental;
            Color tint = reveal ? Wire.Tint(wire.color) : new Color(.66f, .71f, .73f);
            line.startColor = line.endColor = tint;
        }
    }
}
