using System.Collections.Generic;
using System.Linq;
using DelaunayTriangulation;
using MapGeneration;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour, IMapGenerator, IDelaunayTriangulation
{
    #region Fields:Serialized

    public float radius;
    public int minRoomSize;
    public int maxRoomSize;
    public int maxRoomCount;
    [Space(4)]
    [SerializeField] private RoomBehavior roomPrefab;
    
    #endregion

    #region Fields:Delaunay

    private RoomBehavior[] _rooms;
    private RoomBehavior[] _mainRooms;
    private Triangulation _triangulation;
    private bool _isSeperationEnd;

    #endregion
    
    #region UnityLifeCycle

    private void Start()
    {
        Time.timeScale = 30;
        
        _rooms = GenerateRooms(maxRoomCount);
        _mainRooms = GetMainRooms(_rooms);
    }

    private void Update()
    {
        if (_isSeperationEnd) return;
        
        foreach (var room in _rooms)
        {
            _isSeperationEnd = _isSeperationEnd || room.Velocity != Vector3.zero;
        }
        _isSeperationEnd = !_isSeperationEnd;

        if (!_isSeperationEnd) return;
        List<Vertex> vertices = _mainRooms.Select(x => (Vertex)x).ToList();
        _triangulation = new Triangulation(vertices);
    }

    #endregion

    #region Generation
    
    private RoomBehavior CreateRoom(Vector2 pos, Vector2 size, int index)
    {
        RoomBehavior newRoom = Instantiate(roomPrefab, transform);

        newRoom.transform.localPosition = pos;
        newRoom.transform.localScale = size;
        newRoom.Index = index;
        
        return newRoom;
    }

    private RoomBehavior[] GenerateRooms(int roomCount)
    {
        var rooms = new RoomBehavior[roomCount];
        
        for (int i = 0; i < roomCount; i++)
        {
            Vector2 randomPos = Random.insideUnitCircle * radius;
            Vector2 size = new Vector2(Random.value, Random.value) * (maxRoomSize-minRoomSize)/2f;
            size.x = Mathf.Round(size.x + minRoomSize/2f);
            size.y = Mathf.Round(size.y + minRoomSize/2f);
            size.x *= 2f;
            size.y *= 2f;
            
            rooms[i] = CreateRoom(randomPos, size, i);
        }

        return rooms;
    }

    private RoomBehavior[] GetMainRooms(RoomBehavior[] allRooms)
    {
        // // width / height
        // float averageWidth = 0f;
        // float averageHeight = 0f;
        // foreach (var room in allRooms)
        // {
        //     averageWidth += room.transform.localScale.x;
        //     averageHeight += room.transform.localScale.y;
        // }
        // averageWidth /= allRooms.Length;
        // averageHeight /= allRooms.Length;
        //
        // return allRooms.Where(room => room.transform.localScale.x >= 1.25f * averageWidth & room.transform.localScale.y >= 1.25f * averageHeight).ToArray();
        
        // area
        float averageArea = 0f;
        foreach (var room in allRooms)
        {
            averageArea += room.transform.localScale.x * room.transform.localScale.y;
        }

        averageArea /= allRooms.Length;

        return allRooms.Where(room => room.transform.localScale.x * room.transform.localScale.y >= 1.5f * averageArea).ToArray();
    }

    private Triangulation GetDelaunayTriangle(RoomBehavior[] mainRooms)
    {
        List<Vertex> vertices = mainRooms.Select(x => (Vertex)x).ToList();
        Triangulation triangulation = new Triangulation(vertices);
        return triangulation;
    }

    #endregion
    
    public IRoom[] GetRooms()
    {
        return _rooms;
    }
    
    public Triangulation GetTriangulation()
    {
        return _triangulation;
    }
}
