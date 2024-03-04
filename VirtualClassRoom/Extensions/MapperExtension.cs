namespace VirtualClassRoom.Extensions;

public static class MapperExtension
{
    public static T MapTo<T>(this object obj) where T : class, new()
    {
        var objType = obj.GetType();
        var objProperties = objType.GetProperties();

        var dto = new T();
        var dtoType = dto.GetType();
        var dtoProperties = dtoType.GetProperties();

        foreach (var objProperty in objProperties)
        {
            if (dtoProperties.Any(p => p.Name == objProperty.Name))
            {
                var dtoProperty = dtoType.GetProperty(objProperty.Name);
                var value = objProperty.GetValue(obj);
                dtoProperty.SetValue(dto, value);
            }
        }
        return dto;
    }

    public static List<T> MapTo<T>(this IEnumerable<object> objs) where T : class, new()
    {
        var result = new List<T>();


        foreach (var obj in objs)
        {
            var objType = obj.GetType();
            var objProperties = objType.GetProperties();

            var dto = new T();
            var dtoType = dto.GetType();
            var dtoProperties = dtoType.GetProperties();

            foreach (var objProperty in objProperties)
            {
                if (dtoProperties.Any(p => p.Name == objProperty.Name))
                {
                    var dtoProperty = dtoType.GetProperty(objProperty.Name);
                    var value = objProperty.GetValue(obj);
                    dtoProperty.SetValue(dto, value);
                }
            }

            result.Add(dto);
        }

        return result;
    }
}
