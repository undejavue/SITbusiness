using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
//using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using SIT_classLib;

namespace SITBusiness.Web
{
    public static class Configuration
    {
        public static string ConnectionString
        {
            get
            {
                //return WebConfigurationManager.AppSettings["storeDbConnectionString"];
                return "Password=qazwsx;Persist Security Info=True;User ID=scada;Initial Catalog=AMRnMEO;Data Source=MNS1-022N\\WINCC";
            }
        }
    }

    public class SIT_DataManager
    {
        public object objectOrNull(object ob)
        {
            if (ob == null)
                return DBNull.Value;
            else
                return ob;
        }


        public string selectMessageText(int mID)
        {
            string s = "В базе отсутствует расшифровка ошибки!";
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_MessagesText");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@message_id", mID);

                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        if (!Reader.IsDBNull(Reader.GetOrdinal("text")))
                            s = (string)Reader["text"];
                    }
                }

                Reader.Close();
                conn.Close();
            }
            return s;
        }

    }

    
    public class DBTree : wsDBTreeBase
    {
        //[DataMember]
        //public List<wsBaseItem> DBDATA
        //{
        //    get { return _dbdata; }
        //}
        //private List<wsBaseItem> _dbdata;
        private List<wsBaseItem> GetDBTableTree()//"SELECT * FROM tbl_DevType"
        {
            List<wsBaseItem> l = new List<wsBaseItem>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_DevType");
                conn.Open();
                cmd.Connection = conn;

                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {

                        wsBaseItem temp = new wsBaseItem();

                        temp.ID = (int)Reader["ID"];
                        temp.Description = (string)Reader["TypeName"];
                        if (!Reader.IsDBNull(Reader.GetOrdinal("Parent")))
                            temp.ParentID = (int?)Reader["Parent"];
                        else
                            temp.ParentID = null;

                        if (!Reader.IsDBNull(Reader.GetOrdinal("LinkToIcon")))
                            temp.LinkToIcon = (string)Reader["LinkToIcon"];
                        else
                            temp.LinkToIcon = null;

                        temp.IsType = true;

                        if (!Reader.IsDBNull(Reader.GetOrdinal("IsCalibration")))
                            temp.IsCalibrated = (bool)Reader["IsCalibration"];
                        

                        l.Add(temp);

                    }
                }

                Reader.Close();
                conn.Close();
            }

            return l;
        }


        private List<wsBaseItem> GetDBTableTree_Ex()//"SELECT * FROM tbl_DevType"
        {
            List<wsBaseItem> l = new List<wsBaseItem>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_DevType");
                conn.Open();
                cmd.Connection = conn;

                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {

                        wsBaseItem temp = new wsBaseItem();

                        temp.ID = (int)Reader["ID"];
                        temp.Description = (string)Reader["TypeName"];
                        if (!Reader.IsDBNull(Reader.GetOrdinal("Parent")))
                            temp.ParentID = (int?)Reader["Parent"];
                        else
                            temp.ParentID = null;

                        if (!Reader.IsDBNull(Reader.GetOrdinal("LinkToIcon")))
                            temp.LinkToIcon = (string)Reader["LinkToIcon"];
                        else
                            temp.LinkToIcon = null;

                        temp.IsType = true;

                        if (!Reader.IsDBNull(Reader.GetOrdinal("IsCalibration")))
                            temp.IsCalibrated = (bool)Reader["IsCalibration"];

                        l.Add(temp);

                    }
                }

                Reader.Close();

                cmd.CommandText = "SELECT ID,LongDescrRU,DevTypeID FROM tbl_DevList";
                Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        wsBaseItem temp = new wsBaseItem();
                        temp.ID = (int)Reader["ID"];
                        temp.Description = (string)Reader["LongDescrRU"];
                        temp.ParentID = (int?)Reader["DevTypeID"];
                        temp.IsType = false;

                        l.Add(temp);
                    }
                }
                conn.Close();
            }

            return l;
        }

        private List<wsBaseItem> GetDBPlaceTableTree()//"SELECT * FROM tbl_DevPlace"
        {
            List<wsBaseItem> l = new List<wsBaseItem>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_DevPlace");
                conn.Open();
                cmd.Connection = conn;
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        wsBaseItem temp = new wsBaseItem();

                        if (!Reader.IsDBNull(Reader.GetOrdinal("ID")))
                            temp.ID = (int?)Reader["ID"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("PlaceName")))
                            temp.Description = (string)Reader["PlaceName"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("Parent")))
                            temp.ParentID = (int?)Reader["Parent"];
                        else
                            temp.ParentID = null;

                        temp.IsType = true;

                        l.Add(temp);
                    }
                }
                Reader.Close();
                conn.Close();
            }
            return l;
        }

        private List<wsBaseItem> GetDBPlaceTableTree_ex()//"SELECT * FROM tbl_DevPlace"
        {
            List<wsBaseItem> l = new List<wsBaseItem>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_DevPlace");
                conn.Open();
                cmd.Connection = conn;
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        wsBaseItem temp = new wsBaseItem();

                        if (!Reader.IsDBNull(Reader.GetOrdinal("ID")))
                            temp.ID = (int?)Reader["ID"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("PlaceName")))
                            temp.Description = (string)Reader["PlaceName"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("Parent")))
                            temp.ParentID = (int?)Reader["Parent"];
                        else
                            temp.ParentID = null;

                        temp.IsType = true;

                        l.Add(temp);
                    }
                }
                Reader.Close();

                cmd.CommandText = "SELECT ID,LongDescrRU,DevPlaceID FROM tbl_DevList";
                Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        wsBaseItem temp = new wsBaseItem();

                        if (!Reader.IsDBNull(Reader.GetOrdinal("ID")))
                            temp.ID = (int)Reader["ID"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("LongDescrRU")))
                            temp.Description = (string)Reader["LongDescrRU"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("DevPlaceID")))
                            temp.ParentID = (int?)Reader["DevPlaceID"];

                        temp.IsType = false;

                        l.Add(temp);
                    }
                }
                conn.Close();
            }
            return l;
        }

        //private List<wsBaseItem> GetDBModelTableTree()//"SELECT * FROM tbl_Model"
        //{
        //    List<wsBaseItem> l = new List<wsBaseItem>();

        //    using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Model");
        //        conn.Open();
        //        cmd.Connection = conn;
        //        SqlDataReader Reader = cmd.ExecuteReader();

        //        if (Reader.HasRows)
        //        {
        //            while (Reader.Read())
        //            {
        //                wsBaseItem temp = new wsBaseItem();
        //                temp.ID = (int)Reader["ID"];
        //                temp.Description = (string)Reader["ModelName"];
        //                if (!Reader.IsDBNull(Reader.GetOrdinal("Parent")))
        //                    temp.ParentID = (int?)Reader["Parent"];
        //                else
        //                    temp.ParentID = null;

        //                temp.IsType = true;

        //                l.Add(temp);
        //            }
        //        }
        //        conn.Close();
        //    }
        //    return l;
        //}


        public DBTree()
            : this(0)
        { }

        public DBTree(int treeType)//0-типы и устройства,1-места установки, 2-модели
        {
            switch (treeType)
            {

                case 4: DBDATA = GetDBTableTree();              // Devices
                    break;
                case 0: DBDATA = GetDBTableTree_Ex();           // Devices extended
                    break;
                case 3: DBDATA = GetDBPlaceTableTree();         // Places
                    break;
                case 1: DBDATA = GetDBPlaceTableTree_ex();      // Places extended
                    break;
                //case 2: DBDATA = GetDBModelTableTree();         // Models
                //    break;

            }
        }
    }
   
}