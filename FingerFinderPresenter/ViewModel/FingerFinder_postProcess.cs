﻿using FingerprintAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerFinderPresenter.ViewModel
{
    partial class FingerFinder
    {
        private Minutia selectedMinutia = new Minutia { Type = MinutiaType.Unspecified };
        public Minutia SelectedMinutia { get { return selectedMinutia; } set { selectedMinutia = value; NotifyPropertyChanged(); } }

        public RelayCommand CmdAnalyze { get; set; }

        private void InitializePostProcess()
        {
            CmdAnalyze = new RelayCommand(
                o => { Analyzer.analyzeFingerprint(); },
                o => Analyzer.FingerprintData != null
                );
        }

        public List<MinutiaType> MinutiaTypes
        {
            get { return Enum.GetValues(typeof(MinutiaType)).Cast<MinutiaType>().ToList(); }
        }
        public List<FingerprintCategory> FingerprintCategories
        {
            get { return Enum.GetValues(typeof(FingerprintCategory)).Cast<FingerprintCategory>().ToList(); }
        }

    }
}
