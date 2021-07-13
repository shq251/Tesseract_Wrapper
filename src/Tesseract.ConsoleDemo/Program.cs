using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Tesseract;

namespace Tesseract.ConsoleDemo
{
    internal class Program
    {

        static EtextDesc monitor;
        public static void Main(string[] args)
        {
            //DoCropPhase1();

            //DoCropPhaseTwo();

            //DoCrop();

            //DescewTest();

            //DetectOrientationAndRotate();

            //DetectOrientationAndRotate_approach1();
            //pdfConvert();
            RenderMultiplePageDocumentToPdfFile();
            //textConvesion();
            //var testImagePath = "./phototest.tif";
            //if (args.Length > 0)
            //{
            //    testImagePath = args[0];
            //}

            #region code
            //try
            //{
            //    using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            //    {
            //        using (var img = Pix.LoadFromFile(testImagePath))
            //        {
            //            using (var page = engine.Process(img))
            //            {
            //                var text = page.GetText();
            //                Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

            //                Console.WriteLine("Text (GetText): \r\n{0}", text);
            //                Console.WriteLine("Text (iterator):");
            //                using (var iter = page.GetIterator())
            //                {
            //                    iter.Begin();

            //                    do
            //                    {
            //                        do
            //                        {
            //                            do
            //                            {
            //                                do
            //                                {
            //                                    if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
            //                                    {
            //                                        Console.WriteLine("<BLOCK>");
            //                                    }

            //                                    Console.Write(iter.GetText(PageIteratorLevel.Word));
            //                                    Console.Write(" ");

            //                                    if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
            //                                    {
            //                                        Console.WriteLine();
            //                                    }
            //                                } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

            //                                if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
            //                                {
            //                                    Console.WriteLine();
            //                                }
            //                            } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
            //                        } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
            //                    } while (iter.Next(PageIteratorLevel.Block));
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    Trace.TraceError(e.ToString());
            //    Console.WriteLine("Unexpected Error: " + e.Message);
            //    Console.WriteLine("Details: ");
            //    Console.WriteLine(e.ToString());
            //} 
            #endregion
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(true);
        }
        private static void textConvesion()
        {
            try
            {
                { }
                string txtfilePath = string.Empty;
                string inputfilePath = @"C:\Users\qsaddam\Documents\Sharpdesk Desktop\Contract.tif";
                if (!string.IsNullOrEmpty(inputfilePath) && !string.IsNullOrWhiteSpace(inputfilePath))
                {
                    if (System.IO.File.Exists(inputfilePath))
                    {
                        var ENGLISH_LANGUAGE = @"eng";

                        var blogPostImage = inputfilePath;

                        using (var ocrEngine = new TesseractEngine(@"./tessdata", ENGLISH_LANGUAGE, EngineMode.Default))
                        {
                            using (var imageWithText = Pix.LoadFromFile(blogPostImage))
                            {
                                using (var page = ocrEngine.Process(imageWithText, blogPostImage))
                                {
                                    page.AnalyseLayout();
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
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected static bool OnProgress(IntPtr handle, int left, int right, int top, int bottom)
        {
            // back to object (in callback function):
            //GCHandle handle2 = (GCHandle)handle;
            //int val = handle2.Target as int;
            //object obj1;
            //Marshal.PtrToStructure(monitor1, monitor.);
            int percentage = monitor.MonitorGetProgress();
            Console.WriteLine("Percentage : {0}", percentage); return true;
            // OCR canceled at Sharpdesk
            //if (IsCancel)
            //{
            //    // Notification of cancellation to ABBYY. 
            //    cancel = true;
            //}
            //else
            //{
            //    if (ProgressEvent != null)
            //    {
            //        // parameter settings
            //        var e = new OcrEventArgs();
            //        e.Parsent = percentage;
            //        e.PageNum = _pageNum;
            //        e.Status = _status;
            //        e.StatusNum = _statusMax; // OcrStatus.WORKING_NUM;
            //        // Event handler call
            //        ProgressEvent(null, e);
            //    }
            //}
        }
        public static void RenderMultiplePageDocumentToPdfFile()
        {
            Console.WriteLine("Start CanRenderMultiplePageDocumentToPdfFile");
            string pdffilePath = string.Empty;
            var ENGLISH_LANGUAGE = @"eng";
            var tessdata = @"D:\Tesseract\SHQ\Tesseract_Wrapper\src\Tesseract.ConsoleDemo\tessdata";
            string resultPath = @"C:\Users\qsaddam\Documents\Language\2.tif";// "C:\\Users\\qsaddam\\Documents\\Sharpdesk Desktop\\Input_Output\\4-pamphlet.tif";//@"C:\Users\qsaddam\Documents\Sharpdesk Desktop\AA\Test Input\sample1.png";//@"C:\Users\qsaddam\Documents\Sharpdesk Desktop\Invoice.tif";//@"C:\Users\qsaddam\Documents\Sharpdesk Desktop\Input_Output\vertical_align_text.tif";
            using (var ocrEngine = new TesseractEngine(tessdata, ENGLISH_LANGUAGE, EngineMode.TesseractOnly))
            {

                //string ext = Path.GetExtension(resultPath);
                var examplePixPath = System.IO.Path.GetDirectoryName(resultPath) + "\\" + Path.GetFileNameWithoutExtension(resultPath);
                using (var renderer = ResultRenderer.CreatePdfRenderer(examplePixPath, tessdata, false))
                {
                    
                    var imageName = Path.GetFileNameWithoutExtension(resultPath);
                    using (var pixA = ReadImageFileIntoPixArray(resultPath))
                    {
                        int expectedPageNumber = 0;
                        using (renderer.BeginDocument(imageName))
                        {
                            foreach (var pix in pixA)
                            {
                                monitor = new EtextDesc();
                                var ocrProgressHandler = new EtextDesc.TessProgressFunc(OnProgress);
                                monitor.MonitorSetProgressFunc(ocrProgressHandler);
                                using (var page = ocrEngine.Process(pix, imageName))
                                {
                                    page.Monitor = monitor.Handle;
                                    page.AnalyseLayout();
                                    //if (expectedPageNumber > 1) monitor.MonitorGetCancelThis();
                                     var addedPage = renderer.AddPage(page);
                                    Console.WriteLine("page completed : "+ ++expectedPageNumber);
                                }
                            }
                        }
                    }
                }
            }
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
        public static void pdfConvert()
        {
            try
            {
                string pdffilePath = string.Empty;
                string inputfilePath = @"C:\Users\qsaddam\Documents\Sharpdesk Desktop\Contract.tif";
                if (!string.IsNullOrEmpty(inputfilePath) && !string.IsNullOrWhiteSpace(inputfilePath))
                {
                    if (System.IO.File.Exists(inputfilePath))
                    {
                        var ENGLISH_LANGUAGE = @"eng+jpn_vert+jpn";
                        
                        var blogPostImage = inputfilePath;
                        //using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                        using (var ocrEngine = new TesseractEngine(@"./tessdata", ENGLISH_LANGUAGE, EngineMode.Default))
                        {
                            using (var imageWithText = Pix.LoadFromFile(blogPostImage))
                            {
                                using (var page = ocrEngine.Process(imageWithText, blogPostImage))
                                {
                                    var path = System.IO.Path.GetDirectoryName(blogPostImage) + "\\" + System.IO.Path.GetFileNameWithoutExtension(blogPostImage);
                                    using (var renderer = ResultRenderer.CreatePdfRenderer(path, @"./tessdata", false))
                                    {
                                        using (renderer.BeginDocument(System.IO.Path.GetFileNameWithoutExtension(blogPostImage)))
                                        {
                                            renderer.AddPage(page);
                                            pdffilePath = path + ".pdf";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //#region Code
        //public static void DoCrop()
        //{
        //    var testImagePath = @"E:\Users\Sharpdesk V5.3\AutoCrop Feature\tesseract-master\src\Tesseract.ConsoleDemo\Crop3.PNG";
        //    //var testImagePath = "./phototest.tif";
        //    HandleRef handle;
        //    int xb, yb, wb, hb = 0;
        //    int border = 40;

        //    try
        //    {
        //        using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
        //        {
        //            var pixHandle = Tesseract.Interop.LeptonicaApi.Native.pixRead(testImagePath);
        //            if (pixHandle == IntPtr.Zero)
        //            {
        //                throw new IOException(String.Format("Failed to load image '{0}'.", testImagePath));
        //                return;
        //            }
        //            Pix pix = Tesseract.Pix.Create(pixHandle);
        //            handle = new HandleRef(engine, pixHandle);

        //            var pixconv = Tesseract.Interop.LeptonicaApi.Native.pixConvertTo1(handle, 130);

        //            var pixMorp = Tesseract.Interop.LeptonicaApi.Native.pixMorphSequence(new HandleRef(engine, pixconv), "r11 + c10.20 + o5.50 + x4", 0);

        //            var pixconnComp = Tesseract.Interop.LeptonicaApi.Native.pixConnComp(new HandleRef(engine, pixMorp), IntPtr.Zero, 8);

        //            int boxCount = Tesseract.Interop.LeptonicaApi.Native.boxaGetCount(new HandleRef(engine, pixconnComp));

        //            for (int i = 0; i < boxCount; i++)
        //            {
        //                var boxa2 = Tesseract.Interop.LeptonicaApi.Native.boxaSort(new HandleRef(engine, pixconnComp), 10, 1, IntPtr.Zero);

        //                var box = Interop.LeptonicaApi.Native.boxaGetBox(new HandleRef(engine, boxa2), i, PixArrayAccessType.Clone);

        //                int x, y, w, h = 0;
        //                Interop.LeptonicaApi.Native.boxGetGeometry(new HandleRef(engine, box), out x, out y, out w, out h);

        //                xb = (x - border);
        //                yb = (y - border);
        //                wb = w + 2 * border;
        //                hb = h + 2 * border;

        //                //xb = 100;
        //                //yb = 100;
        //                //wb = 600;
        //                //hb = 800;

        //                var box2 = Interop.LeptonicaApi.Native.boxCreate(xb, yb, wb, hb);

        //                var PixCliphandle = Interop.LeptonicaApi.Native.pixClipRectangle(handle, new HandleRef(engine, box2), IntPtr.Zero);

        //                if (Interop.LeptonicaApi.Native.pixWrite(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputCropLep3.png", new HandleRef(engine, PixCliphandle), ImageFormat.Png) != 0)
        //                {
        //                    throw new IOException(String.Format("Failed to save image '{0}'.", @"C: \Users\hdeepak\Documents\Sharpdesk Desktop\OutputLep.png"));
        //                }
        //            }

        //            //Destroying
        //            Interop.LeptonicaApi.Native.pixDestroy(ref pixconv);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.TraceError(e.ToString());
        //    }
        //}

        //public static void DetectOrientationAndRotate()
        //{
        //    var testImagePath = @"E:\Users\Sharpdesk V5.3\AutoCrop Feature\tesseract-master\src\Tesseract.ConsoleDemo\UPSideDown.jpg";
        //    HandleRef handle;


        //    try
        //    {
        //        using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
        //        {
        //            var pixHandle = Tesseract.Interop.LeptonicaApi.Native.pixRead(testImagePath);
        //            if (pixHandle == IntPtr.Zero)
        //            {
        //                throw new IOException(String.Format("Failed to load image '{0}'.", testImagePath));
        //                return;
        //            }
        //            Pix pix = Tesseract.Pix.Create(pixHandle);
        //            handle = new HandleRef(engine, pixHandle);

        //            //"PixOrientDetect" API method will accpet 1bpp image type So below API method will convert to 1bpp.
        //            var pixconv = Tesseract.Interop.LeptonicaApi.Native.pixConvertTo1(handle, 130);

        //            float pupconf; float pleftconf;
        //            int value = Tesseract.Interop.LeptonicaApi.Native.pixOrientDetect(new HandleRef(engine, pixconv), out pupconf, out pleftconf, 0, 0);

        //            //-----------------****************------------------//
        //            //0 deg: upconf >> 1,    abs(upconf) >> abs(leftconf)
        //            //90 deg: leftconf >> 1,  abs(leftconf) >> abs(upconf)
        //            //180 deg: upconf << -1,   abs(upconf) >> abs(leftconf)
        //            //270 deg: leftconf << -1, abs(leftconf) >> abs(upconf)
        //            //-----------------****************------------------//

        //            float up_Conf = Math.Abs(pupconf);
        //            float pleft_Conf = Math.Abs(pleftconf);

        //            Pix RotatedImg;
        //            if (pleftconf > 1 && pleft_Conf > up_Conf)
        //            {
        //                RotatedImg = pix.Rotate90(1);
        //                RotatedImg.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\Test Results\AutoRotate Feature\OutputRotatLep.png");
        //            }
        //            else if (pupconf < -1 && up_Conf > pleft_Conf)
        //            {
        //                var rotatPtr = Tesseract.Interop.LeptonicaApi.Native.pixRotate180(handle, handle);
        //                Pix RotatedImg180 = Tesseract.Pix.Create(rotatPtr);
        //                RotatedImg180.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\Test Results\AutoRotate Feature\OutputRotatLep.png");
        //            }
        //            else if (pleftconf < -1 && pleft_Conf > up_Conf)
        //            {
        //                RotatedImg = pix.Rotate90(-1);
        //                RotatedImg.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\Test Results\AutoRotate Feature\OutputRotatLep.png");
        //            }
        //            else
        //                pix.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\Test Results\AutoRotate Feature\OutputRotatLep.png");

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.TraceError(e.ToString());
        //    }
        //}

        //public static void DescewTest()
        //{
        //    var sourcePixPath = @"E:\Users\Sharpdesk V5.3\AutoCrop Feature\tesseract-master\src\Tesseract.ConsoleDemo\skew.tif";
        //    using (var sourcePix = Pix.LoadFromFile(sourcePixPath))
        //    {
        //        Scew scew;
        //        using (var descewedImage = sourcePix.Deskew(new ScewSweep(range: 45), Pix.DefaultBinarySearchReduction, Pix.DefaultBinaryThreshold, out scew))
        //        {
        //            descewedImage.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputCrop15062021.png", ImageFormat.Png);
        //        }
        //    }
        //}

        //public static void DetectOrientationAndRotate_approach1()
        //{
        //    var testImagePath = @"E:\Users\Sharpdesk V5.3\AutoCrop Feature\tesseract-master\src\Tesseract.ConsoleDemo\02.tif";
        //    try
        //    {
        //        using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
        //        {
        //            //using (var img = Pix.LoadFromFile(testImagePath))
        //            {
        //                var pixHandle = Tesseract.Interop.LeptonicaApi.Native.pixRead(testImagePath);
        //                if (pixHandle == IntPtr.Zero)
        //                {
        //                    throw new IOException(String.Format("Failed to load image '{0}'.", testImagePath));
        //                    return;
        //                }
        //                Pix pix = Tesseract.Pix.Create(pixHandle);
        //                HandleRef handle = new HandleRef(engine, pixHandle);

        //                using (var page = engine.Process(pix))
        //                {
        //                    var text = page.AnalyseLayout();
        //                    ElementProperties elm = text.GetProperties();

        //                    switch (elm.Orientation)
        //                    {
        //                        case Orientation.PageUp:
        //                            Console.WriteLine("image in the correct position, nothing to do\n");
        //                            if (Math.Abs(elm.DeskewAngle) > 0.0001f)
        //                            {
        //                                Console.WriteLine("deskewing...\n");
        //                                IntPtr pixd_1 = Tesseract.Interop.LeptonicaApi.Native.pixRotate(handle, -elm.DeskewAngle, RotationMethod.Shear, RotationFill.White, 0, 0);

        //                                Pix pixres_0 = Tesseract.Pix.Create(pixd_1);
        //                                pixres_0.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputRotApproach1.png");
        //                            }
        //                            break;
        //                        case Orientation.PageRight:
        //                            Console.WriteLine("rotating image by 270 degrees\n");
        //                            IntPtr pixd1 = Tesseract.Interop.LeptonicaApi.Native.pixRotate90(handle, -1);

        //                            Pix pixres_1 = Tesseract.Pix.Create(pixd1);
        //                            pixres_1.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputRotApproach1.png");

        //                            if (elm.DeskewAngle > 0.0001f)
        //                            {
        //                                Console.WriteLine("deskewing...\n");
        //                                IntPtr pixd2 = Tesseract.Interop.LeptonicaApi.Native.pixRotate(handle, -elm.DeskewAngle, RotationMethod.Shear, RotationFill.White, 0, 0);
        //                                Pix pixres1 = Tesseract.Pix.Create(pixd1);
        //                                pixres1.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputRotApproach1.png");
        //                            }
        //                            break;
        //                        case Orientation.PageDown:
        //                            Console.WriteLine("rotating image by 180 degrees\n");
        //                            IntPtr pixd = Tesseract.Interop.LeptonicaApi.Native.pixRotate180(handle, handle);

        //                            Pix pixres_2 = Tesseract.Pix.Create(pixd);
        //                            pixres_2.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputRotApproach1.png");

        //                            if (elm.DeskewAngle > 0.0001f)
        //                            {
        //                                Console.WriteLine("deskewing...\n");
        //                                IntPtr pixd3 = Tesseract.Interop.LeptonicaApi.Native.pixRotate(handle, -elm.DeskewAngle, RotationMethod.Shear, RotationFill.White, 0, 0);
        //                                Pix pixres2 = Tesseract.Pix.Create(pixd3);
        //                                pixres2.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputRotApproach1.png");
        //                            }
        //                            break;
        //                        case Orientation.PageLeft:
        //                            Console.WriteLine("rotating image by 180 degrees\n");
        //                            pixd = Tesseract.Interop.LeptonicaApi.Native.pixRotate90(handle, 1);

        //                            Pix pixres3 = Tesseract.Pix.Create(pixd);
        //                            pixres3.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputRotApproach1.png");

        //                            if (elm.DeskewAngle > 0.0001f)
        //                            {
        //                                Console.WriteLine("deskewing...\n");
        //                                IntPtr pixd4 = Tesseract.Interop.LeptonicaApi.Native.pixRotate(handle, -elm.DeskewAngle, RotationMethod.Shear, RotationFill.White, 0, 0);

        //                                Pix pixres4 = Tesseract.Pix.Create(pixd4);
        //                                pixres4.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputRotApproach1.png");
        //                            }
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Trace.TraceError(e.ToString());
        //    }
        //}
        //#endregion

        //public static void DoCropPhase1()
        //{
        //    var testImagePath = @"E:\Users\Sharpdesk V5.3\AutoCrop Feature\tesseract-master\src\Tesseract.ConsoleDemo\Crop3.png";
        //    //var testImagePath = "./phototest.tif";
        //    HandleRef handle;

        //    try
        //    {
        //        using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
        //        {
        //            var pixHandle = Tesseract.Interop.LeptonicaApi.Native.pixRead(testImagePath);
        //            if (pixHandle == IntPtr.Zero)
        //            {
        //                throw new IOException(String.Format("Failed to load image '{0}'.", testImagePath));
        //                return;
        //            }
        //            Pix pix = Tesseract.Pix.Create(pixHandle);
        //            handle = new HandleRef(engine, pixHandle);

        //            int count = Tesseract.Interop.LeptonicaApi.Native.pixcmapGetDepth(handle);
        //            IntPtr pixg;
        //            if (count == 32)
        //            {
        //                pixg = Tesseract.Interop.LeptonicaApi.Native.pixConvertRGBToGrayFast(handle);
        //            }
        //            else
        //                pixg = Tesseract.Interop.LeptonicaApi.Native.pixClone(handle);

        //            int count1 = Tesseract.Interop.LeptonicaApi.Native.pixcmapGetDepth(new HandleRef(engine, pixg));
        //            IntPtr pixb;
        //            if (count1 == 8)
        //            {
        //                //Tesseract.Interop.LeptonicaApi.Native.
        //                pixb = Tesseract.Interop.LeptonicaApi.Native.pixThresholdToBinary(new HandleRef(engine, pixg), 170);
        //            }
        //            else
        //                pixb = Tesseract.Interop.LeptonicaApi.Native.pixClone(new HandleRef(engine, pixg));

        //            ///* Make seed and mask, and fill seed into mask */
        //            var pixseed4 = Tesseract.Interop.LeptonicaApi.Native.pixMorphSequence(new HandleRef(engine, pixb), "r1143 + o5.5+ x4", 0);
        //            var pixmask4 = Tesseract.Interop.LeptonicaApi.Native.pixMorphSequence(new HandleRef(engine, pixb), "r11", 0);
        //            var pixsf4 = Tesseract.Interop.LeptonicaApi.Native.pixSeedfillBinary(new HandleRef(engine, pixseed4),
        //                new HandleRef(engine, pixseed4), new HandleRef(engine, pixmask4), 8);
        //            var pixd4 = Tesseract.Interop.LeptonicaApi.Native.pixMorphSequence(new HandleRef(engine, pixsf4), "d3.3", 0);


        //            ///* Mask at full resolution */
        //            var pixd = Tesseract.Interop.LeptonicaApi.Native.pixExpandBinaryPower2(new HandleRef(engine, pixd4), 4);
        //            //pixWrite(fileout, pixd, IFF_TIFF_G4);

        //            if (Interop.LeptonicaApi.Native.pixWrite(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputLep01062021.png", new HandleRef(engine, pixd), ImageFormat.Png) != 0)
        //            {
        //                throw new IOException(String.Format("Failed to save image '{0}'.", @"C: \Users\hdeepak\Documents\Sharpdesk Desktop\OutputLep25052021.png"));
        //            }

        //            //var pix6 = Interop.LeptonicaApi.Native.pixSubtract(new HandleRef(engine, IntPtr.Zero), new HandleRef(engine, pix3), new HandleRef(engine, pix5));

        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}

        //public static void DoCropPhase2()
        //{
        //    DescewTest();
        //    var testImagePath = @"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputCrop28052021.png";
        //    HandleRef handle;
        //    try
        //    {
        //        using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
        //        {
        //            var pixHandle = Tesseract.Interop.LeptonicaApi.Native.pixRead(testImagePath);
        //            if (pixHandle == IntPtr.Zero)
        //            {
        //                throw new IOException(String.Format("Failed to load image '{0}'.", testImagePath));
        //                return;
        //            }
        //            Pix pix = Tesseract.Pix.Create(pixHandle);
        //            handle = new HandleRef(engine, pixHandle);

        //            var pixconv = Tesseract.Interop.LeptonicaApi.Native.pixConvertTo1(handle, 130);

        //            //var pixconv = Tesseract.Interop.LeptonicaApi.Native.pixConvertTo8(handle, 0);
        //            var pixMorp = Tesseract.Interop.LeptonicaApi.Native.pixMorphSequence(new HandleRef(engine, pixconv), "c2.2", 0);

        //            //var pixconnComp = Tesseract.Interop.LeptonicaApi.Native.pixConnComp(new HandleRef(engine, pixMorp), IntPtr.Zero, 8);

                    
        //            Pix pixconn = Tesseract.Pix.Create(pixMorp);
        //            pixconn.RemoveLines();
        //            pixconn.Save(@"C:\Users\hdeepak\Documents\Sharpdesk Desktop\OutputCropLeptonica2805.png");
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        // }

        //public static void DoCropPhaseTwo()
        //{

        //    var testImagePath = @"E:\Users\Sharpdesk V5.3\AutoCrop Feature\tesseract-master\src\Tesseract.ConsoleDemo\Crop3.PNG";            
        //    HandleRef handle;
        //    int xb, yb, wb, hb = 0;
        //    int border = 40;

        //    try
        //    {
        //        using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
        //        {
        //            var pixHandle = Tesseract.Interop.LeptonicaApi.Native.pixRead(testImagePath);
        //            if (pixHandle == IntPtr.Zero)
        //            {
        //                throw new IOException(String.Format("Failed to load image '{0}'.", testImagePath));
        //                return;
        //            }
        //            Pix pix = Tesseract.Pix.Create(pixHandle);
        //            handle = new HandleRef(engine, pixHandle);
        //            var pixconv = Tesseract.Interop.LeptonicaApi.Native.pixConvertTo1(handle, 130);

        //            FindCorner fd = new FindCorner();
        //            var boxa = fd.LocateBarcodes(new HandleRef(engine,pixconv), 0);
        //            int boxCount = Tesseract.Interop.LeptonicaApi.Native.boxaGetCount(new HandleRef(engine, boxa));
        //            for (int i = 0; i < boxCount; i++)
        //            {

        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}
    }
}