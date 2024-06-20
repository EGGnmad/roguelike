using UnityEngine;

namespace MVVM
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private PropertySelector _id;

        private void Start()
        {
            _id.AddListener(((sender, args) =>
            {
                Debug.Log("Changed");
            }));
        }
    }
}