using UnityEngine;

public class rotate : MonoBehaviour
{
    public float rotateSpeed = 25f;
    public bool stutter;
    private float startSpeed;
    private float marginSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startSpeed = rotateSpeed;
        marginSpeed = 0;
        if (stutter)
            marginSpeed = rotateSpeed*0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        marginSpeed = Random.Range(-marginSpeed, marginSpeed);
        transform.Rotate(Vector3.up * Time.deltaTime * (rotateSpeed+marginSpeed));
    }
}
