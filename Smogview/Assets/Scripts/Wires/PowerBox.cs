using UnityEngine;
using System.Collections.Generic;

namespace Smogview.Wires
{
    /// <summary>Use the array order for the sockets and wires shown in the menu</summary>
    public class PowerBox : MonoBehaviour
    {
        public string displayName;
        [Tooltip("Uses the closest parent WireNetwork at startup, leave empty when the box is inside a network")]
        public WireNetwork network;
        [Tooltip("Optional socket order, leave empty to use child hierarchy order, new child sockets are appended at startup")]
        public WireSocket[] inputs = new WireSocket[0];
        [Tooltip("Optional wire order, leave empty to use child hierarchy order, new child wires are appended at startup")]
        public Wire[] outputs = new Wire[0];
        public bool breaker;
        public bool breakerOn;
        public bool requireAllInputs;
        // Other gameplay scripts can read IsPowered without saved event bindings
        public bool IsPowered => breaker ? breakerOn : AllInputsPowered;
        public virtual bool OutputsAccessible => isActiveAndEnabled;
        public virtual bool ShowOutputs => true;
        public virtual bool ShowSocketColors => true;
        public virtual bool RequiresTargetBox => true;

        protected virtual void Awake()
        {
            // Boxes inside a network find it through the hierarchy
            var parentNetwork = GetComponentInParent<WireNetwork>(true);
            if (parentNetwork != null) network = parentNetwork;

            // Preserve configured routing indices and append any newly added child ports
            var socketList = new List<WireSocket>(inputs ?? new WireSocket[0]);
            foreach (var socket in GetComponentsInChildren<WireSocket>(true))
            {
                if (socket.GetComponentInParent<PowerBox>(true) != this) continue;
                socket.box = this;
                if (!socketList.Contains(socket)) socketList.Add(socket);
            }
            inputs = socketList.ToArray();

            var wireList = new List<Wire>(outputs ?? new Wire[0]);
            foreach (var wire in GetComponentsInChildren<Wire>(true))
            {
                if (wire.GetComponentInParent<PowerBox>(true) != this) continue;
                wire.source = this;
                if (!wireList.Contains(wire)) wireList.Add(wire);
            }
            outputs = wireList.ToArray();
        }

        public void TurnOnBreaker()
        {
            if (!breaker) return;
            breakerOn = true;
            network?.Recalculate();
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
            // Wait for every input if Require All Inputs is on; otherwise, each input can power its own output
            if (requireAllInputs && !AllInputsPowered) return false;
            // Use -2 when this output should wait for every input to have matching power
            if (wire.sourceInput == -2) return AllInputsPowered;
            // Treat sourceInput as a socket index, not a color number
            return wire.sourceInput >= 0 && wire.sourceInput < inputs.Length &&
                inputs[wire.sourceInput] != null && inputs[wire.sourceInput].Powered;
        }
    }
}
