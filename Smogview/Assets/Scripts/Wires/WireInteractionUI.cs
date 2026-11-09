using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Smogview.Wires
{
    /// <summary>Small overlay with stable port order. Color clues obey the current lens, even in menus.</summary>
    public sealed class WireInteractionUI : MonoBehaviour
    {
        public PlayerWireCarrier carrier;
        public Player player;
        public PlayerMovement movement;
        public PuzzleOnePowerController controller;
        public LensManager lenses;
        public bool IsOpen => station != null;
        private WireStation station;
        private GameObject panel;
        private Text title, hint, status, message;
        private Button closeButton;
        // Preserve generated widget references if Unity reloads scripts during Play mode.
        [SerializeField, HideInInspector] private Button[] inputs = new Button[4], outputs = new Button[4];
        [SerializeField, HideInInspector] private Text[] inputLabels = new Text[4], outputLabels = new Text[4];
        private Font font;
        private float messageUntil;
        private bool computer;

        private void Awake()
        {
            font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            var canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 60;
            var scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = .5f;
            gameObject.AddComponent<GraphicRaycaster>();
            panel = Rect("Wire panel", transform, new Vector2(660, 340), Vector2.zero).gameObject;
            panel.AddComponent<Image>().color = new Color(.07f,.1f,.13f,.97f);
            title = Label("Title", panel.transform, new Vector2(600,40), new Vector2(0,125), 26);
            Label("Outputs", panel.transform, new Vector2(600,24), new Vector2(0,79), 17).text = "OUTGOING WIRES";
            Label("Inputs", panel.transform, new Vector2(600,24), new Vector2(0,-15), 17).text = "INPUT SOCKETS";
            for (int i = 0; i < 4; i++)
            {
                int port = i;
                outputs[i] = MakeButton(panel.transform, new Vector2(-225+i*150,40), out outputLabels[i]);
                outputs[i].onClick.AddListener(() => SelectOutput(port));
                inputs[i] = MakeButton(panel.transform, new Vector2(-225+i*150,-54), out inputLabels[i]);
                inputs[i].onClick.AddListener(() => SelectInput(port));
            }
            hint = Label("Hint",panel.transform,new Vector2(610,60),new Vector2(0,-115),17);
            var close = MakeButton(panel.transform,new Vector2(280,135),out var closeLabel,new Vector2(55,30));
            closeLabel.text = "ESC";
            closeButton = close;
            status = Label("Carry status",transform,new Vector2(1000,32),new Vector2(0,205),19);
            message = Label("Message",transform,new Vector2(1000,54),new Vector2(0,275),21);
            panel.SetActive(false);
        }

        private void OnEnable()
        {
            // Runtime UnityEvent listeners do not survive a script reload; restore our owned buttons.
            for(int i=0;i<4;i++)
            {
                int port=i;
                if(outputs[i]!=null) { outputs[i].onClick.RemoveAllListeners(); outputs[i].onClick.AddListener(()=>SelectOutput(port)); }
                if(inputs[i]!=null) { inputs[i].onClick.RemoveAllListeners(); inputs[i].onClick.AddListener(()=>SelectInput(port)); }
            }
            if(closeButton!=null) { closeButton.onClick.RemoveAllListeners(); closeButton.onClick.AddListener(Close); }
        }

        private RectTransform Rect(string label, Transform parent, Vector2 size, Vector2 position)
        {
            var rect = new GameObject(label,typeof(RectTransform)).GetComponent<RectTransform>();
            rect.SetParent(parent,false); rect.sizeDelta=size; rect.anchoredPosition=position;
            return rect;
        }
        private Text Label(string label, Transform parent, Vector2 size, Vector2 position, int fontSize)
        {
            var text = Rect(label,parent,size,position).gameObject.AddComponent<Text>();
            text.font=font; text.fontSize=fontSize; text.color=new Color(.9f,.94f,.94f);
            text.alignment=TextAnchor.MiddleCenter; text.raycastTarget=false;
            return text;
        }
        private Button MakeButton(Transform parent, Vector2 position, out Text label, Vector2? size = null)
        {
            var rect = Rect("Port",parent,size ?? new Vector2(138,48),position);
            rect.gameObject.AddComponent<Image>().color = new Color(.22f,.29f,.34f);
            var button = rect.gameObject.AddComponent<Button>();
            label=Label("Label",rect,rect.sizeDelta,Vector2.zero,17);
            return button;
        }
        public void Notify(string text) { if(message == null) return; message.text=text; messageUntil=Time.unscaledTime+5; }
        public void OpenBox(WireStation target) { Open(target,false); }
        public void OpenComputer(WireStation target) { Open(target,true); }
        private void Open(WireStation target, bool isComputer)
        {
            station=target; computer=isComputer; panel.SetActive(true);
            // Movement is blocked only while choosing a port; 1-4 lens controls stay enabled.
            player.InteractionBlocked=true; movement.ControlsBlocked=true;
            Refresh();
        }
        public void Close()
        {
            station=null;
            if(panel != null) panel.SetActive(false);
            if(player != null) player.InteractionBlocked=false;
            if(movement != null) movement.ControlsBlocked=false;
        }
        private void OnDisable() { Close(); }
        private void Update()
        {
            if(Time.unscaledTime>messageUntil) message.text="";
            var wire=carrier.CarriedWire;
            string identity = wire != null && controller.MentalUnlocked && lenses.CurrentLens==LensManager.lens.Lmental ? wire.color+" wire" : "wire";
            status.text=wire == null ? "" : "Carrying "+identity+"   |   R: return in Physical lens";
            if(!IsOpen) return;
            if((Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) ||
                Vector2.Distance(carrier.transform.position,station.transform.position)>3f ||
                (computer && !controller.ComputerPowered)) { Close(); return; }
            Refresh();
        }
        private void Refresh()
        {
            // The menu follows the same lens rules as the world: Physical edits, Default socket colors, Mental wire colors.
            bool physical=carrier.CanHandle;
            bool mental=controller.MentalUnlocked && lenses.CurrentLens==LensManager.lens.Lmental;
            bool normal=lenses.CurrentLens==LensManager.lens.Ldefault;
            title.text=computer ? "POWER CONTROL" : station.box.displayName;
            for(int i=0;i<4;i++)
            {
                if(computer)
                {
                    // Buttons stay in the same order as the closet's three physical outputs.
                    outputs[i].gameObject.SetActive(i<3); inputs[i].gameObject.SetActive(false);
                    if(i>=3) continue;
                    var wire=controller.closet.outputs[i];
                    bool off=wire.color==controller.DisabledColor;
                    outputLabels[i].text="Output "+(i+1)+"\n"+(off?"OFF":"ON");
                    outputLabels[i].color=mental?Wire.Tint(wire.color):Color.white;
                    outputs[i].interactable=!off;
                    continue;
                }
                var box=station.box;
                outputs[i].gameObject.SetActive(i<box.outputs.Length && !box.outputsViaTangleOnly);
                inputs[i].gameObject.SetActive(i<box.inputs.Length);
                if(i<box.outputs.Length)
                {
                    var wire=box.outputs[i];
                    // LOCKED prevents picking up this lead; it does not remove its connection or power.
                    bool locked=box.outputsBehindGrate && !controller.GrateOpen;
                    outputLabels[i].text=(mental?wire.color.ToString():"Wire "+(i+1))+(locked?"\nLOCKED":wire.Destination!=null?"\nConnected":"");
                    outputLabels[i].color=mental?Wire.Tint(wire.color):Color.white;
                    outputs[i].interactable=physical && carrier.CarriedWire==null && wire.movable && !locked && !box.outputsViaTangleOnly;
                }
                if(i<box.inputs.Length)
                {
                    var socket=box.inputs[i];
                    // POWERED requires a matching live supply. Connected only means a lead is attached.
                    // This prototype prioritizes LOCKED, so closed-grate sockets hide their power status.
                    inputLabels[i].text="Socket "+(i+1)+(socket.Accessible?(socket.Powered?"\nPOWERED":socket.ConnectedWire!=null?"\nConnected":""):"\nLOCKED");
                    inputLabels[i].color=normal && !box.brokenInputLights?Wire.Tint(socket.requiredColor):Color.white;
                    inputs[i].interactable=physical && carrier.CarriedWire!=null && socket.Accessible;
                }
            }
            hint.text=computer?"Choose an ON output to switch it OFF. The previous OFF output turns ON.":
                physical?(carrier.CarriedWire==null?"Choose a wire, then walk to another box.":"Choose a socket. An occupied socket returns its previous wire."):
                "1 Default: socket colors   |   2 Physical: connect   |   4 Mental: wire colors";
            if(!computer && physical && carrier.CarriedWire==null && station.box.outputsViaTangleOnly)
                hint.text="Outgoing wires are picked up at the tangle below this box.";
            else if(!computer && physical && carrier.CarriedWire==null && station.box.outputs.Length==0)
                hint.text="Bring a wire here, then choose an input socket.";
        }
        private void SelectOutput(int index)
        {
            if(!IsOpen) return;
            if(computer) { controller.SetDisabledColor(controller.closet.outputs[index].color); Refresh(); }
            else if(carrier.PickUp(station.box.outputs[index])) Close();
        }
        private void SelectInput(int index)
        {
            if(IsOpen && !computer && carrier.Insert(station.box.inputs[index])) Close();
        }
    }
}
