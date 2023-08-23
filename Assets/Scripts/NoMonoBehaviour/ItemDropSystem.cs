namespace Assets.Scripts.NoMonoBehaviour
{
    using UnityEngine;

    public static class ItemDropSystem
    {
        private static readonly GameObject[] Items = Resources.LoadAll<GameObject>("");

        public static void Drop(Vector3 position)
        {
            var chance = Random.Range(0, 100);
            if (chance < 100)
            {
                var randomItem = Random.Range(0, Items.Length);
                Object.Instantiate(Items[randomItem], position, Quaternion.identity);
            }
        }
    }
}