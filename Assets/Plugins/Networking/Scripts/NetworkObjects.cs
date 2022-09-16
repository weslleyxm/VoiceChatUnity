using UnityEngine; 

namespace Networking
{
    [CreateAssetMenu(fileName = "NetworkObjects", menuName = "Network/NetworkObjects")]
    public class NetworkObjects : ScriptableObject
    {
        private static NetworkObjects _Instance;
        public static NetworkObjects Instance
        {
            get
            {
                if (_Instance == null)
                {
                    NetworkObjects networkObjects = Resources.Load<NetworkObjects>("NetworkObjects");
                    _Instance = networkObjects;
                }

                return _Instance;
            }
        }
         
        public UDictionary<PrefabId, EntityView> networkObjects = new UDictionary<PrefabId, EntityView>();

        public EntityView GetObject(PrefabId prefabId)
        {
            return networkObjects[prefabId];
        }
    }
}