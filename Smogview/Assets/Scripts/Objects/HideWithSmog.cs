using UnityEngine;

public class HideWithSmog : MonoBehaviour
{
    private GameObject _smogEffect;
    private GameObject _currentSmogEffect;
    ParticleSystem.ShapeModule _smogShape;

    [SerializeField] private float _particleMultiplier = 1;
    [SerializeField] private float _sizeMultiplier = 1;

    private void Awake()
    {
        _smogEffect = Resources.Load<GameObject>("Effects/SmogEffect");
    }

    public void CreateSmog()
    {
        if (_currentSmogEffect)
            Destroy(_currentSmogEffect);
        _currentSmogEffect = Instantiate(_smogEffect, gameObject.transform.position, gameObject.transform.rotation);

        ParticleSystem.MainModule particleMain = _currentSmogEffect.GetComponent<ParticleSystem>().main;
        particleMain.startSizeMultiplier = _sizeMultiplier;

        ParticleSystem.ShapeModule particleShape = _currentSmogEffect.GetComponent<ParticleSystem>().shape;
        particleShape.spriteRenderer = GetComponent<SpriteRenderer>();
        particleShape.scale = transform.localScale;

        ParticleSystem.EmissionModule particleEmission = _currentSmogEffect.GetComponent<ParticleSystem>().emission;
        particleEmission.rateOverTimeMultiplier = _particleMultiplier;
        ParticleSystem.Burst particleBurst = particleEmission.GetBurst(0);
        particleBurst.maxCount *= (short)_particleMultiplier;
    }

    public void RemoveSmog()
    {
        if(_currentSmogEffect)
            Destroy(_currentSmogEffect);
    }
}
