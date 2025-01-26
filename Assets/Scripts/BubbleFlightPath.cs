using GumFly;
using GumFly.Domain;
using UnityEngine;

public class BubbleFlightPath : MonoBehaviour
{
    [SerializeField]
    private Transform _bubblePrefab;

    [SerializeField]
    private static Vector2 _v0 = new Vector2(1500, 185);

    [SerializeField, Range(0.0f, 1.5f)]
    private float _gravityReversal = 0f;

    private float _bubbleFillScale = 15.0f;

    private BubbleBehaviour _bubbleRef;

    private static float _angle = 84.0f;
    private static float _gravity = 2.3f;
    private static float _totalTime = 12.0f;
    private static float _reversalForceY = 17.0f;
    private static float _gravityChangeConst = 6.936f;

    private const float MAX_ADDITIONAL_VELOCITY = 800f;
    private const float MAX_ADDITIONAL_GRAVITY_DACAY = 1.5f;

    private float _bubbleInitScale;
    private GameManager _gameManager;
    private LineRenderer _lineRenderer;
    private Vector3[] points = new Vector3[60];

    [SerializeField]
    private float _bubbleDefaultDistancePerSec = 100.0f;

    private float _finalVelocity = 0.0f;
    private float _finalGravityDecay = 0.0f;

    [SerializeField]
    private float _pathLength = 0.0f;

    [SerializeField]
    private Vector2 _minBounds = Vector2.zero;

    [SerializeField]
    private Vector2 _maxBounds = Vector2.zero;

    public static Vector2 GetPostition(float t, float velocity, float gravityDecay)
    {
        // NOTE(dmytriy): two parameter not in use yet
        float ajustedGravity = _gravity + gravityDecay * _gravityChangeConst;
        Vector2 _v0Final = _v0 + new Vector2(velocity, 0f);
        float x = (_v0Final.x * Mathf.Cos(_angle * Mathf.Deg2Rad)) * t;
        float y = (_v0Final.y * Mathf.Sin(_angle * Mathf.Deg2Rad)) * t -
                  ((ajustedGravity - gravityDecay * t) * t * t * _reversalForceY) / 2f;

        return new Vector2(x, y);
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
        _bubbleInitScale = _bubblePrefab.localScale.x;

        _maxBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
    }

    private void OnEnable()
    {
        if (!_bubbleRef)
        {
            InstantiateBubble();
        }
    }

    private void OnDisable()
    {
        if (_bubbleRef)
        {
            Destroy(_bubbleRef.gameObject);
            _bubbleRef = null;
        }
    }

    public void Shoot()
    {
        SoundManager.Instance.PlaySpit();

        _bubbleRef.Mixture = _gameManager.CurrentMixture;
        _bubbleRef.PathLength = _pathLength;
        _bubbleRef.Weight = _gameManager.CurrentMixture.Gum.Weight;
        _bubbleRef.Velocity = _finalVelocity;
        _bubbleRef.GravityDecay = _finalGravityDecay;
        _bubbleRef.BubbleDistancePerSec = _bubbleDefaultDistancePerSec;

        _bubbleRef.Release();

        // Forget bubble
        _bubbleRef = null;
    }

    private void InstantiateBubble()
    {
        _bubbleRef = Instantiate(_bubblePrefab, this.transform.position, Quaternion.identity)
            .GetComponent<BubbleBehaviour>();
    }

    private void Update()
    {
        if (GameManager.Instance.State != GameState.Aiming) return;

        GumGasMixture gasMix = _gameManager.CurrentMixture;

        _pathLength = 0.0f;
        float velocityFromGas = 0.0f;
        float gravityDecayFromGas = 0.0f;
        float bubbleSize = 0.0f;
        foreach (var gas in gasMix.GasAmounts)
        {
            velocityFromGas += gas.Gas.VelocityScale * gas.Amount;
            gravityDecayFromGas += gas.Gas.GravityDecay * gas.Amount;
            bubbleSize += gas.Gas.SizeScale * gas.Amount;
        }

        _finalVelocity = MAX_ADDITIONAL_VELOCITY * velocityFromGas;
        _finalGravityDecay = MAX_ADDITIONAL_GRAVITY_DACAY * gravityDecayFromGas;

        if (_bubbleRef)
        {
            _bubbleRef.transform.position = transform.position;
            _bubbleRef.transform.localScale = Vector3.one * (_bubbleInitScale + _bubbleFillScale * bubbleSize);
        }

        float tStep = 0.2f;
        for (int stepIndex = 1; stepIndex < points.Length; stepIndex++)
        {
            Vector2 pos = GetPostition((stepIndex * tStep), _finalVelocity, _finalGravityDecay);
            points[stepIndex] = pos;
            _pathLength = (points[stepIndex - 1] - points[stepIndex]).magnitude;
            pos += (Vector2)this.transform.position;
            if (pos.x > _maxBounds.x || pos.y < _minBounds.y || pos.y > _maxBounds.y)
            {
                _lineRenderer.positionCount = stepIndex + 1;
                _lineRenderer.SetPositions(points);
                break;
            }

        }
    }
}