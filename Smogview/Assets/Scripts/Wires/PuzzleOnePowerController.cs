using System;
using UnityEngine;
using UnityEngine.Events;

namespace Smogview.Wires
{
    /// <summary>Only Puzzle One progression belongs here; routing remains reusable in WireNetwork.</summary>
    public sealed class PuzzleOnePowerController : MonoBehaviour
    {
        public WireNetwork network;
        public LensManager lenses;
        public PowerBox breaker, thirdBox, closet, grateBox, elevatorBox;
        public WireSocket computerSocket;
        public WireInteractionUI ui;
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
        public bool ComputerPowered => MentalUnlocked && computerSocket.Powered;
        public event Action Changed;
        private Func<LensManager.lens, bool> previousLensGate;

        private void OnEnable()
        {
            network.Changed += Evaluate;
            // Compose with any previous lens rule and restore it when this controller is disabled.
            previousLensGate = lenses.CanSelectLens;
            lenses.CanSelectLens = CanSelectLens;
        }
        private void OnDisable()
        {
            network.Changed -= Evaluate;
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
        public void TurnOnBreaker() { breaker.breakerOn = true; network.Recalculate(); }
        public bool InspectTangle()
        {
            if (!thirdBox.AllInputsPowered) { ui?.Notify("Power all three inputs on the third box first."); return false; }
            if (!MentalUnlocked)
            {
                onInspectTangle.Invoke();
                // Immediate unlock supports this prototype; team dialogue can call UnlockMental later.
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
            // Exactly one R/Y/B output is off. Green never participates in this switch.
            foreach (var wire in closet.outputs) wire.enabledOutput = wire.color == WireColor.Green || wire.color != DisabledColor;
            network.Recalculate();
        }
        private void Evaluate()
        {
            // Yellow opens access to the grate. Closing it never disconnects or disables red/blue.
            bool grate = grateBox.inputs[0].Powered;
            bool elevator = elevatorBox.AllInputsPowered;
            if (GrateOpen != grate) { GrateOpen = grate; onGratePowerChanged.Invoke(grate); }
            if (ElevatorPowered != elevator) { ElevatorPowered = elevator; onElevatorPowerChanged.Invoke(elevator); }
            Changed?.Invoke();
        }
        public void Escape()
        {
            if (!ElevatorPowered || Escaped) return;
            Escaped = true;
            ui?.Notify("Elevator powered. Puzzle complete!");
            // The prototype reports completion here; a teammate can bind the actual level transition.
            onEscape.Invoke();
            Changed?.Invoke();
        }
    }
}
