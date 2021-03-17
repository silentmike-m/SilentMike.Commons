using Microsoft.EntityFrameworkCore;
using SilentMike.SqlLite.Data.Interfaces.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SilentMike.SqlLite.Data.Interfaces.Services
{
    public class EntityMappingConfig
    {
        public static void CreateMappings(ModelBuilder modelBuilder, List<AssemblyName> assemblyNames)
        {
            foreach (var name in assemblyNames)
            {
                var assemblyTypes = Assembly.Load(name).DefinedTypes;
                var entityTypes = assemblyTypes.Where(t => t.GetCustomAttributes<EntityMapping>().ToList().Any()).ToList();
                foreach (var entityType in entityTypes)
                {

                    entityType.GetDeclaredMethod("CreateEntityMapping").
                        Invoke(null, new object[] { modelBuilder });
                }
            }
        }
    }
}
