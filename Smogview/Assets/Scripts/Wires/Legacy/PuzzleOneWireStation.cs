namespace Smogview.Wires.Legacy
{
    public enum WireStationKind { Box, Breaker, Tangle, Computer, Elevator }

    public sealed class PuzzleOneWireStation : WireStation
    {
        public WireStationKind kind;
        public PuzzleOnePowerController controller;

        public override void OnInteract()
        {
            BeingHeld = false;
            if (!CanInteract) return;
            if (controller != null) controller.InteractStation(this);
            else base.OnInteract();
        }
    }
}
