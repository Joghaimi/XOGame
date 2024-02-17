using Library.GPIOLib;
using NAudio.SoundFont;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.Spi;
using Iot.Device.Mfrc522;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iot.Device.Rfid;
using Iot.Device.Card.Mifare;
using System.Diagnostics;
using Iot.Device.Nmea0183;
using Microsoft.Extensions.Logging;

namespace Library.RFIDLib
{
    public class RFID
    {
        private MfRc522 Mfrc522; // Declare the MfRc522 field
        private static readonly ILogger _logger = LoggerFactory.Create(builder => { builder.AddConsole(); }).CreateLogger<RFID>();
        public bool Init(int RSTPin)
        {
            try
            {
                GpioController GPIOController = new GpioController();
                SpiConnectionSettings Connection = new(0, 0);
                Connection.ClockFrequency = 10_000_000;
                SpiDevice spi = SpiDevice.Create(Connection);
                Mfrc522 = new(spi, RSTPin, GPIOController, false);
                _logger.LogInformation("RFID Initialization Success");
                return true;
            }
            catch
            {
                _logger.LogError("RFID Initialization Faild");
                return false;
            }
        }
        public bool CheckCardExisting()
        {
            try
            {
                if (Mfrc522 != null)
                {
                    byte[] buffer = new byte[2]; // Assuming a buffer of 10 bytes
                    return Mfrc522.IsCardPresent(buffer, false);
                }
                else
                {
                    _logger.LogError("RFID Not Initialized");
                    return false;
                }
            }
            catch
            {
                _logger.LogError("CheckCardExisting Error");
                return false;
            }

        }
        public string ReadCardInfo()
        {
            try
            {
                if (Mfrc522 == null) _logger.LogError("RFID Not Initialized");
                Data106kbpsTypeA card;
                bool isExist = Mfrc522.ListenToCardIso14443TypeA(out card, TimeSpan.FromSeconds(0.5));
                if (isExist)
                {
                    var mifare = new MifareCard(Mfrc522!, 0);
                    mifare.SerialNumber = card.NfcId;
                    var data = BitConverter.ToString(mifare.SerialNumber);
                    return data;
                }
                else
                {
                    _logger.LogError("Card Not Detected");
                    return "";
                }
            }
            catch
            {
                _logger.LogError("ReadCardInfo Error");
                return "";
            }
        }
    }
}
