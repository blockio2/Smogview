using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Smogview.Wires.Legacy
{
    /// <summary>Only Puzzle One progression belongs here; routing remains reusable in WireNetwork</summary>
    public sealed class PuzzleOnePowerController : MonoBehaviour
    {
        public WireNetwork network;
        public LensManager lenses;
        public PuzzleOnePowerBox breaker, thirdBox, closet, grateBox, elevatorBox;
        public WireSocket computerSocket;
        public PuzzleOneWireUI ui;
        [Tooltip("Disable when the team's dialogue should call UnlockMental after its final line.")]
        public bool unlockMentalImmediately = true;
        public UnityEvent onInspectTangle = new UnityEvent();
        public UnityEvent onMentalUnlocked = new UnityEvent();
        public UnityEvent<bool> onGratePowerChanged = new UnityEvent<bool>();
        public UnityEvent<bool> onElevatorPowerChanged = new UnityEvent<bool>();
        public UnityEvent onEscape = new UnityEvent();
        public bool MentalUnlocked { get; private set; }
        public bool GrateOpen { get; private set; }
        public bool ElevatorPowered { get; private set; }
        public bool Escaped { get; private set; }
        public WireColor DisabledColor { get; private set; } = WireColor.Yellow;
        public bool ComputerPowered => MentalUnlocked && computerSocket != null && computerSocket.Powered;
        public event Action Changed;
        private Func<LensManager.lens, bool> previousLensGate;

        private void OnEnable()
        {
            if (network != null) network.Changed += Evaluate;
            if (lenses == null) return;
            // Compose with any previous lens rule and restore it when this controller is disabled
            previousLensGate = lenses.CanSelectLens;
            lenses.CanSelectLens = CanSelectLens;
        }
        private void OnDisable()
        {
            if (network != null) network.Changed -= Evaluate;
            if (lenses == null) return;
            if (lenses.CanSelectLens == CanSelectLens) lenses.CanSelectLens = previousLensGate;
        }
        private void Start() { ApplyOutputs(); }
        private bool CanSelectLens(LensManager.lens lens)
        {
            if (lens == LensManager.lens.Lmental && !MentalUnlocked)
            {
                ui?.Notify("Inspect the wire tangle after powering the third box to unlock Mental.");
                return false;
            }
            return previousLensGate == null || previousLensGate(lens);
        }
        public void TurnOnBreaker() { if (breaker != null) breaker.TurnOnBreaker(); }
        public bool InspectTangle()
        {
            if (thirdBox == null || !thirdBox.AllInputsPowered) { ui?.Notify("Power all three inputs on the third box first."); return false; }
            if (!MentalUnlocked)
            {
                onInspectTangle.Invoke();
                // Immediate unlock the Mental lens
                if (unlockMentalImmediately) UnlockMental();
            }
            return MentalUnlocked;
        }
        public void UnlockMental()
        {
            if (MentalUnlocked) return;
            MentalUnlocked = true;
            onMentalUnlocked.Invoke();
            ui?.Notify("Mental lens unlocked. Press 4 to see wire colors.");
            Evaluate();
        }
        public bool SetDisabledColor(WireColor color)
        {
            if (!ComputerPowered || color == WireColor.Green) return false;
            DisabledColor = color;
            ApplyOutputs();
            return true;
        }
        private void ApplyOutputs()
        {
            // Exactly one R/Y/B output is off; green never participates in this switch
            if (closet == null) return;
            foreach (var wire in closet.outputs) if (wire != null) wire.enabledOutput = wire.color == WireColor.Green || wire.color != DisabledColor;
            if (network != null) network.Recalculate();
        }
        private void Evaluate()
        {
            // Yellow opens access to the grate; closing it never disconnects or disables red/blue
            bool grate = grateBox != null && grateBox.inputs.Length > 0 &&
                grateBox.inputs[0] != null && grateBox.inputs[0].Powered;
            bool elevator = elevatorBox != null && elevatorBox.AllInputsPowered;
            if (GrateOpen != grate) { GrateOpen = grate; onGratePowerChanged.Invoke(grate); }
            if (ElevatorPowered != elevator) { ElevatorPowered = elevator; onElevatorPowerChanged.Invoke(elevator); }
            Changed?.Invoke();
        }
        public void Escape()
        {
            if (!ElevatorPowered || Escaped) return;
            Escaped = true;
            ui?.Notify("Elevator powered. Puzzle complete!");
            // level transition
            onEscape.Invoke();
            Changed?.Invoke();
        }

        // Keep the original puzzle interactions out of the reusable station script
        public void InteractStation(PuzzleOneWireStation station)
        {
            var carrier = station.carrier;
            var box = station.box;
            var menu = station.ui as PuzzleOneWireUI;
            if (carrier == null || box == null || menu == null) return;

            switch (station.kind)
            {
                case WireStationKind.Breaker:
                    TurnOnBreaker();
                    menu.Notify("Breaker on.");
                    break;
                case WireStationKind.Tangle:
                    if (!MentalUnlocked) { InspectTangle(); return; }
                    if (!carrier.CanHandle) { menu.Notify("Use Physical lens to pick up a wire."); return; }
                    if (carrier.CarriedWire != null) { menu.Notify("Connect or return the wire you are carrying first."); return; }

                    var available = box.outputs.Where(w => w != null && w.Destination == null && w.Carrier == null).ToArray();
                    if (available.Length == 0)
                    {
                        if (box.network == null) return;
                        foreach (var wire in box.outputs) box.network.Disconnect(wire, false);
                        box.network.Recalculate();
                        menu.Notify("Tangled wires returned. Try the connections again.");
                    }
                    else
                    {
                        carrier.PickUp(available[UnityEngine.Random.Range(0, available.Length)]);
                    }
                    break;
                case WireStationKind.Computer:
                    if (carrier.CarriedWire != null || !ComputerPowered)
                        menu.OpenBox(station);
                    else
                        menu.OpenComputer(station);
                    break;
                case WireStationKind.Elevator:
                    if (ElevatorPowered) Escape();
                    else menu.Notify("The elevator needs both red and blue power.");
                    break;
                default:
                    menu.OpenBox(station);
                    break;
            }
        }

        // These three output switches belong only to the original computer puzzle
        public void RefreshComputerMenu(WireInteractionUI menu)
        {
            if (menu == null || closet == null) return;
            if (menu.title != null) menu.title.text = "POWER CONTROL";
            if (menu.inputHeading != null) menu.inputHeading.SetActive(false);
            if (menu.outputHeading != null) menu.outputHeading.SetActive(true);
            foreach (var button in menu.inputButtons) if (button != null) button.gameObject.SetActive(false);

            bool mental = MentalUnlocked && lenses != null && lenses.CurrentLens == LensManager.lens.Lmental;
            for (int i = 0; i < menu.outputButtons.Length; i++)
            {
                if (menu.outputButtons[i] == null) continue;
                bool available = i < 3 && i < closet.outputs.Length && closet.outputs[i] != null;
                menu.outputButtons[i].gameObject.SetActive(available);
                if (!available) continue;
                var output = closet.outputs[i];
                bool off = output.color == DisabledColor;
                if (menu.outputLabels != null && i < menu.outputLabels.Length && menu.outputLabels[i] != null)
                {
                    menu.outputLabels[i].text = "Output " + (i + 1) + (off ? "\nOFF" : "\nON");
                    menu.outputLabels[i].color = mental ? Wire.Tint(output.color) : Color.white;
                }
                menu.outputButtons[i].interactable = !off;
            }
            if (menu.hint != null) menu.hint.text = "Switch one output OFF; the previous OFF output turns ON.";
        }
    }
}
