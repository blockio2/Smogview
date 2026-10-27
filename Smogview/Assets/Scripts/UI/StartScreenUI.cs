using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UIButton = UnityEngine.UI.Button;

namespace Smogview.UI
{
    public sealed class StartScreenUI : MonoBehaviour
    {
        [SerializeField] private string gameScene = "Game";
        [SerializeField] private UIButton playButton;
        [SerializeField] private UIButton quitButton;
        private bool loading;

        private void Awake()
        {
            playButton.onClick.AddListener(Play);
            quitButton.onClick.AddListener(Quit);
        }

        private void Start()
        {
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            if (EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        }

        public void Play()
        {
            if (loading) return;
            loading = true;
            playButton.interactable = false;
            quitButton.interactable = false;
            SceneManager.LoadSceneAsync(gameScene);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
