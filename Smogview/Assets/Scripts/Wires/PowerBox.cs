using UnityEngine;

namespace Smogview.Wires
{
    /// <summary>Inspector-configured routing; array order is also the physical/UI left-to-right order.</summary>
    public sealed class PowerBox : MonoBehaviour
    {
        public string displayName;
        public WireNetwork network;
        public PuzzleOnePowerController controller;
        public WireSocket[] inputs = new WireSocket[0];
        public Wire[] outputs = new Wire[0];
        public bool breaker;
        public bool breakerOn;
        public bool requireAllInputs;
        public bool brokenInputLights;
        public bool outputsBehindGrate;
        public bool outputsViaTangleOnly;
        public bool shuffleOutputsOnStart;
        private void Start()
        {
            if (!shuffleOutputsOnStart) return;
            // Shuffle channel/route pairs once, retaining physical port positions throughout the attempt.
            for (int i = outputs.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                var color = outputs[i].color; int route = outputs[i].sourceInput;
                outputs[i].color = outputs[j].color; outputs[i].sourceInput = outputs[j].sourceInput;
                outputs[j].color = color; outputs[j].sourceInput = route;
            }
            network.Recalculate();
        }
        public bool AllInputsPowered
        {
            get
            {
                if (inputs.Length == 0) return false;
                foreach (var socket in inputs) if (socket == null || !socket.Powered) return false;
                return true;
            }
        }

        public bool Supplies(Wire wire)
        {
            if (!wire.enabledOutput) return false;
            if (wire.sourceInput == -1) return breaker && breakerOn;
            // The closet waits for every input; ordinary relays forward each channel independently.
            if (requireAllInputs && !AllInputsPowered) return false;
            // The green supply is independent of the computer's R/Y/B output switches.
            if (wire.sourceInput == -2) return AllInputsPowered;
            // sourceInput is this box's input-array index, not the numeric value of WireColor.
            return wire.sourceInput >= 0 && wire.sourceInput < inputs.Length && inputs[wire.sourceInput].Powered;
        }
    }
}
