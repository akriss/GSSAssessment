using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GSSAssessment.Common.Database.JsonTestDb
{
    public class JsonTestDbContext : IDatabaseContext
    {
        private string FileName { get; set; }

        public JsonTestDbContext()
        {
            FileName = "TestJsonDB.json";
        }

        public JsonTestDbContext(string filename)
        {
            FileName = filename;
        }

        public void InitDb()
        {
            WriteMapToFile(new JsonObjectMap());
        }

        public void InitIfNotExists()
        {
            if(!File.Exists(FileName))
            {
                InitDb();
            }
        }

        public void AddOrUpdateModel<T>(T model)
        {
            if(model == null) throw new ArgumentNullException(nameof(model));

            using (var objectMap = LoadObjectMap())
            {
                var properties = objectMap.GetType().GetProperties();

                // Get the property that corresponds with the type of the provided model
                var property = properties.FirstOrDefault(x => x.PropertyType == typeof(List<T>));

                if (property == null)
                    throw new Exception("No list found on JsonObjectMap corresponding to class: " + typeof(T).Name);

                var items = property.GetValue(objectMap) as List<T>;
                if(items == null)
                    items = new List<T>();

                var itemProperties = model.GetType().GetProperties();

                var idProperty = itemProperties.FirstOrDefault(x => x.Name.ToLower() == "id"
                    && (x.PropertyType == typeof(int) || x.PropertyType == typeof(int?)));

                if (idProperty == null)
                    throw new Exception("No Id column of type int or int? found on class: " + typeof(T).Name);

                // If Id is null, it must be an insert
                if (idProperty.GetValue(model) == null)
                {
                    //Get highest current Id
                    var maxId = items.Max(x => idProperty.GetValue(x)) as int?;

                    if (maxId == null)
                        maxId = 0;

                    idProperty.SetValue(model, maxId + 1);

                    items.Add(model);

                    property.SetValue(objectMap, items);
                }
                // If not, it's an update or an insert with a provided ID
                else
                {
                    // Note, this should probably update the object by iterating through all its properties
                    // and updating instead of just removing and readding but it should be fine so long as
                    // json is only used for testing
                    items.RemoveAll(x => idProperty.GetValue(x) as int? == idProperty.GetValue(model) as int?);

                    items.Add(model);

                    property.SetValue(objectMap, items);
                }

                WriteMapToFile(objectMap);
            }
        }

        public List<T> LoadModels<T>(Func<T, bool>? parameters = null)
        {
            var objectMap = LoadObjectMap();

            var properties = objectMap.GetType().GetProperties();

            // Get the property that corresponds with the type of the provided model
            var property = properties.FirstOrDefault(x => x.PropertyType == typeof(List<T>));

            if (property == null)
                throw new Exception("No list found on JsonObjectMap corresponding to class: " + typeof(T).Name);

            var items = property.GetValue(objectMap) as List<T>;

            if (items == null)
                items = new List<T>();

            if (parameters == null)
                return items;

            else 
                return items.Where(parameters).ToList();
        }

        public void RemoveModel<T>(int id)
        {
            var objectMap = LoadObjectMap();

            var properties = objectMap.GetType().GetProperties();

            // Get the property that corresponds with the type of the provided model
            var property = properties.First(x => x.PropertyType == typeof(List<T>));

            if (property == null)
                throw new Exception("No list found on JsonObjectMap corresponding to class: " + typeof(T).Name);

            var items = property.GetValue(objectMap) as List<T>;

            if(items == null)
                items = new List<T>();

            var itemProperties = items.FirstOrDefault()?.GetType().GetProperties();

            var idProperty = itemProperties?.FirstOrDefault(x => x.Name.ToLower() == "id"
                && (x.PropertyType == typeof(int) || x.PropertyType == typeof(int?)));

            if (idProperty == null)
                throw new Exception("No Id column of type int or int? found on class: " + typeof(T).Name);

            items.RemoveAll(x => idProperty.GetValue(x) as int? == id);

            property.SetValue(objectMap, items);

            WriteMapToFile(objectMap);
        }

        private JsonObjectMap LoadObjectMap()
        {
            try
            {
                var json = GetJsonFromFile();

                return GetObjectMapFromJson(json);
            }
            catch(Exception ex)
            {
                throw new Exception("Error deserializing JsonObjectMap from file: " + ex.Message);
            }
        }

        private void WriteMapToFile(JsonObjectMap map)
        {
            var json = JsonSerializer.Serialize(map);

            using (var file = File.OpenWrite(FileName))
            {
                file.SetLength(0);
                file.Position = 0;
                byte[] bytes = new UTF8Encoding(true).GetBytes(json);
                file.Write(bytes, 0, bytes.Length);
            }
        }

        private string GetJsonFromFile()
        {
            string json;
            // read JSON directly from a file
            using (StreamReader file = File.OpenText(FileName))
            {
                json = file.ReadToEnd();
            }

            return json;
        }

        private JsonObjectMap GetObjectMapFromJson(string json)
        {
            return JsonSerializer.Deserialize<JsonObjectMap>(json);
        }

        public void Dispose()
        {
        }
    }
}
