using BetweenTime._Scripts.@base;
using UnityEngine;

namespace BetweenTime._Scripts.steampunk
{
    public class Gear : MonoBehaviour
    {
        private Collider _collider;

        private void Start()
        {
            _collider = GetComponent<Collider>();
        }

        public void MainStateListener(MainState state)
        {
            if(state == MainState.BookBinarySolved && _collider.enabled == false)
                _collider.enabled = true;
        }
    }
}
