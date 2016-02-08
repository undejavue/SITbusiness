using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using System.Web.Configuration;
using System.Runtime.Serialization;
using SIT_classLib;

namespace SITBusiness.Web
{
    // Класс для работы с устройствами 
    public class DeviceDataManager : SIT_DataManager
    {
        //------ PASSPORTS --------------------------------------------------------------
        public wsPassportExtended selectPasport(int ID)
        {
            wsPassportExtended l = new wsPassportExtended();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_DevicePassport");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ID);

                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    if (!Reader.IsDBNull(Reader.GetOrdinal("DevID")))
                        l.DevID = (int)Reader["DevID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("PassportNo")))
                        l.DevPassportNo = (string)Reader["PassportNo"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("InvNo")))
                        l.DevInvNo = (string)Reader["InvNo"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("LongDescrRU")))
                        l.DevDescrRU = (string)Reader["LongDescrRU"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Model_ID")))
                        l.DevModelID = (int)Reader["Model_ID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Model")))
                        l.DevModel = (string)Reader["Model"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("ProducerID")))
                        l.ProducerID = (int)Reader["ProducerID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("ProducerName")))
                        l.ProducerName = (string)Reader["ProducerName"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Country")))
                        l.ProducerCountry = (string)Reader["Country"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("State")))
                        l.ProducerState = (string)Reader["State"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("City")))
                        l.ProducerCity = (string)Reader["City"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("StreetBuilding")))
                        l.ProducerStreetBuilding = (string)Reader["StreetBuilding"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Phone")))
                        l.ProducerPhone = (string)Reader["Phone"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("ProduceDate")))
                        l.ProduceDate = (DateTime)Reader["ProduceDate"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("PurchaseDate")))
                        l.PurchaseDate = (DateTime)Reader["PurchaseDate"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("ImplementationDate")))
                        l.ImplementationDate = (DateTime)Reader["ImplementationDate"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("CurrentStateID")))
                        l.CurrentStateID = (int)Reader["CurrentStateID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("StateName")))
                        l.CurrentStateName = (string)Reader["StateName"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("DevPlaceID")))
                        l.DevPlaceID = (int)Reader["DevPlaceID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("DevTypeID")))
                        l.DevTypeID = (int)Reader["DevTypeID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("IsCalibration")))
                        l.IsCalibrated = (bool)Reader["IsCalibration"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("CalibrationPeriod")))
                        l.CalibrationPeriod = (int)Reader["CalibrationPeriod"];

                }
            }
            return l;
        }

        public int insertPassport(wsPassportExtended pT, int ID)//lp_ins_Device
        {
            int ret = 0;
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_ins_Device");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.AddWithValue("@DevTypeID", ID);
                cmd.Parameters.AddWithValue("@LongDescrRu", pT.DevDescrRU);
                cmd.Parameters.AddWithValue("@PassportNo", pT.DevPassportNo);
                cmd.Parameters.AddWithValue("@InvNo", pT.DevInvNo);
                cmd.Parameters.AddWithValue("@ProduceDate", pT.ProduceDate);
                cmd.Parameters.AddWithValue("@PurchaseDate", pT.PurchaseDate);
                cmd.Parameters.AddWithValue("@ImplementationDate", pT.ImplementationDate);
                cmd.Parameters.AddWithValue("@Model", pT.DevModel);
                cmd.Parameters.AddWithValue("@ProducerID", pT.ProducerID);
                cmd.Parameters.AddWithValue("@Name", pT.ProducerName);
                cmd.Parameters.AddWithValue("@Country", pT.ProducerCountry);
                cmd.Parameters.AddWithValue("@State", pT.ProducerState);
                cmd.Parameters.AddWithValue("@City", pT.ProducerCity);
                cmd.Parameters.AddWithValue("@StreetBuilding", pT.ProducerStreetBuilding);
                cmd.Parameters.AddWithValue("@Phone", pT.ProducerPhone);
                cmd.Parameters.AddWithValue("@PlaceID", pT.DevPlaceID);
                //cmd.Parameters.AddWithValue("@CalibrationPeriod", pT.CalibrationPeriod;

                cmd.ExecuteNonQuery();
                ret = (int)cmd.Parameters["@ReturnVal"].Value;
                conn.Close();
            }

            return ret;
        }

        public void checkPassport(wsProducerType P, out int OutVal, out string OutText)//lp_sel_CheckDevicePassportData
        {
            wsProducerType l = new wsProducerType();
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_CheckDevicePassportData");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", P.ProducerName);
                cmd.Parameters.AddWithValue("@Country", P.ProducerCountry);

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                SqlDataReader Reader = cmd.ExecuteReader();

                Reader.Close();
                OutVal = (int)cmd.Parameters["@ReturnVal"].Value;
                OutText = selectMessageText(OutVal);
                conn.Close();
            }
        }

        public int updatePassport(wsPassportExtended pT)//lp_upd_DevicePassport
        {
            int ret = 999;
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_upd_DevicePassport");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.AddWithValue("@DevID", pT.DevID);
                cmd.Parameters.AddWithValue("@LongDescrRu", pT.DevDescrRU);
                cmd.Parameters.AddWithValue("@PassportNo", pT.DevPassportNo);
                cmd.Parameters.AddWithValue("@InvNo", pT.DevInvNo);
                cmd.Parameters.AddWithValue("@ProduceDate", pT.ProduceDate);
                cmd.Parameters.AddWithValue("@PurchaseDate", pT.PurchaseDate);
                cmd.Parameters.AddWithValue("@ImplementationDate", pT.ImplementationDate);
                cmd.Parameters.AddWithValue("@Model", pT.DevModel);
                cmd.Parameters.AddWithValue("@ProducerID", pT.ProducerID);
                cmd.Parameters.AddWithValue("@Name", pT.ProducerName);
                cmd.Parameters.AddWithValue("@Country", pT.ProducerCountry);
                cmd.Parameters.AddWithValue("@State", pT.ProducerState);
                cmd.Parameters.AddWithValue("@City", pT.ProducerCity);
                cmd.Parameters.AddWithValue("@StreetBuilding", pT.ProducerStreetBuilding);
                cmd.Parameters.AddWithValue("@Phone", pT.ProducerPhone);
                cmd.Parameters.AddWithValue("@PlaceID", pT.DevPlaceID);
                //cmd.Parameters.AddWithValue("@CalibrationPeriod", pT.CalibrationPeriod;

                cmd.ExecuteNonQuery();
                ret = (int)cmd.Parameters["@ReturnVal"].Value;
                conn.Close();
            }
            return ret;
        }

        public List<wsPassportExtended> selectDevicesPassports(int? TypeID, int? PlaceID) // lp_sel_DevicesPassport
        {
            List<wsPassportExtended> list = new List<wsPassportExtended>();
            
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_DevicesPassport");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TypeID", objectOrNull(TypeID));
                cmd.Parameters.AddWithValue("@PlaceID", objectOrNull(PlaceID));

                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    wsPassportExtended device = new wsPassportExtended();

                    if (!Reader.IsDBNull(Reader.GetOrdinal("DevID")))
                        device.DevID = (int)Reader["DevID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("PassportNo")))
                        device.DevPassportNo = (string)Reader["PassportNo"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("InvNo")))
                        device.DevInvNo = (string)Reader["InvNo"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("LongDescrRU")))
                        device.DevDescrRU = (string)Reader["LongDescrRU"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Model_ID")))
                        device.DevModelID = (int)Reader["Model_ID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Model")))
                        device.DevModel = (string)Reader["Model"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("ProducerID")))
                        device.ProducerID = (int)Reader["ProducerID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("ProducerName")))
                        device.ProducerName = (string)Reader["ProducerName"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Country")))
                        device.ProducerCountry = (string)Reader["Country"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("State")))
                        device.ProducerState = (string)Reader["State"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("City")))
                        device.ProducerCity = (string)Reader["City"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("StreetBuilding")))
                        device.ProducerStreetBuilding = (string)Reader["StreetBuilding"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Phone")))
                        device.ProducerPhone = (string)Reader["Phone"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("ProduceDate")))
                        device.ProduceDate = (DateTime)Reader["ProduceDate"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("PurchaseDate")))
                        device.PurchaseDate = (DateTime)Reader["PurchaseDate"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("ImplementationDate")))
                        device.ImplementationDate = (DateTime)Reader["ImplementationDate"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("CurrentStateID")))
                        device.CurrentStateID = (int)Reader["CurrentStateID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("StateName")))
                        device.CurrentStateName = (string)Reader["StateName"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("DevPlaceID")))
                        device.DevPlaceID = (int)Reader["DevPlaceID"];   

                    if (!Reader.IsDBNull(Reader.GetOrdinal("DevTypeID")))
                        device.DevTypeID = (int)Reader["DevTypeID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("TypeName")))
                        device.DevTypeName = (string)Reader["TypeName"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("PlaceName")))
                        device.DevPlaceName = (string)Reader["PlaceName"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("IsCalibration")))
                        device.IsCalibrated = (bool)Reader["IsCalibration"];

                    //--- !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    device.DevPath = selectDeviceParents(device.DevTypeID);
                    device.DevPlacePath = selectPlaceParents(device.DevPlaceID);

                    list.Add(device);
                }
                Reader.Close();
                conn.Close();
            }
            return list;
        }
        
        //------ DEVICES ----------------------------------------------------------------
        public int modifyType(int TypeID, string TypeName, int? ParentID, string LinkToIcon, bool IsCalibrated)//lp_upd_DeviceType
        {
            int ret = 0;
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_upd_DeviceType");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.AddWithValue("@TypeID", TypeID);
                cmd.Parameters.AddWithValue("@TypeName", TypeName);
                cmd.Parameters.AddWithValue("@ParentID", ParentID);
                cmd.Parameters.AddWithValue("@LinkToIcon", LinkToIcon);
                cmd.Parameters.AddWithValue("@IsCalibration", IsCalibrated);

                cmd.ExecuteNonQuery();
                ret = (int)cmd.Parameters["@ReturnVal"].Value;

                conn.Close();
            }

            return ret;
        }

        public List<wsBaseItem> selectItemsList(int? ID) // lp_sel_DeviceListByDeviceType
        {
            List<wsBaseItem> l = new List<wsBaseItem>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_DeviceListByDeviceType");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TypeID", objectOrNull(ID));
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        l.Add(new wsBaseItem
                        {
                            ID = (int)Reader["ID"],
                            Description = (string)Reader["Description"]

                        });
                    }
                }
                conn.Close();
            }
            return l;
        }

        public int insertDevType(string TypeName, int? parentID, bool IsCalibrated)//lp_ins_DeviceType
        {
            int ret = 0;
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_ins_DeviceType");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.AddWithValue("@TypeName", TypeName);
                cmd.Parameters.AddWithValue("@ParentID", parentID);
                cmd.Parameters.AddWithValue("@IsCalibration", IsCalibrated);
                //cmd.Parameters.AddWithValue("@LinkToIcon", ImageLink);


                cmd.ExecuteNonQuery();
                ret = (int)cmd.Parameters["@ReturnVal"].Value;

                conn.Close();
            }

            return ret;
        }

        public int deleteDevice(int ID)//lp_del_Device
        {
            int ret = 0;

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_del_Device");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.AddWithValue("@DeviceID", ID);

                cmd.ExecuteNonQuery();
                ret = (int)cmd.Parameters["@ReturnVal"].Value;
                conn.Close();
            }

            return ret;
        }

        public int deleteNode(int ID)//lp_del_DeviceType
        {
            int ret = 0;

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_del_DeviceType");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.AddWithValue("@TypeID", ID);

                cmd.ExecuteNonQuery();
                ret = (int)cmd.Parameters["@ReturnVal"].Value;
                conn.Close();

            }

            return ret;
        }

        public List<wsSimpleItem> selectModelsList(int? ID) // lp_sel_ModelList
        {
            List<wsSimpleItem> l = new List<wsSimpleItem>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_ModelList");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ID);
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        wsSimpleItem temp = new wsSimpleItem();
                        temp.ID = (int)Reader["id"];
                        if (!Reader.IsDBNull(Reader.GetOrdinal("ModelName")))
                            temp.Description = (string)Reader["ModelName"];
                        l.Add(temp);
                    }
                }
                Reader.Close();
                conn.Close();
            }
            return l;
        }

        public List<wsSimpleItem> selectModelParents(int? ID) // lp_sel_ParentsOfModel
        {
            List<wsSimpleItem> l = new List<wsSimpleItem>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_ParentsOfModel");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", objectOrNull(ID));
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    
                    while (Reader.Read())
                    {
                        wsSimpleItem temp = new wsSimpleItem();
                        if (!Reader.IsDBNull(Reader.GetOrdinal("ModelID")))
                            temp.ID = (int)Reader["ModelID"];
                        if (!Reader.IsDBNull(Reader.GetOrdinal("Mname")))
                            temp.Description = (string)Reader["Mname"];
                        l.Add(temp);
                    }
                }
                Reader.Close();
                conn.Close();
            }
            return l;
        }

        public string selectDeviceParents(int? ID) // lp_sel_ParentsOfType
        {
            List<wsSimpleItem> l = new List<wsSimpleItem>();
            string s = null;
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_ParentsOfType");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", objectOrNull(ID));
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {

                    while (Reader.Read())
                    {
                        wsSimpleItem temp = new wsSimpleItem();
                        if (!Reader.IsDBNull(Reader.GetOrdinal("TypeID")))
                            temp.ID = (int)Reader["TypeID"];
                        if (!Reader.IsDBNull(Reader.GetOrdinal("TypeName")))
                            temp.Description = (string)Reader["TypeName"];
                        l.Add(temp);
                    }
                }
                Reader.Close();
                conn.Close();
            }

            if (l.Count > 0)
            {

                foreach (wsSimpleItem item in l)
                {
                    if (item != null)
                    {
                        s += item.Description;
                        s += " -> ";
                    }
                }
                s = s.Substring(0, s.LastIndexOf(" -> "));
            }
            else s = "Корневой элемент?..";

            return s;
        }

        public int moveItems(int ID, string xml)//
        {
            int ret = 0;

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_upd_PassportType");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.AddWithValue("@TypeID", ID);

                cmd.Parameters.Add("@DevList", SqlDbType.Text);
                cmd.Parameters["@DevList"].Value = xml;

                cmd.ExecuteNonQuery();

                ret = (int)cmd.Parameters["@ReturnVal"].Value;
                conn.Close();
            }

            return ret;
        }

        public List<string> selectAllTypes()
        {
            List<string> l = new List<string>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT TypeName FROM tbl_DevType");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        l.Add((string)Reader["TypeName"]);
                    }
                }
                conn.Close();
            }
            return l;
        }

        public List<wsModelItem> selectModels()
        {
            List<wsModelItem> l = new List<wsModelItem>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {

                SqlCommand cmd = new SqlCommand("SELECT * FROM ModelList");
                conn.Open();
                cmd.Connection = conn;

                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    wsModelItem t = new wsModelItem();

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Model_ID")))
                        t.ModelID = (int)Reader["Model_ID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Model")))
                        t.ModelName = (string)Reader["Model"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Submodel")))
                        t.SubModelName = (string)Reader["Submodel"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Modification")))
                        t.SubSubModelName = (string)Reader["Modification"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("CalibrationPeriod")))
                        t.CalibrationPeriod = (int)Reader["CalibrationPeriod"];

                    l.Add(t);

                }
                Reader.Close();
                conn.Close();
            }

            return l;
        }


        //-------- PLACES -----------------------------------------------------

        public List<wsBaseItem> selectPlaces(int? TypeID)//lp_sel_PlaceTypes
        {
            List<wsBaseItem> l = new List<wsBaseItem>();
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {


                SqlCommand cmd = new SqlCommand("lp_sel_PlaceTypes");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", TypeID);

                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    l.Add(new wsBaseItem
                    {
                        ID = (int)Reader["ID"],
                        Description = (string)Reader["PlaceName"],
                    });
                }
                conn.Close();
            }
            return l;
        }

        public int insertPlace(string Name, int? parentID)//lp_ins_Place
        {
            int ret = 0;
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_ins_Place");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.AddWithValue("@PlaceName", Name);
                cmd.Parameters.AddWithValue("@ParentID", parentID);

                cmd.ExecuteNonQuery();
                ret = (int)cmd.Parameters["@ReturnVal"].Value;

                conn.Close();
            }
            return ret;
        }

        public int deletePlace(int ID)//lp_del_Place
        {
            int ret = 0;
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_del_Place");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.AddWithValue("@PlaceID", ID);

                cmd.ExecuteNonQuery();
                ret = (int)cmd.Parameters["@ReturnVal"].Value;

                conn.Close();
            }
            return ret;
        }

        public string selectPlaceParents(int? ID) // lp_sel_ParentsOfPlace
        {
            List<wsSimpleItem> l = new List<wsSimpleItem>();
            string s = null;
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_ParentsOfPlace");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", objectOrNull(ID));
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {

                    while (Reader.Read())
                    {
                        wsSimpleItem temp = new wsSimpleItem();
                        if (!Reader.IsDBNull(Reader.GetOrdinal("PlaceID")))
                            temp.ID = (int)Reader["PlaceID"];
                        if (!Reader.IsDBNull(Reader.GetOrdinal("PlaceName")))
                            temp.Description = (string)Reader["PlaceName"];
                        l.Add(temp);
                    }
                }
                Reader.Close();
                conn.Close();
            }

            if (l.Count > 0)
            {
                foreach (wsSimpleItem item in l)
                {
                    if (item != null)
                    {
                        s += item.Description;
                        s += " -> ";
                    }
                }
                s = s.Substring(0, s.LastIndexOf(" -> "));
            }
            else s = "Размещение не задано";
            return s;
        }

        public List<wsBaseItem> selectDevListByPlace(int? PlaceID)//lp_sel_DevListByPlace
        {
            List<wsBaseItem> l = new List<wsBaseItem>();
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_DevListByPlace");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PlaceID", PlaceID);

                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    l.Add(new wsBaseItem
                    {
                        ID = (int)Reader["ID"],
                        Description = (string)Reader["Description"],
                        IsType = false,
                        ParentID = PlaceID
                    });
                }
                conn.Close();
            }
            return l;
        }


        //-------- CALIBRATION ----------------------------------------------------

        public List<wsCalibration> selectDeviceCalibrations(int id) //lp_sel_DeviceCalibration
        {
            List<wsCalibration> l = new List<wsCalibration>();
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_DeviceCalibration");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DevID", id);

                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    wsCalibration t = new wsCalibration();

                    t.ID = id;

                    if (!Reader.IsDBNull(Reader.GetOrdinal("FactDate")))
                        t.factDate = (DateTime)Reader["FactDate"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("PlannedDate")))
                        t.plannedDate = (DateTime)Reader["PlannedDate"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("TypeName")))
                        t.typeName = (string)Reader["TypeName"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Result")))
                        t.result = (bool)Reader["Result"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("DocNo")))
                        t.docNo = (string)Reader["DocNo"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Comments")))
                        t.comment = (string)Reader["Comments"];

                    l.Add(t);

                }
                conn.Close();
            }
            return l;
        }

        public List<wsCalibration> selectNextCalibrations() //lp_sel_NextCalibration
        {
            List<wsCalibration> l = new List<wsCalibration>();
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_NextCalibration");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FuturePeriod", 7); //7 дней

                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    wsCalibration t = new wsCalibration();

                    if (!Reader.IsDBNull(Reader.GetOrdinal("DevID")))
                        t.ID = (int)Reader["DevID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("LongDescrRU")))
                        t.Description = (string)Reader["LongDescrRU"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("NextDate")))
                        t.plannedDate = (DateTime)Reader["NextDate"];

                    l.Add(t);

                }
                conn.Close();
            }
            return l;
        }

        public int insertCalibration(wsCalibration c)//lp_ins_NewCalibration
        {
            int ret = 0;

            // @DevID		        int						-- Устройство 
            //,@FactDate	        datetime				-- Фактическая дата калибровки
            //,@CalibrationTypeID	int				        -- Тип поверки
            //,@Result	            bit						-- Результат поверки
            //,@DocNo		        nvarchar(50) = null		-- Номер документа
            //,@Comments	        nvarchar(max) = null	-- Комментарии

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_ins_NewCalibration");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.AddWithValue("@DevID", c.ID);
                cmd.Parameters.AddWithValue("@FactDate", c.factDate);
                cmd.Parameters.AddWithValue("@CalibrationTypeID", c.typeID);
                cmd.Parameters.AddWithValue("@Result", c.result);
                cmd.Parameters.AddWithValue("@DocNo", c.docNo);
                cmd.Parameters.AddWithValue("@Comments", c.comment);

                cmd.ExecuteNonQuery();
                ret = (int)cmd.Parameters["@ReturnVal"].Value;

                conn.Close();
            }
            return ret;
        }

        public List<wsSimpleItem> selectCalibrationTypes()
        {
            List<wsSimpleItem> l = new List<wsSimpleItem>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID, TypeName FROM tbl_CalibrationType");
                conn.Open();
                cmd.Connection = conn;
                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    wsSimpleItem item = new wsSimpleItem();

                    if (!Reader.IsDBNull(Reader.GetOrdinal("ID")))
                        item.ID = (int)Reader["ID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("TypeName")))
                        item.Description = (string)Reader["TypeName"];

                    l.Add(item);

                }
                conn.Close();
            }
            return l;
        }

        public int ins_upd_Period(int mID, int Period)//lp_ins_upd_CalibrationPeriod
        {
            int ret = 0;

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_ins_upd_CalibrationPeriod");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                cmd.Parameters.AddWithValue("@ModelID", mID);
                cmd.Parameters.AddWithValue("@Period", Period);

                cmd.ExecuteNonQuery();
                ret = (int)cmd.Parameters["@ReturnVal"].Value;

                conn.Close();
            }
            return ret;
        }


        //-------- REPAIRS & TO ----------------------------------------------------

        public List<wsMaintenance> selectDeviceMaintenances(int id) //lp_sel_DeviceCalibration
        {
            List<wsMaintenance> l = new List<wsMaintenance>();
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_DeviceMaintenance");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DevID", id);

                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    wsMaintenance t = new wsMaintenance();

                    t.ID = id;

                    if (!Reader.IsDBNull(Reader.GetOrdinal("FactDate")))
                        t.factDate = (DateTime)Reader["FactDate"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("PlannedDate")))
                        t.plannedDate = (DateTime)Reader["PlannedDate"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("FName")))
                        t.factName = (string)Reader["FName"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("PName")))
                        t.plannedName = (string)Reader["PName"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("DocNo")))
                        t.docNo = (string)Reader["DocNo"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Comments")))
                        t.comment = (string)Reader["Comments"];

                    l.Add(t);

                }
                conn.Close();
            }
            return l;
        }


        public List<wsMaintenance> selectNextMaintenances() //lp_sel_NextCalibration
        {
            List<wsMaintenance> l = new List<wsMaintenance>();
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_NextCalibration");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FuturePeriod", 7); //7 дней

                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    wsMaintenance t = new wsMaintenance();

                    if (!Reader.IsDBNull(Reader.GetOrdinal("DevID")))
                        t.ID = (int)Reader["DevID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("LongDescrRU")))
                        t.Description = (string)Reader["LongDescrRU"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("NextDate")))
                        t.plannedDate = (DateTime)Reader["NextDate"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("PName")))
                        t.plannedName = (string)Reader["PName"];

                    l.Add(t);

                }
                conn.Close();
            }
            return l;
        }
    }

}