using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tesseract;

namespace Tesseract_Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string pdffilePath = "";
        public string txtfilePath = "";
        public bool btextonly = false;
        List<RenderedFormat> RenderedFormatList = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void browseFileBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff;*.bmp;*.JP2;*.ps;*.JPC;*.PCX;*.DCX...";
            dialog.InitialDirectory = @"C:\Users\qsaddam\Documents\Test_input\jpn";//System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Title = "Please select an image file.";
            dialog.Multiselect = false;

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
                filePathTextBox.Text = dialog.FileName;
        }
        EtextDesc monitor;
        string blogPostImage = string.Empty;
        private void convertFileBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(filePathTextBox.Text) && !string.IsNullOrWhiteSpace(filePathTextBox.Text))
                {

                    if (System.IO.File.Exists(filePathTextBox.Text))
                    {
                        blogPostImage = filePathTextBox.Text;
                        var worker = new BackgroundWorker();
                        worker.DoWork += worker_DoWork1;
                        worker.RunWorkerCompleted += worker_Complete;
                        worker.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private void worker_DoWork1(object sender, DoWorkEventArgs e)
        {
            var ENGLISH_LANGUAGE = @"jpn+eng";
            var tessdata = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            string resultPath = blogPostImage;
            using (var ocrEngine = new TesseractEngine(tessdata, ENGLISH_LANGUAGE, EngineMode.Default))
            {
                //string ext = Path.GetExtension(resultPath);
                var examplePixPath = System.IO.Path.ChangeExtension(resultPath, null);
                using (var renderer = GetRendererHandler(resultPath, tessdata, btextonly))
                {
                    var imageName = System.IO.Path.GetFileNameWithoutExtension(resultPath);
                    using (var pixA = ReadImageFileIntoPixArray(resultPath))
                    {
                        int expectedPageNumber = 0;
                        using (renderer.BeginDocument(imageName))
                        {
                            foreach (var pix in pixA)
                            {
                                using (var page = ocrEngine.Process(pix, imageName))
                                {
                                    monitor = new EtextDesc();
                                    var callback = new EtextDesc.TessProgressFunc(OnProgress);
                                    monitor.MonitorSetProgressFunc(callback);
                                    page.Monitor = monitor.Handle;
                                    var addedPage = renderer.AddPage(page);
                                    Console.WriteLine("page completed : " + ++expectedPageNumber);
                                }
                            }
                        }
                    }
                }
            }
        }

        private IResultRenderer GetRendererHandler(string destPath, string fontDir, bool textonly)
        {
            string ext = System.IO.Path.GetExtension(destPath);
            string ImagePathWithoutExtension = System.IO.Path.ChangeExtension(destPath, null);
            if(textonly && ext.ToLower().Equals(".pdf"))
            {
                return ResultRenderer.CreatePdfRenderer(ImagePathWithoutExtension, fontDir, btextonly);
            }
            else
            {
                if(RenderedFormatList.Count > 0)
                    return ResultRenderer.CreateRenderers(ImagePathWithoutExtension, fontDir, RenderedFormatList);
            }
            return ResultRenderer.CreatePdfRenderer(ImagePathWithoutExtension, fontDir, btextonly);
        }

        public bool OnProgress(IntPtr monitor1, int left, int right, int top, int bottom)
        {
            //object obj1;
            //Marshal.PtrToStructure(monitor1, monitor.MonitorGetProgress);
            int percentage = 0;
            if (monitor != null)
                percentage = monitor.MonitorGetProgress();
            Application.Current.Dispatcher.Invoke(() => { percentagetxt.Text = "Perecentage: " + percentage; });
            return true;
        }

        private static PixArray ReadImageFileIntoPixArray(string filename)
        {
            if (filename.ToLower().EndsWith(".tif") || filename.ToLower().EndsWith(".tiff"))
            {
                return PixArray.LoadMultiPageTiffFromFile(filename);
            }
            else
            {
                PixArray pa = PixArray.Create(0);
                pa.Add(Pix.LoadFromFile(filename));
                return pa;
            }
        }
        private void browseFileBtn2_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;*.tiff;*.bmp...";
            dialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.Title = "Please select an image file.";
            dialog.Multiselect = false;

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
                filePathTextBox2.Text = dialog.FileName;
        }

        private void convertFileBtn2_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(filePathTextBox2.Text) && !string.IsNullOrWhiteSpace(filePathTextBox2.Text))
                {
                    if (System.IO.File.Exists(filePathTextBox2.Text))
                    {
                        var worker = new BackgroundWorker();
                        worker.DoWork += worker_DoWork;
                        worker.RunWorkerCompleted += worker_Complete;
                        worker.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void worker_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var ENGLISH_LANGUAGE = @"eng";

            var blogPostImage = filePathTextBox2.Text;

            using (var ocrEngine = new Tesseract.TesseractEngine(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata"), ENGLISH_LANGUAGE, EngineMode.Default))
            {
                using (var imageWithText = Pix.LoadFromFile(blogPostImage))
                {
                    using (var page = ocrEngine.Process(imageWithText, blogPostImage))
                    {
                        var path = System.IO.Path.GetDirectoryName(blogPostImage) + "\\" + System.IO.Path.GetFileNameWithoutExtension(blogPostImage);
                        using (var renderer = ResultRenderer.CreateTextRenderer(path))
                        {
                            using (renderer.BeginDocument(System.IO.Path.GetFileNameWithoutExtension(blogPostImage)))
                            {
                                renderer.AddPage(page);
                                txtfilePath = path + ".txt";
                            }
                        }
                    }
                }
            }
        }

        private void previewFileBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(pdffilePath) && !string.IsNullOrWhiteSpace(pdffilePath))
                {
                    if (System.IO.File.Exists(pdffilePath))
                    {
                        System.Diagnostics.Process.Start(pdffilePath);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        private void previewFileBtn2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtfilePath) && !string.IsNullOrWhiteSpace(txtfilePath))
                {
                    if (System.IO.File.Exists(txtfilePath))
                    {
                        System.Diagnostics.Process.Start(txtfilePath);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            //var cancelHander = new EtextDesc.TessCancelFunc(cancelBack);
            EtextDesc.TessCancelFunc cancelHandler = new EtextDesc.TessCancelFunc(cancelBack);
            //(value, words) =>
            //{
            //    Console.WriteLine("Progress = {0}", words);
            //};
            monitor.MonitorSetCancelFunc(cancelHandler);
            //monitor.MonitorGetCancelThis();
            //monitor.MonitorSetDeadlineMSecs(0);
        }

        private void cancelBack(IntPtr cancelThis, int words)
        {
            Console.WriteLine("Cancel the project");
        }

        private void PerecentageBtn_Click(object sender, RoutedEventArgs e)
        {
            int percentage = 0;
            if (monitor != null)
                percentage = monitor.MonitorGetProgress();
            percentagetxt.Text = "Perecentage: " + percentage;
        }

        private void TextCheckUncheckClick(object sender, RoutedEventArgs e)
        {
            if (TextOnlyCHBtn.IsChecked == true)
            {
                btextonly = true;
            }
            else
            {
                btextonly = false;
            }
        }
        private void AutoRotateBtn_Click(object sender, RoutedEventArgs e)
        {
            var testImagePath = @"C:\Users\hdeepak\Documents\Sharpdesk Desktop\AutoRotate files\Multipage.tif";
            HandleRef handle;
            var tessdata = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");

            try
            {
                using (var engine = new TesseractEngine(tessdata, "LANG_ENG", EngineMode.Default))
                {

                    if (testImagePath.ToLower().EndsWith(".tif") || testImagePath.ToLower().EndsWith(".tiff"))
                    {

                        using (PixArray pages = PixArray.LoadMultiPageTiffFromFile(testImagePath))
                        {

                            foreach (Pix item in pages)
                            {
                                Pix RotatedImage = RotateImage(item.Handle, engine, item);
                                RotatedImage.Save("");
                            }

                        }
                    }
                    else
                    {

                        var pixHandle = Tesseract.Interop.LeptonicaApi.Native.pixRead(testImagePath);
                        if (pixHandle == IntPtr.Zero)
                        {
                            throw new IOException(String.Format("Failed to load image '{0}'.", testImagePath));
                            return;
                        }
                        Pix pix = Tesseract.Pix.Create(pixHandle);
                        handle = new HandleRef(engine, pixHandle);
                        Pix RotatedImage = RotateImage(handle, engine, pix);
                        if (RotatedImage != null)
                            RotatedImage.Save("");
                    }
                }
            }
            catch (Exception x)
            {
                //Trace.TraceError(e.ToString());
            }
        }
        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string RendererFormat = ((ComboBoxItem)CompboConverter.SelectedItem).Content.ToString();
            if (RendererFormat.Equals("PDF"))
            {
                if (TextOnlyCHBtn != null && TextOnlyCHBtn.Visibility == Visibility.Collapsed)
                    TextOnlyCHBtn.Visibility = Visibility.Visible;
            }
            else
            {
                if (TextOnlyCHBtn != null && TextOnlyCHBtn.Visibility == Visibility.Visible)
                    TextOnlyCHBtn.Visibility = Visibility.Collapsed;
            }
            if (RenderedFormatList == null) RenderedFormatList = new List<RenderedFormat>();
            {
                RenderedFormatList.Clear();
                RenderedFormatList.Add(RenderFormatItemChoose(RendererFormat));
            }
        }

        private RenderedFormat RenderFormatItemChoose(string pdf)
        {
            switch (pdf)
            {
                case "PDF": return RenderedFormat.PDF;
                case "TEXT": return RenderedFormat.TEXT;
                case "HOCR": return RenderedFormat.HOCR;
                case "BOX": return RenderedFormat.BOX;
                case "UNLV": return RenderedFormat.UNLV;
                default: return RenderedFormat.PDF;
            }
        }
        private static Pix RotateImage(HandleRef handle, TesseractEngine engine, Pix pix)
        {
            //"PixOrientDetect" API method will accpet 1bpp image type So below API method will convert to 1bpp.
            var pixconv = Tesseract.Interop.LeptonicaApi.Native.pixConvertTo1(handle, 130);

            float pupconf; float pleftconf;
            int value = Tesseract.Interop.LeptonicaApi.Native.pixOrientDetect(new HandleRef(engine, pixconv), out pupconf, out pleftconf, 0, 0);

            //-----------------****************------------------//
            //0 deg: upconf >> 1,    abs(upconf) >> abs(leftconf)
            //90 deg: leftconf >> 1,  abs(leftconf) >> abs(upconf)
            //180 deg: upconf << -1,   abs(upconf) >> abs(leftconf)
            //270 deg: leftconf << -1, abs(leftconf) >> abs(upconf)
            //-----------------****************------------------//

            float up_Conf = Math.Abs(pupconf);
            float pleft_Conf = Math.Abs(pleftconf);

            Pix RotatedImg = null;
            if (pleftconf > 1 && pleft_Conf > up_Conf)
            {
                RotatedImg = pix.Rotate90(1);
            }
            else if (pupconf < -1 && up_Conf > pleft_Conf)
            {
                var rotatPtr = Tesseract.Interop.LeptonicaApi.Native.pixRotate180(handle, handle);
                RotatedImg = Tesseract.Pix.Create(rotatPtr);
            }
            else if (pleftconf < -1 && pleft_Conf > up_Conf)
            {
                RotatedImg = pix.Rotate90(-1);
            }

            return RotatedImg;
        }
    }
}
