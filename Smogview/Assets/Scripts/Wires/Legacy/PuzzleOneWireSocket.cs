namespace Smogview.Wires.Legacy
{
    public sealed class PuzzleOneWireSocket : WireSocket
    {
        public bool behindGrate;

        public override bool Accessible => base.Accessible && (!behindGrate ||
            (box is PuzzleOnePowerBox oldBox && oldBox.controller != null && oldBox.controller.GrateOpen));
    }
}
