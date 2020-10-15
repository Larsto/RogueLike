using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public Color startColor, endColor;
    public int distanceToEnd;

    public Transform generatorPoint;

    public enum Direction { up, right, down,left};
    public Direction selectedDirection;

    public float xOffset = 18f, yOffset = 10f;

    public LayerMask whatIsRoom;

    private GameObject endRoom;

    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomPrefabs rooms;

    private List<GameObject> generatedOutlines = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;

        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        for(int i = 0; i < distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

            layoutRoomObjects.Add(newRoom);

            if(i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                endRoom = newRoom;
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }
        }
        CreateRoomOutline(Vector3.zero);
        foreach(GameObject room in layoutRoomObjects)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);
                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);
                break;
            

        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), 0.2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), 0.2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, -0f, 0f), 0.2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, -0f, 0f), 0.2f, whatIsRoom);

        int directionCount = 0;
        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                Debug.LogError("No room found");
                break;
            case 1:

                if (roomAbove)
                {
                   generatedOutlines.Add(Instantiate(rooms.singleUp, roomPosition, transform.rotation));
                }
                if (roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleDown, roomPosition, transform.rotation));
                }
                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleLeft, roomPosition, transform.rotation));
                }
                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.singleRight, roomPosition, transform.rotation));
                }
                break;
            case 2:
                if(roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpDown, roomPosition, transform.rotation));
                }
                if (roomRight && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleRightLeft, roomPosition, transform.rotation));
                }
                if (roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpRight, roomPosition, transform.rotation));
                }
                if (roomAbove && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleUpLeft, roomPosition, transform.rotation));
                }
                if (roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.doupleDownRight, roomPosition, transform.rotation));
                }
                if (roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.doubleDOwnLeft, roomPosition, transform.rotation));
                }
                break;
            case 3:
                if (roomAbove && roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpDownRight, roomPosition, transform.rotation));
                }
                if (roomAbove && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpDownLeft, roomPosition, transform.rotation));
                }
                if (roomAbove && roomRight && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleUpRightLeft, roomPosition, transform.rotation));
                }
                if (roomBelow && roomRight && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.tripleDownRightLeft, roomPosition, transform.rotation));
                }
                break;
            case 4:
                if (roomAbove && roomBelow && roomRight && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.fourway, roomPosition, transform.rotation));
                }
                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject singleUp, singleDown, singleRight, singleLeft,
        doubleUpDown, doubleRightLeft, doubleUpRight, doubleUpLeft, doupleDownRight, doubleDOwnLeft,
        tripleUpDownRight, tripleUpDownLeft, tripleUpRightLeft, tripleDownRightLeft,
        fourway;
}

     
