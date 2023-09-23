using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro attackText;
    [SerializeField] private TextMeshPro healthText;
    [SerializeField] private SpriteRenderer shieldSprite;

    private bool _isShielded;

    private void SetUpStat(TextMeshPro text, string value)
    {
        text.text = value;
    }

    public void SetUpAllStats(string attackValue, string healthValue, bool isShielded)
    {
        SetUpStat(attackText, attackValue);
        SetUpStat(healthText, healthValue);
        SetShield(isShielded);
        ChangeShieldSprite();
    }

    private void SetShield(bool value)
    {
        _isShielded = value;
        ChangeShieldSprite();
    }

    private void ChangeShieldSprite()
    {
        shieldSprite.enabled = _isShielded;
    }
}