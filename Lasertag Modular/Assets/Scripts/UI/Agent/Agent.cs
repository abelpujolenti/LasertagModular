using Interface.Agent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Agent
{
    public static class ColorExtensions
    {
        public static Color ToUnityColor(this string hex) =>
            ColorUtility.TryParseHtmlString(hex, out var c) ? c : Color.magenta;
    }

    public class Agent : MonoBehaviour, IBaseAgent
    {
        private byte vestCounter = 0;
        private byte gunCounter = 1;
        private byte mobileCounter = 2;

        CheckCell mobileCell;
        CheckCell gunCell;
        CheckCell vestCell;

        [SerializeField] private GameObject _gridLayout;
        [SerializeField] private GameObject _cellPrefab;

        [SerializeField] private string _mobilePath;
        [SerializeField] private string _gunPath;
        [SerializeField] private string _vestPath;

        public TMP_Text Name;
        public TMP_Text Character;
        public Image TeamBack;

        public void DecorateAgentPanel(string name, string character, bool team)
        {
            Name.text = name;
            Character.text = character;
            TeamBack.color = (team ? "#837DC9" : "#E16C80").ToUnityColor();
        }

        public void Initiliaze()
        {
            mobileCell = CreateImage();
            mobileCell.SetImage(_mobilePath);
            AddImageToGrid(mobileCell.gameObject);

            gunCell = CreateImage();
            gunCell.SetImage(_gunPath);
            AddImageToGrid(gunCell.gameObject);

            vestCell = CreateImage();
            vestCell.SetImage(_vestPath);
            AddImageToGrid(vestCell.gameObject);
        }

        public void IncrementCounter()
        {
            mobileCounter++;
            gunCounter++;
            vestCounter++;
        }

        public CheckCell CreateImage()
        {
            return Instantiate(_cellPrefab).GetComponent<CheckCell>();
        }

        public void AddImageToGrid(GameObject cellGameObject) 
        {
            cellGameObject.transform.SetParent(_gridLayout.transform);
        }

        public void CheckState(byte[] checkState)
        {
            mobileCell.ToggleCheck(checkState[mobileCounter] == 1);
            gunCell.ToggleCheck(checkState[gunCounter] == 1);
            vestCell.ToggleCheck(checkState[vestCounter] == 1);
        }
    }
}
