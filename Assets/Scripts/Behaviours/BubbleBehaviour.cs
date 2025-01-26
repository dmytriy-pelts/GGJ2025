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



    private void OnEnable() 
    {
        _initPos = this.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("FlySensor")) { return; }

        _isPopped = true;
        this.gameObject.SetActive(false);
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
            // this.transform.position = newPos + _initPos;
            GetComponent<Rigidbody2D>().velocity = (newPos + _initPos - oldPos) / Time.fixedDeltaTime;
            //Debug.Log("New Pos: " + (newPos + _initPos) + "\nOld Pos: " + oldPos + "\nDiff Pos: " + (newPos + _initPos - oldPos) + "\nVelo: " + GetComponent<Rigidbody2D>().velocity);
            GetComponent<Rigidbody2D>().MovePosition(newPos + _initPos);
        }

        if(_timeSinceReleaseInSec > 10f)
        {
            Destroy(this.gameObject);
        }
    }
}
