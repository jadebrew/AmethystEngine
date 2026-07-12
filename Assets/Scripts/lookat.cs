using UnityEngine;

public class lookat : MonoBehaviour
{
    private Transform target;
    public bool follow;


    void FixedUpdate()
    {
        if (target == null)
        {
            target = GameObject.Find("playercamtarget").transform;
        }
        // Rotate the camera every frame so it keeps looking at the target
        if(follow){
            transform.LookAt(target);
        }
<<<<<<< Updated upstream
=======
        if(moveTowards && follow){
            var pos = target.transform.position;
            if (Vector3.Distance(transform.position, target.position) > 2.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(pos.x,pos.y,pos.z) ,Time.fixedDeltaTime*5);
            }
        }
>>>>>>> Stashed changes
    }
}
