using UnityEngine;

public class OutOFBounds : MonoBehaviour
{
    public float rangeX = 3f;
    public float rangeZ = 3f;
    public float resetY = -10f;
    public Vector3 resetPosition;
    public Quaternion resetRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resetPosition = transform.position;
        resetRotation = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > rangeX)
        {
            transform.position = new Vector3(rangeX, transform.position.y, transform.position.z);  
        }
        else if (transform.position.x < -rangeX)
        {
            transform.position = new Vector3(-rangeX, transform.position.y, transform.position.z);
        }
        if (transform.position.z > rangeZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, rangeZ);  
        }
        else if (transform.position.z < -rangeZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -rangeZ);
        }
        if (transform.position.y < resetY)
        {
            transform.position = resetPosition;
            transform.rotation = resetRotation;
        }
    }
    
}
