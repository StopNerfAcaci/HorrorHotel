using System;
using Managers.FSM;
using UnityEngine;

namespace Managers
{
    public class HomeManager : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;

        private void Awake()
        {
            if(!mainCam) mainCam = Camera.main;
        }
        public void ToggleMainCam(bool isActive) => mainCam.gameObject.SetActive(isActive);
    }
}