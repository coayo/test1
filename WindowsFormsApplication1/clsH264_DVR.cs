using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public delegate bool CallBack(int loginID, ref string DeviceIP, long portNumber, int usersData);
    public delegate bool fMessCallBack(out int loginID, out string data_alarm_info, out uint data_lenght, out int function_parameters, out int type_alarm);


    namespace WindowsFormsApplication1
{
    public class clsH264_DVR
        {
            #region DEFINICION DE ESTRUCTURAS
            [StructLayout(LayoutKind.Explicit, Size = 32, CharSet = CharSet.Ansi)]
            public struct SDK_SYSTEM_TIME
            {
                [FieldOffset(0)] public int year;       ///< Year。  
                [FieldOffset(4)] public int month;     ///< Month，January = 1, February = 2, and so on.   
                [FieldOffset(8)] public int day;       ///< Day。  
                [FieldOffset(12)] public int wday;     ///< Weekday，Sunday = 0, Monday = 1, and so on   
                [FieldOffset(16)] public int hour;     ///< Hour。  
                [FieldOffset(20)] public int minute;   ///< Minute。  
                [FieldOffset(24)] public int second;   ///< Second。  
                [FieldOffset(28)] public int isdst;
            }
            // [StructLayout (LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            enum SocketStyle { TCPSOCKET, UDPSOCKET, SOCKETNR };

            public struct H264_DVR_DEVICEINFO
            {
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string SoftWareVersion;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string HardWareVersion;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string EncryptVersion;
                public SDK_SYSTEM_TIME tmBuildTime;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string SerialNumber;
                public int inChannelNumber;
                public int outChannelNumber;
                public int inChannelAlarmNumber;
                public int outChannelAlarmNumber;
                public int inTalkInChannel;
                public int inTalkOutChannel;
                public int inExtraChannel;
                public int inAudioChannel;
                public int iCombineSwitch;
                public int inDigitalChannel;
                public uint uiDeviceRunTime;
            }

            /*  internal long Play(ref IntPtr hndle)
              {
                  throw new NotImplementedException();
              }
              */
            public struct cbDisConnect
            {
                public long LoginId;
                public string DeviceIP;
                public long PortNumber;
                public ulong UsersData;
            }

            public struct H264_DVR_CLIENTINFO
            {
                public Int32 nChannel;
                public Int32 nStream;
                public Int32 nMode;
                public Int32 nComType;
                public Int32 hWnd;
            }
            #endregion



            #region VARIABLES DE LA CLASE 
            public const int NET_MAX_PATH_LENGTH = 260;
            public H264_DVR_DEVICEINFO Device_Info;
            private int ErrorId = 0;
            public Boolean Error = false;
            public string ErrorMessage = "";
            public Int32 LoginID;
            H264_DVR_CLIENTINFO cinfo;
            public IntPtr videotoshow;
            #endregion

            #region // DECLARACION DE METODOS DE LAS DLL
            [DllImport("NetSdk.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "H264_DVR_Init", ExactSpelling = true)]
            private static extern Boolean H264_DVR_Init(CallBack x, int y);
            [DllImport("NetSdk.dll")]
            private static extern bool H264_DVR_SetConnectTime(int nWaitTime, int nTryTimes);
            /*[DllImport("NetSdk.dll")]
            private static extern Int32 H264_DVR_GetLastError();*/
            [DllImport("NetSdk.dll")]
            private static extern bool H264_DVR_SetDVRMessCallBack(fMessCallBack CallBackStatusDevice, uint UserData);

            [DllImport("NetSdk.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "H264_DVR_Login", ExactSpelling = true)]
            private static extern Int32 H264_DVR_Login([MarshalAs(UnmanagedType.LPStr)] string device_ip,
                                                       UInt16 device_port,
                                                      [MarshalAs(UnmanagedType.LPStr)] string user_name,
                                                      [MarshalAs(UnmanagedType.LPStr)] string user_pass,
                                                      out H264_DVR_DEVICEINFO device_info, out int error_code_login, int socket_type);

            [DllImport("NetSdk.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "H264_DVR_Logout", ExactSpelling = true)]
            public static extern Boolean H264_DVR_Logout(Int32 iLogonID);

            [DllImport("NetSdk.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "H264_DVR_Cleanup", ExactSpelling = true)]
            public static extern Boolean H264_DVR_Cleanup();

            [DllImport("NetSdk.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "H264_DVR_GetLastError", ExactSpelling = true)]
            public static extern Int32 H264_DVR_GetLastError();

            [DllImport("NetSdk.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "H264_DVR_RealPlay", ExactSpelling = true)]
            private static extern Int64 H264_DVR_RealPlay(Int32 Login_ID, ref H264_DVR_CLIENTINFO cliente_info);

            public Int64 Play(ref IntPtr controlhandle)
            {
                int chanel, mode, stream, handle, comtype;
                chanel = 0; mode = 0; stream = 0; handle = 0; comtype = 0;
                cinfo.nChannel = chanel;
                cinfo.nStream = stream;
                cinfo.nMode = mode;
                cinfo.nComType = comtype;
                cinfo.hWnd = controlhandle.ToInt32();
                Int64 result = H264_DVR_RealPlay(LoginID, ref cinfo);
                return result;
            }

            /*       3.1.2 Release SDK resources H264_DVR_Cleanup
           Function:  H264_DVR_API bool CALL_METHOD H264_DVR_Cleanup();
                   Parameter:  None
                   Returned value: TRUE refers to success, FALSE refers to failure

                       Declaration: Empty SDK, release occupied resources, before every SDK function has been called back.If return failed,
                   please call H264_DVR_GetLastError function to get error code, and find reason of error by code

           */

            /*    3.3.2 User logout device H264_DVR_Logout
        Function:	H264_DVR_API long CALL_METHOD H264_DVR_Logout(
        long lLoginID
        );
                Parameter:	[in] lLoginID Login handle
          Returned value:	
                    1refers to success,
                    0 refers to failure.
          Declaration:*/
            #endregion
            //CONSTRUCTOR
            public clsH264_DVR()
            {
                CallBack miCallback = new CallBack(Report);
                unsafe
                {
                    Boolean hrpta = H264_DVR_Init(miCallback, 0);
                    int error_code = H264_DVR_GetLastError();
                    if (hrpta)
                    {
                        if (!H264_DVR_SetConnectTime(1000, 5)) Generar_error();
                    }
                    else
                    {
                        Generar_error();// -1);
                                        //Destructor();
                    }
                }
            } // *************     END  DEL CONSTRUCTOR DE CLASE DVR     ***************

            ///*************** funciones de la clase clsH264_DVR   ****************
            //// fcn de callback de Init()
            public bool Report(int loginID, ref string DeviceIP, long portNumber, int usersData)
            { // MessageBox.Show("hola  " + loginID.ToString());
                return true;
            }////  END FCN Report

            ///  fcn Connect que incluye Init()
            public Boolean Conect(string device_ip, UInt16 device_port, string user_name, string user_pass)
            {
                Boolean conectado;
                Int32 result = new Int32();
                unsafe
                {
                    result = H264_DVR_Login(device_ip, device_port, user_name, user_pass, out this.Device_Info, out this.ErrorId, 0);// SocketStyle.TCPSOCKET);
                    if (result > 0)
                    {
                        this.LoginID = result;
                        conectado = true;
                    }
                    else
                    {
                        conectado = false;
                        Generar_error();
                        MessageBox.Show("No Conectado,  " + this.ErrorMessage);
                        if (!H264_DVR_Cleanup())
                        {
                            Generar_error();
                        }
                        else
                        {
                            Generar_error();
                        }
                    }

                }
                return conectado;
            }

            public void disconect()
            {
                if (this.LoginID != 0)
                {
                    H264_DVR_Logout(this.LoginID);
                    if (H264_DVR_Cleanup())
                    {
                        Generar_error();
                    }
                    else
                    {
                        Generar_error();
                        MessageBox.Show("Problemas en la liberacion de Dll NetSdk");
                    }
                }
            }

            /// <summary>
            /// manejo del codigo, convierte codigo error en mensaje de error
            /// </summary>
            /// <param name="error_code"></param>
            private void Generar_error()
            {
                this.ErrorId = H264_DVR_GetLastError();
                Error = true;
                if (this.ErrorId == 0)
                {
                    ErrorMessage = "No encontrada la cámara";
                }
                else
                {
                    switch (this.ErrorId)
                    {
                        case 0:
                            this.ErrorMessage = "no error";
                            break;
                        case 1:
                            Error = false;
                            this.ErrorMessage = "success";
                            break;
                        case -1:
                            this.ErrorMessage = "Librería no inicializada";
                            break;
                        case -2:
                            this.ErrorMessage = "Camera folder es Null";
                            break;
                        case -3:
                            this.ErrorMessage = "Error en asignacion del Camera folder (Es un archivo)";
                            break;
                        case -10000:
                            this.ErrorMessage = "invalid request";
                            break;
                        case -10001:
                            this.ErrorMessage = "SDK not inited";
                            break;
                        case -10002:
                            this.ErrorMessage = "illegal user parameter";
                            break;
                        case -10003:
                            this.ErrorMessage = "handle is null";
                            break;
                        case -10004:
                            this.ErrorMessage = "SDK clear error";
                            break;
                        case -10005:
                            this.ErrorMessage = "timeout";
                            #region
                            /*                Case - 10006

                                                    Return "memory error"

                                            Case - 10007

                                                    Return "network error"

                                            Case - 10008

                                                    Return "open file fail"

                                            Case - 10009

                                                    Return "unknown error"

                                            Case - 11000

                                                    Return "version mismatch"

                                            Case - 11001

                                                    Return "get data failï (including configure, user information and etc)"

                                            Case - 11200

                                                    Return "open channel fail"

                                            Case - 11201

                                                    Return "close channel fail"

                                            Case - 11202

                                                    Return "open media connet fail"

                                            Case - 11203

                                                    Return "media connet send data fail"

                                            Case - 11300

                                                    Return "no power"

                                            Case - 11301:

                                                    Return "password not valid"

                                            Case - 11302:

                                                    Return "user not exist"

                                            Case - 11303:

                                                    Return "user is locked"

                                            Case - 11304:

                                                    Return "user is in backlist"

                                            Case - 11305:

                                                    Return "user have logined"

                                            Case - 11305:

                                                    Return "no login"

                                            Case - 11306:

                                                    Return "maybe device no exist"

                                            Case - 11400:

                                                    Return "need to restart"

                                            Case - 11401:

                                                    Return "need to reboot"

                                            Case - 11402:  

                                                Return "write file fail"

                                            Case - 11403:

                                                    Return "not support"

                                            Case - 11404:

                                                    Return "validate fail"

                                            Case - 11405:

                                                    Return "config not exist"

                                            Case - 11500:

                                                    Return "pause fail"

                                            Case - 11501:

                                                    Return "not found"

                                            Case - 11502:

                                                    Return "cfg not enable"

                                            default:
                                                    Return "Unknown error"*/
                            #endregion
                            break;
                    }
                }
                // ErrorId = error_code;
                //ErrorMessage = get_error_message(ErrorId);// hacer codigo de metodo...
                // log = log + error
            }


            ///////////////// funciones de prueba
            public int Gettime()
            {
                return this.Device_Info.tmBuildTime.year;
                /* int yr = 0;
                 yr = this.Device_Info.TmBuildTime.year;
                 return yr;*/
            }

            /*3.4.1 Real-time preview   H264_DVR_RealPlay
            Function:	H264_DVR_API long H264_DVR_RealPlay(long lLoginID, 
                                                            LPH264_DVR_CLIENTINFO lpClientInfo); 

            Parameter:	[in]lLoginID,  Returned value of H264_DVR_Login,  [in]lpClientInfo	Play handle
            Returned value:	Return failure less than or equal to 0, when less than 0, H264_DVR_GetLastError function can be used to acquire error type.
            If returned successfully, real-time monitor ID (real-time handle) will be taken as parameters of related function. 

                        Analysis of some error code: 
            (1)H264_DVR_SUB_CONNECT_ERROR = -11202: Failed to establish video sub-connection, device may not be online or in rebooting.
            Handling method: login after receiving disconnected callback.
            (2)H264_DVR_SUB_CONNECT_SEND_ERROR = -11203:
            a. LAN access: Sub-connection communication failed, that is the sub-connection was established successfully, but the communication failed,
            and the device was disconnected after sub-connection was established successfully. Handling method: the return value of (1) 
            will appear when H264_DVR_RealPlay is called again.
            b. Active registeration access: Main connection communication failed, the device was disconnected, and interior SDK has received 
            disconnected callback. Handling method: Logout after receiving the disconnected callback.
            (3)H264_DVR_NOTVALID=-11206: Illegal error, main connection disconnected, the device has been disconnected, and the reboot succeed,
            but no disconnected callback has been received, and the previous login handle is still in use. How to deal with it: logout after receiving
            the wire break callback. 
            Declaration:		Call this interface, according to the obtained device information when login, and then you can turn on any 
            effective real-time monitoring in one channel, and obtain raw data by H264_DVR_SetRealDataCallBack  of device
            callback (note: if playing can be completed by assigning value to  hWnd in lpClientInfo,  assignment can be completed, 
            without the need for decoding broadcast to the callback data). Successfully return to real-time monitoring ID, in order to monitor
            the following video channels. */

            [DllImport("user32.dll", EntryPoint = "MessageBox")]
            public static extern int MessageBox3(int hWnd, string lpText, string lpCaption, uint uType);

            public void msg()
            {
                string h = "mensaje prueba";
                // IntPtr hw = *h;

                string lptext = "lptext mensaje";
                string caption = "lpcaptopm";
                uint w = 0;
                Int32 eaa = MessageBox3(1, lptext, caption, w);

            }

            /*   public int Login2(string device_ip, int device_port, string user_name, string user_pass, out H264_DVR_DEVICEINFO device_info, out int error_code_login, SocketStyle sock_style);
               {
               int y = 0;
               return y;
               }*/
            /// fcn de callback de alarma
            /*   bool CallBackStatusDevice(out int loginID, out string data_alarm_info, out uint data_lenght, out int function_parameters, out int type_alarm);
               {
                   Boolean resultado = true;


                   return resultado;
               }*///////  END FCN  //////

        }///*********     END CLASE DVR    ******** //////////
    }
