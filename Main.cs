using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace RevitAPIWritingDataJSONFile
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand

    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            var rooms = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Rooms)
                .Cast<Room>()
                .ToList();

            //создаем roomDataList со списком RoomData, который будем в процессе выполнения цикла foreach заполнять
            var roomDataList = new List<RoomData>();

            //проходимся по каждому помещению
            foreach (var room in rooms)
            {
                roomDataList.Add(new RoomData
                {
                    Name = room.Name,
                    Number = room.Number

                });

            }

            //преобразуем roomDataList в формат json
            //создаем переменную
            string json = JsonConvert.SerializeObject(roomDataList, Formatting.Indented);

            //вызываем, записываем папку куда это все будем записывать, на рабочий стол по умолчанию
            File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "data.json"), json);

            return Result.Succeeded;
        }
    }
}
