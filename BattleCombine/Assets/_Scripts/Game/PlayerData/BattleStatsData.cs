public class BattleStatsData
{
    private string name;
    private int health;
    private int healthModifier;
    private int healthDefault;
    private int damage;
    private int damageModifier;
    private int damageDefault;
    private int speed;
    private int speedModifier;
    private int speedDefault;
    private bool shield;

    public string Name { get => name; set => name = value; }
    public int CurrentHealth { get => health; set => health = value; }
    public int HealthModifier { get => healthModifier; set => healthModifier = value; }
    public int HealthDefault { get => healthDefault; set => healthDefault = value; }
    public int CurrentDamage { get => damage; set => damage = value; }
    public int DamageModifier { get => damageModifier; set => damageModifier = value; }
    public int DamageDefault { get => damageDefault; set => damageDefault = value; }
    public int CurrentSpeed { get => speed; set => speed = value; }
    public int SpeedModifier { get => speedModifier; set => speedModifier = value; }
    public int SpeedDefault { get => speedDefault; set => speedDefault = value; }
    public bool Shield { get => shield; set => shield = value; }
}

