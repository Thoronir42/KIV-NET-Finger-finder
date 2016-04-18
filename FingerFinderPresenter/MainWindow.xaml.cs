﻿using System;
using System.Windows;
using FingerprintAnalyzer;
using Microsoft.Win32;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;
using FingerprintAnalyzer.Model;

namespace FingerFinderPresenter
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected Analyzer Analyzer { get; set; } = new Analyzer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void tabControl_fingerprintDrawer_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedIndex = tabControl_fingerprintDrawer.SelectedIndex;
            drawFingerprint(selectedIndex);
        }

        /// <summary>
        /// Import obrazku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.tif, *.png) | *.jpg; *.jpeg; *.tif; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                Console.WriteLine(openFileDialog.FileName);

                try
                {
                    System.Drawing.Image fingerprint = System.Drawing.Image.FromFile(openFileDialog.FileName);

                    this.Analyzer.createNewFromImage(fingerprint);
                    MenuItem_save.IsEnabled = true;

                    this.changeStage(Analyzer.STAGE_ORIGINAL);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            }
                
        }

        private void prevStage(object sender, EventArgs e)
        {
            var selectedIndex = tabControl_fingerprintDrawer.SelectedIndex;
            changeStage(selectedIndex - 1);
        }

        private void nextStage(object sender, EventArgs e)
        {
            var selectedIndex = tabControl_fingerprintDrawer.SelectedIndex;
            changeStage(selectedIndex + 1);
        }

        private void changeStage(int newStage)
        {
            // TODO: refactor
            int currentStage = tabControl_fingerprintDrawer.SelectedIndex;

            button_prevStage.IsEnabled = newStage > Analyzer.STAGE_ORIGINAL;
            button_nextStage.IsEnabled = newStage < Analyzer.STAGE_SKELETIZED;


            // Stage is going forward - create new stage if newStage is a valid one and draw is
            switch (newStage) {
                default: return;
                case Analyzer.STAGE_ORIGINAL:
                    break;
                case Analyzer.STAGE_EQUALIZED:
                    Analyzer.transformEqualization();
                    break;
                case Analyzer.STAGE_TRESHOLDED:
                    Analyzer.transformTresholding(160); // TODO: user controls
                    break;
                case Analyzer.STAGE_SKELETIZED:
                    Analyzer.transformSkeletonize();
                    break;
            }
            this.drawFingerprint(newStage);
            tabControl_fingerprintDrawer.SelectedIndex = newStage;
        }

        private void drawFingerprint(int stage)
        {
            var currentImage = Analyzer.getImage(stage);

            if (currentImage == null)
            {
                SolidColorBrush fill = new SolidColorBrush(Color.FromRgb(240,80,160));
                canvas.Background = fill;
                //image_fingerprint;
                return;
            }

            SolidColorBrush fillW = new SolidColorBrush(Color.FromRgb(255,255,255));
            canvas.Background = fillW;

            var bmp = FiFiPrToolkit.imageToRenderTargetBitmap(currentImage, (int)canvas.ActualWidth, (int)canvas.ActualHeight);

            image_fingerprint.Source = bmp;
        }

        private void MenuItem_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_save_Click(object sender, RoutedEventArgs e)
        {
            Analyzer.mockAnalyzeAndClassify();

            SaveFileDialog saver = new SaveFileDialog();
            saver.Filter = "Otisk prstu (*.fpr) | *.fpr;";
            if(saver.ShowDialog() != true)
            {
                return;
            }
            Analyzer.saveToFile(saver.FileName);
        }
    }
}
