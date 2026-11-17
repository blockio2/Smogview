using UnityEngine;

namespace Smogview.Wires
{
    public sealed class WireSocketVisual : MonoBehaviour
    {
        public WireSocket socket;
        public Wire breakerOutput;
        public LensManager lenses;
        public SpriteRenderer indicator;
        private void Start()
        {
            if (socket == null && breakerOutput == null) socket = GetComponentInParent<WireSocket>(true);
            if (indicator == null) indicator = GetComponent<SpriteRenderer>();
            var box = socket != null ? socket.box : breakerOutput != null ? breakerOutput.source : null;
            if (box != null && box.network != null && box.network.ui != null) lenses = box.network.ui.lenses;
        }
        private void LateUpdate()
        {
            if (lenses == null || indicator == null || (socket == null && breakerOutput == null) ||
                (socket != null && socket.box == null)) return;
            // Show the required colors in Default
            bool visible = lenses.CurrentLens == LensManager.lens.Ldefault && (socket == null || socket.box.ShowSocketColors);
            Color color = visible ? Wire.Tint(socket != null ? socket.requiredColor : breakerOutput.color) : new Color(.4f,.44f,.46f);
            bool powered = socket != null ? socket.Powered : breakerOutput.Powered;
            // Keep unpowered lamps dim as a color clue and brighten them when they have matching power
            indicator.color = color * (powered ? 1f : .6f);
        }
    }
}
