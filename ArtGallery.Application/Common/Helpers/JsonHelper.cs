using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace ArtGallery.Application.Common.Helpers;

public class JsonHelper
{
    public static JsonSerializerSettings GetJsonSerializer
    {
        get
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            settings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy(), true)); //

            return settings;
        }
    }

    public static bool IsValidJson(string value)
    {
        try
        {
            var json = JToken.Parse(value);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsNumeric(Type type)
    {
        if (type == null) { return false; }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            type = type.GetGenericArguments()[0];
        }

        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }

    public static T MaskValues<T>(T payload)
    {
        MethodInfo method = payload.GetType().GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);
        var tempData = (T)method.Invoke(payload, null);

        //var tempData = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(payload));
        var properties = new string[] { "Password", "TransactionPin", "Pin", "OldTransactionPin", "NewTransactionPin" };
        var valueList = tempData.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => properties.Contains(x.Name, StringComparer.OrdinalIgnoreCase)).ToList(); // x.Name.Equals("Password", StringComparison.OrdinalIgnoreCase)

        if (valueList.Count > 0)
        {
            foreach (var property in valueList)
            {
                // set password to value
                if (property != null)
                {
                    property.SetValue(tempData, "********");
                }
            }
        }
        return tempData;
    }

    public static JObject MaskValues(JObject payload)
    {
        var properties = new string[] { "password", "transactionPin", "pin", "oldTransactionPin", "newTransactionPin" };

        foreach (var item in properties)
        {
            // set password to value
            if (payload.ContainsKey(item))
            {
                payload[item] = "********";
            }
        }

        return payload;
    }

    public static JsonSerializerSettings IgnoreOrRenameJsonProperties<T>(string[] ignoreList = null, Dictionary<string, string> renameList = null) where T : class
    {
        //jsonResolver.RenameProperty(typeof(CustomerModel), "FirstName", "firstName");
        var jsonResolver = new JsonPropertyRenameAndIgnoreSerializerContractResolver();

        if (ignoreList != null)
        {
            foreach (var property in ignoreList)
            {
                jsonResolver.IgnoreProperty(typeof(T), property);
            }
        }

        if (renameList != null)
        {
            foreach (var property in renameList)
            {
                jsonResolver.RenameProperty(typeof(T), property.Key, property.Value);
            }
        }

        var serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = jsonResolver
        };

        return serializerSettings;
    }

    public static string CleanJson(string json, string[] properties) // , object value = null
    {
        if (string.IsNullOrEmpty(json))
        {
            return json;
        }

        var token = JToken.Parse(json);

        foreach (var property in properties)
        {
            foreach (var field in token.SelectTokens(property))
            {
                if (field != null && field.Type != JTokenType.String)
                {
                    JValue jvalue = null;
                    field.Replace(jvalue);
                    //var prop = field as JProperty;
                    //var value = field.Value<string>();
                }

                //JArray resources = (JArray)jObject["resources"];
                //foreach (var type3Resource in resources
                //    .Where(obj => obj["type"].Value<string>() == "type3"))
                //{
                //    type3Resource["SpecialValue"] = 3;
                //}

                //if (field.Type == JTokenType.Array)
                //{
                //    var items = (JArray)property.Value;

                //    // Proceed as before.
                //}
            }
            // var field = token.SelectToken(property); // $..Component.Content
        }
        return JsonConvert.SerializeObject(token, Formatting.Indented);
    }

    public static string ObjectToArray(string json, string[] properties) // , object value = null
    {
        if (string.IsNullOrEmpty(json))
        {
            return json;
        }

        var token = JToken.Parse(json);

        foreach (var property in properties)
        {
            foreach (var field in token.SelectTokens(property))
            {
                if (field != null)
                {
                    if (field.Type != JTokenType.Array)
                    {
                        var jvalue = new JArray();
                        jvalue.Add(field.ToObject<object>());
                        field.Replace(jvalue); // new List<object> { field.ToObject<object>() }

                        //JToken token = JToken.Load(reader);
                        //if (token.Type == JTokenType.Array)
                        //    return token.ToObject<List<T>>();
                        //return new List<T> { token.ToObjec<T>() };
                    }
                }
            }
            // var field = token.SelectToken(property); // $..Component.Content
        }
        return JsonConvert.SerializeObject(token, Formatting.Indented);
    }
}

public class JsonPropertyRenameAndIgnoreSerializerContractResolver : DefaultContractResolver
{
    private readonly Dictionary<Type, HashSet<string>> _ignores;
    private readonly Dictionary<Type, Dictionary<string, string>> _renames;

    public JsonPropertyRenameAndIgnoreSerializerContractResolver()
    {
        _ignores = new Dictionary<Type, HashSet<string>>();
        _renames = new Dictionary<Type, Dictionary<string, string>>();
    }

    public void IgnoreProperty(Type type, params string[] jsonPropertyNames)
    {
        if (!_ignores.ContainsKey(type))
            _ignores[type] = new HashSet<string>();

        foreach (var prop in jsonPropertyNames)
            _ignores[type].Add(prop);
    }

    public void RenameProperty(Type type, string propertyName, string newJsonPropertyName)
    {
        if (!_renames.ContainsKey(type))
            _renames[type] = new Dictionary<string, string>();

        _renames[type][propertyName] = newJsonPropertyName;
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (IsIgnored(property.DeclaringType, property.PropertyName))
        {
            property.ShouldSerialize = i => false;
            property.Ignored = true;
        }

        if (IsRenamed(property.DeclaringType, property.PropertyName, out var newJsonPropertyName))
            property.PropertyName = newJsonPropertyName;

        return property;
    }

    private bool IsIgnored(Type type, string jsonPropertyName)
    {
        if (!_ignores.ContainsKey(type))
            return false;

        return _ignores[type].Contains(jsonPropertyName);
    }

    private bool IsRenamed(Type type, string jsonPropertyName, out string newJsonPropertyName)
    {
        Dictionary<string, string> renames;

        if (!_renames.TryGetValue(type, out renames) || !renames.TryGetValue(jsonPropertyName, out newJsonPropertyName))
        {
            newJsonPropertyName = null;
            return false;
        }

        return true;
    }
}
