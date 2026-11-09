using UnityEngine;

namespace Smogview.Wires
{
    public sealed class WireSocketVisual : MonoBehaviour
    {
        public WireSocket socket;
        public Wire breakerOutput;
        public LensManager lenses;
        public SpriteRenderer indicator;
        private void LateUpdate()
        {
            // Only Default reveals required socket/source colors; the relay intentionally has broken lamps.
            bool visible = lenses.CurrentLens == LensManager.lens.Ldefault && (socket == null || !socket.box.brokenInputLights);
            Color color = visible ? Wire.Tint(socket != null ? socket.requiredColor : breakerOutput.color) : new Color(.4f,.44f,.46f);
            bool powered = socket != null ? socket.Powered : breakerOutput.Powered;
            // Unpowered lamps remain dimly visible as matching clues; bright lamps indicate valid power.
            indicator.color = color * (powered ? 1f : .6f);
        }
    }
}
