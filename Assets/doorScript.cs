using UnityEngine;

public class doorScript : MonoBehaviour
{
    public bool openDoor;

    private void Update()
    {
        if (openDoor)
            transform.position += new Vector3(0, Time.deltaTime);
    }
}
