using System.Reflection;

namespace Assignment1
{
    public class StructureTest
    {
        private Dictionary<string, FieldInfo> composerFields = new Dictionary<string, FieldInfo>();
        private Dictionary<string, FieldInfo> pieceFields = new Dictionary<string, FieldInfo>();
        private Dictionary<string, FieldInfo> recordingFields = new Dictionary<string, FieldInfo>();
        private Dictionary<string, FieldInfo> purchaseFields = new Dictionary<string, FieldInfo>();

        private Dictionary<string, Dictionary<string, FieldInfo>> classOverview = new Dictionary<string, Dictionary<string, FieldInfo>>();

        public bool PublicFound { get; set; } = false;

        private static StructureTest instance = new StructureTest();

        private StructureTest()
        {
            classOverview.Add("Composer", composerFields);
            classOverview.Add("Piece", pieceFields);
            classOverview.Add("Recording", recordingFields);
            classOverview.Add("Purchase", purchaseFields);
        }

        public static StructureTest GetInstance()
        {
            return instance;
        }

        private void CheckForLocal(Type checkType, string typeName)
        {
            FieldInfo[] publicInfos = checkType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            if (publicInfos.Length != 0)
            {
                PublicFound = true;
                Console.WriteLine($"\tPublic field found in {typeName}:");
                foreach (FieldInfo field in publicInfos)
                {
                    Console.WriteLine($"\t\t{field.Name}");
                }
            }
        }

        private bool FindField(FieldInfo[] classFields, string fieldName)
        {
            foreach (FieldInfo field in classFields)
            {
                if (field.Name == fieldName)
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckClassStructure(Type checkType, string typeName, string[] fields)
        {
            CheckForLocal(checkType, typeName);

            FieldInfo[] classFields = checkType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (fields.Length > classFields.Length)
            {
                Console.WriteLine($"\tMissing fields in {typeName}, expected at least {fields.Length}, got {classFields.Length}");
                return;
            }



            foreach (string correctField in fields)
            {
                if (!FindField(classFields, correctField))
                {
                    Console.WriteLine($"\tCannot find field {correctField} in {typeName}");
                    return;
                }

                foreach (FieldInfo field in classFields)
                {
                    if (field.Name == correctField)
                    {
                        classOverview[typeName].Add(correctField, field);
                    }
                }
            }

            Console.WriteLine($"\t{typeName} structure check finished: OK");
        }

        public void CheckClasses()
        {
            Console.WriteLine("Class structure check starting");

            CheckClassStructure(typeof(Composer), "Composer", new string[] { "firstName", "lastName" });
            CheckClassStructure(typeof(Piece), "Piece", new string[] { "title", "composer", "catalogue" });
            CheckClassStructure(typeof(Recording), "Recording", new string[] { "piece", "code" });
            CheckClassStructure(typeof(Purchase), "Purchase", new string[] { "recording", "price", "amount", "time" });

            Console.WriteLine("Class structure check finished\n");
        }

        public object GetFieldValue(object obj, string objectClass, string fieldName)
        {
            try
            {
                return classOverview[objectClass][fieldName].GetValue(obj);
            }
            catch (KeyNotFoundException ex)
            {
                return null;
            }
        }

        public void SetFieldValue(object obj, string objectClass, string fieldName, object value)
        {
            classOverview[objectClass][fieldName].SetValue(obj, value);
        }
    }
}
