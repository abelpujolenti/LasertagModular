using Network.NetEntities;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    namespace Editor
    {
        [CustomEditor(typeof(Client)), CanEditMultipleObjects]
        public class NavMeshGraphButtonEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
            
                Client client = (Client)target;

                if (GUILayout.Button("Connect"))
                {
                    client.Connect();
                }
            }
        }
    }
}