namespace Assets.Scripts.Enums
{
    public enum EquipmentSets
    {
        None = 0b_0000_0000,
        FireHelmet = 0b_0000_0001,
        FireArmor = 0b_0000_0010,
        FireBoot = 0b_0000_0100,
        FireSet = FireHelmet | FireArmor | FireBoot,
    }
}