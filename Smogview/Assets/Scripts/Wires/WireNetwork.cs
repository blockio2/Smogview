using System;
using System.Collections.Generic;
using UnityEngine;

namespace Smogview.Wires
{
    public sealed class WireNetwork : MonoBehaviour
    {
        public Wire[] wires = new Wire[0];
        public event Action Changed;
        private void Start() { Recalculate(); }

        public void Disconnect(Wire wire, bool notify = true)
        {
            if (wire.Destination != null) wire.Destination.ConnectedWire = null;
            wire.Destination = null;
            if (notify) Recalculate();
        }

        public bool TryConnect(Wire wire, WireSocket socket, out string reason)
        {
            reason = "";
            if (wire == null || socket == null || wire.source.network != this || socket.box.network != this)
                { reason = "That wire belongs to another circuit."; return false; }
            if (!socket.Accessible) { reason = "Open the grate first."; return false; }
            if (Vector2.Distance(wire.transform.position, socket.transform.position) > wire.maximumLength)
                { reason = "The wire cannot reach this socket."; return false; }
            if (Reaches(socket.box, wire.source, new HashSet<PowerBox>()))
                { reason = "This connection would loop back to its source."; return false; }
            // A wrong color may be plugged in. It remains unpowered until the player corrects it.
            // Replacing an occupied socket returns its old lead to the source; do not leave two owners.
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
                if (lead.Destination != null && Reaches(lead.Destination.box, target, visited)) return true;
            return false;
        }

        public void Recalculate()
        {
            // Rebuild from unpowered state so disconnected downstream circuits cannot retain stale power.
            foreach (var wire in wires) wire.Powered = false;
            // Multiple passes propagate supply through chained boxes regardless of array order.
            // Rejected cycles plus this bound keep recalculation finite.
            for (int pass = 0; pass <= wires.Length; pass++)
            {
                bool changed = false;
                foreach (var wire in wires)
                {
                    bool powered = wire.source.Supplies(wire);
                    if (wire.Powered != powered) { wire.Powered = powered; changed = true; }
                }
                if (!changed) break;
            }
            Changed?.Invoke();
        }
    }
}
