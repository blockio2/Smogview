using UnityEngine;
using UnityEngine.UI;

namespace Smogview.UI
{
    // A passive HUD: keyboard input remains owned by the player's lens controller.
    public sealed class LensIndicatorUI : MonoBehaviour
    {
        [SerializeField] private LensSwitching source;
        // Keep all arrays in the lens enum order: Default, Physical, Social, Mental.
        [SerializeField] private Image[] icons;
        // Each group includes the whole icon, including Default's ring and eye layers.
        [SerializeField] private CanvasGroup[] iconGroups;
        [SerializeField] private Image[] activeMarks;
        [SerializeField] private Text[] labels;
        [SerializeField] private Color[] lensColors =
        {
            new Color32(224, 225, 218, 255),
            new Color32(232, 187, 107, 255),
            new Color32(137, 203, 172, 255),
            new Color32(91, 199, 142, 255)
        };
        private LensSwitching boundSource;
        public int SelectedIndex { get; private set; }

        private void OnEnable()
        {
            if (source == null) source = FindAnyObjectByType<LensSwitching>();
            boundSource = source;
            if (boundSource != null)
            {
                // Listen to the gameplay controller instead of handling number keys in the HUD.
                boundSource.LensChanged += Refresh;
                // Show the current selection immediately, including when the HUD is re-enabled.
                Refresh(boundSource.CurrentLens);
            }
        }

        private void OnDisable()
        {
            // Remove the listener while hidden so re-enabling cannot add duplicate callbacks.
            if (boundSource != null) boundSource.LensChanged -= Refresh;
            boundSource = null;
        }

        // Only changes the display; lens switching and world visibility stay in LensSwitching.
        private void Refresh(LensSwitching.lens activeLens)
        {
            SelectedIndex = (int)activeLens;
            for (int i = 0; i < labels.Length; i++)
            {
                bool active = i == SelectedIndex;
                if (icons[i] != null)
                {
                    // Fade all child graphics together without changing the artwork's colors.
                    iconGroups[i].alpha = active ? 1f : .65f;
                    // Enlarge the selected icon slightly without shifting the row or labels.
                    icons[i].rectTransform.localScale = Vector3.one * (active ? 1.08f : 1f);
                }
                // The underline and bold label also identify selection without relying on color.
                activeMarks[i].enabled = active;
                activeMarks[i].color = lensColors[i];
                labels[i].color = active ? lensColors[i] : new Color32(206, 210, 214, 255);
                labels[i].fontStyle = active ? FontStyle.Bold : FontStyle.Normal;
            }
        }
    }
}

