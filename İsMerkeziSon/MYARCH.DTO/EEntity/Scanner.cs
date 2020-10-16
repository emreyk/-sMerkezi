using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIA;

namespace MYARCH.DTO.EEntity
{
    public class Scanner
    {
        private readonly DeviceInfo _deviceInfo;
        private int resolution = 150;
        private int width_pixel = 1250;
        private int height_pixel = 1700;
        private int color_mode = 1;

        public Scanner(DeviceInfo deviceInfo)
        {
            this._deviceInfo = deviceInfo;
        }

        public List<object> TarayiciAdi()
        {

            // Clear the ListBox.
            //listBox1.Items.Clear();

            // Create a DeviceManager instance
            var deviceManager = new DeviceManager();

            List<object> tara = new List<object>();
            // Loop through the list of devices and add the name to the listbox
            for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
            {
                // Add the device only if it's a scanner
                if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType)
                {
                    continue;
                }


                // Add the Scanner device to the listbox (the entire DeviceInfos object)
                // Important: we store an object of type scanner (which ToString method returns the name of the scanner)

                //listBox1.Items.Add(
                //    new Scanner(deviceManager.DeviceInfos[i])
                //);

               
                tara.Add(new Scanner(deviceManager.DeviceInfos[i]));
            }

            return tara;
        }


    }
}
