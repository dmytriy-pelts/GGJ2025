using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("FlySensor")) { return; }

        this.gameObject.SetActive(false);
    }
}
