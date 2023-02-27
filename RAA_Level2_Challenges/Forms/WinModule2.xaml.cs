using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;


namespace RAA_Level2_Challenges
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class WinModule2 : Window
    {
        public List<Element> elemList;

        public WinModule2(Document doc, List<Reference> refList)
        {
            InitializeComponent();

            for(int i = 1; i <= 20; i++)
            {
                cmbNumber.Items.Add(i.ToString());
            }

            cmbNumber.SelectedIndex = 0;

            if(refList != null)
            {
                lbxElements.Items.Clear();
                elemList = new List<Element>();

                foreach(Reference curRef in refList)
                {
                    Element curElem = doc.GetElement(curRef);

                    if(curElem is Viewport)
                    {
                        elemList.Add(curElem);
                        //Viewport curVP = curElem as Viewport;
                        //View curView = doc.GetElement(curVP.ViewId) as View;

                        Parameter curParam = curElem.get_Parameter(BuiltInParameter.VIEWPORT_VIEW_NAME);
                        Parameter curParam2 = curElem.get_Parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER);

                        lbxElements.Items.Add(curParam2.AsString() + ": " + curParam.AsString() + " (" + curElem.Id.ToString() + ")");
                        //lbxElements.Items.Add(curElem.Id.ToString());
                    }
                }
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        internal int GetStartNumber()
        {
            string selectedNumber = cmbNumber.SelectedItem.ToString();
            int returnValue = Convert.ToInt32(selectedNumber);
            return returnValue;
        }

        internal List<Element> GetSelectedViews()
        {
            if (elemList != null)
                return elemList;
            else
                return null;
        }
    }
}
