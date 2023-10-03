namespace Assets.Scripts.Items.Equipments
{
    using Enums;
    using Interfaces;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class HelpLibraryForEquipmentSet
    {
        public static void CheckSet(Player owner, GameObject prefab, EquipmentSets set)
        {
            if (((owner.Inventory.Helmet as IEquipment)?.EquipmentSet
                 |
                 (owner.Inventory.Armor as IEquipment)?.EquipmentSet
                 |
                 (owner.Inventory.Boot as IEquipment)?.EquipmentSet) == set)
            {
                var skill = CheckSkill(owner);
                if (skill is null)
                    Object.Instantiate(prefab, owner.gameObject.transform);
            }
        }

        public static IEffect CheckSkill(Player owner)
        {
            if (owner.GetComponentsInChildren<IEffect>() is { } effects)
            {
                foreach (var effect in effects)
                {
                    if (effect.IsActive is true)
                    {
                        return effect;
                    }
                }
            }

            return null;
        }
    }
}