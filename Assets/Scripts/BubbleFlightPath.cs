using GumFly;
using GumFly.Domain;
using UnityEngine;

public class BubbleFlightPath : MonoBehaviour
{
    [SerializeField]
    private Transform gum;
    [SerializeField]
    private Vector2 _v0;
    [SerializeField, Range(0.0f, 1.5f)]
    private float _gravityReversal = 0f;

    private const float _angle = 84.0f;
    private const float _gravity = 2.3f;
    private const float _totalTime = 12.0f;
    private const float _reversalForceY = 17.0f;
    private const float _gravityChangeConst = 6.936f;

    private GameManager _gameManager;
    private LineRenderer _lineRenderer;
    private Vector3[] points = new Vector3[60];

    private Vector2 getPostition(float t, float velocity, float gravityDecay)
    {
        // NOTE(dmytriy): two parameter not in use yet
        float ajustedGravity = (_gravityReversal > 0.0f) ? _gravity + _gravityReversal * _gravityChangeConst : _gravity; 
        Vector2 _v0Final = _v0;
        float x = (_v0Final.x  * Mathf.Cos(_angle * Mathf.Deg2Rad)) * t;
        float y = (_v0Final.y * Mathf.Sin(_angle * Mathf.Deg2Rad)) * t - ((ajustedGravity - _gravityReversal * t) * t * t * _reversalForceY) / 2f;
        
        return new Vector2(x, y);
    }

    private void Start() 
    {
        _gameManager = GameManager.Instance;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
    }

    private void Update() 
    {
        GumGasMixture gasMix = _gameManager.CurrentMixture;
        
        float velocity = 0.0f;
        float gravityDecay = 0.0f;
        foreach(var gas in gasMix.GasAmounts)
        {
            velocity += gas.Gas.VelocityScale * gas.Amount;
            gravityDecay += gas.Gas.GravityDecay * gas.Amount;
        }

        float tStep = 0.2f;
        for (int stepIndex = 1; stepIndex < points.Length; stepIndex++)
        {
            Vector2 pos = getPostition((stepIndex * tStep), velocity, gravityDecay);
            points[stepIndex] = pos;
        }

        _lineRenderer.SetPositions(points);
    }
}
