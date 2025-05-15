using UnityEngine;
using UnityEngine.UI;

namespace UI.Agent
{
    public class CheckCell : MonoBehaviour
    {
        [SerializeField] Image item;
        [SerializeField] GameObject check;

        public void SetImage(string path)
        {
            item.sprite = Resources.Load<Sprite>("ItemsSprites/" + path);
        }

        public void ToggleCheck(bool state)
        {
            check.SetActive(state);
        }

    }
}
