using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using Tesseract.Internal;
using Tesseract.Interop;

namespace Tesseract
{
    /// <summary>
    /// The tesseract OCR engine.
    /// </summary>
    public class EtextDesc : DisposableBase
    {
        private static readonly TraceSource trace = new TraceSource("Tesseract");

        private HandleRef handle;
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool TessProgressFunc(IntPtr monitor,
                                              int left,
                                              int right,
                                              int top,
                                              int bottom);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void TessCancelFunc(IntPtr cancelThis, int words);

        public EtextDesc()
        {
            handle = new HandleRef(this, Interop.TessApi.Native.MonitorCreate());
        }
        public void MonitorSetCancelFunc(TessCancelFunc ocrCancelHandler)
        {
            Interop.TessApi.Native.MonitorSetCancelFunc((IntPtr)handle, ocrCancelHandler);
        }
        public void MonitorSetProgressFunc(TessProgressFunc ocrProgressHandler)
        {
            Interop.TessApi.Native.MonitorSetProgressFunc((IntPtr)handle, ocrProgressHandler);
        }
        /// <summary>
        /// Return the percetage completed of OCRing.
        /// </summary>
        /// <returns>Percentage completed of OCRing</returns>
        public int MonitorGetProgress()
        {
            return Interop.TessApi.Native.MonitorGetProgress(handle);
        }

        public void MonitorGetCancelThis()
        {
            Interop.TessApi.Native.MonitorGetCancelThis(handle);
        }

        //public void MonitorSetCancelThis()
        //{
        //   // Interop.TessApi.Native.TessMonitorSetCancelThis(handle);
        //}
        /// <summary>
        /// To terminate the OCRing within given time in milliseconds
        /// </summary>
        /// <param name="time" in milli seconds></param>
        public void MonitorSetDeadlineMSecs(int time)
        {
            Interop.TessApi.Native.MonitorSetDeadlineMSecs(handle, time);
        }
        public HandleRef Handle
        {
            get { return handle; }
        }
        protected override void Dispose(bool disposing)
        {
            if (handle.Handle != IntPtr.Zero)
            {
                Interop.TessApi.Native.MonitorDelete(handle);
                handle = new HandleRef(this, IntPtr.Zero);
            }
        }
    }
}
