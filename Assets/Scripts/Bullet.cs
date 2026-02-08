using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print("hit " + collision.gameObject.name + " !");

        if (collision.gameObject.CompareTag("DS"))
        {
            Destroy(this.gameObject);
        }
    }
}
