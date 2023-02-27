#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace RAA_Level2_Challenges
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            if(doc.ActiveView is ViewSheet == false)
            {
                TaskDialog.Show("Error", "The current view is not a sheet.");
                return Result.Failed;
            }

            // open form
            WinModule2 currentForm = new WinModule2(doc, null)
            {
                Width = 600,
                Height = 450,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            currentForm.ShowDialog();

            if(currentForm.DialogResult == false)
            {
                return Result.Failed;
            }

            List<Reference> refList = new List<Reference>();
            bool flag = true;

            while(flag == true)
            {
                try
                {
                    Reference curRef = uidoc.Selection.PickObject(ObjectType.Element, "Select views to renumber in order. Please ESC when done.");
                    refList.Add(curRef); 
                }
                catch (Exception)
                {
                    flag = false;
                }
            }

            // open form again
            WinModule2 currentForm2 = new WinModule2(doc, refList)
            {
                Width = 600,
                Height = 450,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            currentForm2.ShowDialog();

            if (currentForm2.DialogResult == false)
            {
                return Result.Failed;
            }

            int counter = 0;
            int startNum = currentForm2.GetStartNumber();
            int currentNum = startNum;
            List<Element> viewList = currentForm2.GetSelectedViews();

            using(Transaction t = new Transaction(doc))
            {
                t.Start("Renumber Views");

                foreach (Element curElem in viewList)
                {
                    Parameter curParam = curElem.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);
                    curParam.Set("zzzzz" + currentNum.ToString());

                    currentNum++;
                }

                currentNum = startNum;

                foreach (Element curElem2 in viewList)
                {
                    Parameter curParam = curElem2.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);
                    curParam.Set(currentNum.ToString());

                    currentNum++;
                    counter++;
                }

                t.Commit();
            }

            List<string> results = new List<string>();
            results.Add("Renumbered " + counter.ToString() + " views.");

            WinResults currentResults = new WinResults(results)
            {
                Width = 500,
                Height = 350,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };
            currentResults.ShowDialog();

            //TaskDialog.Show("Complete", "Renumbered " + counter.ToString() + " views.");

            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
