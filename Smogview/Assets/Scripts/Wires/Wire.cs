using UnityEngine;

namespace Smogview.Wires
{
    public enum WireColor { Red, Yellow, Blue, Green }

    /// <summary>A permanent source lead with one movable end. Connection and power are separate.</summary>
    public sealed class Wire : MonoBehaviour
    {
        public PowerBox source;
        public WireColor color;
        [Tooltip("Input that supplies this output; -1 is a breaker source, -2 is the closet's auxiliary supply.")]
        public int sourceInput;
        [Min(0.1f)] public float maximumLength = 12f;
        public bool movable = true;
        public bool enabledOutput = true;
        // Save the endpoint reference (including the prewired breaker); power is recomputed at runtime.
        [SerializeField] private WireSocket destination;
        public WireSocket Destination { get => destination; internal set => destination = value; }
        public PlayerWireCarrier Carrier { get; internal set; }
        public bool Powered { get; internal set; }
        // Only the free end moves: it follows the carrier, its socket, or hangs below the source.
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
