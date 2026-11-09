using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Destroys the game object when called
/// </summary>
[System.Serializable]
public class ChangeScene : Interaction
{
    [SerializeField] private string gameScene;

    public override void Behaviour()
    {
        SceneManager.LoadSceneAsync(gameScene);
    }
}
