using System;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Reflection;

namespace Specflow.RestSharp.Utils
{
    static class Utilities
    {
        public static Dictionary<string, string> ToDictionary(Table table)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var row in table.Rows)
            {
                dictionary.Add(row[0], row[1]);
            }
            return dictionary;
        }

        public static String ToCamelCase(String stringToSplit)
        {
            String[] lines = stringToSplit.Split(' ');
            lines[0] = lines[0].ToLower();
            return string.Join("", lines);
        }

        public static Object CreatePayload(String className, String payloadModification)
        {
            String packageName = "Specflow.RestSharp.Payload.";

            String packageClass = packageName + className;

            Object classInstance = GetClassInstance(packageClass);

            MethodInfo method = getMethod("DeserilizeJsonToClass", packageClass);

            String fileLocation = GetFileLocation("Payloads/JsonFiles/", className);

            //executing method DeserilizeJsonToClass in the Utilities.cs
            object payload = method.Invoke(classInstance, new object[] { fileLocation });

            UpdateValuesInClass(payloadModification, payload);

            return payload;
        }

        public static T DeserilizeJsonToClass<T>(String fileLocation)
        {
            using (StreamReader file = File.OpenText(fileLocation))
            {
                JsonSerializer serializer = new JsonSerializer();
                T deserilizedClass = (T)serializer.Deserialize(file, typeof(T));
                return deserilizedClass;
            }
        }

        private static void UpdateValuesInClass(String payloadModification, Object payload)
        {
            JsonConvert.PopulateObject(payloadModification, payload);
        }

        private static Object GetClassInstance(String packageClass)
        {
            try
            {
                Type classType = Type.GetType(packageClass);

                ConstructorInfo constructor = classType.GetConstructor(Type.EmptyTypes);

                object classInstance = constructor.Invoke(new object[] { });

                return classInstance;
            }
            catch (NullReferenceException e)
            {
                throw new ArgumentException(String.Format("The class: {0} was not found", packageClass), e);
            }
        }

        private static MethodInfo getMethod(String methodName, String packageClass)
        {
            Type classType = Type.GetType(packageClass);

            try
            {
                MethodInfo method = typeof(Utils.Utilities).GetMethod(methodName);

                MethodInfo genericMethod = method.MakeGenericMethod(classType);

                return genericMethod;
            }
            catch (NullReferenceException e)
            {
                throw new ArgumentException(String.Format("The method: {0} was not found in class: {1}", methodName, classType.FullName), e);
            }
        }

        private static String GetFileLocation(String fileLocation, String fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileLocation + fileName + ".json");
        }

        public static Boolean CompareSchema(String schemaName, JObject response)
        {
            String schemaFileLocation = GetFileLocation("Schemas/", schemaName);
            JsonSchema schema = JsonSchema.Parse(File.ReadAllText(schemaFileLocation));
            return response.IsValid(schema);
        }
    }
}

