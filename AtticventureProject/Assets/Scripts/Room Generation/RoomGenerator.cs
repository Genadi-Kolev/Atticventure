using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGeneration
{
    public class RoomGenerator : Singleton<RoomGenerator>
    {
        [SerializeField] private bool generateMaze = true;
        [SerializeField] private int maxRounds = 1;

        [SerializeField] private RoomTemplates templates;
        public RoomTemplates Templates { get => templates; }

        [SerializeField] private ItemPools pools;
        public ItemPools Pools { get => pools; }

        public List<GameObject> rooms = new List<GameObject>();


        void Start()
        {
            if (generateMaze)
                StartCoroutine(GenerateMaze());
        }

        private IEnumerator GenerateMaze()
        {
            int round = 0;
            while (round < maxRounds) {
                for (int i = rooms.Count - 1; i >= 0 ; i--) {
                    var spawnPoints = rooms[i].GetComponentInChildren<Room>().roomSpawnPoints;
                    for (int j = spawnPoints.Count - 1; j >= 0 ; j--)
                        spawnPoints[j].SpawnRoom();
                }
                yield return new WaitForSeconds(.5f);
                round++;
            }
            for (int i = rooms.Count - 1; i >= 0 ; i--) {
                var spawnPoints = rooms[i].GetComponentInChildren<Room>().roomSpawnPoints;
                for (int j = spawnPoints.Count - 1; j >= 0 ; j--)
                    spawnPoints[j].ClosingRoomSpawn();
            }

            //  ASSIGN ITEM ROOM
            var itemRoom = PickRoom();
            ReplaceRoom(itemRoom, Templates.ItemRoom);

            //  ASSIGN BOSS LADDER ROOM
            var bossLadderRoom = rooms[rooms.Count - 1];
            ReplaceRoom(bossLadderRoom, Templates.BossLadderRoom);
        }

        public static GameObject PickRoom() {
            int index = UnityEngine.Random.Range(1, RoomGenerator.Instance.rooms.Count - 2);
            var room = RoomGenerator.Instance.rooms[index];
            if ((int)room.GetComponentInChildren<Room>().state != (int)RoomState.Regular)
                room = PickRoom();
            
            return room;
        }

        private void ReplaceRoom(GameObject oldRoom, GameObject newRoom) {
            var transform = oldRoom.transform;                                      //  Cache position to place new room
            var index = rooms.IndexOf(oldRoom);                                     //  Cache index of old room to insert new one at
            var positions = new List<Transform>();
            var mapTile = oldRoom.GetComponentInChildren<Room>().mapTile;
            foreach (var point in oldRoom.GetComponentsInChildren<SpawnPoint>()) {  //  Cache positions of oldRoom's spawnPoints
                positions.Add(point.transform);
            }

            Destroy(oldRoom);
            var tempRoom = Instantiate(newRoom, transform.position, Quaternion.identity);   //  Place new room
            foreach (var spawnPoint in tempRoom.GetComponentsInChildren<SpawnPoint>()) {    //  Compare positions of new points
                if (!CheckIfPointIsInMaze(spawnPoint, positions))                           //  with that of the old ones'
                    spawnPoint.CleanDestroy();                                              //  Remove unnecessary doors
            }
            tempRoom.GetComponentInChildren<Room>().mapTile = mapTile;

            if (index != rooms.Count-1) rooms.Remove(tempRoom);     //  Remove duplucate entry
            rooms[index] = tempRoom;
        }

        private bool CheckIfPointIsInMaze(SpawnPoint spawnPoint, List<Transform> positions) {
            foreach (var position in positions)
            {
                if (Vector2.Equals(position.position, spawnPoint.transform.position))
                    return true;
            }
            return false;
        }
    }
}
