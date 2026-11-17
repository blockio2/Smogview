using UnityEngine;

namespace Smogview.Wires
{
    public class WireSocket : MonoBehaviour
    {
        [Tooltip("Uses the closest parent PowerBox at startup, leave empty for a child socket")]
        public PowerBox box;
        public WireColor requiredColor;
        [SerializeField] private Wire connectedWire;
        public Wire ConnectedWire { get => connectedWire; internal set => connectedWire = value; }
        protected virtual void Awake()
        {
            var parentBox = GetComponentInParent<PowerBox>(true);
            if (parentBox != null) box = parentBox;
        }
        // A connected wire needs power, its assigned box and a matching socket color
        public bool Powered => ConnectedWire != null && ConnectedWire.Powered &&
            ConnectedWire.MatchesTarget(box) && ConnectedWire.color == requiredColor;
        public virtual bool Accessible => isActiveAndEnabled;
    }
}
