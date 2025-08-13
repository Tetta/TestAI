using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    public int value = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.GetComponent<PlayerController>())
        {
            var gc = FindObjectOfType<GameController>();
            if (gc) gc.Add(value);
            Destroy(gameObject);
        }
    }
}
