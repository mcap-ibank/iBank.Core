﻿using iBank.Core.Database;

using System;
using System.ComponentModel;
using System.Data;

namespace iBank.Core.BankProvider
{
    public class BankProviderPerson : INotifyPropertyChanged, IPersonData
    {

        public event PropertyChangedEventHandler PropertyChanged;


        public DateTime BirthDateDate => Utils.GetDateTime(BirthDate);
        public DateTime PassportIssueDateDate => Utils.GetDateTime(PassportIssueDate);

        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string Patronymic { get; set; } = "";
        public string BirthDate { get; set; } = "";
        public string DocumentSerialNumber { get; set; } = "";
        public string PassportIssue { get; set; } = "";
        public string PassportIssueDate { get; set; } = "";
        public string PassportDivisionCode { get; set; } = "";
        public string PhoneHome { get; set; } = "";
        public string Codeword { get; set; } = "";
       //public DateTime DateImport { get; set; } = DateTime.MinValue;
        public string PhoneMobile { get; set; } = "";
        public string Address { get; set; } = "";
        public string RecruitmentOfficeID { get; set; } = "";
        public string AccountNumber { get; set; } = "";
        public string BirthPlace { get; set; } = "";
        public bool? CardGiven { get; set; } = false;
        //public DateTime DateCardGiven { get; set; } = DateTime.MinValue;
        //public DateTime DatePrint { get; set; } = DateTime.MinValue;


        public BankProviderPerson() { }
        public BankProviderPerson(IDataRecord reader)
        {
            LastName = reader.GetValue(1).ToString().Trim();
            FirstName = reader.GetValue(2).ToString().Trim();
            Patronymic = reader.GetValue(3).ToString().Equals(" ") ? " " : reader.GetValue(3).ToString().Trim();
            BirthDate = reader.GetValue(5).ToString().Trim();

            var pass0 = reader.GetValue(9).ToString().Trim();
            if (string.IsNullOrEmpty(pass0))
                return;
            var pass1 = reader.GetString(10).Trim();
            DocumentSerialNumber = $"{pass0.Insert(2, " ")} {pass1}".Trim();
            //PassportSerial = reader.GetValue(1).ToString();

            PassportIssue = reader.GetValue(11).ToString().Trim();
            PassportIssueDate = reader.GetValue(12).ToString().Trim();
            PassportDivisionCode = reader.GetValue(13).ToString().Trim();
            PhoneHome = reader.GetValue(22).ToString().Trim();
            Codeword = reader.GetValue(33).ToString().Trim();
            //DateImport = CommonUtils.GetDateTime(reader.GetValue(35).ToString().Trim());
            PhoneMobile = reader.GetValue(39).ToString().Trim();
            Address = reader.GetValue(40).ToString().Trim();
            RecruitmentOfficeID = reader.GetValue(41).ToString().Trim();
            AccountNumber = reader.GetValue(43).ToString().Trim();
            BirthPlace = reader.GetValue(44).ToString().Trim();
            CardGiven = bool.TryParse(reader.GetValue(49).ToString(), out var val) ? val : (bool?)null;
            //DateCardGiven = CommonUtils.GetDateTime(reader.GetValue(53).ToString());
            //DatePrint = CommonUtils.GetDateTime(reader.GetValue(54).ToString());
        }
    }
}