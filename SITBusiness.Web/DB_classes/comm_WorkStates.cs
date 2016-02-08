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
    public class WorkDataManager : SIT_DataManager
    {
        public List<WorkStateMessage> selectDevWorkStates(int devID, DateTime beginDate, DateTime endDate)
        {
            List<WorkStateMessage> l = new List<WorkStateMessage>();
            
            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                //SqlCommand cmd = new SqlCommand(Resources.StoredProcedures.select_DevWorkStates);
                SqlCommand cmd = new SqlCommand("lp_sel_DevWorkStateMessage");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", objectOrNull(devID));
                        
                cmd.Parameters.Add("@START_DATE",SqlDbType.DateTime);
                cmd.Parameters["@START_DATE"].Value = beginDate;
                cmd.Parameters["@START_DATE"].Direction = ParameterDirection.Input;

                cmd.Parameters.Add("@END_DATE", SqlDbType.DateTime);
                cmd.Parameters["@END_DATE"].Value = endDate;
                cmd.Parameters["@END_DATE"].Direction = ParameterDirection.Input;

                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        WorkStateMessage item = new WorkStateMessage();

                        if (!Reader.IsDBNull(Reader.GetOrdinal("ID")))
                            item.ID = (int)Reader["ID"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("SCADA_ID")))
                            item.scadaID = (int)Reader["SCADA_ID"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("SCADA_MsgNr")))
                            item.msgNr = (int)Reader["SCADA_MsgNr"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("SCADA_State")))
                            item.state = (string)Reader["SCADA_State"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("LongDescrRU")))
                            item.LongDescrRU = (string)Reader["LongDescrRU"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("SCADA_DateTime_LocalTime")))
                            item.DateTimeBegin = (DateTime)Reader["SCADA_DateTime_LocalTime"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("SCADA_DateTime_LocalTime2")))
                            item.DateTimeEnd = (DateTime)Reader["SCADA_DateTime_LocalTime2"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("DURATION")))
                            item.Duration = (int)Reader["DURATION"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("SCADA_Ms")))
                            item.ms = (int)((short)Reader["SCADA_Ms"]);

                        l.Add(item);
                    }
                }
                Reader.Close();
                conn.Close();
            }
            return l;
        }

        public List<WStateNDuration> selectDevWorkStatesDuration(int devID, DateTime beginDate, DateTime endDate)
        {
            List<WStateNDuration> l = new List<WStateNDuration>();

            using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("lp_sel_DevSumWorkStateDuration");
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", objectOrNull(devID));

                cmd.Parameters.Add("@START_DATE", SqlDbType.DateTime);
                cmd.Parameters["@START_DATE"].Value = beginDate;
                cmd.Parameters["@START_DATE"].Direction = ParameterDirection.Input;

                cmd.Parameters.Add("@END_DATE", SqlDbType.DateTime);
                cmd.Parameters["@END_DATE"].Value = endDate;
                cmd.Parameters["@END_DATE"].Direction = ParameterDirection.Input;

                SqlDataReader Reader = cmd.ExecuteReader();

                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        WStateNDuration item = new WStateNDuration();

                        if (!Reader.IsDBNull(Reader.GetOrdinal("SumDuration")))
                            item.durationBase = (double)(int)Reader["SumDuration"];

                        if (!Reader.IsDBNull(Reader.GetOrdinal("_state_")))
                            item.stateName = (string)Reader["_state_"];

                        l.Add(item);
                    }
                }
                Reader.Close();
                conn.Close();
            }
            return l;
        }
    }
}