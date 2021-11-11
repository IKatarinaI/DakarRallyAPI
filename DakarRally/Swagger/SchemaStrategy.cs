using System;

namespace DakarRally.API.Swagger
{
    public class SchemaStrategy
    {
        public static string RemoveSuffix(Type currentClass)
        {
            string returnedValue = currentClass.Name;

            if (returnedValue.EndsWith("ApiModel"))
                returnedValue = returnedValue.Replace("ApiModel", string.Empty);

            return returnedValue;
        }
    }
}
