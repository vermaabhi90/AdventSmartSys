using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.Calandar
{
    public class EditParams
    {
        public string key { get; set; }
        public string action { get; set; }
        public List<DefaultSchedule> added { get; set; }
        public List<DefaultSchedule> changed { get; set; }
        public DefaultSchedule value { get; set; }
    }
    public partial class DefaultSchedule : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _Id;

        private string _Subject;

        private string _Location;

        private System.DateTime _StartTime;

        private System.DateTime _EndTime;

        private string _Description;

        private System.Nullable<int> _Owner;

        private string _Participant;

        private System.Nullable<int> _Priority;

        private System.Nullable<byte> _Recurrence;

        private string _RecurrenceType;

        private System.Nullable<int> _RecurrenceTypeCount;

        private System.Nullable<int> _Reminder;

        private string _Categorize;

        private string _CustomStyle;

        private System.Nullable<bool> _AllDay;

        private System.Nullable<System.DateTime> _RecurrenceStartDate;

        private System.Nullable<System.DateTime> _RecurrenceEndDate;

        private string _RecurrenceRule;

        private string _StartTimeZone;

        private string _EndTimeZone;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnSubjectChanging(string value);
        partial void OnSubjectChanged();
        partial void OnLocationChanging(string value);
        partial void OnLocationChanged();
        partial void OnStartTimeChanging(System.DateTime value);
        partial void OnStartTimeChanged();
        partial void OnEndTimeChanging(System.DateTime value);
        partial void OnEndTimeChanged();
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        partial void OnOwnerChanging(System.Nullable<int> value);
        partial void OnParticipantChanging(string value);
        partial void OnOwnerChanged();
        partial void OnParticipantChanged();
        partial void OnPriorityChanging(System.Nullable<int> value);
        partial void OnPriorityChanged();
        partial void OnRecurrenceChanging(System.Nullable<byte> value);
        partial void OnRecurrenceChanged();
        partial void OnRecurrenceTypeChanging(string value);
        partial void OnRecurrenceTypeChanged();
        partial void OnRecurrenceTypeCountChanging(System.Nullable<int> value);
        partial void OnRecurrenceTypeCountChanged();
        partial void OnReminderChanging(System.Nullable<int> value);
        partial void OnReminderChanged();
        partial void OnCategorizeChanging(string value);
        partial void OnCategorizeChanged();
        partial void OnCustomStyleChanging(string value);
        partial void OnCustomStyleChanged();
        partial void OnAllDayChanging(System.Nullable<bool> value);
        partial void OnAllDayChanged();
        partial void OnRecurrenceStartDateChanging(System.Nullable<System.DateTime> value);
        partial void OnRecurrenceStartDateChanged();
        partial void OnRecurrenceEndDateChanging(System.Nullable<System.DateTime> value);
        partial void OnRecurrenceEndDateChanged();
        partial void OnRecurrenceRuleChanging(string value);
        partial void OnRecurrenceRuleChanged();
        partial void OnStartTimeZoneChanging(string value);
        partial void OnStartTimeZoneChanged();
        partial void OnEndTimeZoneChanging(string value);
        partial void OnEndTimeZoneChanged();
        #endregion

        public DefaultSchedule()
        {
            OnCreated();
        }

        public DefaultSchedule(int _id, string _subject, string _location, string _startTime, string _endTime, string _description, string _owner, string _Participant, string _priority, bool _recurrence, string _recurrenceType, string _recurrenceTypeCount, string _remainderCategorize, string _customStyle, bool _allDay, string _recurrenceStartDate, string _recurrenceEndDate, string _recurrenceRule, string _StartTimeZone, string _EndTimeZone)
        {
            this.Id = _id;
            this.Subject = _subject;
            this.Location = _location;
            this.StartTime = Convert.ToDateTime(_startTime);
            this.EndTime = Convert.ToDateTime(_endTime);
            this.Description = _description;
            if (_owner != null)
                this.Owner = Int32.Parse(_owner);
            else
                this.Owner = null;
            if (_Participant != null)
                this.Participant = _Participant;
            else
                this.Participant = null;
            this.Priority = null;
            this.Recurrence = Convert.ToByte(_recurrence);
            this.RecurrenceType = _recurrenceType;
            this.RecurrenceTypeCount = null;
            this.Categorize = _remainderCategorize;
            this.CustomStyle = null;
            this.AllDay = _allDay;
            this.RecurrenceStartDate = null;
            this.RecurrenceEndDate = null;
            this.RecurrenceRule = _recurrenceRule;
            this.StartTimeZone = _StartTimeZone;
            this.EndTimeZone = _EndTimeZone;
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Id", DbType = "Int NOT NULL", IsPrimaryKey = true)]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if ((this._Id != value))
                {
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Subject", DbType = "NVarChar(100)")]
        public string Subject
        {
            get
            {
                return this._Subject;
            }
            set
            {
                if ((this._Subject != value))
                {
                    this.OnSubjectChanging(value);
                    this.SendPropertyChanging();
                    this._Subject = value;
                    this.SendPropertyChanged("Subject");
                    this.OnSubjectChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Location", DbType = "NVarChar(100)")]
        public string Location
        {
            get
            {
                return this._Location;
            }
            set
            {
                if ((this._Location != value))
                {
                    this.OnLocationChanging(value);
                    this.SendPropertyChanging();
                    this._Location = value;
                    this.SendPropertyChanged("Location");
                    this.OnLocationChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_StartTime", DbType = "DateTime NOT NULL")]
        public System.DateTime StartTime
        {
            get
            {
                return this._StartTime;
            }
            set
            {
                if ((this._StartTime != value))
                {
                    this.OnStartTimeChanging(value);
                    this.SendPropertyChanging();
                    this._StartTime = value;
                    this.SendPropertyChanged("StartTime");
                    this.OnStartTimeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EndTime", DbType = "DateTime NOT NULL")]
        public System.DateTime EndTime
        {
            get
            {
                return this._EndTime;
            }
            set
            {
                if ((this._EndTime != value))
                {
                    this.OnEndTimeChanging(value);
                    this.SendPropertyChanging();
                    this._EndTime = value;
                    this.SendPropertyChanged("EndTime");
                    this.OnEndTimeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Description", DbType = "NVarChar(MAX)")]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                if ((this._Description != value))
                {
                    this.OnDescriptionChanging(value);
                    this.SendPropertyChanging();
                    this._Description = value;
                    this.SendPropertyChanged("Description");
                    this.OnDescriptionChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Owner", DbType = "Int")]
        public System.Nullable<int> Owner
        {
            get
            {
                return this._Owner;
            }
            set
            {
                if ((this._Owner != value))
                {
                    this.OnOwnerChanging(value);
                    this.SendPropertyChanging();
                    this._Owner = value;
                    this.SendPropertyChanged("Owner");
                    this.OnOwnerChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Participant", DbType = "Int")]
        public string Participant
        {
            get
            {
                return this._Participant;
            }
            set
            {
                if ((this._Participant != value))
                {
                    this.OnParticipantChanging(value);
                    this.SendPropertyChanging();
                    this._Participant = value;
                    this.SendPropertyChanged("Participant");
                    this.OnParticipantChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Priority", DbType = "Int")]
        public System.Nullable<int> Priority
        {
            get
            {
                return this._Priority;
            }
            set
            {
                if ((this._Priority != value))
                {
                    this.OnPriorityChanging(value);
                    this.SendPropertyChanging();
                    this._Priority = value;
                    this.SendPropertyChanged("Priority");
                    this.OnPriorityChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Recurrence", DbType = "TinyInt")]
        public System.Nullable<byte> Recurrence
        {
            get
            {
                return this._Recurrence;
            }
            set
            {
                if ((this._Recurrence != value))
                {
                    this.OnRecurrenceChanging(value);
                    this.SendPropertyChanging();
                    this._Recurrence = value;
                    this.SendPropertyChanged("Recurrence");
                    this.OnRecurrenceChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RecurrenceType", DbType = "NVarChar(10)")]
        public string RecurrenceType
        {
            get
            {
                return this._RecurrenceType;
            }
            set
            {
                if ((this._RecurrenceType != value))
                {
                    this.OnRecurrenceTypeChanging(value);
                    this.SendPropertyChanging();
                    this._RecurrenceType = value;
                    this.SendPropertyChanged("RecurrenceType");
                    this.OnRecurrenceTypeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RecurrenceTypeCount", DbType = "Int")]
        public System.Nullable<int> RecurrenceTypeCount
        {
            get
            {
                return this._RecurrenceTypeCount;
            }
            set
            {
                if ((this._RecurrenceTypeCount != value))
                {
                    this.OnRecurrenceTypeCountChanging(value);
                    this.SendPropertyChanging();
                    this._RecurrenceTypeCount = value;
                    this.SendPropertyChanged("RecurrenceTypeCount");
                    this.OnRecurrenceTypeCountChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Reminder", DbType = "Int")]
        public System.Nullable<int> Reminder
        {
            get
            {
                return this._Reminder;
            }
            set
            {
                if ((this._Reminder != value))
                {
                    this.OnReminderChanging(value);
                    this.SendPropertyChanging();
                    this._Reminder = value;
                    this.SendPropertyChanged("Reminder");
                    this.OnReminderChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_Categorize", DbType = "NVarChar(100)")]
        public string Categorize
        {
            get
            {
                return this._Categorize;
            }
            set
            {
                if ((this._Categorize != value))
                {
                    this.OnCategorizeChanging(value);
                    this.SendPropertyChanging();
                    this._Categorize = value;
                    this.SendPropertyChanged("Categorize");
                    this.OnCategorizeChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CustomStyle", DbType = "NVarChar(1000)")]
        public string CustomStyle
        {
            get
            {
                return this._CustomStyle;
            }
            set
            {
                if ((this._CustomStyle != value))
                {
                    this.OnCustomStyleChanging(value);
                    this.SendPropertyChanging();
                    this._CustomStyle = value;
                    this.SendPropertyChanged("CustomStyle");
                    this.OnCustomStyleChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AllDay", DbType = "Bit")]
        public System.Nullable<bool> AllDay
        {
            get
            {
                return this._AllDay;
            }
            set
            {
                if ((this._AllDay != value))
                {
                    this.OnAllDayChanging(value);
                    this.SendPropertyChanging();
                    this._AllDay = value;
                    this.SendPropertyChanged("AllDay");
                    this.OnAllDayChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RecurrenceStartDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> RecurrenceStartDate
        {
            get
            {
                return this._RecurrenceStartDate;
            }
            set
            {
                if ((this._RecurrenceStartDate != value))
                {
                    this.OnRecurrenceStartDateChanging(value);
                    this.SendPropertyChanging();
                    this._RecurrenceStartDate = value;
                    this.SendPropertyChanged("RecurrenceStartDate");
                    this.OnRecurrenceStartDateChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RecurrenceEndDate", DbType = "DateTime")]
        public System.Nullable<System.DateTime> RecurrenceEndDate
        {
            get
            {
                return this._RecurrenceEndDate;
            }
            set
            {
                if ((this._RecurrenceEndDate != value))
                {
                    this.OnRecurrenceEndDateChanging(value);
                    this.SendPropertyChanging();
                    this._RecurrenceEndDate = value;
                    this.SendPropertyChanged("RecurrenceEndDate");
                    this.OnRecurrenceEndDateChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RecurrenceRule", DbType = "NVarChar(2000)")]
        public string RecurrenceRule
        {
            get
            {
                return this._RecurrenceRule;
            }
            set
            {
                if ((this._RecurrenceRule != value))
                {
                    this.OnRecurrenceRuleChanging(value);
                    this.SendPropertyChanging();
                    this._RecurrenceRule = value;
                    this.SendPropertyChanged("RecurrenceRule");
                    this.OnRecurrenceRuleChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_StartTimeZone", CanBeNull = false)]
        public string StartTimeZone
        {
            get
            {
                return this._StartTimeZone;
            }
            set
            {
                if ((this._StartTimeZone != value))
                {
                    this.OnStartTimeZoneChanging(value);
                    this.SendPropertyChanging();
                    this._StartTimeZone = value;
                    this.SendPropertyChanged("StartTimeZone");
                    this.OnStartTimeZoneChanged();
                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EndTimeZone", CanBeNull = false)]
        public string EndTimeZone
        {
            get
            {
                return this._EndTimeZone;
            }
            set
            {
                if ((this._EndTimeZone != value))
                {
                    this.OnEndTimeZoneChanging(value);
                    this.SendPropertyChanging();
                    this._EndTimeZone = value;
                    this.SendPropertyChanged("EndTimeZone");
                    this.OnEndTimeZoneChanged();
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
