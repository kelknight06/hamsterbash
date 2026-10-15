using UnityEngine;

public class Bullet : MonoBehaviour
{
    // When the bullet hits an object
    private void OnCollisionEnter(Collision collision)
    {
        // If the bullet hits a target, tell the target it's hit before destroying bullet.
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name + " !");

            Target target = collision.gameObject.GetComponent<Target>();

            if (target != null)
            {
                target.TargetHit();
            }

            Destroy(gameObject);
        }
    }
}
