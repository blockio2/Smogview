using UnityEngine;

namespace Smogview.Wires
{
    public sealed class WireSocket : MonoBehaviour
    {
        public PowerBox box;
        public WireColor requiredColor;
        public bool behindGrate;
        [SerializeField] private Wire connectedWire;
        public Wire ConnectedWire { get => connectedWire; internal set => connectedWire = value; }
        // Connected alone is insufficient: the lead needs upstream power AND the matching color.
        public bool Powered => ConnectedWire != null && ConnectedWire.Powered && ConnectedWire.color == requiredColor;
        // Grate access governs changing a connection, not electricity through an existing connection.
        public bool Accessible => !behindGrate || box.controller == null || box.controller.GrateOpen;
    }
}
