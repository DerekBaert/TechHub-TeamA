using UnityEngine;

public class CollisonsDemo: MonoBehaviour
{


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>())
        {
            Debug.Log("Hit tower");
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }
}
