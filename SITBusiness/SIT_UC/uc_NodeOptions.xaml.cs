using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Browser;
using SIT_classLib;
using SITBusiness.WS_Link;
using SITBusiness.Views;

namespace SITBusiness
{
    public partial class uc_NodeOptions : UserControl
    {
        WServiceClient wc = new WServiceClient();
        public InfoWindow w = new InfoWindow("Сообщение","Сообщение");
        public int uc_deleteMode; // 0 for delete Type node, 1 for delete Place node
        public wsBaseItem uc_BaseItem = new wsBaseItem();

        public uc_NodeOptions()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(uc_NodeOptions_Loaded);
        }

        void uc_NodeOptions_Loaded(object sender, RoutedEventArgs e)
        {
            wc.ws_updateNodeCompleted += new EventHandler<ws_updateNodeCompletedEventArgs>(wc_ws_updateNodeCompleted);
            wc.ws_deleteNodeCompleted += new EventHandler<ws_deleteNodeCompletedEventArgs>(wc_ws_deleteNodeCompleted);
        }

        public void uc_configure(wsBaseItem bi, int mode=0)
        {
            uc_BaseItem = bi;
            uc_deleteMode = mode;

            grid_modifyItem.DataContext = uc_BaseItem;
        }

        /********************************************* ОБРАБОТЧИКИ   ***************************************************************/ 
        // Удаление элемента дерева
        void wc_ws_deleteNodeCompleted(object sender, WS_Link.ws_deleteNodeCompletedEventArgs e)
        {
            if (e.Result == 1)
            {
                w.txt_Message.Text = "Раздел удален";
                w.Show();
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_DELETE, e.OpStatus.ToString());
                w.Show();
            }
        }

        void wc_ws_updateNodeCompleted(object sender, WS_Link.ws_updateNodeCompletedEventArgs e)
        {
            if (e.Result == 1)
            {
                w.txt_Message.Text = "Раздел обновлен";
                w.Show();
            }
            else
            {
                cwnd_ShitHappens w = new cwnd_ShitHappens(ErrorResources.err_UPDATE, e.OpStatus.ToString());
                w.Show();
            }
        }

        /********************************************* КОД СТРАНИЦЫ   ***************************************************************/ 

        // Удаление раздела
        private void btn_deleteNode_Click(object sender, RoutedEventArgs e)
        {
            switch (uc_deleteMode)
            {
                case 0:
                    wc.ws_deleteNodeAsync((int)uc_BaseItem.ID);
                    break;
                case 1:
                    wc.ws_deletePlaceAsync((int)uc_BaseItem.ID);
                    break;
            }
        }

        //---- Загрузка файлов на сервер --------------------------------------------------------------
        private void btn_chooseupload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "PNG Images (*.png)|*.png";

            bool? retval = dlg.ShowDialog();

            if (retval != null && retval == true) // open/uploaded
            {
                string fileName = dlg.File.Name;
                UploadFile(fileName, dlg.File.OpenRead());
                txt_fileName.Text = fileName;
                txt_imageLink.Text = fileName;

            }
            else //cancel
            {
                txt_fileName.Text = "No file selected...";
            }
        }

        private void UploadFile(string fileName, Stream data)
        {
            string uri = HtmlPage.Document.DocumentUri.ToString();
            string _rootUri;
            _rootUri = uri.Remove(uri.LastIndexOf('/'), uri.Length - uri.LastIndexOf('/'));
            uri = _rootUri;
            _rootUri = uri.Remove(uri.LastIndexOf('/'), uri.Length - uri.LastIndexOf('/'));
            _rootUri += "/receiver.ashx";

            UriBuilder ub = new UriBuilder(_rootUri);
            ub.Query = string.Format("filename={0}", fileName);

            WebClient c = new WebClient();
            c.OpenWriteCompleted += (sender, e) =>
            {
                PushData(data, e.Result);
                e.Result.Close();
                data.Close();
            };
            c.OpenWriteAsync(ub.Uri);

        }

        private void PushData(Stream input, Stream output)
        {

            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        // Редактирование ветви 
        private void btn_modifyItem_Click(object sender, RoutedEventArgs e)
        {
            int ParentID = 0;
            int NodeID = Convert.ToInt32(txt_ID.Text);
            string NodeName = txt_Description.Text;
            if (txt_ParentID.Text != "")
                ParentID = Convert.ToInt32(txt_ParentID.Text);
            string LinkToIcon = txt_imageLink.Text;
            wc.ws_updateNodeAsync(NodeID, NodeName, ParentID, LinkToIcon, (bool)box_NodeIsCalibrated.IsChecked);
        }
    }
}
