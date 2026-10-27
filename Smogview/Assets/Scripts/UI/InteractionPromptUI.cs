using UnityEngine;

namespace Smogview.UI
{
    public sealed class InteractionPromptUI : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private GameObject prompt;

        private void OnEnable()
        {
            if (player == null) player = FindAnyObjectByType<Player>();
            prompt.SetActive(false);
        }

        private void LateUpdate()
        {
            bool available = player != null && player.CanInteract;
            if (prompt.activeSelf != available) prompt.SetActive(available);
        }

        private void OnDisable()
        {
            if (prompt != null) prompt.SetActive(false);
        }
    }
}
