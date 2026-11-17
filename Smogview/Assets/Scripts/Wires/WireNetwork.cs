using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Smogview.Wires
{
    public sealed class WireNetwork : MonoBehaviour
    {
        [Tooltip("Share one wire menu between the puzzles used by the same player")]
        public WireInteractionUI ui;
        [Tooltip("Collected from child wires at startup, manual lists still support older scene layouts")]
        public Wire[] wires = new Wire[0];
        public event Action Changed;

        private void Awake()
        {
            // Child wires belong to this puzzle, nested networks keep their own wires
            var children = GetComponentsInChildren<Wire>(true);
            if (children.Length > 0)
                wires = children.Where(w => w.GetComponentInParent<WireNetwork>(true) == this).ToArray();
        }
        private void Start() { Recalculate(); }

        public void Disconnect(Wire wire, bool notify = true)
        {
            if (wire == null) return;
            if (wire.Destination != null) wire.Destination.ConnectedWire = null;
            wire.Destination = null;
            if (notify) Recalculate();
        }

        public bool TryConnect(Wire wire, WireSocket socket, out string reason)
        {
            reason = "";
            if (wire == null || socket == null || wire.source == null || socket.box == null ||
                wire.source.network != this || socket.box.network != this)
                { reason = "That wire belongs to another circuit."; return false; }
            if (!wire.MatchesTarget(socket.box))
            {
                reason = wire.targetBox == null ? "This wire has no destination." : "This wire belongs at another box.";
                return false;
            }
            if (!socket.Accessible) { reason = "This socket is locked."; return false; }
            if (Vector2.Distance(wire.transform.position, socket.transform.position) > wire.maximumLength)
                { reason = "The wire cannot reach this socket."; return false; }
            if (Reaches(socket.box, wire.source, new HashSet<PowerBox>()))
                { reason = "This connection would loop back to its source."; return false; }
            // A wrong color may be plugged in; it remains unpowered until the player corrects it
            // Replacing an occupied socket returns its old lead to the source; do not leave two owners
            if (socket.ConnectedWire != null) Disconnect(socket.ConnectedWire, false);
            Disconnect(wire, false);
            wire.Destination = socket;
            socket.ConnectedWire = wire;
            if (wire.Carrier != null) wire.Carrier.ClearCarriedWire();
            Recalculate();
            return true;
        }

        private bool Reaches(PowerBox from, PowerBox target, HashSet<PowerBox> visited)
        {
            if (from == target) return true;
            if (!visited.Add(from)) return false;
            foreach (var lead in from.outputs)
                if (lead != null && lead.Destination != null && lead.Destination.box != null &&
                    Reaches(lead.Destination.box, target, visited)) return true;
            return false;
        }

        public void Recalculate()
        {
            // Rebuild from unpowered state so disconnected downstream circuits cannot retain stale power
            foreach (var wire in wires) if (wire != null) wire.Powered = false;
            // Multiple passes propagate supply through chained boxes regardless of array order
            // Rejected cycles plus this bound keep recalculation finite
            for (int pass = 0; pass <= wires.Length; pass++)
            {
                bool changed = false;
                foreach (var wire in wires)
                {
                    if (wire == null) continue;
                    bool powered = wire.source != null && wire.source.network == this && wire.source.Supplies(wire);
                    if (wire.Powered != powered) { wire.Powered = powered; changed = true; }
                }
                if (!changed) break;
            }
            Changed?.Invoke();
        }
    }
}
