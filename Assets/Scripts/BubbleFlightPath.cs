using GumFly;
using GumFly.Domain;
using GumFly.Extensions;
using UnityEngine;

public class BubbleFlightPath : MonoBehaviour
{
    [SerializeField]
    private Transform _bubbleRef;
    [SerializeField]
    private Vector2 _v0;
    [SerializeField, Range(0.0f, 1.5f)]
    private float _gravityReversal = 0f;
    [SerializeField]
    private float _bubbleFillScale = 2.0f;

    private const float _angle = 84.0f;
    private const float _gravity = 2.3f;
    private const float _totalTime = 12.0f;
    private const float _reversalForceY = 17.0f;
    private const float _gravityChangeConst = 6.936f;

    private float _bubbleInitScale;
    private GameManager _gameManager;
    private LineRenderer _lineRenderer;
    private Vector3[] points = new Vector3[60];

    private bool _isBubbleFlying = false;
    private float _bubbleFlyTimeInSec = 0.0f;
    private float _bubbleDistanceFlew = 0.0f;
    [SerializeField]
    private float _bubbleDefaultDistancePerSec = 100.0f;
    private Vector2 _releasePoint = Vector2.zero;

    private float _finalVelocity = 0.0f;
    private float _finalGravityDecay = 0.0f;

    [SerializeField]
    private float _pathLength = 0.0f;
    [SerializeField]
    private Vector2 _minBounds = Vector2.zero;
    [SerializeField]
    private Vector2 _maxBounds = Vector2.zero;

    private Vector2 getPostition(float t, float velocity, float gravityDecay)
    {
        // NOTE(dmytriy): two parameter not in use yet
        float ajustedGravity = (_gravityReversal > 0.0f) ? _gravity + _gravityReversal * _gravityChangeConst : _gravity;
        Vector2 _v0Final = _v0;
        float x = (_v0Final.x * Mathf.Cos(_angle * Mathf.Deg2Rad)) * t;
        float y = (_v0Final.y * Mathf.Sin(_angle * Mathf.Deg2Rad)) * t - ((ajustedGravity - _gravityReversal * t) * t * t * _reversalForceY) / 2f;

        return new Vector2(x, y);
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
        //_bubbleRef.gameObject.SetActive(false);
        _bubbleFillScale = _bubbleRef.localScale.x;

        _maxBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
    }

    private void Update()
    {
        /*
        if (_gameManager.State == GameState.Aiming)
        {
            _bubbleRef.gameObject.SetActive(true);
        }
*/
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _isBubbleFlying = true;
            _releasePoint = this.transform.position;
        }
        if (!_isBubbleFlying)
        {
            GumGasMixture gasMix = _gameManager.CurrentMixture;

            _pathLength = 0.0f;
            _finalVelocity = 0.0f;
            _finalGravityDecay = 0.0f;
            float bubbleSize = 0.0f;
            foreach (var gas in gasMix.GasAmounts)
            {
                _finalVelocity += gas.Gas.VelocityScale * gas.Amount;
                _finalGravityDecay += gas.Gas.GravityDecay * gas.Amount;
                bubbleSize += gas.Amount;
            }

            float tStep = 0.2f;
            for (int stepIndex = 1; stepIndex < points.Length; stepIndex++)
            {
                Vector2 pos = getPostition((stepIndex * tStep), _finalVelocity, _finalGravityDecay);
                points[stepIndex] = pos;
                _pathLength = (points[stepIndex - 1] - points[stepIndex]).magnitude;
                pos += (Vector2) this.transform.position;
                if(pos.x > _maxBounds.x || pos.y < _minBounds.y || pos.y > _maxBounds.y)
                {
                    _lineRenderer.positionCount = stepIndex+1;
                    _lineRenderer.SetPositions(points);
                    break;
                }
            }

            //_bubbleRef.localScale = Vector3.one * (_bubbleInitScale + _bubbleFillScale * bubbleSize);
        }
        else
        {
            // _bubbleFlyTimeInSec += Time.deltaTime;
            float timeStep = Time.deltaTime * (_bubbleDefaultDistancePerSec / _pathLength);
            _bubbleFlyTimeInSec += timeStep;// (_lineRenderer.positionCount * 0.2f / _bubbleDefaultDistancePerSec) * Time.deltaTime;
            Vector2 pos = getPostition(_bubbleFlyTimeInSec, _finalVelocity, _finalGravityDecay);
            Vector2 adjustedToReleasePointPos = pos + _releasePoint;
            _bubbleDistanceFlew += ((Vector2)_bubbleRef.transform.position - adjustedToReleasePointPos).magnitude;
            Debug.Log(_bubbleDistanceFlew);
            _bubbleRef.transform.position = adjustedToReleasePointPos;

            Vector2 bubblePos = _bubbleRef.transform.position;
            bool bubbleIsOutOfBounds = (bubblePos.x > _maxBounds.x || bubblePos.y < _minBounds.y || bubblePos.y > _maxBounds.y);
            if(_bubbleRef.gameObject.activeSelf == false || bubbleIsOutOfBounds)
            {
                _isBubbleFlying = false;
                _bubbleFlyTimeInSec = 0.0f;
                // TODO(dmytriy): Reset everything bubble related
            }
        }
    }
}
