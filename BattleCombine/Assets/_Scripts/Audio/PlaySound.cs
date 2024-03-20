using UnityEngine;
using Zenject;

namespace _Scripts.Audio
{
    public class PlaySound : MonoBehaviour
    {
        //[SerializeField] private string _clipName;

        [Inject] private AudioService service;
    
        public void PlayClip(string clipName)
        {
            service.PlaySound(clipName);
        }
    }
}