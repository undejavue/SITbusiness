using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Linq;


[assembly: CLSCompliant(true)]
namespace SIT_classLib
{   
    //--- simple item class
    [DataContract(Name = "wsSimpleItem",
     Namespace = "http://www.SITBusiness.com/SITBusiness_classLib/wsSimpleItem")]
    public class wsSimpleItem
    {
        private int? _ID;
        [DataMember]
        [Display(Name = "ID")]
        public int? ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ID"));
            }
        }
        private string _Description;
        [DataMember]
        [Display(Name = "Описание элемента")]
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        public wsSimpleItem()
        {
            ID = null;
            Description = null;
        }

        public wsSimpleItem(int? initID, string initDescription)
        {
            ID = initID;
            Description = initDescription;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }


    //--- Base item class from simple item
    [DataContract(Name = "wsBaseItem",
     Namespace = "http://www.SITBusiness.com/SITBusiness_classLib/wsBaseItem")]
    public class wsBaseItem : wsSimpleItem
    {
        private bool _IsType;
        [DataMember]
        [Display(Name = "Является ли типом",
        Description = "Свойство определяет, является ли элемент классом или конечным элементом")]
        public bool IsType // 0 - Device; 1 - Type
        {                   
            get { return _IsType; }
            set
            {
                _IsType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsType"));
            }
        }

        private bool _IsChecked;
        [DataMember]
        [Display(Name = "Выбран для операций",
        Description = "Поле введено для групповых операций над элементами")]
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsChecked"));
            }
        }

        private int? _ParentID;
        [DataMember]
        [Display(Name = "Уникальный идентификатор родителя элемента")]
        public int? ParentID
        {
            get { return _ParentID; }
            set
            {
                _ParentID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ParentID"));
            }
        }

        private string _LinkToIcon;
        [DataMember]
        [Display(Name = "Ссылка на пиктограмму элемента")]
        public string LinkToIcon
        {
            get { return _LinkToIcon; }
            set
            {
                _LinkToIcon = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkToIcon"));
            }
        }

        private bool _IsCalibrated;
        [DataMember]
        [Display(Name = "Подлежит калибровке",
        Description = "Свойство определяет, подлежит ли данный тип калибровке")]
        public bool IsCalibrated 
        {
            get { return _IsCalibrated; }
            set
            {
                _IsCalibrated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalibrated"));
            }
        }

        public wsBaseItem()
        {
            IsType = false;
            IsChecked = false;
            IsCalibrated = false;
            ID = 0;
            ParentID = 0;
            Description = null;
            LinkToIcon = "nolink";
        }
    }


    //--- Model item class from simple item
    [DataContract(Name = "wsModelItem",
     Namespace = "http://www.SITBusiness.com/SITBusiness_classLib/wsModelItem")]
    public class wsModelItem : wsSimpleItem
    {
        [DataMember]
        [Display(Name="Модель",Description="Модель")]
        public string ModelName { get; set; }
        [DataMember]
        public int ModelID { get; set; }

        [DataMember]
        [Display(Name = "Тип", Description = "Тип")]
        public string SubModelName { get; set; }
        [DataMember]
        public int SubModelID { get; set; }

        [DataMember]
        [Display(Name = "Модификация", Description = "Модификация")]
        public string SubSubModelName { get; set; }
        [DataMember]
        public int SubSubModelID { get; set; }

        [DataMember]
        [Display(Name = "Период поверки", Description = "Период поверки, измеряется в неделях")]
        public int CalibrationPeriod { get; set; }

        public wsModelItem() { }
    }


    //--- producer type class
    [DataContract(Name = "wsProducerType",
    Namespace = "http://www.SITBusiness.com/SITBusiness_classLib/wsProducerType")]
    public class wsProducerType : wsSimpleItem
    {

        private int? _ProducerID;
        [DataMember]
        [Display(Name = "ID производителя")]
        public int? ProducerID
        {
            get { return _ProducerID; }
            set
            {
                _ProducerID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerID"));
            }
        }

        private string _ProducerName;
        [DataMember]
        [Display(Name = "Производитель")]
        [StringLength(25,ErrorMessage="Максимальная длина строки 25 символов")]
        public string ProducerName
        {
            get { return _ProducerName; }
            set
            {
                Validator.ValidateProperty(value,
                    new ValidationContext(this, null, null) { MemberName = "ProducerName" });

                _ProducerName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerName"));
            }
        }

        private string _ProducerCountry;
        [DataMember]
        [Display(Name = "Страна")]
        public string ProducerCountry
        {
            get { return _ProducerCountry; }
            set
            {
                _ProducerCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerCountry"));
            }
        }

        private string _ProducerState;
        [DataMember]
        [Display(Name = "Область")]
        public string ProducerState
        {
            get { return _ProducerState; }
            set
            {
                _ProducerState = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerState"));
            }
        }

        private string _ProducerCity;
        [DataMember]
        [Display(Name = "Город")]
        public string ProducerCity
        {
            get { return _ProducerCity; }
            set
            {
                _ProducerCity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerCity"));
            }
        }

        private string _ProducerStreetBuilding;
        [DataMember]
        [Display(Name = "Адрес")]
        public string ProducerStreetBuilding
        {
            get { return _ProducerStreetBuilding; }
            set
            {
                _ProducerStreetBuilding = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerStreetBuilding"));
            }
        }

        private string _ProducerPhone;
        [DataMember]
        [Display(Name = "Телефон")]
        public string ProducerPhone
        {
            get { return _ProducerPhone; }
            set
            {
                _ProducerPhone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerPhone"));
            }
        }

        public wsProducerType() { }
    }


    //--- Тип расширенный паспорт
    [DataContract(Name = "wsPasspotrExtended",
    Namespace = "http://www.SITBusiness.com/SITBusiness_classLib/wsPasspotrExtended")]
    public class wsPassportExtended : INotifyPropertyChanged
    {
        private int _DevID;
        [DataMember]
        [Display(Name = "Идентификатор устройства")]
        public int DevID
        {
            get { return _DevID; }
            set
            {
                _DevID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevID"));
            }
        }

        private string _DevPassportNo;
        [DataMember]
        [Display(Name="Номер паспорта", Description="Номер паспорта устройства согласно нормативной документации")]       
        public string DevPassportNo
        {
            get { return _DevPassportNo; }
            set
            {
                    _DevPassportNo = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("DevPassportNo"));
            }
        }

        private string _DevInvNo;
        [DataMember]
        [Display(Name = "Инвентарный номер")]
        public string DevInvNo
        {
            get { return _DevInvNo; }
            set
            {
                _DevInvNo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevInvNo"));
            }
        }

        private string _DevDescrRU;
        [DataMember]
        [Display(Name = "Наименование устройства")]
        [RegularExpression(@"^.*[А-Яа-яA-Za-z0-9' '№\""]$", ErrorMessage = "Разрешены только буквы и цифры")]
        public string DevDescrRU
        {
            get { return _DevDescrRU; }
            set
            {
                
                Validator.ValidateProperty(value,
                new ValidationContext(this, null, null) { MemberName = "DevDescrRU" });

                _DevDescrRU = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevDescrRU"));
            }
        }

        //private int? _DevCompoundModelID;
        //[DataMember]
        //[Display(Name = "Обобщенный ID модели")]
        //public int? DevCompoundModelID
        //{
        //    get { return _DevCompoundModelID; }
        //    set
        //    {
        //        _DevCompoundModelID = value;
        //        OnPropertyChanged(new PropertyChangedEventArgs("DevCompoundModelID"));
        //    }
        //}

        private string _DevModel;
        [DataMember]
        [Display(Name = "Модель")]
        public string DevModel
        {
            get { return _DevModel; }
            set
            {
                _DevModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevModel"));
            }
        }

        private int? _DevModelID;
        [DataMember]
        [Display(Name = "ID модели")]
        public int? DevModelID
        {
            get { return _DevModelID; }
            set
            {
                _DevModelID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevModelID"));
            }
        }

        private string _DevSubModel;
        [DataMember]
        [Display(Name = "Тип")]
        public string DevSubModel
        {
            get { return _DevSubModel; }
            set
            {
                _DevSubModel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevSubModel"));
            }
        }

        private int? _DevSubModelID;
        [DataMember]
        [Display(Name = "ID субмодели")]
        public int? DevSubModelID
        {
            get { return _DevSubModelID; }
            set
            {
                _DevSubModelID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevSubModelID"));
            }
        }

        private string _DevModification;
        [DataMember]
        [Display(Name = "Модификация")]
        public string DevModification
        {
            get { return _DevModification; }
            set
            {
                _DevModification = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevModification"));
            }
        }

        private int? _DevModificationID;
        [DataMember]
        [Display(Name = "ID модификации")]
        public int? DevModificationID
        {
            get { return _DevModificationID; }
            set
            {
                _DevModificationID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevModificationID"));
            }
        }

        private int? _ProducerID;
        [DataMember]
        [Display(Name = "ID производителя")]
        public int? ProducerID
        {
            get { return _ProducerID; }
            set
            {
                _ProducerID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerID"));
            }
        }

        private string _ProducerName;
        [DataMember]
        [Display(Name = "Производитель")]
        public string ProducerName
        {
            get { return _ProducerName; }
            set
            {
                _ProducerName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerName"));
            }
        }

        private string _ProducerCountry;
        [DataMember]
        [Display(Name = "Страна")]
        public string ProducerCountry 
        {
            get { return _ProducerCountry; }
            set
            {
                _ProducerCountry = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerCountry"));
            }
        }

        private string _ProducerState;
        [DataMember]
        [Display(Name = "Область")]
        public string ProducerState
        {
            get { return _ProducerState; }
            set
            {
                _ProducerState = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerState"));
            }
        }

        private string _ProducerCity;
        [DataMember]
        [Display(Name = "Город")]
        public string ProducerCity
        {
            get { return _ProducerCity; }
            set
            {
                _ProducerCity = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerCity"));
            }
        }

        private string _ProducerStreetBuilding;
        [DataMember]
        [Display(Name = "Адрес")]
        public string ProducerStreetBuilding
        {
            get { return _ProducerStreetBuilding; }
            set
            {
                _ProducerStreetBuilding = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerStreetBuilding"));
            }
        }

        private string _ProducerPhone;
        [DataMember]
        [Display(Name = "Телефон")]
        public string ProducerPhone
        {
            get { return _ProducerPhone; }
            set
            {
                _ProducerPhone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProducerPhone"));
            }
        }

        private DateTime? _ProduceDate;
        [DataMember]
        [Display(Name = "Дата производства")]
        public DateTime? ProduceDate
        {
            get { return _ProduceDate; }
            set
            {
                _ProduceDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProduceDate"));
            }
        }

        private DateTime? _PurchaseDate;
        [DataMember]
        [Display(Name = "Дата продажи")]
        public DateTime? PurchaseDate
        {
            get { return _PurchaseDate; }
            set
            {
                _PurchaseDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PurchaseDate"));
            }
        }

        private DateTime? _ImplementationDate;
        [DataMember]
        [Display(Name = "Дата ввода в эксплуатацию")]
        public DateTime? ImplementationDate
        {
            get { return _ImplementationDate; }
            set
            {
                _ImplementationDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImplementationDate"));
            }
        }

        private int? _CurrentStateID;
        [DataMember]
        [Display(Name = "ID состояния")]
        public int? CurrentStateID
        {
            get { return _CurrentStateID; }
            set
            {
                _CurrentStateID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentStateID"));
            }
        }

        private string _CurrentStateName;
        [DataMember]
        [Display(Name = "Состояние")]
        public string CurrentStateName
        {
            get { return _CurrentStateName; }
            set
            {
                _CurrentStateName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentStateName"));
            }
        }

        private string _DevPlaceName;
        [DataMember]
        [Display(Name = "Размещение")]
        public string DevPlaceName
        {
            get { return _DevPlaceName; }
            set
            {
                _DevPlaceName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevPlaceName"));
            }
        }

        private int? _DevPlaceID;
        [DataMember]
        [Display(Name = "ID размещения")]
        public int? DevPlaceID
        {
            get { return _DevPlaceID; }
            set
            {
                _DevPlaceID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevPlaceID"));
            }
        }
        private string _DevPlacePath;
        [DataMember]
        [Display(Name = "Место размещения")]
        public string DevPlacePath
        {
            get { return _DevPlacePath; }
            set
            {
                _DevPlacePath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevPlacePath"));
            }
        }

        private string _DevTypeName;
        [DataMember]
        [Display(Name = "Класс устройства")]
        public string DevTypeName
        {
            get { return _DevTypeName; }
            set
            {
                _DevTypeName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevTypeName"));
            }
        }

        private int? _DevTypeID;
        [DataMember]
        [Display(Name = "ID класса/типа")]
        public int? DevTypeID
        {
            get { return _DevTypeID; }
            set
            {
                _DevTypeID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevTypeID"));
            }
        }

        private string _DevPath;
        [DataMember]
        [Display(Name = "Иерархия классификации")]
        public string DevPath
        {
            get { return _DevPath; }
            set
            {
                _DevPath = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DevPath"));
            }
        }

        private List<wsBaseItem> _tbl_Models;
        [DataMember]
        [Display(Name = "Таблица моделей")]
        public List<wsBaseItem> tbl_Models
        {
            get { return _tbl_Models; }
            set
            {
                _tbl_Models = value;
                OnPropertyChanged(new PropertyChangedEventArgs("tbl_Models"));
            }
        }

        private List<wsProducerType> _list_Producers;
        [DataMember]
        [Display(Name = "Список производителей")]
        public List<wsProducerType> list_Producers
        {
            get { return _list_Producers; }
            set
            {
                _list_Producers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("list_Producers"));
            }
        }

        private List<string> _helper_DevDescr;
        [DataMember]
        [Display(Name = "База подсказок по наименованиям")]
        public List<string> helper_DevDescr
        {
            get { return _helper_DevDescr; }
            set
            {
                _helper_DevDescr = value;
                OnPropertyChanged(new PropertyChangedEventArgs("helper_DevDescr"));
            }
        }

        private bool _IsCalibrated;
        [DataMember]
        [Display(Name = "Подлежит калибровке", Description = "Свойство определяет принадлежность устройства к классу калибруемых")]
        public bool IsCalibrated
        {
            get { return _IsCalibrated; }
            set
            {
                _IsCalibrated = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsCalibrated"));
            }
        }

        private int _CalibrationPeriod;
        [DataMember]
        [Display(Name = "Период калибровки", Description = "Период калибровки устройства, недель")]
        public int CalibrationPeriod
        {
            get { return _CalibrationPeriod; }
            set
            {
                _CalibrationPeriod = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CalibrationPeriod"));
            }
        }

        //--- Методы класса ----------------------------------------------------------

        //--- INotifyPropertyChanged interface realisation     
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }


        public void downgradeToDataOnly()
        {
            this.tbl_Models = null;
            this.list_Producers = null;
            this.helper_DevDescr = null;
        }

        public wsProducerType extractProducerType()
        {
            wsProducerType p = new wsProducerType();

            p.ProducerName = this.ProducerName;
            p.ProducerCountry = this.ProducerCountry;
            p.ProducerState = this.ProducerState;
            p.ProducerCity = this.ProducerCity;
            p.ProducerStreetBuilding = this.ProducerStreetBuilding;
            p.ProducerPhone = this.ProducerPhone;

            return p;
        }

        public void insertProducerType(wsProducerType p)
        {

            this.ProducerName = p.ProducerName;
            this.ProducerCountry = p.ProducerCountry; 
            this.ProducerState = p.ProducerState;
            this.ProducerCity = p.ProducerCity;
            this.ProducerStreetBuilding = p.ProducerStreetBuilding;
            this.ProducerPhone = p.ProducerPhone;
        }

        public wsPassportExtended()
        {
            //DevID = 0;
            //DevPassportNo = null;
            //DevInvNo = null;
            DevDescrRU = " ";
            //DevCompoundModelID = null;
            //DevModel = null;
            //DevModelID = null;
            //DevSubModel = null;
            //DevSubModelID = null;
            //DevModification = null;
            //DevModificationID = null;
            //ProducerID = null;
            //ProducerName = null;
            //ProducerCountry = null;
            //ProducerState = null;
            //ProducerCity = null;
            //ProducerStreetBuilding = null;
            //ProducerPhone = null;
            //ProduceDate = null;
            //PurchaseDate = null;
            //ImplementationDate = null;
            //CurrentStateID = null;
            //CurrentStateName = null;
            //DevPlaceName = null;
            //DevPlaceID = null;
            //DevPlacePath = null;
            //DevTypeName = null;
            //DevTypeID = null;
            //DevPath = null;

            //tbl_Models = null;
            //list_Producers = null;
            //helper_DevDescr = null;
        }

    }


    [DataContract(Name = "wsDBTreeBase",
    Namespace = "http://www.SITBusiness.com/SITBusiness_classLib/wsDBTreeBase")]
    public class wsDBTreeBase
    {
        [DataMember]
        public List<wsBaseItem> DBDATA
        {
            get { return _dbdata; }
            set { _dbdata = value; }
        }
        private List<wsBaseItem> _dbdata;

        public List<wsBaseItem> GetAnyNodeinDB(int? ID)
        {
            var result = from n in DBDATA where n.ParentID == ID select n;

            return result.ToList();
        }
        public wsDBTreeBase() { }

    }

    //--- calibrated item class
    [DataContract(Name = "wsCalibration",
     Namespace = "http://www.SITBusiness.com/SITBusiness_classLib/wsCalibrate")]
    public class wsCalibration : wsSimpleItem
    {
        private DateTime? _plannedDate;
        [DataMember]
        [Display(Name = "Дата след. поверки")]
        public DateTime? plannedDate
        {
            get { return _plannedDate; }
            set
            {
                _plannedDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("plannedDate"));
            }
        }

        private DateTime _factDate;
        [DataMember]
        [Display(Name = "Дата поверки")]
        public DateTime factDate
        {
            get { return _factDate; }
            set
            {
                _factDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("factDate"));
            }
        }

        private int _typeID;
        [DataMember]
        [Display(Name = "ID типа поверки")]
        public int typeID
        {
            get { return _typeID; }
            set
            {
                _typeID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("typeID"));
            }
        }

        private string _typeName;
        [DataMember]
        [Display(Name = "Тип поверки")]
        public string typeName
        {
            get { return _typeName; }
            set
            {
                _typeName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("typeName"));
            }
        }

        private bool _result;
        [DataMember]
        [Display(Name = "Заключение")]
        public bool result
        {
            get { return _result; }
            set
            {
                _result = value;
                if (_result == true)
                {
                    _resultName = "годен";
                }
                if (_result == false)
                {
                    _resultName = "не годен";
                }
                
                OnPropertyChanged(new PropertyChangedEventArgs("result"));
            }
        }

        private string _resultName;
        [DataMember]
        [Display(Name = "Заключение")]
        public string resultName
        {
            get { return _resultName; }
            set
            {
                _resultName = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("resultName"));
            }
        }

        private string _docNo;
        [DataMember]
        [Display(Name = "№ документа")]
        public string docNo
        {
            get { return _docNo; }
            set
            {
                _docNo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("docNo"));
            }
        }

        private string _comment;
        [DataMember]
        [Display(Name = "Комментарий")]
        public string comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("comment"));
            }
        }

        public wsCalibration()
        {
            result = false;
            plannedDate = null;
        }

    }


    //--- calibrated item class
    [DataContract(Name = "wsMaintenance",
     Namespace = "http://www.SITBusiness.com/SITBusiness_classLib/wsMaintenance")]
    public class wsMaintenance : wsSimpleItem
    {
        private DateTime? _plannedDate;
        [DataMember]
        [Display(Name = "След. обслуживание")]
        public DateTime? plannedDate
        {
            get { return _plannedDate; }
            set
            {
                _plannedDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("plannedDate"));
            }
        }

        private DateTime _factDate;
        [DataMember]
        [Display(Name = "Дата обслуживания")]
        public DateTime factDate
        {
            get { return _factDate; }
            set
            {
                _factDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("factDate"));
            }
        }


        private string _factName;
        [DataMember]
        [Display(Name = "Имя фактического обслуживания")]
        public string factName
        {
            get { return _factName; }
            set
            {
                _factName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("factName"));
            }
        }

        private string _plannedName;
        [DataMember]
        [Display(Name = "Имя планируемого обслуживания")]
        public string plannedName
        {
            get { return _plannedName; }
            set
            {
                _plannedName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("plannedName"));
            }
        }

        private string _docNo;
        [DataMember]
        [Display(Name = "№ документа")]
        public string docNo
        {
            get { return _docNo; }
            set
            {
                _docNo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("docNo"));
            }
        }

        private string _comment;
        [DataMember]
        [Display(Name = "Комментарий")]
        public string comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("comment"));
            }
        }

        public wsMaintenance()
        {
            plannedDate = null;
        }
    }

    //--- work state class
    [DataContract(Name = "wsWorkState",
    Namespace = "http://www.SITBusiness.com/SITBusiness_classLib/wsWorkState")]
    public class wsWorkState : wsSimpleItem
    {
        private List<WorkStateMessage> _messageList;
        [DataMember]
        [Display(Name = "Список сообщений")]
        public List<WorkStateMessage> list_SCADAMessages
        {
            get { return _messageList; }
            set
            {
                _messageList = value;
                OnPropertyChanged(new PropertyChangedEventArgs("messageList"));
            }
        }
         
        private DateTime _startDate;
        [DataMember]
        [Display(Name = "Начало выборки")]
        public DateTime startDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("startDate"));
            }
        }

        private DateTime _finishDate;
        [DataMember]
        [Display(Name = "Конец выборки")]
        public DateTime finishDate
        {
            get { return _finishDate; }
            set
            {
                _finishDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("finishDate"));
            }
        }

        private List<WStateNDuration> _list_StatesDurations;
        [DataMember]
        [Display(Name = "Состояние - продолжительность")]
        public List<WStateNDuration> list_StatesDurations
        {
            get { return _list_StatesDurations; }
            set
            {
                _list_StatesDurations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("list_StatesDurations"));
            }
        }

        private List<wsSimpleItem> _list_ED;
        [DataMember]
        [Display(Name = "Ед.изм.")]
        public List<wsSimpleItem> list_ED
        {
            get { return _list_ED; }
            set { _list_ED = value; }
        }

        public wsWorkState()
        {
            startDate = DateTime.Now.AddDays(-30);
            finishDate = DateTime.Now;

            list_ED = new List<wsSimpleItem>();

            list_ED.Add(new wsSimpleItem { ID = 0, Description = "секунды" });
            list_ED.Add(new wsSimpleItem { ID = 1, Description = "минуты" });
            list_ED.Add(new wsSimpleItem { ID = 2, Description = "часы" });
            list_ED.Add(new wsSimpleItem { ID = 3, Description = "дни" });
            list_ED.Add(new wsSimpleItem { ID = 4, Description = "недели" });

            list_StatesDurations = new List<WStateNDuration>();
            list_SCADAMessages = new List<WorkStateMessage>();
        }

        public void setDefaults()
        {
            startDate = DateTime.Now.AddDays(-30);
            finishDate = DateTime.Now;

            setDurationFormat(0);
        }

        public void setDurationFormat(int formatID)
        {
            foreach (WStateNDuration w in list_StatesDurations)
            {
                w.selectFormat(formatID);
            }
        }

    }

    //--- work state message class
    [DataContract(Name = "WorkStateMessage",
    Namespace = "http://www.SITBusiness.com/SITBusiness_classLib/WorkStateMessage")]
    public class WorkStateMessage //: wsSimpleItem
    {
        private int _ID;
        [DataMember]
        [Display(Name = "ID")]
        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("ID"));
            }
        }
        
        private int _scadaID;
        [DataMember]
        [Display(Name = "SCADA ID")]
        public int scadaID
        {
            get { return _scadaID; }
            set
            {
                _scadaID = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("scadaID"));
            }
        }

        private int _msgNr;
        [DataMember]
        [Display(Name = "№ сообщения")]
        public int msgNr
        {
            get { return _msgNr; }
            set
            {
                _msgNr = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("msgNr"));
            }
        }

        private string _LongDescrRU;
        [DataMember]
        [Display(Name = "Наименование")]
        public string LongDescrRU
        {
            get { return _LongDescrRU; }
            set
            {
                _LongDescrRU = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("LongDescrRU"));
            }
        }

        private string _state;
        [DataMember]
        [Display(Name = "Состояние")]
        public string state
        {
            get { return _state; }
            set
            {
                _state = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("state"));
            }
        }

        private int _ms;
        [DataMember]
        [Display(Name = "Милисекунды")]
        public int ms
        {
            get { return _ms; }
            set
            {
                _ms = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("ms"));
            }
        }

        private DateTime _localDateTimeBegin;
        [DataMember]
        [Display(Name = "Дата начала")]
        public DateTime DateTimeBegin
        {
            get { return _localDateTimeBegin; }
            set
            {
                _localDateTimeBegin = value;

                //OnPropertyChanged(new PropertyChangedEventArgs("DateTimeBegin"));
            }
        }

        private DateTime _localDateTimeEnd;
        [DataMember]
        [Display(Name = "Дата окончания")]
        public DateTime DateTimeEnd
        {
            get { return _localDateTimeEnd; }
            set
            {
                //_localDateTimeStart = TimeZoneInfo.ConvertTime(value, TimeZoneInfo.Local);
                _localDateTimeEnd = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("localDateTimeEnd"));
            }
        }

        private int _Duration;
        [DataMember]
        [Display(Name = "Продолжительность")]
        public int Duration
        {
            get { return _Duration; }
            set
            {
                _Duration = value;
                //OnPropertyChanged(new PropertyChangedEventArgs("Duration"));
            }
        }
    }

    public class WStateNDuration :wsSimpleItem
    {
        [DataMember]
        [Display(Name = "Состояние")]
        private string _stateName;
        public string stateName
        {
            get { return _stateName; }
            set 
            { 
                _stateName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("stateName"));
            }
        }

        [DataMember]
        [Display(Name = "Продолж., база")]
        private double _durationBase;
        public double durationBase
        {
            get { return _durationBase; }
            set { _durationBase = value; }
        }

        [DataMember]
        [Display(Name = "Продолжительность")]
        private double _duration;
        public double duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged(new PropertyChangedEventArgs("duration"));
            }
        }

        [DataMember]
        [Display(Name = "Ед.изм.")]
        private string _ed;
        public string ed
        {
            get { return _ed; }
            set
            {
                _ed = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ed"));
            }
        }

        public void selectFormat(int toWhat)
        {
            switch (toWhat)
            {
                case 0:
                    duration = durationBase;
                    ed = "секунды";
                    break;
                case 1:
                    duration = durationBase / 60;
                    ed = "минуты";
                    break;
                case 2:
                    duration = durationBase / 60 / 60;
                    ed = "часы";
                    break;
                case 3:
                    duration = durationBase / 60 / 60 / 24;
                    ed = "дни";
                    break;
                case 4:
                    duration = durationBase / 60 / 60 / 24 / 7;
                    ed = "недели";
                    break;
            }    
        }

        public WStateNDuration() 
        {
            duration = durationBase;
            ed = "секунды";
        }
    }




}

