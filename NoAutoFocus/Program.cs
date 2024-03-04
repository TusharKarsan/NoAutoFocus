using System;
using System.Runtime.InteropServices;
using DirectShowLib;

namespace NoAutoFocus
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize DirectShow
            DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if (devices.Length == 0)
            {
                Console.WriteLine("No webcam found.");
                return;
            }

            // Select the first webcam (you can choose a specific one if needed)
            IBaseFilter webcam = null;
            var moniker = devices[0].Mon;
            moniker.BindToObject(null, null, typeof(IBaseFilter).GUID, out object obj);
            if (obj != null)
                webcam = obj as IBaseFilter;

            if (webcam == null)
            {
                Console.WriteLine("Failed to access webcam.");
                return;
            }

            // Get the camera control interface
            var cameraControl = (IAMCameraControl)webcam;
            if (cameraControl == null)
            {
                Console.WriteLine("Webcam does not support camera control.");
                return;
            }

            // Disable auto-focus (set to manual mode)
            int focusMode = 0; // Manual focus - might be infinity
            cameraControl.Set(CameraControlProperty.Focus, focusMode, CameraControlFlags.Manual);

            Console.WriteLine("Auto-focus disabled. You can now capture images without interference.");

            // Clean up
            Marshal.ReleaseComObject(webcam);
        }
    }
}
