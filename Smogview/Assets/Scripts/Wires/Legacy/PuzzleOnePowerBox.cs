using UnityEngine;

namespace Smogview.Wires.Legacy
{
    // Keep the old level's access rules out of PowerBox
    public sealed class PuzzleOnePowerBox : PowerBox
    {
        public PuzzleOnePowerController controller;
        public bool brokenInputLights;
        public bool outputsBehindGrate;
        public bool outputsViaTangleOnly;
        public bool shuffleOutputsOnStart;

        public override bool OutputsAccessible => base.OutputsAccessible &&
            (!outputsBehindGrate || (controller != null && controller.GrateOpen));
        public override bool ShowOutputs => !outputsViaTangleOnly;
        public override bool ShowSocketColors => !brokenInputLights;
        // Older scenes keep their existing color-based connections
        public override bool RequiresTargetBox => false;

        private void Start()
        {
            if (!shuffleOutputsOnStart) return;
            // Keep each wire's color and input together when shuffling the old extension box
            for (int i = outputs.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                if (outputs[i] == null || outputs[j] == null) continue;
                var color = outputs[i].color;
                int route = outputs[i].sourceInput;
                outputs[i].color = outputs[j].color;
                outputs[i].sourceInput = outputs[j].sourceInput;
                outputs[j].color = color;
                outputs[j].sourceInput = route;
            }
            if (network != null) network.Recalculate();
        }
    }
}
