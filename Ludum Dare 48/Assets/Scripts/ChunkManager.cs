using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Material activeGradientMaterial;
    [SerializeField] private Material offsetGradientMaterial;

    [Header("Fields")]
    [SerializeField] private Gradient gradient;
    [SerializeField] private float fadePeriodInSeconds = 100f;

    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    //private float _startTime;
    private float _secondsSinceStart = 0f;

    private void Start()
    {
        //GameManager.Instance.OnGameStart.AddListener(() => _secondsSinceStart = 0);
    }

    private void Update()
    {
        _secondsSinceStart += Time.deltaTime;
        if (fadePeriodInSeconds <= 0f) { return; }

        //var secondsSinceStart = Time.time - _startTime;
        var remainder = _secondsSinceStart % fadePeriodInSeconds;
        var gradientProgress = remainder / fadePeriodInSeconds;
        //activeGradientMaterial.color = gradient.Evaluate(gradientProgress);

        activeGradientMaterial.SetColor(BaseColor, gradient.Evaluate(gradientProgress));

        //print(activeGradientMaterial.color);

        //var offsetGradientProgress = gradientProgress + 0.25f;
        //if (offsetGradientProgress > 1) { offsetGradientProgress -= 1f; }
        //offsetGradientMaterial.color = gradient.Evaluate(offsetGradientProgress);
    }

}
