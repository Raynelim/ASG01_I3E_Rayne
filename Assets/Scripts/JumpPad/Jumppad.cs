using UnityEngine;

public class Jumppad : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);

        AudioManager.Instance.PlaySound("Bounce");
    }
}
