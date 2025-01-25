using GumFly.ScriptableObjects;
using System.Collections;
using UnityEngine;

[RequireComponent (typeof(CircleCollider2D)), RequireComponent(typeof(Rigidbody2D))]
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
        Vector2 initPos = _anchorPos;
        initPos.x = Random.Range(-_flightRadius, _flightRadius);

        StartCoroutine(WingFlapping());
    }

    Vector2 velo2D = Vector2.one;
    void Update()
    {
        float time = Time.time;
        Vector2 oldPos = transform.position;
        Vector2 finalPos = Vector2.zero;

        if(_isEvading && _isAbleToEvade)
        {
            finalPos = Vector2.SmoothDamp(oldPos, _evadingPos, ref velo2D, 0.5f, _evadingSpeed);

            if(Vector2.Distance(oldPos, finalPos) < 0.001f)
            {
                _isEvading = false;
            }
        }
        else
        {
            if(Mathf.Abs(oldPos.x - _anchorPos.x) >= _flightRadius)
            {
                _flightDirection *= -1;

                Vector3 dir = this.transform.localScale;
                dir.x = _flightDirection * -1; // NOTE(dmytriy): (* -1) as correction because the model faces left as prefab
                this.transform.localScale = dir;
            }

            float heighAjustment = Mathf.Cos(time * _elevationFrequency) * _maxFlightHeight;
            Debug.Log(heighAjustment);
            Vector2 target = new Vector2(_anchorPos.x + (_flightDirection * (_flightRadius + 0.2f)), _anchorPos.y + heighAjustment);
            finalPos = Vector2.SmoothDamp(oldPos, target, ref velo2D, 0.5f, _flightSpeed);
        }

        this.transform.position = finalPos;
        _evasionCooldown -= Time.deltaTime;
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
    }

    private IEnumerator WingFlapping()
    {
        while(_isAlive)
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
