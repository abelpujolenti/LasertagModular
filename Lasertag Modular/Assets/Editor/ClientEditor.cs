using Network.NetEntities;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Client))]
    public class ClientEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Client client = (Client)target;

            if (GUILayout.Button("Connect"))
            {
                client.Connect();
            }
        }
    }
}