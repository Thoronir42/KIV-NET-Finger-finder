﻿using FingerprintAnalyzer;
using FingerprintAnalyzer.Analyze;
using FingerprintAnalyzer.Model;
using System;
using System.Windows.Media;

namespace FingerFinderPresenter.ViewModel
{
    partial class FingerFinder : BaseModel
    {
        private Analyzer analyzer = new Analyzer();
        private int selectedIndex = 0;
        private SolidColorBrush canvasBackground;
        private ImageSource fingerprintImageSource;

        public Analyzer Analyzer { get { return analyzer; } private set { analyzer = value; NotifyPropertyChanged(); } }
        public int SelectedIndex { get { return selectedIndex; } set { selectedIndex = value; NotifyPropertyChanged(); selectedIndexChanged(value); } }

        public SolidColorBrush CanvasBackground { get { return canvasBackground; } set { canvasBackground = value; NotifyPropertyChanged(); } }
        public ImageSource FingerprintImageSource { get { return fingerprintImageSource; } set { fingerprintImageSource = value; NotifyPropertyChanged(); } }

        public int CanvasWidth { get; } = 350;
        public int CanvasHeight { get; } = 350;

        public FingerFinder()
        {
            InitializeCommands();
            Analyzer.StageChanged += Analyzer_StageChanged;
        }

        private void drawFingerprint(Analyzer.Stages stage)
        {
            var currentImage = Analyzer.CurrentImage;

            if (currentImage == null)
            {
                CanvasBackground = new SolidColorBrush(Color.FromRgb(240, 80, 160));
                return;
            }
            CanvasBackground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            Console.WriteLine($"canvas {CanvasWidth}x{CanvasHeight}");

            var bmp = FiFiPrToolkit.imageToRenderTargetBitmap(currentImage, CanvasWidth, CanvasHeight);
            FingerprintImageSource = bmp;
        }

        private void selectedIndexChanged(int newValue)
        {
            drawFingerprint((Analyzer.Stages)newValue);
        }
    }

}