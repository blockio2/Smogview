using UnityEngine;

namespace Smogview.Wires.Legacy
{
    /// <summary>Update the grate bars, elevator light, and status text in the older scenes</summary>
    public sealed class PuzzlePowerFeedback : MonoBehaviour
    {
        public PuzzleOnePowerController controller;
        public GameObject grateBars;
        public SpriteRenderer elevatorLamp;
        public TextMesh objective;
        public bool showProgressHints = true;
        private void Update()
        {
            if (controller == null) return;
            // Hide the grate bars without disabling the box or disconnecting its wires
            if (grateBars != null) grateBars.SetActive(!controller.GrateOpen);
            if (elevatorLamp != null) elevatorLamp.color=controller.ElevatorPowered?new Color(.3f,.9f,.55f):new Color(.3f,.36f,.4f);
            if (objective == null) return;
            // Keep the hint next to the elevator when Show Progress Hints is off
            if (!showProgressHints)
            {
                objective.text=controller.Escaped?"PUZZLE COMPLETE":controller.ElevatorPowered?"E: USE ELEVATOR":"ELEVATOR";
                return;
            }
            objective.text=controller.Escaped?"PUZZLE COMPLETE":
                controller.ElevatorPowered?"Elevator ready - press E at the elevator":
                (controller.breaker == null || !controller.breaker.breakerOn)?"Turn on the breaker":
                !controller.MentalUnlocked?"Trace the wires to the third box, then inspect its tangle":
                !controller.ComputerPowered?"Identify the tangled wires and connect the closet + computer":
                "Power yellow to open the grate. Route red + blue to the elevator.";
        }
    }
}
