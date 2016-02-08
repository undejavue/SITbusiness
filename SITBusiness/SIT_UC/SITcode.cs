using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SITBusiness.WS_Link;
using SIT_classLib;

namespace SITBusiness
{
    public class SITcode
    {
        public object IsNullOrNot(object a)
        {
            if (a == null)
                return 0;
            else
                return a;
        }

        // Поиск индекса в Combobox из элементов wsProducerItem
        public int SetBoxIndex_wsSimpleItem(ComboBox box, int? itemID)
        {
            if (itemID == null)
            {
                return 0;
            }

            int itemIndex = 0;

            foreach ( wsSimpleItem item in box.ItemsSource as ObservableCollection<wsSimpleItem>)
            {
                if (item.ID == (int)itemID)
                {
                    itemIndex = box.Items.IndexOf(item);
                    break;
                }
            }
            return itemIndex;
        }

        // Поиск индекса в Combobox из элементов wsSimpleItem
        public int SetBoxIndex_wsSimpleItem(ComboBox box, string itemValue)
        {
            if (itemValue == null)
            {
                return 0;
            }
            int i = 1;
            int itemIndex = 0;
            foreach (wsSimpleItem item in box.ItemsSource as ObservableCollection<wsSimpleItem>)
            {
                i++;
                if (item.Description == (string)itemValue)
                {
                    itemIndex = box.Items.IndexOf(item);
                    break;
                }
            }
            return itemIndex;
        }

        // Поиск индекса в Combobox из элементов wsProducerItem
        public int SetBoxIndex_wsProducerType(ComboBox box, int? itemValue)
        {
            if (itemValue == null)
            {
                return 0;
            }
            int i = 1;
            int itemIndex = 0;
            foreach (wsProducerType item in box.ItemsSource as List<wsProducerType>)
            {
                i++;
                if (item.ProducerID == (int)itemValue)
                {
                    itemIndex = box.Items.IndexOf(item);
                    break;
                }
            }
            return itemIndex;
        }
    }


    //--- Динамическое дерево ----------------------------------------------------------------------
    public delegate List<wsBaseItem> GetDataFromDB(int? param);

    public class Node
    {
        private Node _parentNode;

        private wsBaseItem _bi = new wsBaseItem();

        private List<Node> _chldlist = new List<Node>();

        public int countOfElements;

        public wsBaseItem bi { get { return _bi; } set { _bi = value; } }

        public int Level { get { return _parentNode == null ? 0 : _parentNode.Level + 1; } }

        public List<Node> chldlist { get { return _chldlist; } set { _chldlist = value; } }

        public Node ParentNode { get { return _parentNode; } }

        public Node() { }

        public Node(GetDataFromDB f) : this(null, null, f, null) { }

        public Node(wsBaseItem data, int? parentID, GetDataFromDB f, Node parentNode)//,int? parent)
        {
            _parentNode = parentNode;
            _bi = data;

            bool itemType = (data == null) ? true : data.IsType;

            if (itemType == true)
                this.GetChildList(parentID, f, this);

        }
       
        private void GetChildList(int? parentID, GetDataFromDB f, Node parentNode)
        {
            List<wsBaseItem> l = new List<wsBaseItem>();
            l = f(parentID);
            if (l.Count > 0)
            {
                foreach (wsBaseItem bi in l)
                {
                    //Console.WriteLine("{0} {1}  ", bi.ID, bi.Description);
                    _chldlist.Add(new Node(bi, bi.ID, f, parentNode));
                }
            }
        }

        private void GetParentNode(Node n, List<Node> l)
        {
            if (n._parentNode != null && n.ParentNode.ParentNode != null)
            {
                l.Add(n.ParentNode);
                GetParentNode(n.ParentNode, l);
            }

        }

        public List<Node> GetListParentsNode(Node n)
        {
            List<Node> l = new List<Node>();
            GetParentNode(n, l);
            return l;
        }

        public ObservableCollection<wsSimpleItem> getListByLevel(int? level, int? _id)
        {
            List<Node> NodeList = new List<Node>();
            ObservableCollection<wsSimpleItem> listB = new ObservableCollection<wsSimpleItem>();

            switch (level)
            {
                case 1:
                    foreach (Node node1 in chldlist)
                    {
                        NodeList.Add(node1);
                    }
                    break;
                case 2:
                    foreach (Node node1 in chldlist)
                    {
                        if (node1.bi.ID == _id)
                        {
                            foreach (Node node2 in node1.chldlist)
                            {
                                NodeList.Add(node2);
                            }
                            break;
                        }
                    }
                    break;
                case 3:
                    foreach (Node node1 in chldlist)
                    {
                            foreach (Node node2 in node1.chldlist)
                            {
                                if (node2.bi.ID == _id)
                                {
                                    foreach (Node node3 in node2.chldlist)
                                    {
                                        NodeList.Add(node3);
                                    }
                                    break;
                                }
                            }
                    }
                    break;
                //case 4:
                //    foreach (Node node1 in chldlist)
                //    {
                //        if (node1.bi.ID == _id)
                //        {
                //            foreach (Node node2 in node1.chldlist)
                //            {
                //                if (node2.bi.ID == _id)
                //                {
                //                    foreach (Node node3 in node2.chldlist)
                //                    {
                //                        if (node3.bi.ID == _id)
                //                        {
                //                            foreach (Node node4 in node3.chldlist)
                //                            {
                //                                NodeList.Add(node4);
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //    break;
            }

            foreach (Node n in NodeList)
            {
                wsSimpleItem sitem = new wsSimpleItem();
                sitem.ID = n.bi.ID;
                sitem.Description = n.bi.Description;
                listB.Add(sitem);
            }

            return listB;
        }

        public void ThrowTree()
        {
           
        }
    }


}
