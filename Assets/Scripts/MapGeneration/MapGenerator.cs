using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DelaunayTriangulation;
using MapGeneration;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour, IAsyncGenerator, IMapGenerator, IDelaunayTriangulation, ISpanningTree
{
    #region Fields:Serialized

    [TabGroup("Config")] public float boostTimeScale = 50;
    [TabGroup("Config"), Required, AssetsOnly, SerializeField] private RoomBehavior roomPrefab;
    [TabGroup("Config"), Required, AssetsOnly, SerializeField] private HallwayBehavior hallwayPrefab;

    [TabGroup("Room"), Unit(Units.Meter)] public float radius;
    [TabGroup("Room")] public int minRoomSize;
    [TabGroup("Room")] public int maxRoomSize;
    [TabGroup("Room")] public int maxRoomCount;

    [TabGroup("MST"), Range(0, 1)] public float randomPathValue = 0.1f;

    #endregion

    #region Fields:private
    [Inject] private IObjectResolver _container;
    
    private RoomBehavior[] _rooms;
    private RoomBehavior[] _mainRooms;
    private Edge[] _edges;
    private Edge[] _minimumEdges;
    private HallwayBehavior[] _hallways;

    #endregion

    #region Methods:Interface

    public IRoom[] GetRooms()
    {
        return _rooms;
    }

    public Edge[] GetEdges()
    {
        return _edges;
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
        RoomBehavior newRoom = _container.Instantiate(roomPrefab, transform);

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

    private Edge[] GetEdges(RoomBehavior[] mainRooms)
    {
        List<Vertex> vertices = mainRooms.Select(x => (Vertex)x).ToList();
        Triangulation triangulation = new Triangulation(vertices);
        
        Edge[] edges = new Edge[triangulation.triangles.Count * 3];
        for (int i = 0; i < triangulation.triangles.Count; i++)
        {
            edges[i * 3] = triangulation.triangles[i].edge0;
            edges[i * 3 + 1] = triangulation.triangles[i].edge1;
            edges[i * 3 + 2] = triangulation.triangles[i].edge2;
        }

        return edges;
    }

    private HallwayBehavior CreateHallway(Edge edge)
    {
        HallwayBehavior newHallway = _container.Instantiate(hallwayPrefab, transform);

        Vector2 pos = Vector2.Lerp(edge.point0.position, edge.point1.position, 0.5f);
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);
        newHallway.transform.localPosition = pos;
        newHallway.transform.localScale = new Vector2(Mathf.Abs((edge.point0.position - edge.point1.position).x), Mathf.Abs((edge.point0.position - edge.point1.position).y))+2*Vector2.one;

        return newHallway;
    }
    
    private HallwayBehavior[] CreateHallways(Edge[] mst)
    {
        List<HallwayBehavior> hallways = new();
        
        foreach (var edge in mst)
        {
            RoomBehavior room0 = _mainRooms.FirstOrDefault(x => x.Index == edge.point0.index);
            RoomBehavior room1 = _mainRooms.FirstOrDefault(x => x.Index == edge.point1.index);
            
            if(!room0 || !room1) continue;

            Vector2 middlePoint = Vector2.Lerp(edge.point0.position, edge.point1.position, 0.5f);
            
            // in x boundary
            Vector2 min0 = (Vector2)room0.transform.position - room0.GetSize()/2;
            Vector2 max0 = (Vector2)room0.transform.position + room0.GetSize()/2;
            Vector2 min1 = (Vector2)room1.transform.position - room1.GetSize()/2;
            Vector2 max1 = (Vector2)room1.transform.position + room1.GetSize()/2;

            if ((min0.x <= middlePoint.x && middlePoint.x <= max0.x) && (min1.x <= middlePoint.x && middlePoint.x <= max1.x))
            {
                Vector2 room0Pos = new Vector2(middlePoint.x, room0.transform.position.y);
                Vector2 room1Pos = new Vector2(middlePoint.x, room1.transform.position.y);

                Edge hallwayEdge = new Edge(new Vertex(room0Pos, room0.Index), new Vertex(room1Pos, room1.Index));
                HallwayBehavior hallway = CreateHallway(hallwayEdge);
                hallways.Add(hallway);
            }
            
            // in y boundary
            else if ((min0.y <= middlePoint.y && middlePoint.y <= max0.y) && (min1.y <= middlePoint.y && middlePoint.y <= max1.y))
            {
                Vector2 room0Pos = new Vector2(room0.transform.position.x, middlePoint.y);
                Vector2 room1Pos = new Vector2(room1.transform.position.x, middlePoint.y);

                Edge hallwayEdge = new Edge(new Vertex(room0Pos, room0.Index), new Vertex(room1Pos, room1.Index));
                HallwayBehavior hallway = CreateHallway(hallwayEdge);
                hallways.Add(hallway);
            }
            
            // L
            else
            {
                Vector2 room0Pos = room0.transform.position;
                Vector2 room1Pos = room1.transform.position;

                Vector2 middlePos0 = new Vector2(room0Pos.x, room1Pos.y);
                Vector2 middlePos1 = new Vector2(room1Pos.x, room0Pos.y);

                int cnt0 = Physics2D.LinecastAll(room0Pos, middlePos0).Where(x => x.transform.GetComponent<HallwayBehavior>() != null).Count() + Physics2D.LinecastAll(middlePos0, room1Pos).Where(x => x.transform.GetComponent<HallwayBehavior>() != null).Count();
                int cnt1 = Physics2D.LinecastAll(room0Pos, middlePos1).Where(x => x.transform.GetComponent<HallwayBehavior>() != null).Count() + Physics2D.LinecastAll(middlePos1, room1Pos).Where(x => x.transform.GetComponent<HallwayBehavior>() != null).Count();

                if (cnt0 < cnt1)
                {
                    Edge edge0 = new Edge(new Vertex(room0Pos, room0.Index), new Vertex(middlePos0, -1));
                    Edge edge1 = new Edge(new Vertex(middlePos0, -1), new Vertex(room1Pos, room1.Index));
                    HallwayBehavior hallway0 = CreateHallway(edge0);
                    HallwayBehavior hallway1 = CreateHallway(edge1);
                    hallways.Add(hallway0);
                    hallways.Add(hallway1);
                }
                else
                {
                    Edge edge0 = new Edge(new Vertex(room0Pos, room0.Index), new Vertex(middlePos1, -1));
                    Edge edge1 = new Edge(new Vertex(middlePos1, -1), new Vertex(room1Pos, room1.Index));
                    HallwayBehavior hallway0 = CreateHallway(edge0);
                    HallwayBehavior hallway1 = CreateHallway(edge1);
                    hallways.Add(hallway0);
                    hallways.Add(hallway1);
                }
            }
        }

        return hallways.ToArray();
    }

    private RoomBehavior[] GetResultRooms(HallwayBehavior[] hallways)
    {
        List<RoomBehavior> resultRooms = new();
        foreach (var hallway in hallways)
        {
            Transform hTr = hallway.transform;
            Collider2D[] room = Physics2D.OverlapBoxAll(hTr.position, hTr.localScale, hTr.rotation.eulerAngles.z);
            resultRooms.AddRange(room.Select(x => x.GetComponent<RoomBehavior>()).Where(x => x != null));
        }
        
        resultRooms.AddRange(_mainRooms);
        return resultRooms.Distinct().ToArray();
    }

    private void RoomsGeneration()
    {
        foreach (var hallway in _hallways)
        {
            foreach (var generator in hallway.GetComponents<IGenerator>())
            {
                generator.Generate();
            }
        }
        
        foreach (var room in _rooms)
        {
            foreach (var generator in room.GetComponents<IGenerator>())
            {
                generator.Generate();
            }
        }
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

        // get delaunay triangles(mesh)
        _edges = GetEdges(_mainRooms);

        // get MST
        _minimumEdges = new MinimumSpanningTree(_edges, randomPathValue).GetSpanningTree();
        
        // get hallway edges
        _hallways = CreateHallways(_minimumEdges);
        
        // get result rooms
        var mainRooms = GetResultRooms(_hallways);
        ActiveOnly(mainRooms); // for debug
        _rooms = mainRooms;
        
        // fill rooms
        RoomsGeneration();

        // back to normal timescale
        Time.timeScale = 1f;
    }

    #endregion

    #region Methods:Visual

    private void ActiveOnly(params RoomBehavior[][] allRooms)
    {
        foreach (var room in _rooms)
        {
            room.gameObject.SetActive(false);
        }

        foreach (var rooms in allRooms)
        {
            foreach (var room in rooms)
            {
                room.gameObject.SetActive(true);

            }
        }
    }

    #endregion
}