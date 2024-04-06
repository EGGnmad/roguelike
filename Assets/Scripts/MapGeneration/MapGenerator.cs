using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DelaunayTriangulation;
using MapGeneration;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour, IMapGenerator, IDelaunayTriangulation, ISpanningTree
{
    #region Fields:Serialized

    [Header("Generation:Config")] public float boostTimeScale = 50;
    [SerializeField] private RoomBehavior roomPrefab;

    [Header("Generation:Room")] public float radius;
    public int minRoomSize;
    public int maxRoomSize;
    public int maxRoomCount;

    [Header("Generation:MST")] [Range(0, 1)]
    public float randomPathValue = 0.1f;

    #endregion

    #region Fields:private

    private RoomBehavior[] _rooms;
    private RoomBehavior[] _mainRooms;
    private Triangulation _triangulation;
    private Edge[] _minimumEdges;

    #endregion

    #region Methods:Interface

    public IRoom[] GetRooms()
    {
        return _rooms;
    }

    public Triangulation GetTriangulation()
    {
        return _triangulation;
    }

    public Edge[] GetSpanningTree()
    {
        return _minimumEdges;
    }

    #endregion

    #region Methods:UnityLifeCycle

    private void Start()
    {
        UniTask.Create(Generate);
    }

    #endregion

    #region Methods:Generation

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
            Vector2 size = new Vector2(Random.value, Random.value) * (maxRoomSize - minRoomSize) / 2f;
            size.x = Mathf.Round(size.x + minRoomSize / 2f);
            size.y = Mathf.Round(size.y + minRoomSize / 2f);
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

        return allRooms.Where(room => room.transform.localScale.x * room.transform.localScale.y >= 1.5f * averageArea)
            .ToArray();
    }

    private async UniTask SeperationTask(RoomBehavior[] rooms)
    {
        await UniTask.SwitchToThreadPool();

        bool isRoomsStop = false;

        while (!isRoomsStop)
        {
            isRoomsStop = true;
            foreach (var room in rooms)
            {
                isRoomsStop = isRoomsStop && room.Velocity == Vector3.zero && room.isSeperationStart;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }

        await UniTask.SwitchToMainThread();
    }

    private Triangulation GetDelaunayTriangle(RoomBehavior[] mainRooms)
    {
        List<Vertex> vertices = mainRooms.Select(x => (Vertex)x).ToList();
        Triangulation triangulation = new Triangulation(vertices);
        return triangulation;
    }
    
    public async UniTask Generate()
    {
        // boost map generation speed
        Time.timeScale = boostTimeScale;

        // generate rooms
        _rooms = GenerateRooms(maxRoomCount);
        _mainRooms = GetMainRooms(_rooms);

        // wait until SeperationTask end
        await SeperationTask(_rooms);

        // get delaunay triangles
        _triangulation = GetDelaunayTriangle(_mainRooms);

        // get MST
        _minimumEdges = new MinimumSpanningTree(_triangulation, randomPathValue).GetSpanningTree();

        // back to normal time scale
        Time.timeScale = 1f;
    }

    #endregion
}