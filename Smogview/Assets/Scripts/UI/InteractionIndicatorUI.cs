using UnityEngine;
using UnityEngine.UI;

public class InteractionIndicatorUI : MonoBehaviour
{
    private Image _interactionIndicator;
    public bool Active { get; set; }

    private void Awake()
    {
        _interactionIndicator = GetComponent<Image>();
    }

    public void Move(Vector2 position)
    {
        transform.position = position;
    }

    public void SetFill(float amount)
    {
        _interactionIndicator.fillAmount = amount;
    }
}
