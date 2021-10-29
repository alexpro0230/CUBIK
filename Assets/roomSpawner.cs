using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomSpawner : MonoBehaviour
{
    public enum rooms
    {
        top,
        down,
        left,
        right
    }

    public rooms exit;

    private GameObject room;

    private void Start()
    {
        if(exit == rooms.down)
        {
            //insantiate room that has an exit on it's upper part 
            //rooms up

            int arrayLength = GameObjHodler._i.upRooms.Length;

            int rand = Random.Range(0, arrayLength - 1);

            Instantiate(GameObjHodler._i.upRooms[rand], transform.position, Quaternion.identity);
        }
        else if(exit == rooms.left)
        {
            //instantiate room that has and exit on it's right part
            //rooms right
            
            int arrayLength = GameObjHodler._i.rightRooms.Length;

            int rand = Random.Range(0, arrayLength - 1);

            Instantiate(GameObjHodler._i.rightRooms[rand], transform.position, Quaternion.identity);
        }
        else if(exit == rooms.right)
        {
            //instantiate room that has and exit on it's left part
            //rooms left

            int arrayLength = GameObjHodler._i.leftRooms.Length;

            int rand = Random.Range(0, arrayLength - 1);

            Instantiate(GameObjHodler._i.leftRooms[rand], transform.position, Quaternion.identity);
        }
        else if(exit == rooms.top)
        {
            //instantiate room that has and exit on it's lower part
            //Rooms down

            int arrayLength = GameObjHodler._i.downRooms.Length;

            int rand = Random.Range(0, arrayLength - 1);

            Instantiate(GameObjHodler._i.downRooms[rand], transform.position, Quaternion.identity);
        }
    }
}
