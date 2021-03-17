using System.Linq;
using System.Reflection;

namespace SilentMike.SqlLite.Data.Interfaces.Services
{
    public class ReflectionMapper<TS, TD>
    {
        public static TD Map(TS source, TD destination)
        {
            var sourceType = source.GetType();
            var sourceProperities = sourceType.GetRuntimeProperties().ToList();

            var destinationType = destination.GetType();
            var destinationProperities = destinationType.GetRuntimeProperties().ToList();

            foreach (var sourceProperity in sourceProperities)
            {
                var sourceValue = sourceProperity.GetValue(source, null);
                if (sourceValue == null)
                    continue;
                var destinationProperity = destinationProperities.FirstOrDefault(d => d.Name.Equals(sourceProperity.Name));
                if (destinationProperity == null || destinationProperity.CustomAttributes.Any(i => i.AttributeType.Name == "NotMappedAttribute"))
                    continue;
                destinationProperity.SetValue(destination, sourceValue);
            }

            return destination;
        }
    }
}
