using UnityEngine;

namespace Smogview.Wires
{
    /// <summary>Open a wire box through the existing E interaction</summary>
    public class WireStation : Interactable
    {
        [Header("Resolved from the parent box and network at startup")]
        public PowerBox box;
        public PlayerWireCarrier carrier;
        public WireInteractionUI ui;

        protected virtual void Start()
        {
            if (box == null) box = GetComponentInParent<PowerBox>(true);
            if (box != null && box.network != null && box.network.ui != null) ui = box.network.ui;
            if (ui != null) carrier = ui.carrier;
        }

        public override bool CanInteract => base.CanInteract && box != null && ui != null && ui.Ready && !ui.IsOpen &&
            (ui.movement == null || ui.movement.enabled);

        public override void OnInteract()
        {
            // Clear BeingHeld so the player can still walk while carrying a wire
            BeingHeld = false;
            if (!CanInteract) return;
            if (box.breaker)
            {
                box.TurnOnBreaker();
                ui.Notify("Breaker on.");
            }
            ui.OpenBox(this);
        }
    }
}
