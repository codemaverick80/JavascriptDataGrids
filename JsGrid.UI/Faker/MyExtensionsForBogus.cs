using Bogus.DataSets;

namespace JsDataGrids.UI.Faker
{
    public static class MyExtensionsForBogus
    {
        public static string Prefix2(this Bogus.DataSets.Name name, Name.Gender gender)
        {
            if (gender == Name.Gender.Male)
            {
                return name.Random.ArrayElement(new[] { "Mr.", "Dr." });
            }
            return name.Random.ArrayElement(new[] { "Miss", "Ms.", "Mrs." });
        }
    }
}