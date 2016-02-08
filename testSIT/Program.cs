using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIT_classLib;
using SITBusiness.Web;

namespace testSIT
{
    class Program
    {
        static void Main(string[] args)
        {
            //DateTime beginDate = DateTime.Now.AddDays(-7);
            //DateTime endDate = DateTime.Now;

            //int id = 77;

            //wsWorkState WSt = new wsWorkState();
            //WorkDataManager WDM = new WorkDataManager();
            //WSt.list_SCADAMessages = WDM.selectDevWorkStates(id, beginDate, endDate);
            //WSt.ID = id;
            //WSt.startDate = beginDate;
            //WSt.finishDate = endDate;
            //WSt.list_StatesDurations = WDM.selectDevWorkStatesDuration(id, beginDate, endDate);

            DeviceDataManager DDM = new DeviceDataManager();
            wsPassportExtended P_EX = new wsPassportExtended();

            P_EX = DDM.selectPasport(77);
            P_EX.DevPath = DDM.selectDeviceParents(P_EX.DevTypeID);
            P_EX.DevPlacePath = DDM.selectPlaceParents(P_EX.DevPlaceID);



        }
    }
}
