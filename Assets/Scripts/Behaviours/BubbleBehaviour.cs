using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.collider.tag.Equals("FlySensor")) { return; }

        this.gameObject.SetActive(false);
    }
}
