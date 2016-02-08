using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    // Класс для работы со справочниками
    public class CatalogsDataManager : SIT_DataManager
    {
        public List<wsProducerType> GetProducerList() //lp_sel_ProducerList
        {
            List<wsProducerType> l = new List<wsProducerType>();
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_ProducerList");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        wsProducerType temp = new wsProducerType();

                        if (!Reader.IsDBNull(Reader.GetOrdinal("ID")))
                            temp.ProducerID = (int)Reader["ID"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("Name")))
                            temp.ProducerName = (string)Reader["Name"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("Country")))
                            temp.ProducerCountry = (string)Reader["Country"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("State")))
                            temp.ProducerState = (string)Reader["State"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("City")))
                            temp.ProducerCity = (string)Reader["City"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("StreetBuilding")))
                            temp.ProducerStreetBuilding = (string)Reader["StreetBuilding"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("Phone")))
                            temp.ProducerPhone = (string)Reader["Phone"];

                        l.Add(temp);
                    }
                }
                conn.Close();
            }
            return l;
        }

        public wsProducerType GetProducerData(int ID)//lp_sel_ProducerData
        {
            wsProducerType l = new wsProducerType();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_ProducerData");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProducerID", ID);
                SqlDataReader Reader = cmd.ExecuteReader();

                while (Reader.Read())
                {
                    l.ProducerID = (int)Reader["ID"];

                    if (!Reader.IsDBNull(Reader.GetOrdinal("Name")))
                        l.ProducerName = (string)Reader["Name"];

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
                }
            }
            return l;
        }

        public int updateProducer(wsProducerType P)//lp_upd_ProducerData
        {
            int ReturnVal = 999;
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_upd_ProducerData");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProducerID", P.ProducerID);
                cmd.Parameters.AddWithValue("@Name", P.ProducerName);
                cmd.Parameters.AddWithValue("@Country", P.ProducerCountry);
                cmd.Parameters.AddWithValue("@State", P.ProducerState);
                cmd.Parameters.AddWithValue("@City", P.ProducerCity);
                cmd.Parameters.AddWithValue("@StreetBuilding", P.ProducerStreetBuilding);
                cmd.Parameters.AddWithValue("@Phone", P.ProducerPhone);

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                cmd.ExecuteNonQuery();


                ReturnVal = (int)cmd.Parameters["@ReturnVal"].Value;

                conn.Close();
            }

            return ReturnVal;
        }

        public int insertProducer(wsProducerType P)//lp_ins_Producer
        {
            int ReturnVal = 999;
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_ins_Producer");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ProducerID_out", SqlDbType.Int);
                cmd.Parameters["@ProducerID_out"].Direction = ParameterDirection.Output;
                

                cmd.Parameters.AddWithValue("@Name", P.ProducerName);
                cmd.Parameters.AddWithValue("@Country", P.ProducerCountry);
                cmd.Parameters.AddWithValue("@State", P.ProducerState);
                cmd.Parameters.AddWithValue("@City", P.ProducerCity);
                cmd.Parameters.AddWithValue("@StreetBuilding", P.ProducerStreetBuilding);
                cmd.Parameters.AddWithValue("@Phone", P.ProducerPhone);

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;

                cmd.ExecuteNonQuery();

                ReturnVal = (int)cmd.Parameters["@ReturnVal"].Value;

                conn.Close();
            }

            return ReturnVal;
        }

        public ObservableCollection<wsSimpleItem> checkProducer(int ID, out int OutVal, out string OutText)//lp_sel_ProducerDataRelationToDeviceData
        {
            ObservableCollection<wsSimpleItem> l = new ObservableCollection<wsSimpleItem>();
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_ProducerDataRelationToDeviceData");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProducerID", ID);

                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;


                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        l.Add(new wsSimpleItem
                        {
                            ID = (int)Reader["ID"],
                            Description = (string)Reader["LongDescrRu"]
                        });
                    }
                    OutText = "В случае удаления этого производителя он также будет удален из паспортов перечисленных ниже устройств";
                }
                else
                {
                    OutText = "Удаление данного производителя никак не повлияет на имеющиеся в базе паспорта устройств.";
                }
                Reader.Close();
                OutVal = (int)cmd.Parameters["@ReturnVal"].Value;

                conn.Close();
            }
            return l;
        }

        public void deleteProducer(int ID, out int OutVal, out string OutText)//lp_del_ProducerData
        {
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_del_ProducerData");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProducerID", ID);
                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                cmd.Parameters["@ReturnVal"].Direction = ParameterDirection.ReturnValue;
                try
                {
                    cmd.ExecuteNonQuery();
                    OutVal = (int)cmd.Parameters["@ReturnVal"].Value;
                    OutText = "Процедура lp_del_ProducerData выполнена";
                }
                catch (Exception err)
                {
                    OutText = "Ошибка lp_del_ProducerData: " + err.Message.ToString();
                    OutVal = 0;
                }

                conn.Close();
            }
        }

    }
}