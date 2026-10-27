using UnityEngine;
using UnityEngine.UI;

namespace Smogview.UI
{
    // A passive HUD: keyboard input remains owned by the player's lens controller.
    public sealed class LensIndicatorUI : MonoBehaviour
    {
        [SerializeField] private LensSwitching source;
        [SerializeField] private Image[] cards;
        [SerializeField] private Image[] activeMarks;
        [SerializeField] private Text[] labels;
        [SerializeField] private Color[] lensColors =
        {
            new Color32(224, 225, 218, 255),
            new Color32(232, 187, 107, 255),
            new Color32(137, 203, 172, 255)
        };
        private LensSwitching boundSource;
        public int SelectedIndex { get; private set; }

        private void OnEnable()
        {
            if (source == null) source = FindAnyObjectByType<LensSwitching>();
            boundSource = source;
            if (boundSource != null)
            {
                boundSource.LensChanged += Refresh;
                Refresh(boundSource.CurrentLens);
            }
        }

        private void OnDisable()
        {
            if (boundSource != null) boundSource.LensChanged -= Refresh;
            boundSource = null;
        }

        private void Refresh(LensSwitching.lens activeLens)
        {
            SelectedIndex = (int)activeLens;
            for (int i = 0; i < cards.Length; i++)
            {
                bool active = i == SelectedIndex;
                cards[i].color = active
                    ? new Color32(36, 42, 46, 235)
                    : new Color32(20, 24, 28, 180);
                activeMarks[i].enabled = active;
                activeMarks[i].color = lensColors[i];
                labels[i].color = active ? lensColors[i] : new Color32(206, 210, 214, 255);
                labels[i].fontStyle = active ? FontStyle.Bold : FontStyle.Normal;
            }
        }
    }
}

