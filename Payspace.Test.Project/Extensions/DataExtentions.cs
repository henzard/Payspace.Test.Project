using FastMember;
using Microsoft.Data.SqlClient;
using Payspace.Test.Project.Models;

namespace Payspace.Test.Project.Extensions;

public static class DataExtensions
{
    public static T ConvertToObject<T>(this SqlDataReader rd) where T : class, new()
    {
        var type = typeof(T);
        var accessor = TypeAccessor.Create(type);
        var members = accessor.GetMembers();
        var t = new T();

        for (var i = 0; i < rd.FieldCount; i++)
        {
            if (rd.IsDBNull(i)) continue;
            var fieldName = rd.GetName(i);

            if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
            {
                accessor[t, fieldName] = rd.GetValue(i);
            }
        }

        return t;
    }
}