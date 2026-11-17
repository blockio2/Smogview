using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Smogview.Wires
{
    // Keep the UI in WireOverlay.prefab for editing in Unity
    // Use this script to update the labels and handle the player's choices
    public class WireInteractionUI : MonoBehaviour
    {
        [Header("Assign the player carrying wires")]
        public PlayerWireCarrier carrier;
        [Header("Resolved from Carrier at startup")]
        public Player player;
        public PlayerMovement movement;
        public LensManager lenses;
        public pausemenucontrols pauseControls;

        [Header("Editable prefab widgets")]
        public GameObject panel;
        public Text title, hint, status, message;
        public GameObject inputHeading, outputHeading;
        public Button[] inputButtons, outputButtons;
        public Text[] inputLabels, outputLabels;
        public Button closeButton;

        public bool IsOpen { get; private set; }
        public bool Ready => isActiveAndEnabled && carrier != null && player != null &&
            movement != null && lenses != null && panel != null;
        private WireStation station;
        private bool CanUseMenu => IsOpen && Ready && station != null && station.box != null;
        protected virtual bool CanStayOpen => true;
        private UnityAction[] inputActions, outputActions;
        private float messageUntil;
        private int pauseReleaseFrame = -1;

        protected virtual void Awake()
        {
            // One menu serves all puzzles for this player
            if (carrier == null && player != null) carrier = player.GetComponent<PlayerWireCarrier>();
            if (carrier != null)
            {
                player = carrier.GetComponent<Player>();
                movement = carrier.GetComponent<PlayerMovement>();
                lenses = carrier.lenses != null ? carrier.lenses : carrier.GetComponent<LensManager>();
                pauseControls = carrier.GetComponent<pausemenucontrols>();
                carrier.ui = this;
            }
            if (panel != null) panel.SetActive(false);
            inputButtons ??= new Button[0];
            outputButtons ??= new Button[0];
            inputActions = BindButtons(inputButtons, SelectInput);
            outputActions = BindButtons(outputButtons, SelectOutput);
            if (closeButton != null) closeButton.onClick.AddListener(Close);
        }

        public void Notify(string text)
        {
            if (message != null) message.text = text;
            messageUntil = Time.unscaledTime + 5f;
        }

        public virtual void OpenBox(WireStation target)
        {
            if (!Ready || target == null || target.box == null) return;
            pauseReleaseFrame = -1;
            station = target;
            SetOpen(true);
            // Block the pause menu while this panel is open, but still allow lens switching
            if (pauseControls != null) pauseControls.otherUiOpen = true;
            Refresh();
        }

        public void Close()
        {
            if (!IsOpen) return;
            station = null;
            SetOpen(false);
            pauseReleaseFrame = Time.frameCount;
        }

        private void SetOpen(bool open)
        {
            IsOpen = open;
            if (panel != null) panel.SetActive(open);
            if (player != null) player.InteractionBlocked = open;
            if (movement != null) movement.ControlsBlocked = open;
        }

        protected virtual void OnDisable()
        {
            bool ownedControls = IsOpen || pauseReleaseFrame >= 0;
            Close();
            pauseReleaseFrame = -1;
            if (panel != null) panel.SetActive(false);
            if (ownedControls && pauseControls != null) pauseControls.otherUiOpen = false;
        }

        protected virtual void Update()
        {
            // Release the pause lock on a later frame after the close key is up
            if (pauseReleaseFrame >= 0 && Time.frameCount > pauseReleaseFrame && !Input.GetButton("WireClose"))
            {
                if (pauseControls != null) pauseControls.otherUiOpen = false;
                pauseReleaseFrame = -1;
            }
            if (message != null && Time.unscaledTime > messageUntil) message.text = "";
            var wire = carrier != null ? carrier.CarriedWire : null;
            bool mental = lenses != null && lenses.CurrentLens == LensManager.lens.Lmental;
            if (status != null) status.text = wire == null ? "" : "Carrying " + (mental ? wire.color + " wire" : "wire") + "   |   R: return in Physical";
            if (!IsOpen) return;
            if (!CanUseMenu || !station.isActiveAndEnabled || !CanStayOpen || Input.GetButtonDown("WireClose") ||
                Vector2.Distance(carrier.transform.position, station.transform.position) > 3f)
                Close();
            else Refresh();
        }

        protected virtual void Refresh()
        {
            bool physical = carrier.CanHandle;
            bool mental = lenses.CurrentLens == LensManager.lens.Lmental;
            bool normal = lenses.CurrentLens == LensManager.lens.Ldefault;
            bool carrying = carrier.CarriedWire != null;
            var box = station.box;
            if (title != null) title.text = box.displayName;
            if (inputHeading != null) inputHeading.SetActive(box.inputs.Length > 0);
            if (outputHeading != null) outputHeading.SetActive(box.outputs.Length > 0 && box.ShowOutputs);

            for (int i = 0; i < outputButtons.Length; i++)
            {
                var outputWire = i < box.outputs.Length ? box.outputs[i] : null;
                if (!ShowButton(outputButtons[i], outputWire != null && box.ShowOutputs)) continue;
                bool locked = !box.OutputsAccessible;
                SetLabel(outputLabels, i, (mental ? outputWire.color.ToString() : "Wire " + (i + 1)) +
                    (locked ? "\nLOCKED" : outputWire.Destination != null ? "\nConnected" : ""),
                    mental ? Wire.Tint(outputWire.color) : Color.white);
                outputButtons[i].interactable = physical && !carrying && outputWire.movable && !locked;
            }
            for (int i = 0; i < inputButtons.Length; i++)
            {
                var socket = i < box.inputs.Length ? box.inputs[i] : null;
                if (!ShowButton(inputButtons[i], socket != null)) continue;
                string state = socket.Powered ? "POWERED" : socket.ConnectedWire != null ? "Connected" : "Empty";
                if (!socket.Accessible) state += " / LOCKED";
                SetLabel(inputLabels, i, "Socket " + (i + 1) + "\n" + state,
                    normal && box.ShowSocketColors ? Wire.Tint(socket.requiredColor) : Color.white);
                inputButtons[i].interactable = physical && carrying && socket.Accessible;
            }
            if (hint == null) return;
            if (!physical) hint.text = "1 Default: socket colors   |   2 Physical: connect   |   4 Mental: wire colors";
            else if (carrying) hint.text = "Choose a socket for the wire you are carrying.";
            else if (box.outputs.Length == 0) hint.text = "Bring a wire here to connect it.";
            else hint.text = "Choose a wire, then walk to another box.";
        }

        public virtual void SelectOutput(int index)
        {
            if (CanUseMenu && index >= 0 && index < station.box.outputs.Length &&
                carrier.PickUp(station.box.outputs[index])) Close();
        }
        public virtual void SelectInput(int index)
        {
            if (CanUseMenu && index >= 0 && index < station.box.inputs.Length &&
                carrier.Insert(station.box.inputs[index])) Close();
        }

        private bool ShowButton(Button button, bool visible)
        {
            if (button == null) return false;
            button.gameObject.SetActive(visible);
            return visible;
        }

        // Bind only to this menu instance, no saved scene callbacks are needed
        private UnityAction[] BindButtons(Button[] buttons, System.Action<int> select)
        {
            var actions = new UnityAction[buttons.Length];
            for (int i = 0; i < buttons.Length; i++)
            {
                int index = i;
                actions[i] = () => select(index);
                if (buttons[i] != null) buttons[i].onClick.AddListener(actions[i]);
            }
            return actions;
        }

        protected virtual void OnDestroy()
        {
            UnbindButtons(inputButtons, inputActions);
            UnbindButtons(outputButtons, outputActions);
            if (closeButton != null) closeButton.onClick.RemoveListener(Close);
        }

        private void UnbindButtons(Button[] buttons, UnityAction[] actions)
        {
            if (buttons == null || actions == null) return;
            for (int i = 0; i < buttons.Length && i < actions.Length; i++)
                if (buttons[i] != null) buttons[i].onClick.RemoveListener(actions[i]);
        }

        private void SetLabel(Text[] labels, int index, string text, Color color)
        {
            if (labels == null || index >= labels.Length || labels[index] == null) return;
            labels[index].text = text;
            labels[index].color = color;
        }
    }
}
