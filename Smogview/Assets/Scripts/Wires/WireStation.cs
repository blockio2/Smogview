using System.Linq;
using UnityEngine;

namespace Smogview.Wires
{
    public enum WireStationKind { Box, Breaker, Tangle, Computer, Elevator }

    /// <summary>Adapter to the team's E/InteractionHitbox system.</summary>
    public sealed class WireStation : Interactable
    {
        public WireStationKind kind;
        public PowerBox box;
        public PuzzleOnePowerController controller;
        public PlayerWireCarrier carrier;
        public WireInteractionUI ui;
        public override bool CanInteract => base.CanInteract && ui != null && !ui.IsOpen;

        public override void OnInteract()
        {
            // E begins as a held interaction in Player; release that flag so wire carrying allows walking.
            BeingHeld = false;
            if (!CanInteract) return;
            switch (kind)
            {
                case WireStationKind.Breaker:
                    controller.TurnOnBreaker();
                    ui.Notify("Breaker on. Trace its wires to the relay, then connect the third box.");
                    break;
                case WireStationKind.Tangle:
                    if (!controller.MentalUnlocked) { controller.InspectTangle(); return; }
                    if (!carrier.CanHandle) { ui.Notify("Use Physical lens to pick up a wire."); return; }
                    if (carrier.CarriedWire != null) { ui.Notify("Connect or return the wire you are carrying first."); return; }
                    // Tangle leads are chosen randomly, rather than revealing their identity in a menu.
                    var available = box.outputs.Where(w => w.Destination == null && w.Carrier == null).ToArray();
                    if (available.Length == 0)
                    {
                        // Once every tangle lead is connected, E resets those leads for another attempt.
                        foreach (var wire in box.outputs) box.network.Disconnect(wire, false);
                        box.network.Recalculate();
                        ui.Notify("Tangled wires returned. Try the connections again.");
                    }
                    else carrier.PickUp(available[Random.Range(0, available.Length)]);
                    break;
                case WireStationKind.Computer:
                    // Reuse the level's single computer: E offers its input while carrying a wire,
                    // then becomes power controls once the green supply and Mental unlock are ready.
                    if (carrier.CarriedWire != null || !controller.ComputerPowered) ui.OpenBox(this);
                    else ui.OpenComputer(this);
                    break;
                case WireStationKind.Elevator:
                    if (controller.ElevatorPowered) controller.Escape();
                    else ui.Notify("The elevator needs both red and blue power.");
                    break;
                default: ui.OpenBox(this); break;
            }
        }
    }
}
