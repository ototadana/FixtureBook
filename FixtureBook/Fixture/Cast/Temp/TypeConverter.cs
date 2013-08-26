using System;
using System.Collections.Generic;
using XPFriend.Fixture.Staff;
using XPFriend.Junk;

namespace XPFriend.Fixture.Cast.Temp
{
    internal class TypeConverter
    {
        private static Dictionary<string, Type> typeMap = new Dictionary<string, Type>();

        static TypeConverter()
        {
            InitializeTypeMap();
        }

        private static void InitializeTypeMap()
        {
            typeMap["string"] = typeof(string);

            typeMap["boolean"] = typeof(bool);
            typeMap["bool"] = typeof(bool);

            typeMap["sbyte"] = typeof(sbyte);
            typeMap["byte"] = typeof(byte);

            typeMap["short"] = typeof(short);
            typeMap["ushort"] = typeof(ushort);
            typeMap["int16"] = typeof(Int16);
            typeMap["uint16"] = typeof(UInt16);

            typeMap["int"] = typeof(int);
            typeMap["integer"] = typeof(int);
            typeMap["uint"] = typeof(uint);
            typeMap["int32"] = typeof(Int32);
            typeMap["uint32"] = typeof(UInt32);

            typeMap["long"] = typeof(long);
            typeMap["ulong"] = typeof(ulong);
            typeMap["int64"] = typeof(Int64);
            typeMap["uint64"] = typeof(UInt64);

            typeMap["float"] = typeof(float);
            typeMap["single"] = typeof(Single);
            typeMap["double"] = typeof(double);

            typeMap["char"] = typeof(char);
            typeMap["character"] = typeof(char);

            typeMap["bigdecimal"] = typeof(decimal);
            typeMap["decimal"] = typeof(decimal);

            typeMap["datetime"] = typeof(DateTime);
            typeMap["timestamp"] = typeof(DateTime);
            typeMap["date"] = typeof(DateTime);
            typeMap["time"] = typeof(DateTime);

            typeMap["map"] = typeof(Dictionary<,>);
            typeMap["dictionary"] = typeof(Dictionary<,>);
            typeMap["list"] = typeof(List<>);
        }

        // このへんの実装はイマイチなので後でどうにかしたい。
        internal static Type ToType(string typeName, string genericTypeParamters)
        {
            return ToGenericType(ToType(typeName), genericTypeParamters);
        }

        private static Type ToGenericType(Type type, string genericTypeParamters) 
        {
            if (!type.IsGenericTypeDefinition)
            {
                return type;
            }
            if (Strings.IsEmpty(genericTypeParamters))
            {
                genericTypeParamters = GetDefaultGenericParameters(type);
            }
            string[] arguments = genericTypeParamters.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Type[] typeArguments = ToType(arguments);
            return type.MakeGenericType(typeArguments);
        }

        // FIXME : これでいいのか？たぶんよくない。
        private static string GetDefaultGenericParameters(Type type)
        {
            if(type.Equals(typeof(Dictionary<,>)))
            {
                return "string,object";
            }
            return "object";
        }

        private static Type[] ToType(string[] typeName)
        {
            Type[] type = new Type[typeName.Length];
            for (int i = 0; i < typeName.Length; i++)
            {
                type[i] = ToGenericType(ToType(typeName[i]), null); // FIXME : 入れ子にに対応していない
            }
            return type;
        }

        private static Type ToType(string typeName)
        {
            if (typeName == null)
            {
                return typeof(string);
            }
            Type type = null;
            typeMap.TryGetValue(typeName.ToLower(), out type);
            if (type != null)
            {
                return type;
            }
            type = Types.GetType(typeName);
            if (type == null)
            {
                return typeof(object);
            }
            return type;
        }

        internal DynaType GetDynaType(Table table)
        {
            return new DynaType(table);
        }

        public static string GetDateTimeFormat(string instance)
        {
            string format = GetDateTimeFormatInternal(instance);
            if (instance.IndexOf('/') > -1)
            {
                format = format.Replace("yyyy-MM-dd", "yyyy/MM/dd");
            }
            return format;
        }

        private static string GetDateTimeFormatInternal(string instance)
        {
            if (instance.Length == 8)
            {
                return "HH:mm:ss";
            }
            if (instance.Length == 10)
            {
                return "yyyy-MM-dd";
            }
            if (instance.Length > 19)
            {
                string formatText = "yyyy-MM-dd HH:mm:ss";
                int scale = GetScale(instance);
                if (scale > 0)
                {
                    formatText = formatText + "." + new String('f', scale);
                }

                if (instance.IndexOfAny(new char[] { '+', '-' }, 19) > -1)
                {
                    formatText = formatText + " zzz";
                }
                return formatText;
            }
            return "yyyy-MM-dd HH:mm:ss";
        }

        private static int GetScale(string instance)
        {
            int dotIndex = instance.IndexOf('.');
            if (dotIndex == -1)
            {
                return 0;
            }

            int scale = 0;
            for (int i = dotIndex + 1; i < instance.Length; i++)
            {
                if (instance[i] == ' ')
                {
                    return scale;
                }
                scale++;
            }
            return scale;
        }

        public static object ChangeType(string textValue, Type type)
        {
            if (Strings.IsEmpty(textValue))
            {
                return null;
            }
            if (typeof(object).Equals(type))
            {
                return textValue;
            }
            if (typeof(DateTime).IsAssignableFrom(type))
            {
                return DateTime.ParseExact(textValue, GetDateTimeFormat(textValue), null);
            }
            if (typeof(DateTimeOffset).IsAssignableFrom(type))
            {
                return DateTimeOffset.ParseExact(textValue, GetDateTimeFormat(textValue), null);
            }
            if (typeof(TimeSpan).IsAssignableFrom(type))
            {
                return TimeSpan.Parse(textValue);
            }
            if (typeof(Guid).IsAssignableFrom(type))
            {
                return new Guid(textValue);
            }
            return Convert.ChangeType(textValue, type);
        }
    }

    internal class DynaRow<T>
    {
        public DynaType DynaType { get; set; }
        public T Instance { get; set; }
        public DynaRow(DynaType dynaType)
        {
            DynaType = dynaType;
            Instance = Activator.CreateInstance<T>();
        }
    }

    internal class DynaType
    {
        private Dictionary<string, DynaColumn> properties = new Dictionary<string, DynaColumn>();
        public DynaColumn this[string name] { get { return properties[name]; } }

        public DynaType(Table table)
        {
            foreach (Column column in table.Columns)
            {
                if (column != null)
                {
                    properties[column.Name] = new DynaColumn(column);
                }
            }
        }

        public DynaRow<T> NewInstance<T>()
        {
            return new DynaRow<T>(this);
        }
    }

    internal class DynaColumn
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public Type ComponentType { get; set; }

        public DynaColumn(Column column)
        {
            Name = column.Name;
            Type = ToType(column);
            ComponentType = TypeConverter.ToType(column.ComponentType, null);
        }

        private Type ToType(Column column)
        {
            if (column.IsArray())
            {
                return ToArrayType(column.ComponentType);
            }

            return TypeConverter.ToType(column.Type, column.ComponentType);
        }

        private Type ToArrayType(string type)
        {
            Type componentType = TypeConverter.ToType(type, null);
            return componentType.MakeArrayType();
        }

        public override string ToString()
        {
            return Name + " (" + Type.Name + ")";
        }
    }
}
