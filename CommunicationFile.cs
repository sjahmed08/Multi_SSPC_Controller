using System;
using System.Collections.Generic;
using Peak.Can.Basic;
using TPCANHandle = System.Byte;
using System.Timers;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace SSPC_DemoUI
{
    public partial class CommunicationFile : Control
    {
        private static System.Timers.Timer bTimer;
        private static System.Timers.Timer aTimer;
        private static System.Timers.Timer cTimer;
        private static System.Timers.Timer dTimer;
        private static System.Timers.Timer fTimer;

        public double[,] inComingData = new double[16, 6];
        public double[,] inComingBrdData = new double[1, 2];

        public double[,] inComingData2 = new double[16, 6];
        public double[,] inComingBrdData2 = new double[1, 2];

        public static TPCANTimestamp TPCANTimestamp;
        public static TPCANHandle m_PcanHandle = 81;
        public static TPCANBaudrate m_Baudrate = TPCANBaudrate.PCAN_BAUD_250K;
        public static TPCANMsg CANMsgRead; // can message read
        public static TPCANMsg CANMsgRead2;
        public static TPCANMsg CANMsgWrite; //can message write
        public static TPCANMsg CANMsgWrite2;
        public static TPCANMsg CANMsgWrite3;
        public static TPCANMsg CANMsgRead3;

        public static TPCANType m_HwType = TPCANType.PCAN_TYPE_ISA;
        public static byte chnlNum = 0x00;  //channel number
        public static byte boardNum = 0x00; //board number
        public static byte[] chnlArray = { 0, 1, 2, 3, 5, 8, 9, 10, 11 };
        public static byte[] chnlArray2 = { 0, 1, 2, 3, 5, 8, 9, 10, 11 };
        public static double[] filterArray1 = new double[10];
        public static double[] filterArray2 = new double[10];
        public static double[] filterArray3 = new double[10];
        public static double[] filterArray4 = new double[10];
        public static byte chnlNum2 = 0x00;  //channel number
        public static byte boardNum2 = 0x00; //board number
        public static uint ID1 = 418369289;
        public static uint ID2 = 418369033;
        public static bool xmt_error = false;


        /*******************************************************************************************
         * THIS OUTLINES THE COMMUNICATION ID'S USED BY THE APPLICATION, EACH ID SENDS DIFFERENT 
         * MESSAGES ASSOCIATED WITH DIFFERENT DEVICE PARAMETER(S)
         * 
         *      
        *********************************************************************************************/

        public CommunicationFile()
        {
            InitializeLifetimeService();
        }

        public void alarmInterrupt()
        {
            TPCANStatus stsResult;
            UInt32 iBuffer = 1;
            stsResult = PCANBasic.SetValue(m_PcanHandle, TPCANParameter.PCAN_RECEIVE_EVENT, ref iBuffer, sizeof(UInt32));
        }

        public void Initializing()
        {
            TPCANStatus stsResult;
            bool leftOFF = false;
            bool rightOFF = false;
            byte[] writeMSG = new byte[8];
            uint iD1 = ID1;           //device 1
            uint iD2 = ID2;
            PCANBasic.Uninitialize(m_PcanHandle);
            stsResult = PCANBasic.Initialize(m_PcanHandle, m_Baudrate, m_HwType, Convert.ToUInt32(0x04),
            Convert.ToUInt16(3));

            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
            {
                system_Exit(0x00);
            }
            else
            {
                MessageBox.Show("Connection Successful!");
            }

            writeMSG[0] = 0x05;
            writeMSG[1] = 0x40;
            writeMSG[2] = 0x01;
            writeMSG[3] = 0xDC;
            writeMSG[4] = 0x0D;
            writeMSG[5] = 0x00;
            writeMSG[6] = 0x00;
            writeMSG[7] = 0x00;

            SSPC_Ctrl(writeMSG, iD1);

            if (SSPC_feedback() == false)
            {
                leftOFF = true;
                system_Exit(01);
            }
            else
            {
                writeMSG[0] = 0x67;
                writeMSG[1] = 0x40;
                writeMSG[2] = 0x00;
                writeMSG[3] = 0x00;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                SSPC_Ctrl(writeMSG, iD1);

                writeMSG[0] = 0x05;
                writeMSG[1] = 0x40;
                writeMSG[2] = 0x00;
                writeMSG[3] = 0xDC;
                writeMSG[4] = 0x0D;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                SSPC_Ctrl(writeMSG, iD1);

                cTimer = new System.Timers.Timer(150);
                cTimer.Elapsed += can1BrdDataRead;
                cTimer.Enabled = true;

                aTimer = new System.Timers.Timer(240);
                //channel read
                aTimer.Elapsed += can1ChannelRead;
                aTimer.Enabled = true;
            }
            System.Threading.Thread.Sleep(10);

            writeMSG[0] = 0x05;
            writeMSG[1] = 0x40;
            writeMSG[2] = 0x01;
            writeMSG[3] = 0xDC;
            writeMSG[4] = 0x0D;
            writeMSG[5] = 0x00;
            writeMSG[6] = 0x00;
            writeMSG[7] = 0x00;
            SSPC_Ctrl(writeMSG, iD2);

            if (SSPC_feedback() == false)
            {
                rightOFF = true;
                system_Exit(02);
            }
            else
            {
                writeMSG[0] = 0x67;
                writeMSG[1] = 0x40;
                writeMSG[2] = 0x00;
                writeMSG[3] = 0x00;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                SSPC_Ctrl(writeMSG, iD2);

                writeMSG[0] = 0x05;
                writeMSG[1] = 0x40;
                writeMSG[2] = 0x00;
                writeMSG[3] = 0xDC;
                writeMSG[4] = 0x0D;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                SSPC_Ctrl(writeMSG, iD2);

                bTimer = new System.Timers.Timer(250);
                //channel read
                bTimer.Elapsed += can2ChannelRead;
                bTimer.Enabled = true;

                dTimer = new System.Timers.Timer(250);
                dTimer.Elapsed += can2BrdDataRead;
                dTimer.Enabled = true;
            }

            if ((leftOFF == true) && (rightOFF == true))
            {
                uinitializePort();
                MessageBox.Show("No devices found, unitializing CAN port");
            }
            else
            {                
                fTimer = new System.Timers.Timer(500);
                fTimer.Elapsed += system_OvrCurr;
                fTimer.Enabled = true;
            }
         }

        public void primary_Start()
        {
            byte[] writeMSG = new byte[8];
            uint iD1 = ID1;           //device 1
            uint iD2 = ID2;
       
            writeMSG[0] = 0x01;
            writeMSG[1] = 0x7F;
            writeMSG[2] = 0x02;
            writeMSG[3] = 0xFD;
            writeMSG[4] = 0x00;
            writeMSG[5] = 0x00;
            writeMSG[6] = 0x00;
            writeMSG[7] = 0x00;

            SSPC_Ctrl(writeMSG, iD1);
            writeMSG[2] = 0x04;
            SSPC_Ctrl(writeMSG, ID1);
            PauseForMilliSeconds(1);
          //  MessageBox.Show("Left SSPC Turned on");
            writeMSG[2] = 0x02;
            SSPC_Ctrl(writeMSG, iD2);
            writeMSG[2] = 0x04;
            SSPC_Ctrl(writeMSG, iD2);
            PauseForMilliSeconds(1);
            //MessageBox.Show("Right SSPC Turned on");
         
            writeMSG[2] = 0x00;
           // writeMSG[3] = 0xFC;
            PauseForMilliSeconds(1);

            SSPC_Ctrl(writeMSG, iD1);
            PauseForMilliSeconds(1);   
            SSPC_Ctrl(writeMSG, iD2);
            PauseForMilliSeconds(1);

            writeMSG[2] = 0x03;
            SSPC_Ctrl(writeMSG, iD1);
            PauseForMilliSeconds(1);
            SSPC_Ctrl(writeMSG, iD2);
            PauseForMilliSeconds(1);    
            
    

        }

        private void uinitializePort()
        {
            PCANBasic.Uninitialize(m_PcanHandle);
        }
        
        public static DateTime PauseForMilliSeconds(int MillisecondsToPauseFor)
        {
            System.DateTime thisMoment = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(0, 0, 0, MillisecondsToPauseFor);
            System.DateTime Afterwards = thisMoment.Add(duration);

            while (Afterwards >= thisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                thisMoment = System.DateTime.Now;
            }
            return System.DateTime.Now;
        }

        public void secondary_Start()
        {
            byte[] writeMSG = new byte[8];
            uint iD1 = ID1;          //device 1
            uint iD2 = ID2;
            iD1++;
            iD2++;
            byte[] chnlArray = {0x05, 0x0B, 0x09};
            byte chnlINC = 0;
            writeMSG[0] = 0x01;
            writeMSG[1] = 0x7F;
            writeMSG[2] = chnlArray[chnlINC];
            writeMSG[3] = 0xFD;
            writeMSG[4] = 0x00;
            writeMSG[5] = 0x00;
            writeMSG[6] = 0x00;
            writeMSG[7] = 0x00;

            while (chnlINC < 3)
            {
                writeMSG[2] = chnlArray[chnlINC];
                SSPC_Ctrl(writeMSG, iD1);
                PauseForMilliSeconds(1);
                SSPC_Ctrl(writeMSG, iD2);
                PauseForMilliSeconds(1);               
                chnlINC++;
            }

        }

        public async void load_Shedding()
        {
            byte[] writeMSG = new byte[8];
            uint iD1 = ID1;          //device 1
            uint iD2 = ID2;
            byte chnlINC = 0;
                  
            byte[] chnlArray = {0x08, 0x0A, 0x09, 0x0B, 0x05};
           
            writeMSG[0] = 0x01;
            writeMSG[1] = 0x7F;
            writeMSG[2] = chnlArray[chnlINC];
            writeMSG[3] = 0xFD;
            writeMSG[4] = 0x00;
            writeMSG[5] = 0x00;
            writeMSG[6] = 0x00;
            writeMSG[7] = 0x00;
            MessageBox.Show(new Form { TopMost = true }, "Turning on all loads");
            while (chnlINC < 5)
            {
                writeMSG[2] = chnlArray[chnlINC];
                SSPC_Ctrl(writeMSG, iD1);
               
                SSPC_Ctrl(writeMSG, iD2);
                await waitMethod();
                chnlINC++;
            }
           
            chnlINC = 0;   
           
        }

        private void can1ChannelRead(Object source, ElapsedEventArgs e)
        //  private void canWRITE()
        {
            TPCANStatus stsResult;
            CANMsgWrite = new TPCANMsg();
            byte counTer = 0;
            byte flagCounter = 0;
            bool skipProcess = false;
            bool chnlReadSucc = false;
            bool chnlStatSucc = false;
            CANMsgWrite.LEN = 8;
            CANMsgWrite.DATA = new byte[8];
            CANMsgWrite.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_EXTENDED;
            //Message

            //Can read
            CANMsgRead.ID = 0;          //18EFAADC
            CANMsgRead.LEN = 8;
            int[] dataInPUT = new int[15];
            TPCANTimestamp.micros = 185;
            TPCANTimestamp.millis = 158903185;
            TPCANTimestamp.millis_overflow = 0;
                             //18EFDCAA
            while (chnlNum <= 8)
            {
                try
                {
                    CANMsgWrite.ID = 418369284;
                    CANMsgWrite.DATA[0] = 0x21;  
                    CANMsgWrite.DATA[1] = 0x00;
                    CANMsgWrite.DATA[2] = chnlArray[chnlNum];
                    CANMsgWrite.DATA[3] = 0x00;
                    CANMsgWrite.DATA[4] = 0x00;
                    CANMsgWrite.DATA[5] = 0x00;
                    CANMsgWrite.DATA[6] = 0x00;
                    CANMsgWrite.DATA[7] = 0x00;
                }
                catch (System.NullReferenceException)
                {
                }

                stsResult = PCANBasic.GetStatus(m_PcanHandle);
                if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                {
                    break;                   
                }

                stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgWrite);

                if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                {      
                    break;                
                }

                //Time delay for read back
                System.Threading.Thread.Sleep(5);
                while (CANMsgRead.ID != 418317519)
                {
                    if (counTer > 5)
                    {
                        counTer = 0;
                        skipProcess = true;
                        break;
                    }
                    stsResult = PCANBasic.Read(m_PcanHandle, out CANMsgRead, out TPCANTimestamp);
                    counTer++;
                }

                if ((stsResult == TPCANStatus.PCAN_ERROR_OK) && (skipProcess == false))
                {
                    for (int i = 0; i < CANMsgRead.LEN; i++)
                    {
                        dataInPUT[i] = CANMsgRead.DATA[i];
                    }
                    //Current
                    inComingData[chnlArray[chnlNum], 0] = chnlArray[chnlNum];
                    inComingData[chnlArray[chnlNum], 1] = ((dataInPUT[5] * 65536) + (dataInPUT[4] * 256) + dataInPUT[3]);
                    inComingData[chnlArray[chnlNum], 1] = ((inComingData[chnlArray[chnlNum], 1] * .01) - 80000);
                    inComingData[chnlArray[chnlNum], 1] = Math.Round(inComingData[chnlArray[chnlNum], 1], 2);

                
                    /***********************************************************************************************/
                  

                    //Voltage
                    inComingData[chnlArray[chnlNum], 2] = (dataInPUT[7] * 256) + dataInPUT[6];
                    inComingData[chnlArray[chnlNum], 2] = ((inComingData[chnlArray[chnlNum], 2] * .05) - 1606);
                    inComingData[chnlArray[chnlNum], 2] = Math.Round(inComingData[chnlArray[chnlNum], 2], 2);
                    chnlReadSucc = true;
                    //change channel                             
                    //maximum channel n-1
                }

                counTer = 0;
                skipProcess = false;

                CANMsgWrite.ID = 418369285;
                try
                {

                    CANMsgWrite.DATA[0] = 0x25;
                    CANMsgWrite.DATA[1] = 0x00;
                    CANMsgWrite.DATA[2] = chnlArray[chnlNum];
                    CANMsgWrite.DATA[3] = 0x00;
                    CANMsgWrite.DATA[4] = 0x00;
                    CANMsgWrite.DATA[5] = 0x00;
                    CANMsgWrite.DATA[6] = 0x00;
                    CANMsgWrite.DATA[7] = 0x00;
                }
                catch (System.NullReferenceException)
                {
                }

                stsResult = PCANBasic.GetStatus(m_PcanHandle);
                if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                {
                    break;
                }
                stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgWrite);

                if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                {   
                    break;             
                }

                System.Threading.Thread.Sleep(5);
                while (CANMsgRead.ID != 418317775)
                {
                    if (counTer > 5)
                    {
                        counTer = 0;
                        skipProcess = true;
                        break;
                    }
                    stsResult = PCANBasic.Read(m_PcanHandle, out CANMsgRead, out TPCANTimestamp);
                    counTer++;
                }

                if ((stsResult == TPCANStatus.PCAN_ERROR_OK) && (skipProcess == false))
                {
                    for (int i = 0; i < CANMsgRead.LEN; i++)
                    {
                        dataInPUT[i] = CANMsgRead.DATA[i];
                    }

                    if ((dataInPUT[2] == chnlArray[chnlNum]) && (dataInPUT[0] == 0x26))
                    {
                        inComingData[chnlArray[chnlNum], 3] = (dataInPUT[3] & 0x03);
                        inComingData[chnlArray[chnlNum], 4] = (dataInPUT[4] & 0x01);
                        chnlStatSucc = true;   
                    }
                    else
                    {
                        chnlStatSucc = false;
                    }
                }
            
               if ((chnlNum == 0) || (chnlNum == 2))
                {
                    counTer = 0;
                    skipProcess = false;
                    CANMsgWrite.ID = 418369286;
                    try
                    {

                        CANMsgWrite.DATA[0] = 0xC1;
                        CANMsgWrite.DATA[1] = 0x00;
                        CANMsgWrite.DATA[2] = chnlArray[chnlNum];
                        CANMsgWrite.DATA[3] = 0x00;
                        CANMsgWrite.DATA[4] = 0x00;
                        CANMsgWrite.DATA[5] = 0x00;
                        CANMsgWrite.DATA[6] = 0x00;
                        CANMsgWrite.DATA[7] = 0x00;
                    }
                    catch (System.NullReferenceException)
                    {
                    }

                    stsResult = PCANBasic.GetStatus(m_PcanHandle);
                    if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                    {
                        break;
                    }

                    stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgWrite);
                    if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                    {
                        break;
                    }

                    System.Threading.Thread.Sleep(5);
                    while (CANMsgRead.ID != 418318031)
                    {
                        if (counTer > 5)
                        {
                            counTer = 0;
                            skipProcess = true;
                            break;
                        }
                        stsResult = PCANBasic.Read(m_PcanHandle, out CANMsgRead, out TPCANTimestamp);
                        counTer++;
                    }

                    if ((stsResult == TPCANStatus.PCAN_ERROR_OK) && (skipProcess == false))
                    {
                        for (int i = 0; i < CANMsgRead.LEN; i++)
                        {
                            dataInPUT[i] = CANMsgRead.DATA[i];
                        }

                        if ((dataInPUT[2] == chnlArray[chnlNum]) && (dataInPUT[0] == 0xC2))
                        {
                            inComingData[chnlArray[chnlNum], 5] = ((((dataInPUT[5] * 65536) + (dataInPUT[4] * 256) + dataInPUT[3]) * .01) - 80000);
                            inComingData[chnlArray[chnlNum], 5] = Math.Round(inComingData[chnlArray[chnlNum], 5], 2);

                            if (chnlNum == 0)
                            {
                                filterArray1[9] = filterArray1[8];
                                filterArray1[8] = filterArray1[7];
                                filterArray1[7] = filterArray1[6];
                                filterArray1[6] = filterArray1[5];
                                filterArray1[5] = filterArray1[4];
                                filterArray1[4] = filterArray1[3];
                                filterArray1[3] = filterArray1[2];
                                filterArray1[2] = filterArray1[1];
                                filterArray1[1] = inComingData[chnlArray[chnlNum], 5];
                                filterArray1[0] = filterArray1.Sum() / 10;
                                inComingData[chnlArray[chnlNum], 5] = filterArray1[0];
                            }

                            if (chnlNum == 2)
                            {
                                filterArray2[9] = filterArray2[8];
                                filterArray2[8] = filterArray2[7];
                                filterArray2[7] = filterArray2[6];
                                filterArray2[6] = filterArray2[5];
                                filterArray2[5] = filterArray2[4];
                                filterArray2[4] = filterArray2[3];
                                filterArray2[3] = filterArray2[2];
                                filterArray2[2] = filterArray2[1];
                                filterArray2[1] = inComingData[chnlArray[chnlNum], 5];
                                filterArray2[0] = filterArray2.Sum() / 10;
                                inComingData[chnlArray[chnlNum], 5] = filterArray2[0]; 
                            }


                            if (inComingData[chnlArray[chnlNum], 5] > 1)
                            {
                                if (inComingData[chnlArray[chnlNum], 5] > 5)
                                {
                                    inComingData[chnlArray[chnlNum], 5] = 4.0;
                                }
                                else
                                {
                                    inComingData[chnlArray[chnlNum], 5] = 1.0;
                                }
                            }
                            else
                            {
                                inComingData[chnlArray[chnlNum], 5] = 0.0;
                            }

                            chnlStatSucc = true;
                        }
                            
                        }
                        else
                        {
                            chnlStatSucc = false;
                        }
                    }
                                             
                if ((chnlReadSucc == false) || (chnlStatSucc == false))                                                 //&&*************************change 4/24
                {
                    if (flagCounter > 5)
                    {
                        flagCounter = 0;
                        break;
                    }
                    flagCounter++;
                }

                if ((chnlReadSucc == true) && (chnlStatSucc == true))
                {
                    chnlNum++;
                }
            }
            chnlNum = 0;
        }

        private void can1BrdDataRead(Object source, ElapsedEventArgs e)
        {

            TPCANStatus stsResult;
            CANMsgWrite = new TPCANMsg();
            CANMsgWrite.ID = 418369282;                //18EFDC07
            CANMsgWrite.LEN = 8;
            CANMsgWrite.DATA = new byte[8];
            CANMsgWrite.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_EXTENDED;
            //Board data read message
            int counTer = 0;
            bool skipProcess = false;

            //  CANMsgRead.ID = 0;              //18EF07DC
            CANMsgRead.LEN = 8;
            int[] dataInPUT = new int[15];
            TPCANTimestamp.micros = 185;
            TPCANTimestamp.millis = 158903185;
            TPCANTimestamp.millis_overflow = 0;
            //Time delay for read back
            try
            {
                CANMsgWrite.DATA[0] = 0x27;
                CANMsgWrite.DATA[1] = 0x00;
                CANMsgWrite.DATA[2] = 0xBE;
                CANMsgWrite.DATA[3] = 0x00;
                CANMsgWrite.DATA[4] = 0x00;
                CANMsgWrite.DATA[5] = 0x00;
                CANMsgWrite.DATA[6] = 0x00;
                CANMsgWrite.DATA[7] = 0x00;
            }

            catch (System.NullReferenceException)
            {
            }
            //Write method, status read
                   
            stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgWrite);


            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
            {
         //       PCANBasic.Reset(m_PcanHandle);
          ///      MessageBox.Show("PCAN BUS QUEUE CLEARED");
            }

            System.Threading.Thread.Sleep(5);
            while (CANMsgRead.ID != 418317007)
            {
                if (counTer > 5)
                {
                    counTer = 0;
                    skipProcess = true;
                    break;
                }
                stsResult = PCANBasic.Read(m_PcanHandle, out CANMsgRead, out TPCANTimestamp);
                counTer++;
            }

            if ((stsResult == TPCANStatus.PCAN_ERROR_OK) && (skipProcess == false))
            {
                for (int i = 0; i < CANMsgRead.LEN; i++)
                {
                    dataInPUT[i] = CANMsgRead.DATA[i];
                }
                //board input voltage
                if (dataInPUT[0] == 0x28)
                {
                    inComingBrdData[boardNum, 0] = (dataInPUT[7] * 256) + dataInPUT[6];
                    inComingBrdData[boardNum, 0] = ((inComingBrdData[boardNum, 0] * .05) - 1606);
                    inComingBrdData[boardNum, 0] = Math.Round(inComingBrdData[boardNum, 0], 2);
                }
            }
            
            counTer = 0;
            skipProcess = false;
            CANMsgWrite.ID = 418369283;             //18EFDC07
            //CANMsgRead.ID = 418358476;
            try
            {
                CANMsgWrite.DATA[0] = 0x33;
                CANMsgWrite.DATA[1] = 0x00;
                CANMsgWrite.DATA[2] = 0x97;
                CANMsgWrite.DATA[3] = 0x00;
                CANMsgWrite.DATA[4] = 0x00;
                CANMsgWrite.DATA[5] = 0x00;
                CANMsgWrite.DATA[6] = 0x00;
                CANMsgWrite.DATA[7] = 0x00;
            }
            catch (System.NullReferenceException)
            {
            }

            stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgWrite);

            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
            {
            //    PCANBasic.Reset(m_PcanHandle);
           //     MessageBox.Show("PCAN BUS QUEUE CLEARED");
            }

            System.Threading.Thread.Sleep(5);
            while (CANMsgRead.ID != 418317263)
            {
                if (counTer > 5)
                {
                    counTer = 0;
                    skipProcess = true;
                    break;
                }
                stsResult = PCANBasic.Read(m_PcanHandle, out CANMsgRead, out TPCANTimestamp);
                counTer++;
            }


            if ((stsResult == TPCANStatus.PCAN_ERROR_OK) && (skipProcess == false))
            {
                for (int i = 0; i < CANMsgRead.LEN; i++)
                {
                    dataInPUT[i] = CANMsgRead.DATA[i];
                }
                if (dataInPUT[0] == 0x34)
                {
                    inComingBrdData[boardNum, 1] = ((dataInPUT[4] * 256) + (dataInPUT[3]));
                    inComingBrdData[boardNum, 1] = ((inComingBrdData[boardNum, 1] * .03125) - 273);
                    inComingBrdData[boardNum, 1] = Math.Round(inComingBrdData[boardNum, 1], 2);
                }
            }
        }

        private void can2BrdDataRead(Object source, ElapsedEventArgs e)
        {

            TPCANStatus stsResult;
            CANMsgWrite2 = new TPCANMsg();
            CANMsgWrite2.ID = 418369187;                 //18EFDC07
            CANMsgWrite2.LEN = 8;
            CANMsgWrite2.DATA = new byte[8];
            CANMsgWrite2.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_EXTENDED;
            int counTER = 0;
            bool skipProcess = false;
            // CANMsgRead2.ID = 0;              //18EF07DC
            CANMsgRead2.LEN = 8;
            int[] dataInPUT = new int[15];
            TPCANTimestamp.micros = 185;
            TPCANTimestamp.millis = 158903190;
            TPCANTimestamp.millis_overflow = 0;

            try
            {

                CANMsgWrite2.DATA[0] = 0x27;
                CANMsgWrite2.DATA[1] = 0x00;
                CANMsgWrite2.DATA[2] = 0xBE;
                CANMsgWrite2.DATA[3] = 0x00;
                CANMsgWrite2.DATA[4] = 0x00;
                CANMsgWrite2.DATA[5] = 0x00;
                CANMsgWrite2.DATA[6] = 0x00;
                CANMsgWrite2.DATA[7] = 0x00;
            }
            catch (System.NullReferenceException)
            {
            }
            //Write method, status read
            stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgWrite2);

            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
            {

           
            }


            //Time delay for read back
            System.Threading.Thread.Sleep(5);

            while (CANMsgRead2.ID != 418358222)
            {
                if (counTER > 5)
                {
                    counTER = 0;
                    skipProcess = true;
                    break;
                }
                stsResult = PCANBasic.Read(m_PcanHandle, out CANMsgRead2, out TPCANTimestamp);
                counTER++;
            }


            if ((stsResult == TPCANStatus.PCAN_ERROR_OK) && (skipProcess == false))
            {
                for (int i = 0; i < CANMsgRead2.LEN; i++)
                {
                    dataInPUT[i] = CANMsgRead2.DATA[i];
                }
                inComingBrdData2[boardNum2, 0] = (dataInPUT[7] * 256) + dataInPUT[6];
                inComingBrdData2[boardNum2, 0] = ((inComingBrdData2[boardNum2, 0] * .05) - 1606);
                inComingBrdData2[boardNum2, 0] = Math.Round(inComingBrdData2[boardNum2, 0], 2);
            }

            counTER = 0;
            skipProcess = false;
            CANMsgWrite2.ID = 418369188;
            CANMsgWrite2.DATA[0] = 0x33;
            CANMsgWrite2.DATA[1] = 0x00;
            CANMsgWrite2.DATA[2] = 0x97;
            CANMsgWrite2.DATA[3] = 0x00;
            CANMsgWrite2.DATA[4] = 0x00;
            CANMsgWrite2.DATA[5] = 0x00;
            CANMsgWrite2.DATA[6] = 0x00;
            CANMsgWrite2.DATA[7] = 0x00;

            //Write method, status read

            stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgWrite2);

            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
            {
           //     PCANBasic.Reset(m_PcanHandle);
            //    MessageBox.Show("PCAN BUS QUEUE CLEARED");
            }

            System.Threading.Thread.Sleep(5);
            while (CANMsgRead2.ID != 418358478)
            {
                if (counTER > 5)
                {
                    counTER = 0;
                    skipProcess = true;
                    break;
                }
                stsResult = PCANBasic.Read(m_PcanHandle, out CANMsgRead2, out TPCANTimestamp);
                counTER++;
            }

            if ((stsResult == TPCANStatus.PCAN_ERROR_OK) && (skipProcess == false))
            {
                for (int i = 0; i < CANMsgRead2.LEN; i++)
                {
                    dataInPUT[i] = CANMsgRead2.DATA[i];
                }
                inComingBrdData2[boardNum2, 1] = ((dataInPUT[4] * 256) + (dataInPUT[3]));
                inComingBrdData2[boardNum2, 1] = ((inComingBrdData2[boardNum2, 1] * .03125) - 273);
                inComingBrdData2[boardNum2, 1] = Math.Round(inComingBrdData2[boardNum2, 1], 2);
            }
        }

        private void can2ChannelRead(Object source, ElapsedEventArgs e)
        //  private void canWRITE()
        {
            TPCANStatus stsResult;
            CANMsgWrite = new TPCANMsg();
            CANMsgWrite.LEN = 8;
            CANMsgWrite.DATA = new byte[8];
            CANMsgWrite.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_EXTENDED;
            //Message

            CANMsgRead.ID = 0;          //18EFAADC
            CANMsgRead.LEN = 8;
            int[] dataInPUT = new int[15];
            TPCANTimestamp.micros = 185;
            TPCANTimestamp.millis = 158903185;
            TPCANTimestamp.millis_overflow = 0;

            byte counTer = 0;
            byte flagCounTer = 0;
            bool skipProcess = false;
            bool chnlReadSucc = false;
            bool chnlStatSucc = false;

            while (chnlNum2 <= 8)
            {
                try
                {
                    CANMsgWrite.ID = 418369185;                //18EFDCAA
                    CANMsgWrite.DATA[0] = 0x21;
                    CANMsgWrite.DATA[1] = 0x00;
                    CANMsgWrite.DATA[2] = chnlArray2[chnlNum2];
                    CANMsgWrite.DATA[3] = 0x00;
                    CANMsgWrite.DATA[4] = 0x00;
                    CANMsgWrite.DATA[5] = 0x00;
                    CANMsgWrite.DATA[6] = 0x00;
                    CANMsgWrite.DATA[7] = 0x00;
                }
                catch (System.NullReferenceException)
                {
                }
                //Write method, status read
                stsResult = PCANBasic.GetStatus(m_PcanHandle);
                if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                {
                    break;
                }
                stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgWrite);


                if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                {
                    break;
                }

                System.Threading.Thread.Sleep(5);
                while (CANMsgRead.ID != 418357710)
                {
                    if (counTer > 5)
                    {
                        counTer = 0;
                        skipProcess = true;
                        break;
                    }
                    stsResult = PCANBasic.Read(m_PcanHandle, out CANMsgRead, out TPCANTimestamp);
                    counTer++;
                }


                if ((stsResult == TPCANStatus.PCAN_ERROR_OK) && (skipProcess == false))
                {
                    for (int i = 0; i < CANMsgRead.LEN; i++)
                    {
                        dataInPUT[i] = CANMsgRead.DATA[i];
                    }
                    //Current
                    inComingData2[chnlArray2[chnlNum2], 0] = chnlArray2[chnlNum2];
                    inComingData2[chnlArray2[chnlNum2], 1] = ((dataInPUT[5] * 65536) + (dataInPUT[4] * 256) + dataInPUT[3]);
                    inComingData2[chnlArray2[chnlNum2], 1] = ((inComingData2[chnlArray2[chnlNum2], 1] * .01) - 80000);
                    inComingData2[chnlArray2[chnlNum2], 1] = Math.Round(inComingData2[chnlArray2[chnlNum2], 1], 2);

                    //Voltage
                    inComingData2[chnlArray2[chnlNum2], 2] = (dataInPUT[7] * 256) + dataInPUT[6];
                    inComingData2[chnlArray2[chnlNum2], 2] = ((inComingData2[chnlArray2[chnlNum2], 2] * .05) - 1606);
                    inComingData2[chnlArray2[chnlNum2], 2] = Math.Round(inComingData2[chnlArray2[chnlNum2], 2], 2);
                    chnlReadSucc = true;
                }

                counTer = 0;
                skipProcess = false;
                CANMsgWrite.ID = 418369186;
                try
                {
                    CANMsgWrite.DATA[0] = 0x25;
                    CANMsgWrite.DATA[1] = 0x00;
                    CANMsgWrite.DATA[2] = chnlArray2[chnlNum2];
                    CANMsgWrite.DATA[3] = 0x00;
                    CANMsgWrite.DATA[4] = 0x00;
                    CANMsgWrite.DATA[5] = 0x00;
                    CANMsgWrite.DATA[6] = 0x00;
                    CANMsgWrite.DATA[7] = 0x00;
                }
                catch (System.NullReferenceException)
                {
                }

                stsResult = PCANBasic.GetStatus(m_PcanHandle);
                if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                {

                    break;
                }

                stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgWrite);
                if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                {
                    break;             
                }

                System.Threading.Thread.Sleep(5);

                while (CANMsgRead.ID != 418357966)
                {
                    if (counTer > 5)
                    {
                        counTer = 0;
                        skipProcess = true;
                        break;
                    }
                    stsResult = PCANBasic.Read(m_PcanHandle, out CANMsgRead, out TPCANTimestamp);
                    counTer++;
                }

                if ((stsResult == TPCANStatus.PCAN_ERROR_OK) && (skipProcess == false))
                {
                    for (int i = 0; i < CANMsgRead.LEN; i++)
                    {
                        dataInPUT[i] = CANMsgRead.DATA[i];
                    }
                    //operating status
                    if ((dataInPUT[2] == chnlArray2[chnlNum2]) && (dataInPUT[0] == 0x26))
                    {
                        inComingData2[chnlArray2[chnlNum2], 3] = (dataInPUT[3] & 0x03);
                        inComingData2[chnlArray2[chnlNum2], 4] = (dataInPUT[4] & 0x01);
                        chnlStatSucc = true;
                    }
                    else
                    {
                        chnlStatSucc = false;
                    }
                }

                if ((chnlNum2 == 0) || (chnlNum2 == 2))
                {
                    counTer = 0;
                    skipProcess = false;
                    CANMsgWrite.ID = 418369189;
                    try
                    {
                        CANMsgWrite.DATA[0] = 0xC1;
                        CANMsgWrite.DATA[1] = 0x00;
                        CANMsgWrite.DATA[2] = chnlArray2[chnlNum2];
                        CANMsgWrite.DATA[3] = 0x00;
                        CANMsgWrite.DATA[4] = 0x00;
                        CANMsgWrite.DATA[5] = 0x00;
                        CANMsgWrite.DATA[6] = 0x00;
                        CANMsgWrite.DATA[7] = 0x00;
                    }
                    catch (System.NullReferenceException)
                    {
                    }

                    stsResult = PCANBasic.GetStatus(m_PcanHandle);
                    if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                    {
                        break;
                    }

                    stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgWrite);
                    if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(5);

                    while (CANMsgRead.ID != 418358734)
                    {
                        if (counTer > 5)
                        {
                            counTer = 0;
                            skipProcess = true;
                            break;
                        }
                        stsResult = PCANBasic.Read(m_PcanHandle, out CANMsgRead, out TPCANTimestamp);
                        counTer++;
                    }

                    if ((stsResult == TPCANStatus.PCAN_ERROR_OK) && (skipProcess == false))
                    {
                        for (int i = 0; i < CANMsgRead.LEN; i++)
                        {
                            dataInPUT[i] = CANMsgRead.DATA[i];
                        }

                        if ((dataInPUT[2] == chnlArray2[chnlNum2]) && (dataInPUT[0] == 0xC2))
                        {
                            inComingData2[chnlArray2[chnlNum2], 5] = ((((dataInPUT[5] * 65536) + (dataInPUT[4] * 256) + dataInPUT[3]) * .01) - 80000);
                            inComingData2[chnlArray2[chnlNum2], 5] = Math.Round(inComingData2[chnlArray[chnlNum], 5], 2);

                            if (chnlNum2 == 0)
                            {
                                filterArray3[9] = filterArray3[8];
                                filterArray3[8] = filterArray3[7];
                                filterArray3[7] = filterArray3[6];
                                filterArray3[6] = filterArray3[5];
                                filterArray3[5] = filterArray3[4];
                                filterArray3[4] = filterArray3[3];
                                filterArray3[3] = filterArray3[2];
                                filterArray3[2] = filterArray3[1];
                                filterArray3[1] = inComingData2[chnlArray2[chnlNum2], 5];
                                filterArray3[0] = filterArray3.Sum() / 10;
                                inComingData2[chnlArray2[chnlNum2], 5] = filterArray3[0];
                            }

                            if (chnlNum2 == 2)
                            {                                                              
                                filterArray4[9] = filterArray4[8];
                                filterArray4[8] = filterArray4[7];
                                filterArray4[7] = filterArray4[6];
                                filterArray4[6] = filterArray4[5];
                                filterArray4[5] = filterArray4[4];
                                filterArray4[4] = filterArray4[3];
                                filterArray4[3] = filterArray4[2];
                                filterArray4[2] = filterArray4[1];
                                filterArray4[1] = inComingData2[chnlArray2[chnlNum2], 5];
                                filterArray4[0] = filterArray4.Sum() / 10;
                                inComingData2[chnlArray2[chnlNum2], 5] = filterArray4[0];          //average course current
                            }

                            if (inComingData2[chnlArray2[chnlNum2], 5] > 1)
                            {
                                if (inComingData2[chnlArray2[chnlNum2], 5] > 4)
                                {
                                    inComingData2[chnlArray2[chnlNum2], 5] = 4.0;
                                }
                                else
                                {
                                    inComingData2[chnlArray2[chnlNum2], 5] = 1.0;
                                }
                            }
                            else
                            {
                                inComingData2[chnlArray2[chnlNum2], 1] = 0.0;
                            }                
                            chnlStatSucc = true;
                        }
                        else
                        {
                            chnlStatSucc = false;
                        }
                    }
                }


                if ((chnlReadSucc == false) || (chnlStatSucc == false))                 //&&***************changed
                {
                    if (flagCounTer > 5)
                    {
                        flagCounTer = 0;
                        break;
                    }
                    flagCounTer++;
                }

                if ((chnlReadSucc == true) && (chnlStatSucc == true))
                {
                    chnlNum2++;
                } 
           }
            chnlNum2 = 0;
        }

        public void buffeR_clear()
        {
            TPCANStatus stsResult;
            //stsResult = PCANBasic.Reset(m_PcanHandle);
            stsResult = PCANBasic.Uninitialize(m_PcanHandle);
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                    MessageBox.Show("PCAN CHANNEL UNINITIALIZED");
            }

            stsResult = PCANBasic.Initialize(m_PcanHandle, m_Baudrate, m_HwType, Convert.ToUInt32(0x04),
            Convert.ToUInt16(3));
            if (stsResult == TPCANStatus.PCAN_ERROR_OK)
            {
                MessageBox.Show("PCAN CHANNEL REINITIALIZED");
            }
        }

        public void create_XMT_ERROR()
        {

        }

        private void function_Tmer(bool ch)
        {
            if (ch == true)
            {
                aTimer.Enabled = true;
                cTimer.Enabled = true;
            }
            if (ch == false)
            {
                bTimer.Enabled = true;
                dTimer.Enabled = true;
            }
        }

        public void left_SSPC(bool relay, byte chnlNo)
        {
            byte[] writeMSG = new byte[8];
            uint ID = ID1;

            if (relay == false)
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = chnlNo;
                writeMSG[3] = 0xFC;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
            }

            if (relay == true)
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = chnlNo;
                writeMSG[3] = 0xFD;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
            }            
            SSPC_Ctrl(writeMSG, ID);          
        }

        public void right_SSPC(bool relay, byte chnlNo)
        {
            byte[] writeMSG = new byte[8];

            uint ID = ID2;

            if (relay == false)
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = chnlNo;
                writeMSG[3] = 0xFC;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
               
            }

            if (relay == true)
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = chnlNo;
                writeMSG[3] = 0xFD;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
            }

            SSPC_Ctrl(writeMSG, ID);
        }

        async System.Threading.Tasks.Task waitMethod()
        {
            await System.Threading.Tasks.Task.Delay(1000);
        }

        private void system_OvrCurr(Object source, ElapsedEventArgs e)
        {
            byte[] writeMSG = new byte[8];
            uint iD1 = ID1;
            uint iD2 = ID2;
            double totalChanlsOn = 0;
            byte chnlINC = 0;
            byte[] chnlArray = { 0x08, 0x0A, 0x09, 0x0B, 0x05 };
           
            for (int i = 0; i <= 15; i++)
            {
                totalChanlsOn += inComingData[i, 3];
                totalChanlsOn += inComingData2[i, 3];
            }
             
            writeMSG[0] = 0x01;
            writeMSG[1] = 0x7F;
            writeMSG[2] = chnlArray[chnlINC];
            writeMSG[3] = 0xFD;
            writeMSG[4] = 0x00;
            writeMSG[5] = 0x00;
            writeMSG[6] = 0x00;
            writeMSG[7] = 0x00;

            if (totalChanlsOn >= 16)                                                     //((totalCurrent >= 8) && (totalCurrent <= 15)) 4/24
            {
                fTimer.Enabled = false;

                MessageBox.Show(new Form { TopMost = true }, "Over current limit reached - going into power management", "Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                
                writeMSG[3] = 0xFC;
                while (chnlINC < 3)
                {
                    writeMSG[2] = chnlArray[chnlINC];
                    SSPC_Ctrl(writeMSG, iD1);                   
                    SSPC_Ctrl(writeMSG, iD2);                  
                    chnlINC++;
                }
                      
            }
            fTimer.Enabled = true;
        }

        public void system_Down()
        {
          
            byte[] writeMSG = new byte[8];

            writeMSG[0] = 0x07;
            writeMSG[1] = 0x7F;
            writeMSG[2] = 0x00;
            writeMSG[3] = 0x00;
            writeMSG[4] = 0x00;
            writeMSG[5] = 0x00;
            writeMSG[6] = 0x00;
            writeMSG[7] = 0x00;

            SSPC_Ctrl(writeMSG, ID1);
            SSPC_Ctrl(writeMSG, ID2);
           
            //fTimer.Enabled = false;
        }

        private void busTie_monitor(Object source, ElapsedEventArgs e)
        {
            uint ID;

            byte[] writeMSG = new byte[8];
            if (inComingData[2, 3] != 0)
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = 0x01;
                writeMSG[3] = 0xFD;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                ID = ID1;
            }
            else
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = 0x01;
                writeMSG[3] = 0xFC;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                ID = ID1;
            }
          //  SSPC_Ctrl(writeMSG, ID);

            if (inComingData2[2, 3] != 0)          
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = 0x01;
                writeMSG[3] = 0xFD;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                ID = ID2; 
            }
            else
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = 0x01;
                writeMSG[3] = 0xFC;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                ID = ID2;
            }
          //  SSPC_Ctrl(writeMSG, ID);
        }

        private void SSPC_Ctrl(byte[] msg, uint ID)
        {
            TPCANStatus stsResult;
            CANMsgWrite3 = new TPCANMsg();
            CANMsgWrite3.LEN = 8;
            CANMsgWrite3.DATA = new byte[8];
            CANMsgWrite3.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_EXTENDED;
            CANMsgWrite3.ID = ID;
            CANMsgWrite3.DATA = msg;

            stsResult = PCANBasic.GetStatus(m_PcanHandle);
            if (stsResult == TPCANStatus.PCAN_ERROR_QXMTFULL)
            {
                MessageBox.Show("PCAN_ERROR_ISSUE FROM: SSPC_CTRL STATUS");
                xmt_error = false;             
            }
            else
            {
                stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgWrite3);
                if (stsResult != TPCANStatus.PCAN_ERROR_OK)
                {
                    if (stsResult == TPCANStatus.PCAN_ERROR_QXMTFULL)
                    {
                        //        PCANBasic.Reset(m_PcanHandle);
                        //         MessageBox.Show("channel reset");
                        //aTimer.Enabled = false;
                       // bTimer.Enabled = false;
                        //cTimer.Enabled = false;
                        //dTimer.Enabled = false;
                        // eTimer.Enabled = false;
                        //fTimer.Enabled = false;
                        MessageBox.Show(Convert.ToString(stsResult));
                        //xmt_error = false;
                    }
                    else
                    {
                       MessageBox.Show(Convert.ToString(stsResult));                       
                    }
                    stsResult = PCANBasic.Write(m_PcanHandle, ref CANMsgRead3);
                }
            }
        }

        private bool SSPC_feedback()
        {
            //  CANMsgRead.ID = 0;              //18EF07DC
            bool feedback_SSPC;
            TPCANStatus stsResult;
            CANMsgRead3.LEN = 8;
            TPCANTimestamp.micros = 185;
            TPCANTimestamp.millis = 158903185;
            TPCANTimestamp.millis_overflow = 0;
            System.Threading.Thread.Sleep(5);
            stsResult = PCANBasic.Read(m_PcanHandle, out CANMsgRead3, out TPCANTimestamp);

            if (stsResult != TPCANStatus.PCAN_ERROR_OK)
            {
                feedback_SSPC = false;
            }
            else
            {
                feedback_SSPC = true;
            }
            return feedback_SSPC;
        }

        public void secLftCont()
        {
            uint iD = ID1;
            byte[] writeMSG = new byte[8];
            writeMSG[0] = 0x01;
            writeMSG[1] = 0x7F;
            writeMSG[2] = 0x03;
            writeMSG[3] = 0x00;
            writeMSG[4] = 0x00;
            writeMSG[5] = 0x00;
            writeMSG[6] = 0x00;
            writeMSG[7] = 0x00;


            if (inComingData[3, 3] == 0)
            {
                writeMSG[3] = 0xFD;
                SSPC_Ctrl(writeMSG, iD);
            }

            if (inComingData[3, 3] != 0)
            {
                writeMSG[3] = 0xFC;
                SSPC_Ctrl(writeMSG, iD);
                byte[] shutoff_loads = { 0x08, 0x09, 0x0A, 0x0B, 0x05};
                load_Management(shutoff_loads, 4, 0xFC, 1);
            }
        }

        public void secRghtCont()
        {
            uint iD = ID2;
            byte[] writeMSG = new byte[8];
            writeMSG[0] = 0x01;
            writeMSG[1] = 0x7F;
            writeMSG[2] = 0x03;
            writeMSG[3] = 0x00;
            writeMSG[4] = 0x00;
            writeMSG[5] = 0x00;
            writeMSG[6] = 0x00;
            writeMSG[7] = 0x00;

            if (inComingData2[3, 3] == 0)
            {
                writeMSG[3] = 0xFD;
                SSPC_Ctrl(writeMSG, iD);
            }
            if (inComingData2[3, 3] != 0)
            {
                writeMSG[3] = 0xFC;
                SSPC_Ctrl(writeMSG, iD);
                byte[] shutoff_loads = { 0x08, 0x09, 0x0A, 0x0B, 0x05 };
                load_Management(shutoff_loads, 4, 0xFC, 2);
            }
            
        }

        public void appClosing()
        {
            try
            {
                aTimer.Enabled = false;
                bTimer.Enabled = false;
                cTimer.Enabled = false;
                dTimer.Enabled = false;              
                fTimer.Enabled = false;
            }
            catch (System.NullReferenceException)
            {
            }
            PCANBasic.Uninitialize(m_PcanHandle);
        }

        private void system_Exit(byte error_type)
        {
            switch (error_type)
            {
                case 0x00:
                    MessageBox.Show("Error Connecting to CAN Port");
                    break;

                case 0x01:
                    MessageBox.Show("Left SSPC powered off");
                    break;

                case 0x02:
                    MessageBox.Show("Right SSPC powered off");
                    break;

                case 0x03:
                    MessageBox.Show("Error writing message to device");
                    break;
                default:
                    break;
            }
        }

        public void leftChannel_sw(byte chnl_num, bool onOff)
        {
            uint id = ID1;
            byte[] writeMSG = new byte[8];
            writeMSG[0] = 0x01;
            writeMSG[1] = 0x7F;
            writeMSG[2] = chnl_num;
            writeMSG[3] = 0xFD;
            writeMSG[4] = 0x00;
            writeMSG[5] = 0x00;
            writeMSG[6] = 0x00;
            writeMSG[7] = 0x00;
            if (onOff == true)
            {
                writeMSG[3] = 0xFD;
            }

            if (onOff == false)
            {
                writeMSG[3] = 0xFC;
            }

            SSPC_Ctrl(writeMSG, id);
        }

        public void rightChannel_sw(byte chnl_num, bool onOff)
        {
            uint id = ID2;
            byte[] writeMSG = new byte[8];
            writeMSG[0] = 0x01;
            writeMSG[1] = 0x7F;
            writeMSG[2] = chnl_num;
            writeMSG[3] = 0xFD;
            writeMSG[4] = 0x00;
            writeMSG[5] = 0x00;
            writeMSG[6] = 0x00;
            writeMSG[7] = 0x00;
            if (onOff == true)
            {
                writeMSG[3] = 0xFD;
            }

            if (onOff == false)
            {
                writeMSG[3] = 0xFC;
            }
            SSPC_Ctrl(writeMSG, id);
        }

        public void load_Management(byte[] chnls, byte numOFchnls, byte onOff, byte devId)
        {
            uint id1 = ID1;
            uint id2 = ID2;
            byte decNumOfChnls = numOFchnls;
            byte[] CHNls = new byte[decNumOfChnls];
            CHNls = chnls;

            byte[] writeMSG = new byte[8];
            while(decNumOfChnls != 255)
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = CHNls[decNumOfChnls];
                writeMSG[3] = onOff;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;

                if (devId == 1)
                {
                    SSPC_Ctrl(writeMSG, id1);
                }
                
                if (devId == 2)
                {
                    SSPC_Ctrl(writeMSG, id2);
                }

                if (devId == 3)
                {
                    SSPC_Ctrl(writeMSG, id1);
                    SSPC_Ctrl(writeMSG, id2);
                }
                
                decNumOfChnls--;
            }
            
        }

        private void generator_failure(uint ID)
        {
            byte[] writeMSG = new byte[8];
            byte[] chnls = { 0x02, 0x00, 0x03};
            byte chnlInc = 0;

            while (chnlInc < 3)
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = chnls[chnlInc];
                writeMSG[3] = 0xFC;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                SSPC_Ctrl(writeMSG, ID);
                chnlInc++;
            }
        }

        private void busTIE_on(uint ID)
        {
            byte[] writeMSG = new byte[8];
            byte[] chnls = { 0x01, 0x00, 0x03, 0x08, 0x05, 0x0B };
            byte chnlInc = 0;
            while (chnlInc < 6)
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = chnls[chnlInc];
                writeMSG[3] = 0xFD;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                SSPC_Ctrl(writeMSG, ID);
                chnlInc++;
            }
        }

        private void generator_back(uint ID)
        {
            byte[] writeMSG = new byte[8];

            byte[] chnls = { 0x02, 0x00, 0x03 };
            byte chnlInc = 0;
            while (chnlInc < 3)
            {
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = chnls[chnlInc];
                writeMSG[3] = 0xFD;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                SSPC_Ctrl(writeMSG, ID);
                chnlInc++;
            }
            writeMSG[2] = 0x01;
            writeMSG[3] = 0x0FC;
            SSPC_Ctrl(writeMSG, ID);
        }

        public async void autoMatic_leftGen(bool leftGen, bool rightGen)
        {
            if ((leftGen == false) && (rightGen == true))
            {
                generator_failure(ID1);
                await waitMethod();
                busTIE_on(ID1);

                MessageBox.Show("LEFT SIDE GENERATOR FAILED - AUTOMATIC BUSTIE CONTACTOR TURN ON INITIATED");
            }
            if ((leftGen == false) && (rightGen == false))
            {
                byte[] writeMSG = new byte[8];
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = 0x01;
                writeMSG[3] = 0xFC;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                generator_failure(ID1);
                SSPC_Ctrl(writeMSG, ID1);
                generator_failure(ID2);
                SSPC_Ctrl(writeMSG, ID2);
            }
            if (leftGen == true) 
            {
                await waitMethod();
                generator_back(ID1);
                MessageBox.Show("LEFT SIDE GENERATOR BACK - BUS TIE SHUTS OFF");
            }
        }

        public async void autoMatic_rightGen(bool rightGen, bool leftGn)
        {
            if ((rightGen == false) && (leftGn == true))
            {
                generator_failure(ID2);
                await waitMethod();
                busTIE_on(ID2);
                MessageBox.Show("RIGHT SIDE GENERATOR FAILED - AUTOMATIC BUSTIE CONTACTOR TURN ON INITIATED");
            }
            if ((rightGen == false) && (leftGn == false))
            {
                byte[] writeMSG = new byte[8];
                writeMSG[0] = 0x01;
                writeMSG[1] = 0x7F;
                writeMSG[2] = 0x01;
                writeMSG[3] = 0xFC;
                writeMSG[4] = 0x00;
                writeMSG[5] = 0x00;
                writeMSG[6] = 0x00;
                writeMSG[7] = 0x00;
                generator_failure(ID2);
                SSPC_Ctrl(writeMSG, ID2);
                generator_failure(ID1);
                SSPC_Ctrl(writeMSG, ID1);
            }
            if (rightGen == true)
            {
                await waitMethod();
                generator_back(ID2);
                MessageBox.Show("RIGHT SIDE GENERATOR BACK - BUS TIE SHUTS OFF");
            }
        }

        public void manual_LeftGen(bool onOFf)
        {
            if (onOFf == false)
            {
                generator_failure(ID1);
                MessageBox.Show("LEFT SIDE GENERATOR FAILED. TURN BUS TIE ON");
            }
            else
            {
                generator_back(ID1);
                MessageBox.Show("LEFT GENERATOR BACK");
            }
        }

        public void Left_busContactor()
        {
            busTIE_on(ID1);
        }

        public void Right_busContactor()
        {
            busTIE_on(ID2);
        }
       
        public void manual_RightGen(bool onOFf)
        {
            if (onOFf == false)
            {
                generator_failure(ID2);
                MessageBox.Show("RIGHT SIDE GENERATOR FAILED. TURN BUS TIE ON");
            }
            else
            {
                generator_back(ID2);
                MessageBox.Show("RIGHT GENERATOR BACK");
            }
        }

        private void Suspend_timers()
        {
            aTimer.Enabled = false;
            bTimer.Enabled = false;
            cTimer.Enabled = false;
            dTimer.Enabled = false;        
            fTimer.Enabled = false;
        }

        private void start_timers()
        {
            aTimer.Enabled = true;
            bTimer.Enabled = true;
            cTimer.Enabled = true;
            dTimer.Enabled = true;
            fTimer.Enabled = true;
        }
    }
}
 




      




