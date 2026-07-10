using UnityEngine;

public class lookat : MonoBehaviour
{
    public Transform target;
    public bool follow;
    public bool moveTowards;

    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        if(follow){
            transform.LookAt(target);
        }
        if(moveTowards){
            var pos = target.transform.position;
            if (Vector3.Distance(transform.position, target.position) > 2.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos.x,pos.y,pos.z) , 0.01f);
            }
        }
    }
}
