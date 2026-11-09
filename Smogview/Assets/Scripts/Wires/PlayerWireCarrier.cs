using UnityEngine;
using UnityEngine.InputSystem;

namespace Smogview.Wires
{
    /// <summary>Wire carrying deliberately does not use Interactable.BeingHeld, which stops walking.</summary>
    public sealed class PlayerWireCarrier : MonoBehaviour
    {
        public LensManager lenses;
        public WireInteractionUI ui;
        public Wire CarriedWire { get; private set; }
        // Switching lenses does not drop a held lead; only picking up/inserting requires Physical.
        public bool CanHandle => lenses != null && lenses.CurrentLens == LensManager.lens.Lphysical;

        public bool PickUp(Wire wire)
        {
            if (!CanHandle || wire == null || !wire.movable || CarriedWire != null) return false;
            if (wire.source.outputsBehindGrate && !wire.source.controller.GrateOpen) return false;
            if (wire.Carrier != null && wire.Carrier != this) return false;
            wire.source.network.Disconnect(wire);
            CarriedWire = wire;
            wire.Carrier = this;
            return true;
        }

        public bool Insert(WireSocket socket)
        {
            if (!CanHandle || CarriedWire == null) return false;
            if (CarriedWire.source.network.TryConnect(CarriedWire, socket, out string reason)) return true;
            ui?.Notify(reason);
            return false;
        }

        public void ClearCarriedWire()
        {
            if (CarriedWire != null) CarriedWire.Carrier = null;
            CarriedWire = null;
        }

        public void ReturnWire()
        {
            ClearCarriedWire();
            ui?.Notify("Wire returned to its box.");
        }

        public void CheckReach()
        {
            // Prototype reach is straight-line source-to-player distance, not rope length around walls.
            if (CarriedWire != null && Vector2.Distance(transform.position, CarriedWire.transform.position) > CarriedWire.maximumLength)
                ReturnWire();
        }

        private void Update()
        {
            CheckReach();
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame && CanHandle) ReturnWire();
        }
        private void OnDisable() { ClearCarriedWire(); }
    }
}
