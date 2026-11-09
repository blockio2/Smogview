using UnityEngine;

namespace Smogview.Wires
{
    /// <summary>Grate and elevator feedback for the existing level; team art/audio can replace the placeholders.</summary>
    public sealed class PuzzlePowerFeedback : MonoBehaviour
    {
        public PuzzleOnePowerController controller;
        public GameObject grateBars;
        public SpriteRenderer elevatorLamp;
        public TextMesh objective;
        public AudioSource audioSource;
        public bool showProgressHints = true;
        private bool wasPowered;
        private AudioClip chime;
        private void Awake()
        {
            // A short generated placeholder chime; replace with team audio in the final level.
            const int rate = 22050;
            var data = new float[rate/3];
            for(int i=0;i<data.Length;i++) data[i]=Mathf.Sin(i*2*Mathf.PI*660/rate)*.1f*(1f-(float)i/data.Length);
            chime=AudioClip.Create("Elevator ready",data.Length,1,rate,false); chime.SetData(data,0);
        }
        private void Update()
        {
            // Toggle only the visual bars; keep the box and its existing connections active.
            grateBars.SetActive(!controller.GrateOpen);
            elevatorLamp.color=controller.ElevatorPowered?new Color(.3f,.9f,.55f):new Color(.3f,.36f,.4f);
            if(controller.ElevatorPowered && !wasPowered) audioSource.PlayOneShot(chime);
            wasPowered=controller.ElevatorPowered;
            // In puzzle 1 only the elevator gets a local status label; do not add a walkthrough over the map.
            if (!showProgressHints)
            {
                objective.text=controller.Escaped?"PUZZLE COMPLETE":controller.ElevatorPowered?"E: USE ELEVATOR":"ELEVATOR";
                return;
            }
            objective.text=controller.Escaped?"PUZZLE COMPLETE":
                controller.ElevatorPowered?"Elevator ready - press E at the elevator":
                !controller.breaker.breakerOn?"Turn on the breaker":
                !controller.MentalUnlocked?"Trace the wires to the third box, then inspect its tangle":
                !controller.ComputerPowered?"Identify the tangled wires and connect the closet + computer":
                "Power yellow to open the grate. Route red + blue to the elevator.";
        }
        private void OnDestroy() { if(chime != null) Destroy(chime); }
    }
}
