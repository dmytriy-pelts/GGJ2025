using GumFly.Domain;
using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    public GumGasMixture Mixture;
    public float Weight;
    public float Velocity;
    public float GravityDecay;

    public float BubbleDistancePerSec;
    public float PathLength;

    public bool IsReleased = false;

    private float _flightTime;
    private Vector2 _initPos;
    private bool _isPopped = false;

    private float _timeSinceReleaseInSec = 0.0f;

    private Rigidbody2D _rigidbody;

    private void OnEnable() 
    {
        _initPos = this.transform.position;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("FlySensor")) { return; }

        _isPopped = true;
        this.gameObject.SetActive(false);
    }

    public void Release()
    {
        _initPos = this.transform.position;
        IsReleased = true;
    }

    private void FixedUpdate()
    {
        if(IsReleased && !_isPopped)
        {  
            Vector2 oldPos = this.transform.position;
            _timeSinceReleaseInSec += Time.deltaTime;
            float timeStep = Time.deltaTime * (BubbleDistancePerSec / PathLength);
            _flightTime += timeStep;
            Vector2 newPos = BubbleFlightPath.GetPostition(_flightTime, Velocity, GravityDecay);
            _rigidbody.velocity = (newPos + _initPos - oldPos) / Time.fixedDeltaTime;
            _rigidbody.MovePosition(newPos + _initPos);
        }

        if(_timeSinceReleaseInSec > 10f)
        {
            Destroy(this.gameObject);
        }
    }
}
