using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

namespace Game.Level
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private List<Room> _rooms;
        [SerializeField] private NavMeshSurface _navMeshSurface;

        public void Initialize()
        {
            if (_rooms.Count == 0)
            {
                Debug.LogError("Rooms in this level arn't set");
                return;
            }
        
            foreach (var room in _rooms)
            {
                if (room == null)
                {
                    Debug.LogError("Room is null");
                    continue;
                }
            
                room.InitializeDoors();
                room.InitializeRoom();
            }
        }
        
        public void UpdateNavMesh()
        {
            _navMeshSurface.UpdateNavMesh(_navMeshSurface.navMeshData);
        }
    }
}

