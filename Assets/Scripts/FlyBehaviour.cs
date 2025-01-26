using Cysharp.Threading.Tasks;
using GumFly;
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
    public Transform GumRef;
    [SerializeField]
    private float _wingFlapDurationInSec;

    [SerializeField]
    private float _maxFlightHeight;
    [SerializeField]
    private float _elevationFrequency;
    [SerializeField]
    private Vector2 _anchorPos = Vector2.zero;

    private AudioSource _audioSource;

    private int _flightDirection;

    private bool _isAbleToEvade = false;
    private bool _isEvading = false;
    private Vector2 _evadingPos = Vector2.zero;
    private float _evadingSpeed = 4.0f;

    private float _evasionTotalCooldown = 1.0f;
    private float _evasionCooldown = 1.0f;

    public bool IsDead = false;
    public float WeightAccumulated = 0.0f;

    void Start()
    {
        _anchorPos = transform.position;

        _flightDirection = (Random.value < 0.5f) ? -1 : 1;
        _targetPos = GetNextRegularTargetPosition(_flightDirection);
        _previousPos = GetNextRegularTargetPosition(-_flightDirection);
        
        this.transform.position = Vector2.Lerp(_previousPos, _targetPos, Random.value);
        SetFacing();

        StartCoroutine(WingFlapping());

        _audioSource = GetComponent<AudioSource>();
        _audioSource.pitch = _flyProperty.SoundPitch + (Random.value - 0.5f) * 0.1f;
        // _audioSource.PlayDelayed(Random.value);
        _audioSource.time = _audioSource.clip.length * Random.value;
        _audioSource.Play();
    }
    [SerializeField]
    Vector2 _previousPos = Vector2.zero;
    [SerializeField]
    Vector2 _targetPos = Vector2.zero;

    private Vector2 GetNextRegularTargetPosition(float dir)
    {
        Vector3 newPos = _anchorPos;
        newPos.x = _anchorPos.x + (_flyProperty.MaxRange * dir);
        
        return newPos;
    }

    void Update()
    {
        if(!IsDead)
        {
            float time = Time.time;
        Vector2 currentPos = this.transform.position;

        float totalLength = (_targetPos - _previousPos).magnitude;
        float distanceToTarget = (currentPos - _targetPos).magnitude;
        float distanceAsScale = distanceToTarget / totalLength;
        float speedScale = getSphericalSpeedFromScale01(distanceAsScale);

        if (distanceAsScale <= 0.1f)
        {
            _flightDirection *= -1;
            
            _previousPos = _targetPos;
            _targetPos = GetNextRegularTargetPosition(_flightDirection);

            SetFacing();

            _isEvading = false;
        }

        Vector2 nextPos = _targetPos;
        if (!_isEvading)
        {
            float elevation = Mathf.Sin(time * _elevationFrequency) * _maxFlightHeight;
            nextPos.y += elevation;
        }

        this.transform.position = Vector2.MoveTowards(currentPos, nextPos, _flyProperty.MaxSpeed *  speedScale);
        _evasionCooldown -= Time.deltaTime;
        }
    }

    private float getSphericalSpeedFromScale01(float scale)
    {
        float result = Mathf.Abs(Mathf.Sin(scale * Mathf.PI)) * 0.5f;
        result *= 0.8f;
        result += 0.1f;
        return result;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Bubble")) { return; }
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
        }
    }

    private async void OnCollisionEnter2D(Collision2D other) 
    {
        Color gumColor = Color.white;

        if(other.collider.CompareTag("Bubble"))
        {
            BubbleBehaviour bubbleBehaviour = other.collider.GetComponent<BubbleBehaviour>();
            gumColor = bubbleBehaviour.Mixture.Gum.Color;

            float weight = bubbleBehaviour.Weight;
            WeightAccumulated += weight;
            IsDead = WeightAccumulated >= _flyProperty.WeightThreshold;
            
            if(IsDead)
            {
                Vector2 diff = this.transform.position - other.transform.position;
                Rigidbody2D rigid = GetComponent<Rigidbody2D>();
                rigid.bodyType = RigidbodyType2D.Dynamic;
                rigid.AddForce(diff.normalized * 30000);
                await UniTask.DelayFrame(2, PlayerLoopTiming.FixedUpdate);
                rigid.mass = 100;
                rigid.gravityScale = 10f;       
                
                SoundManager.Instance.PlayEek(_flyProperty.SoundDeathIndex);

                _wingPivot.gameObject.SetActive(false);

                this.enabled = false;
            }
        }
        else if (other.collider.CompareTag("Fly"))
        {
            FlyBehaviour otherFly = other.collider.GetComponent<FlyBehaviour>();

            if(otherFly.IsDead == false) { return; }
            
            gumColor = otherFly.GumRef.GetComponent<SpriteRenderer>().color;
            float weight = otherFly.WeightAccumulated;
            WeightAccumulated += weight;
            IsDead = WeightAccumulated >= _flyProperty.WeightThreshold;

            if(IsDead)
            {
                Vector2 diff = this.transform.position - other.transform.position;
                Rigidbody2D rigid = GetComponent<Rigidbody2D>();
                rigid.bodyType = RigidbodyType2D.Dynamic;
                rigid.AddForce(diff.normalized * 20000);
                await UniTask.DelayFrame(2, PlayerLoopTiming.FixedUpdate);
                rigid.mass = 100;
                rigid.gravityScale = 10f;

                SoundManager.Instance.PlayEek(_flyProperty.SoundDeathIndex);

                _wingPivot.gameObject.SetActive(false);

                this.enabled = false;
            }
        }

        GumRef.GetComponent<SpriteRenderer>().color = gumColor;
        GumRef.gameObject.SetActive(true);
    }

    private void SetFacing()
    {
        Vector3 dir = this.transform.localScale;
        dir.x = _flightDirection * -1; // NOTE(dmytriy): (* -1) as correction because the model faces left as prefab
        this.transform.localScale = dir;
    }

    private IEnumerator WingFlapping()
    {
        while (!IsDead)
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
            Gizmos.DrawSphere(_evadingPos, 10f);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawCube(_anchorPos, Vector3.one * 30);
    }
}
