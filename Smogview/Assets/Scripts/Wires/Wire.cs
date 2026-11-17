using UnityEngine;

namespace Smogview.Wires
{
    public enum WireColor { Red, Yellow, Blue, Green }

    /// <summary>Keep one end at the source and let the other move; track connection and power separately</summary>
    public sealed class Wire : MonoBehaviour
    {
        [Tooltip("Uses the closest parent PowerBox at startup, leave empty for a child wire")]
        public PowerBox source;
        [Tooltip("Assign the box this wire should connect to")]
        public PowerBox targetBox;
        public WireColor color;
        [Tooltip("Input supplying this wire: 0/1/2 = socket index, using the box's Inputs order or child hierarchy order when Inputs is empty; -1 = breaker; -2 = all inputs powered")]
        public int sourceInput;
        [Min(0.1f)] public float maximumLength = 12f;
        public bool movable = true;
        public bool enabledOutput = true;
        // Save where the wire is connected and recalculate its power when the game runs
        [SerializeField] private WireSocket destination;
        public WireSocket Destination { get => destination; internal set => destination = value; }
        public PlayerWireCarrier Carrier { get; internal set; }
        public bool Powered { get; internal set; }

        public bool MatchesTarget(PowerBox box) => source != null && box != null &&
            (!source.RequiresTargetBox || targetBox == box);

        private void Awake()
        {
            var parentBox = GetComponentInParent<PowerBox>(true);
            if (parentBox != null) source = parentBox;
        }
        // Move only the loose end; it follows the player, stays in a socket, or hangs below the source
        public Vector3 EndPosition => Carrier != null ? Carrier.transform.position :
            Destination != null ? Destination.transform.position : transform.position + Vector3.down * .6f;

        public static Color Tint(WireColor value)
        {
            switch (value)
            {
                case WireColor.Red: return new Color(.9f, .28f, .25f);
                case WireColor.Yellow: return new Color(1f, .8f, .25f);
                case WireColor.Blue: return new Color(.28f, .63f, 1f);
                default: return new Color(.35f, .85f, .48f);
            }
        }
    }
}
