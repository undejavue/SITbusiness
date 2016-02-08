using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using SIT_classLib;


namespace SITBusiness.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class WService
    {
        private string DBConnectionString = WebConfigurationManager.AppSettings["storeDbConnectionString"];
        private string AuthConnectionString = WebConfigurationManager.AppSettings["authDBConnectionString"];


//----------- ОБЩИЕ ОПЕРАЦИИ ---------------------------------------------------------------------------------
        [OperationContract]
        public int TestService()
        {
            int i = 1;
            return i;
        }

        [OperationContract]
        public int ws_TestDBConnection(out string OpStatus)
        {
            OpStatus = "Тест соединения с сервером БД: ";
            
            try
            {
                using (SqlConnection conn = new SqlConnection(DBConnectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        OpStatus += "Соединение ОК!";
                    }
                }
                return 1;
            }
            catch (Exception err)
            {
                OpStatus += "Ошибка соединения с сервером! Подробнее: ";
                OpStatus += err.Message.ToString();
                return 0;
            }
            
        }

        [OperationContract]
        public int ws_TestAuthConnection(out string OpStatus)
        {
            OpStatus = "Тест соединения с сервером ASP.NET авторизации: ";

            try
            {
                using (SqlConnection conn = new SqlConnection(DBConnectionString))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                        OpStatus += "Соединение ОК!";
                    }
                }
                return 1;
            }
            catch (Exception err)
            {
                OpStatus += "Ошибка соединения с сервером! Подробнее: ";
                OpStatus += err.Message.ToString();
                return 0;
            }
        }

//----------- DEVICES ---------------------------------------------------------------------------------

        // Получение данных для динамического дерева
        [OperationContract]
        public DBTree ws_selectDBTree(int TreeID, out string OpStatus)
        {
            
            try
            {
                OpStatus = "Выполнено";
                return new DBTree(TreeID);
            }
            catch (Exception err)
            {
                OpStatus = err.ToString();
                return null;
            }
        }

        // Получить список элементов ветви по ID
        [OperationContract]
        public List<wsBaseItem> ws_selectTreeItemsList(int? ID, out string OpStatus, out string Path)
        {
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();

                OpStatus = Resources.Messages.m_OpStatusDone;
                Path = DDM.selectDeviceParents(ID);
                return DDM.selectItemsList(ID);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                Path = "";
                return null;
            }
        }

        // Удалить устройство
        [OperationContract]
        public int ws_deleteTreeItem(List<int> IDlist, out string OpStatus)
        {
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();

                string s = "Удалены устройства: \n";

                foreach (int _id in IDlist)
                {
                    DDM.deleteDevice(_id);

                    s += "id = "+ _id + "\n";
                }

                OpStatus = Resources.Messages.m_OpStatusDone;

                //---- Создать оповещение через сервер оповещений--------------------------------------
                NotifyServerClient.unClientClass notifyClient = new NotifyServerClient.unClientClass();
                notifyClient.sendNotify(s);
                //--------------------------------------------------------------------------------------

                return 1;
            }
            catch (Exception err)
            {
                OpStatus = Resources.Messages.m_OpStatusFail;
                OpStatus += err.Message.ToString();
                return 0;
            }

        }

        // Вставить раздел
        [OperationContract]
        public int ws_insertNode(string NodeName, int? ParentID,bool IsCalibrated, out string OpStatus)
        {
            try
            {
                int code = 0;
                DeviceDataManager DDM = new DeviceDataManager();
                code = DDM.insertDevType(NodeName, ParentID, IsCalibrated);
                if (code == 0)
                {
                    OpStatus = Resources.Messages.m_OpStatusDone;
                }
                else
                {
                    OpStatus = Resources.Messages.m_OpStatusFail;
                    OpStatus += code;
                }
                return 1;
            }
            catch (Exception err)
            {
                OpStatus = Resources.Messages.m_OpStatusFail;
                OpStatus += err.Message.ToString();
                return 0;
            }
        }

        // Изменить раздел
        [OperationContract]
        public int ws_updateNode(int NodeID, string NodeName, int? ParentID, string LinkToIcon, bool IsCalibrated, out string OpStatus)
        {
            try
            {
                int code = 0;
                DeviceDataManager DDM = new DeviceDataManager();
                code = DDM.modifyType(NodeID, NodeName, ParentID, LinkToIcon, IsCalibrated);
                if (code == 0)
                {
                    OpStatus = Resources.Messages.m_OpStatusDone;
                }
                else
                {
                    OpStatus = Resources.Messages.m_OpStatusFail;
                    OpStatus += code;
                }
                return 1;
            }
            catch (Exception err)
            {
                OpStatus = Resources.Messages.m_OpStatusFail;
                OpStatus += err.Message.ToString();
                return 0;
            }
        }

        // Удалить раздел
        [OperationContract]
        public int ws_deleteNode(int NodeID, out string OpStatus)
        {
            try
            {
                int code = 0;
                DeviceDataManager DDM = new DeviceDataManager();
                code = DDM.deleteNode(NodeID);
                if (code == 0)
                {
                    OpStatus = Resources.Messages.m_OpStatusDone;
                }
                else
                {
                    OpStatus = Resources.Messages.m_OpStatusFail;
                    OpStatus += code;
                }
                return 1;
            }
            catch (Exception err)
            {
                OpStatus = Resources.Messages.m_OpStatusFail;
                OpStatus += err.Message.ToString();
                return 0;
            }
        }

        // Получить список только имен девайсов
        [OperationContract]
        public List<string> ws_selectDevDescrList(out string OpStatus)
        {
            try
            {
                List<string> s = new List<string>();
                List<wsBaseItem> blist = new List<wsBaseItem>();
                DeviceDataManager DDM = new DeviceDataManager();             
                blist = DDM.selectItemsList(null);
                foreach (wsBaseItem b in blist)
                {
                    s.Add(b.Description);
                }

                OpStatus = Resources.Messages.m_OpStatusDone;
                return s;
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Получить список только имен типов
        [OperationContract]
        public List<string> ws_helperTypesList(out string OpStatus)
        {
            try
            {
                List<string> s = new List<string>();
                DeviceDataManager DDM = new DeviceDataManager();
                s = DDM.selectAllTypes();
                OpStatus = Resources.Messages.m_OpStatusDone;
                return s;
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Перемещение группы устройств в другой раздел
        [OperationContract]
        public int ws_moveItems(int ID, string xml, out string OpStatus)
        {
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();
                DDM.moveItems(ID, xml);

                OpStatus = Resources.Messages.m_OpStatusDone;
                return 1;
            }
            catch (Exception err)
            {
                OpStatus = Resources.Messages.m_OpStatusFail;
                OpStatus += err.Message.ToString();
                return 0;
            }
        }

//----------- DevicePassport ---------------------------------------------------------------------------------

        // Получить паспорт по ID
        [OperationContract]
        public wsPassportExtended ws_selectPassport(int ID, out string OpStatus)
        {
            try
            {
                
                wsPassportExtended P = new wsPassportExtended();
                DeviceDataManager DDM = new DeviceDataManager();

                P = DDM.selectPasport(ID);
                
                P.DevPath = DDM.selectDeviceParents(P.DevTypeID);
                P.DevPlacePath = DDM.selectPlaceParents(P.DevPlaceID);

                OpStatus = Resources.Messages.m_OpStatusDone;
                return P;
            }
            catch (Exception err)
            {
                OpStatus = err.ToString();
                return null;
            }
        }

        // Получить расширенный паспорт устройства !
        [OperationContract]
        public wsPassportExtended ws_selectPassport_EX(int? ID, out string OpStatus)
        {
            try
            {
                wsPassportExtended P_EX = new wsPassportExtended();
                CatalogsDataManager CDM = new CatalogsDataManager();
                DeviceDataManager DDM = new DeviceDataManager();
                if (ID != null)
                {
                    P_EX = DDM.selectPasport((int)ID);
                    P_EX.DevPath = DDM.selectDeviceParents(P_EX.DevTypeID);
                    P_EX.DevPlacePath = DDM.selectPlaceParents(P_EX.DevPlaceID);
                }

                P_EX.list_Producers = CDM.GetProducerList();

                DBTree DBT = new DBTree(2); // 2 = models tree        
                //wsNode NNODE = new wsNode(DBTREE.GetAnyNodeinDB);
               
                P_EX.tbl_Models = DBT.DBDATA;

                List<string> s = new List<string>();
                List<wsBaseItem> blist = new List<wsBaseItem>();
                blist = DDM.selectItemsList(null);
                foreach (wsBaseItem b in blist)
                {
                    s.Add(b.Description);
                }

                P_EX.helper_DevDescr = s;

                OpStatus = Resources.Messages.m_OpStatusDone;
                return P_EX;
            }
            catch (Exception err)
            {
                OpStatus = err.ToString();
                return null;
            }
        }

        // Проверить паспорт перед вставкой
        [OperationContract]
        public int ws_checkPassportProducer(wsProducerType P, out int OpValue, out string OpStatus)
        {
            OpValue = 555;
            wsProducerType RP = new wsProducerType();
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();
                DDM.checkPassport(P, out OpValue, out OpStatus);

                if (OpValue == 50018) return 1; // Можно вставлять паспорт
                else return 0;
            }
            catch (Exception err)
            {
                OpStatus = "Ошибка SQL: " + err.Message.ToString();
                return 0;
            }
        }
          
        // Вставить паспорт
        [OperationContract]
        public int ws_insertPassport(wsPassportExtended p, int ID, out string OpStatus)
        {
            try
            {
                int code = 0;
                DeviceDataManager DDM = new DeviceDataManager();
                code = DDM.insertPassport(p,ID);
                if (code == 0)
                {
                    OpStatus = Resources.Messages.m_OpStatusDone;
                }
                else
                {
                    OpStatus = Resources.Messages.m_OpStatusFail;
                    OpStatus += code;
                }

                //---- Создать оповещение через сервер оповещений--------------------------------------
                string s = "Добавлен новый паспорт в базу устройств \n";
                s += "Наименование: " + p.DevDescrRU + "\n";
                s += "инв.№ = " + p.DevInvNo + "\n";
                s += "№ паспорта = " + p.DevPassportNo + "\n";

                NotifyServerClient.unClientClass notifyClient = new NotifyServerClient.unClientClass();
                notifyClient.sendNotify(s);
                //--------------------------------------------------------------------------------------

                return 1;
            }
            catch (Exception err)
            {
                OpStatus = Resources.Messages.m_OpStatusFail;
                OpStatus += err.Message.ToString();
                return 0;
            }
        }

        // Обновить паспорт
        [OperationContract]
        public int ws_updatePassport(wsPassportExtended p, out string OpStatus)
        {
            try
            {
                int code = 0;
                DeviceDataManager DDM = new DeviceDataManager();
                code = DDM.updatePassport(p);
                OpStatus = "Выполнено с кодом " + code.ToString();
                return 1;
            }
            catch (Exception err)
            {
                OpStatus = Resources.Messages.m_OpStatusFail;
                OpStatus += err.Message.ToString();
                return 0;
            }
        }

        // Получение списка для формирования пути устройства
        [OperationContract]
        public string ws_selectDeviceParents(int? ID, out string OpStatus)
        {
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();
                OpStatus = "Выполнено";
                return DDM.selectDeviceParents(ID);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        [OperationContract]
        public List<wsPassportExtended> ws_selectPassportsList(int? TypeID, int? PlaceID, out string OpStatus)
        {
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();
                OpStatus = "Done";
                return DDM.selectDevicesPassports(TypeID,PlaceID);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        
//----------- Models ------------------------------------------------------------------------------------------

        // Получение списка моделей
        [OperationContract]
        public List<wsSimpleItem> ws_selectModelsList(int? ID, out string OpStatus)
        {
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();
                OpStatus = "Выполнено";
                return DDM.selectModelsList(ID);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Получение списка Model - SubModel - Modification
        [OperationContract]
        public List<wsSimpleItem> ws_selectModelParents(int? ID, out string OpStatus)
        {
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();
                OpStatus = "Выполнено";
                return DDM.selectModelParents(ID);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Получение списка Subмоделей
        [OperationContract]
        public List<wsSimpleItem> ws_selectSubModelsList(int? ID, out string OpStatus)
        {
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();
                OpStatus = "Выполнено";
                return DDM.selectModelsList(ID);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Получение списка модификаций
        [OperationContract]
        public List<wsSimpleItem> ws_selectModificationsList(int? ID, out string OpStatus)
        {
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();
                OpStatus = "Выполнено";
                return DDM.selectModelsList(ID);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Получение данных для динамического дерева Models
        [OperationContract]
        public DBTree ws_selectDBTreeModels(out string OpStatus)
        {

            try
            {
                OpStatus = "Выполнено";
                return new DBTree(2);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Получение списка модификаций
        [OperationContract]
        public List<wsModelItem> ws_selectModels(out string OpStatus)
        {
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();
                OpStatus = "Выполнено";
                return DDM.selectModels();
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }


//----------- CATALOGS ---------------------------------------------------------------------------------
        // Получение списка производителей
        [OperationContract]
        public List<wsProducerType> ws_selectProducersList(out string OpStatus)
        {
            try
            {
                CatalogsDataManager CDM = new CatalogsDataManager();
                OpStatus = "Выполнено";
                return CDM.GetProducerList();
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Получение данных производителя
        [OperationContract]
        public wsProducerType ws_selectProducersData(int ID, out string OpStatus)
        {
            try
            {
                CatalogsDataManager CDM = new CatalogsDataManager();
                OpStatus = "Выполнено";
                return CDM.GetProducerData(ID);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Обновление данных производителя
        [OperationContract]
        public int ws_updateProducer(wsProducerType LocalProducer, out string OpStatus)
        {
            try
            {
                OpStatus = "Выполнено";
                CatalogsDataManager CDM = new CatalogsDataManager();
                CDM.updateProducer(LocalProducer);
                return 1;
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return 0;
            }
        }

        // Вставка производителя
        [OperationContract]
        public int ws_insertProducer(wsProducerType LocalProducer, out string OpStatus)
        {
            int res = 555;
            try
            {
                OpStatus = "Выполнено";
                CatalogsDataManager CDM = new CatalogsDataManager();
                if ((res = CDM.insertProducer(LocalProducer)) == 0) return 1;
                else return res;
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return 0;
            }
        }

        // Проверка перед удалением производителя
        [OperationContract]
        public ObservableCollection<wsSimpleItem> ws_checkProducer(int ID, out int OpValue, out string OpStatus)
        {
            OpValue = 555;
            ObservableCollection<wsSimpleItem> l = new ObservableCollection<wsSimpleItem>();
            try
            {
                CatalogsDataManager CDM = new CatalogsDataManager();
                l = CDM.checkProducer(ID, out OpValue, out OpStatus);
                return l;
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Удаление производителя
        [OperationContract]
        public int ws_deleteProducer(int ID,out int OpValue, out string OpStatus)
        {
            OpValue = 0;
            try
            {
                CatalogsDataManager CDM = new CatalogsDataManager();
                CDM.deleteProducer(ID, out OpValue, out OpStatus);
                return 1;       
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return 0;
            }
        }


//----------- PLACES ---------------------------------------------------------------------------------
        
        // Получить список Places
        [OperationContract]
        public List<wsBaseItem> ws_selectPlaces(int? _ID, out string OpStatus)
        {
            try
            {
                OpStatus = "Выполнено";
                DeviceDataManager DDM = new DeviceDataManager();
                return DDM.selectPlaces(_ID);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Вставка Place
        [OperationContract]
        public int ws_insertPlace(string Name, int? parentID, out string OpStatus)
        {
            try
            {
                OpStatus = "Выполнено";
                DeviceDataManager DDM = new DeviceDataManager();
                DDM.insertPlace(Name, parentID);
                return 1;
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return 0;
            }
        }

        // Удаление Place
        [OperationContract]
        public int ws_deletePlace(int ID, out string OpStatus)
        {
            try
            {
                OpStatus = "Выполнено";
                DeviceDataManager DDM = new DeviceDataManager();
                DDM.deletePlace(ID);
                return 1;
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return 0;
            }
        }

        // Получение данных для динамического дерева Places
        [OperationContract]
        public DBTree ws_selectDBTreePlaces(out string OpStatus)
        {

            try
            {
                OpStatus = "Выполнено";
                return new DBTree(1);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Получение списка размещения
        [OperationContract]
        public string ws_selectPlaceParents(int? ID, out string OpStatus)
        {
            try
            {
                DeviceDataManager DDM = new DeviceDataManager();
                OpStatus = "Выполнено";
                return DDM.selectPlaceParents(ID);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Получить список Places
        [OperationContract]
        public List<wsBaseItem> ws_selectDevListByPlace(int? ID, out string OpStatus, out string Path)
        {
            try
            {
                OpStatus = "Выполнено";
                DeviceDataManager DDM = new DeviceDataManager();
                Path = DDM.selectPlaceParents(ID);
                return DDM.selectDevListByPlace(ID);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                Path = "";
                return null;
            }
        }


//----------------- ОБСЛУЖИВАНИЕ ----------------------------------------------------------

        // Получить список нуждающихся в калибровке
        [OperationContract]
        public List<wsCalibration> ws_selectNextCalibrations(out string OpStatus)
        {
            try
            {
                OpStatus = "Выполнено";
                DeviceDataManager DDM = new DeviceDataManager();
                return DDM.selectNextCalibrations();
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Получить список калибровок для девайса
        [OperationContract]
        public List<wsCalibration> ws_selectDeviceCalibrations(int id, out string OpStatus)
        {
            try
            {
                OpStatus = "Выполнено";
                DeviceDataManager DDM = new DeviceDataManager();
                return DDM.selectDeviceCalibrations(id);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Калибровка
        [OperationContract]
        public int ws_insertCalibration(wsCalibration c, out string OpStatus)
        {
            try
            {
                OpStatus = "Выполнено";
                DeviceDataManager DDM = new DeviceDataManager();
                DDM.insertCalibration(c);
                return 1;
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return 0;
            }
        }

        // Получить типы калибровок
        [OperationContract]
        public List<wsSimpleItem> ws_selectCalibrationTypes(out string OpStatus)
        {
            try
            {
                OpStatus = "Выполнено";
                DeviceDataManager DDM = new DeviceDataManager();
                return DDM.selectCalibrationTypes();
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Период калибровки/поверки
        [OperationContract]
        public int ws_ins_upd_Period(int _id, int period, out string OpStatus)
        {
            try
            {
                OpStatus = "Выполнено";
                DeviceDataManager DDM = new DeviceDataManager();
                DDM.ins_upd_Period(_id, period);
                return 1;
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return 0;
            }
        }


        // Получить список нуждающихся в ремонте
        [OperationContract]
        public List<wsMaintenance> ws_selectNextMaintenances(out string OpStatus)
        {
            try
            {
                OpStatus = "Выполнено";
                DeviceDataManager DDM = new DeviceDataManager();
                return DDM.selectNextMaintenances();
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }

        // Получить список ремонтов для девайса
        [OperationContract]
        public List<wsMaintenance> ws_selectDeviceMaintenances(int id, out string OpStatus)
        {
            try
            {
                OpStatus = "Выполнено";
                DeviceDataManager DDM = new DeviceDataManager();
                return DDM.selectDeviceMaintenances(id);
            }
            catch (Exception err)
            {
                OpStatus = err.Message.ToString();
                return null;
            }
        }


//----------------- СОСТОЯНИЯ ----------------------------------------------------------

        // Получить список сообщений о состоянии устройства
        [OperationContract]
        public wsWorkState ws_selectDeviceWorkStates(int id, DateTime beginDate,DateTime endDate, out string OpStatus)
        {
            try
            {
                wsWorkState WSt = new wsWorkState();
                WorkDataManager WDM = new WorkDataManager();
                WSt.list_SCADAMessages = WDM.selectDevWorkStates(id,beginDate,endDate);
                WSt.ID = id;
                WSt.startDate = beginDate;
                WSt.finishDate = endDate;
                WSt.list_StatesDurations = WDM.selectDevWorkStatesDuration(id, beginDate, endDate);
                OpStatus = "Выполнено";
                return WSt;
            }
            catch (Exception err)
            {
                OpStatus = err.ToString();
                return null;
            }
        } 

    }
}
