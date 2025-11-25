using UnityEngine;

public class FollowScript : MonoBehaviour
{

    public GameObject Target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        {
            transform.position = Vector2.MoveTowards(transform.position , Target.transform.position , 2 * Time.deltaTime);
        }
    }
}
