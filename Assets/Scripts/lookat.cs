using UnityEngine;

public class lookat : MonoBehaviour
{
    public Transform target;
    public bool follow;

    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        if(follow){
            transform.LookAt(target);
        }
    }
}
