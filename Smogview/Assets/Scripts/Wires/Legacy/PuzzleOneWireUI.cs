namespace Smogview.Wires.Legacy
{
    // Only the older computer puzzle needs a second mode in the wire menu
    public sealed class PuzzleOneWireUI : WireInteractionUI
    {
        public PuzzleOnePowerController controller;
        private bool computer;

        protected override bool CanStayOpen => !computer || (controller != null && controller.ComputerPowered);

        public override void OpenBox(WireStation target)
        {
            computer = false;
            base.OpenBox(target);
        }

        public void OpenComputer(PuzzleOneWireStation target)
        {
            if (controller == null || !controller.ComputerPowered) return;
            computer = true;
            base.OpenBox(target);
        }

        protected override void Refresh()
        {
            if (computer && controller != null) controller.RefreshComputerMenu(this);
            else base.Refresh();
        }

        public override void SelectOutput(int index)
        {
            if (!computer) { base.SelectOutput(index); return; }
            if (!IsOpen || controller == null || controller.closet == null ||
                index < 0 || index >= controller.closet.outputs.Length) return;
            var wire = controller.closet.outputs[index];
            if (wire != null) controller.SetDisabledColor(wire.color);
        }

        public override void SelectInput(int index)
        {
            if (!computer) base.SelectInput(index);
        }
    }
}
