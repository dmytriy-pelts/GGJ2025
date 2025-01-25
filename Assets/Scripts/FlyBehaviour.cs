using GumFly.ScriptableObjects;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D)), RequireComponent(typeof(Rigidbody2D))]
public class FlyBehaviour : MonoBehaviour
{
    [SerializeField]
    private Fly _flyProperty;
    [SerializeField]
    private Transform _wingPivot;
    [SerializeField]
    private float _wingFlapDurationInSec;

    [SerializeField]
    private float _flightRadius = 2.0f;
    [SerializeField]
    private float _maxFlightHeight;
    [SerializeField]
    private float _elevationFrequency;
    [SerializeField]
    private Vector2 _anchorPos = Vector2.zero;

    [SerializeField]
    private float _flightSpeed = 1.0f;

    private int _flightDirection;

    private bool _isAbleToEvade = false;
    private bool _isEvading = false;
    private Vector2 _evadingPos = Vector2.zero;
    private float _evadingSpeed = 4.0f;

    private float _evasionTotalCooldown = 1.0f;
    private float _evasionCooldown = 1.0f;

    private bool _isAlive = true;

    void Start()
    {
        _flightDirection = (Random.Range(0, 1) < 0.5f) ? -1 : 1;
        _targetPos = GetNextRegularTargetPosition(_flightDirection);
        _previousPos = GetNextRegularTargetPosition(-_flightDirection);

        StartCoroutine(WingFlapping());
    }
    [SerializeField]
    Vector2 _previousPos = Vector2.zero;
    [SerializeField]
    Vector2 _targetPos = Vector2.zero;

    private Vector2 GetNextRegularTargetPosition(float dir)
    {
        Vector3 newPos = _anchorPos;
        newPos.x = _anchorPos.x + (_flightRadius * dir);

        Debug.Log(newPos);
        return newPos;
    }

    void Update()
    {
        float time = Time.time;
        Vector2 currentPos = this.transform.position;

        float totalLength = (_targetPos - _previousPos).magnitude;
        float distanceToTarget = (currentPos - _targetPos).magnitude;
        float distanceAsScale = distanceToTarget / totalLength;
        float speedScale = getSphericalSpeedFromScale01(distanceAsScale);
        Debug.Log(distanceAsScale + "   " + speedScale);

        if (distanceAsScale <= 0.1f)
        {
            _flightDirection *= -1;
            // TODO(dmytriy): Set new target
            _previousPos = _targetPos;
            _targetPos = GetNextRegularTargetPosition(_flightDirection);

            Vector3 dir = this.transform.localScale;
            dir.x = _flightDirection * -1; // NOTE(dmytriy): (* -1) as correction because the model faces left as prefab
            this.transform.localScale = dir;

            _isEvading = false;
        }

        Vector2 nextPos = _targetPos;

        if (!_isEvading)
        {
            nextPos.y += Mathf.Sin(time * _elevationFrequency) * _maxFlightHeight;
        }

        this.transform.position = Vector2.MoveTowards(currentPos, _targetPos, _flightSpeed *  speedScale);

        _evasionCooldown -= Time.deltaTime;
    }

    private float getSphericalSpeedFromScale01(float scale)
    {
        return Mathf.Abs(Mathf.Sin(scale * Mathf.PI * 0.5f)) * 0.5f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isAbleToEvade || _evasionCooldown > 0.0f) { return; }

        _isEvading = true;
        Vector2 bubblePos = other.transform.position;
        Vector2 diff = (Vector2)transform.position - bubblePos;
        _evadingPos = (Vector2)transform.position + diff;
        _flightDirection = 1;
        _evasionCooldown = _evasionTotalCooldown;

        if (_isEvading && _isAbleToEvade)
        {
            _targetPos = _evadingPos;
            // TODO(dmytriy): Set new target
        }
    }

    private IEnumerator WingFlapping()
    {
        while (_isAlive)
        {
            yield return new WaitForSeconds(_wingFlapDurationInSec);

            Vector3 wingScale = _wingPivot.localScale;
            wingScale.y *= -1;
            _wingPivot.localScale = wingScale;

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_isEvading)
        {
            Gizmos.DrawSphere(_evadingPos, 0.1f);
        }
    }
}
