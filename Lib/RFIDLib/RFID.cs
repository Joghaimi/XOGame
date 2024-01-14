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

namespace Library.RFIDLib
{
    public class RFID
    {
        private MfRc522 Mfrc522; // Declare the MfRc522 field
        public bool Init(int RSTPin)
        {
            try
            {
                GpioController GPIOController = new GpioController();
                SpiConnectionSettings Connection = new(0, 0);
                Connection.ClockFrequency = 10_000_000;
                SpiDevice spi = SpiDevice.Create(Connection);
                Mfrc522 = new(spi, RSTPin, GPIOController, false);
                Console.WriteLine("RFID reader found!");
                return true;
            }
            catch
            {
                Console.WriteLine("RFID Not Initialized");
                return false;
            }
        }
        public bool CheckCardExisting()
        {
            if (Mfrc522 != null)
            {
                byte[] buffer = new byte[2]; // Assuming a buffer of 10 bytes
                return Mfrc522.IsCardPresent(buffer, false);
            }
            else {
                Console.WriteLine("RFID Not Initialized");
               
                return false;
            }
            
        }
        public string ReadCardInfo()
        {
            if (Mfrc522 == null) Console.WriteLine("RFID Not Initialized ..");
            Data106kbpsTypeA card;
            bool isExist = Mfrc522.ListenToCardIso14443TypeA(out card, TimeSpan.FromSeconds(0.5));
            if (isExist)
            {
                var mifare = new MifareCard(Mfrc522!, 0);
                mifare.SerialNumber = card.NfcId;
                mifare.Capacity = MifareCardCapacity.Mifare1K;
                mifare.KeyA = MifareCard.DefaultKeyA.ToArray();
                mifare.KeyB = MifareCard.DefaultKeyB.ToArray();
                // Reading Process 
                int ret;
                for (byte block = 0; block < 64; block++)
                {
                    mifare.BlockNumber = block;
                    mifare.Command = MifareCardCommand.AuthenticationB;
                    ret = mifare.RunMifareCardCommand();
                    if (ret < 0)
                    {
                        // If you have an authentication error, you have to deselect and reselect the card again and retry
                        // Those next lines shows how to try to authenticate with other known default keys
                        mifare.ReselectCard();
                        // Try the other key
                        mifare.KeyA = MifareCard.DefaultKeyA.ToArray();
                        mifare.Command = MifareCardCommand.AuthenticationA;
                        ret = mifare.RunMifareCardCommand();
                        if (ret < 0)
                        {
                            mifare.ReselectCard();
                            mifare.KeyA = MifareCard.DefaultBlocksNdefKeyA.ToArray();
                            mifare.Command = MifareCardCommand.AuthenticationA;
                            ret = mifare.RunMifareCardCommand();
                            if (ret < 0)
                            {
                                mifare.ReselectCard();
                                mifare.KeyA = MifareCard.DefaultFirstBlockNdefKeyA.ToArray();
                                mifare.Command = MifareCardCommand.AuthenticationA;
                                ret = mifare.RunMifareCardCommand();
                                if (ret < 0)
                                {
                                    mifare.ReselectCard();
                                    Console.WriteLine($"Error reading bloc: {block}");
                                }
                            }
                        }
                    }
                    if (ret >= 0)
                    {
                        mifare.BlockNumber = block;
                        mifare.Command = MifareCardCommand.Read16Bytes;
                        ret = mifare.RunMifareCardCommand();
                        if (ret >= 0)
                        {
                            if (mifare.Data is object)
                            {
                                Console.WriteLine($"Bloc: {block}, Data: {BitConverter.ToString(mifare.Data)}");
                                Console.WriteLine(Encoding.UTF8.GetString(mifare.Data));
                            }
                        }
                        else
                        {
                            mifare.ReselectCard();
                            Console.WriteLine($"Error reading bloc: {block}");
                        }
                        //if (block % 4 == 3)
                        //{
                        //    if (mifare.Data != null)
                        //    {
                        //        // Check what are the permissions
                        //        for (byte j = 3; j > 0; j--)
                        //        {
                        //            var access = mifare.BlockAccess((byte)(block - j), mifare.Data);
                        //            Console.WriteLine($"Bloc: {block - j}, Access: {access}");
                        //        }
                        //        var sector = mifare.SectorTailerAccess(block, mifare.Data);
                        //        Console.WriteLine($"Bloc: {block}, Access: {sector}");
                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine("Can't check any sector bloc");
                        //    }
                        //}
                    }
                    else
                    {
                        Console.WriteLine($"Authentication error");
                    }
                }
            }
            return "";
        }
    }
}
