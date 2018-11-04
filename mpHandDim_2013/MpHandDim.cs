namespace mpHandDim
{
    using AcApp = Autodesk.AutoCAD.ApplicationServices.Core.Application;
    using System.Collections.Generic;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Runtime;
    using ModPlusAPI;
    using ModPlusAPI.Windows;

    public class MpHandDim
    {
        private const string LangItem = "mpHandDim";

        [CommandMethod("ModPlus", "MpHandDim", CommandFlags.UsePickSet | CommandFlags.Redraw)]
        public static void Main()
        {
            Statistic.SendCommandStarting(new Interface());
            try
            {
                var doc = AcApp.DocumentManager.MdiActiveDocument;
                var db = doc.Database;
                var ed = doc.Editor;
                // Выбираем размеры
                var options = new PromptSelectionOptions
                {
                    MessageForAdding = "\n" + Language.GetItem(LangItem, "msg1")
                };
                var valueArray = new[] { new TypedValue(0, "Dimension") };
                var filter = new SelectionFilter(valueArray);
                var selection = ed.GetSelection(options, filter);
                if (selection.Status != PromptStatus.OK) return;
                // Задаем условие
                var pko = new PromptKeywordOptions(Language.GetItem(LangItem, "msg2"),
                    "Color Select Restore Delete")
                {
                    AllowArbitraryInput = true, AllowNone = true
                };
                var pr = ed.GetKeywords(pko);
                if (pr.Status != PromptStatus.OK || string.IsNullOrEmpty(pr.StringResult)) return;
                // Далее в зависимости от выбранного условия
                switch (pr.StringResult)
                {
                    case "Color":
                        SetColor(selection.Value.GetObjectIds(), db);
                        break;
                    case "Select":
                        SetSelection(selection.Value.GetObjectIds(), db);
                        break;
                    case "Restore":
                        Restore(selection.Value.GetObjectIds(), db);
                        break;
                    case "Delete":
                        Delete(selection.Value.GetObjectIds(), db);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionBox.Show(ex);
            }
        }
        // Выделение цветом
        private static void SetColor(IEnumerable<ObjectId> objids, Database db)
        {
            var colorDialog = new Autodesk.AutoCAD.Windows.ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    foreach (var objid in objids)
                    {
                        var dim = (Dimension)tr.GetObject(objid, OpenMode.ForWrite);
                        if (!string.IsNullOrEmpty(dim.DimensionText))
                            dim.Color = colorDialog.Color;
                    }
                    tr.Commit();
                }
            }
        }
        // Восстановление
        private static void Restore(IEnumerable<ObjectId> objIds, Database db)
        {
            using (var tr = db.TransactionManager.StartTransaction())
            {
                foreach (var objectId in objIds)
                {
                    var dim = (Dimension)tr.GetObject(objectId, OpenMode.ForWrite);
                    if (!string.IsNullOrEmpty(dim.DimensionText))
                        dim.DimensionText = string.Empty;
                }
                tr.Commit();
            }
        }
        // Выделение
        private static void SetSelection(IEnumerable<ObjectId> objIds, Database db)
        {
            using (var tr = db.TransactionManager.StartTransaction())
            {
                var objectIds = new List<ObjectId>();
                foreach (var objectId in objIds)
                {
                    var dim = (Dimension)tr.GetObject(objectId, OpenMode.ForWrite);
                    if (!string.IsNullOrEmpty(dim.DimensionText))
                        objectIds.Add(dim.ObjectId);
                }
                AcApp.DocumentManager.MdiActiveDocument.Editor.SetImpliedSelection(objectIds.ToArray());

                tr.Commit();
            }
        }
        // Удаление
        private static void Delete(IEnumerable<ObjectId> objids, Database db)
        {
            using (var tr = db.TransactionManager.StartTransaction())
            {
                foreach (var objectId in objids)
                {
                    var dim = (Dimension)tr.GetObject(objectId, OpenMode.ForWrite);
                    if (!string.IsNullOrEmpty(dim.DimensionText))
                        dim.Erase();
                }
                tr.Commit();
            }
        }
    }
}
