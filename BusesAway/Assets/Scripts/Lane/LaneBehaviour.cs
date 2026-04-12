using UnityEngine;

namespace BA.Lane
{
    public class LaneBehaviour : MonoBehaviour, ISetup
    {
        private Grid grid;

        void ISetup.Invoke()
        {
            this.grid = GetComponent<Grid>();
        }
    }
}