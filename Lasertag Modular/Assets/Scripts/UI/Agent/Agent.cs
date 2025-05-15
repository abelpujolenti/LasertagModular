using Interface.Agent;
using UnityEngine;

namespace UI.Agent
{
    public class Agent : MonoBehaviour, IBaseAgent
    {
        private ushort vestCounter = 0;
        private ushort gunCounter = 1;
        private ushort mobileCounter = 2;

        CheckCell mobileCell;
        CheckCell gunCell;
        CheckCell vestCell;

        [SerializeField] private GameObject gridLayout;
        [SerializeField] private GameObject _cellPrefab;

        [SerializeField] private string _mobilePath;
        [SerializeField] private string _gunPath;
        [SerializeField] private string _vestPath;

        private void Start()
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
            cellGameObject.transform.SetParent(gridLayout.transform);
        }

        public void CheckState(byte[] checkState)
        {
            vestCell.ToggleCheck(checkState[vestCounter] == 1);
            gunCell.ToggleCheck(checkState[gunCounter] == 1);
            mobileCell.ToggleCheck(checkState[mobileCounter] == 1);
        }
    }
}
