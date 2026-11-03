using Mono.Cecil;
using UnityEngine;

public class HideWithSmog : MonoBehaviour
{
    private GameObject _smogEffect;
    private GameObject _currentSmogEffect;
    ParticleSystem.ShapeModule _smogShape;

    private void Awake()
    {
        _smogEffect = Resources.Load<GameObject>("Effects/SmogEffect");
    }

    public void CreateSmog()
    {
        _currentSmogEffect = Instantiate(_smogEffect, gameObject.transform.position, gameObject.transform.rotation);
        ParticleSystem.ShapeModule particleShape = _currentSmogEffect.GetComponent<ParticleSystem>().shape;
        particleShape.spriteRenderer = GetComponent<SpriteRenderer>();
        particleShape.scale = transform.localScale;
    }

    public void RemoveSmog()
    {
        Destroy(_currentSmogEffect);
    }
}
