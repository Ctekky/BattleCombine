using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class BoostButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private TMP_Text _cooldownText;
        [SerializeField] private Image _image;

        public TMP_Text GetValueText {get => _valueText; set => _valueText = value;}
        public TMP_Text GetCooldownText {get => _cooldownText; set => _cooldownText = value;}
        public Image GetImage {get => _image; set => _image = value;}
    }
}