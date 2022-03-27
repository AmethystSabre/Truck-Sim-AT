using Ets2SdkClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETS2_DualSenseAT_Mod
{
    public partial class Form1 : Form
    {
        public Ets2SdkTelemetry Telemetry;

        public Form1()
        {
            InitializeComponent();

            Telemetry = new Ets2SdkTelemetry();
            Telemetry.Data += Telemetry_Data;

            Telemetry.JobFinished += TelemetryOnJobFinished;
            Telemetry.JobStarted += TelemetryOnJobStarted;

            if (Telemetry.Error != null)
            {
                statusLbl.Text = "Status: " + Telemetry.Error.Message;
            }

            if (!File.Exists(Application.StartupPath + "\\DualSenseX_CommandLineArgs.bat"))
            {
                statusLbl.Text = "Status: DualSenseX Batch file not found!";
            }
            else
            {
                Process.Start(Application.StartupPath + "\\DualSenseX_CommandLineArgs.bat");
                statusLbl.Text = "Status: Ready!";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("eurotrucks2");
            if (pname.Length == 0)
            {
                MessageBox.Show("Euro Truck Simulator 2 is not running, please open game first!", "DualSense AT Mod");
                Application.Exit();
            }

            if (!File.Exists(Application.StartupPath + "\\DualSenseX_CommandLineArgs.bat"))
            {
                MessageBox.Show("DualSenseX Command Line not found.", "DualSense AT Mod");
                Application.Exit();
            }

        }

        private void TelemetryOnJobFinished(object sender, EventArgs args)
        {
            //MessageBox.Show("Job finished, or at least unloaded nearby cargo destination.");
            //JobLbl.Text = "Job Status: Finished!";
        }

        private void TelemetryOnJobStarted(object sender, EventArgs e)
        {
            //MessageBox.Show("Just started job OR loaded game with active.");
            //JobLbl.Text = "Job Status: Started!";
        }

        private void Telemetry_Data(Ets2Telemetry data, bool updated)
        {


            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new TelemetryData(Telemetry_Data), new object[2] { data, updated });
                    return;
                }



                if (data.Paused)
                {
                    Controller.WriteController.ResetTrigger(Controller.Triggers.LeftTrigger);
                    Controller.WriteController.ResetTrigger(Controller.Triggers.RightTrigger);
                    statusLbl.Text = "Status: Game paused.";
                    return;
                }
                else
                {
                    //Here's our algorithm that changes how the triggers work according to how fast the truck is

                    if (data.Drivetrain.SpeedKmh < 1)
                    {
                        Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(8)");
                        Controller.WriteController.SetLeftTrigger(Controller.Types.Normal, "(0)(0)(0)(0)(0)(0)"); //0,9,4,3,19,2

                    }
                    else if (data.Drivetrain.SpeedKmh < 5)
                    {
                        Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(7)");
                        Controller.WriteController.SetLeftTrigger(Controller.Types.Normal, "(0)(0)(0)(0)(0)(0)"); //0,9,4,3,19,2

                    }

                    else if (data.Drivetrain.SpeedKmh < 10)
                    {
                        Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(7)");
                        Controller.WriteController.SetLeftTrigger(Controller.Types.Machine, "(0)(9)(4)(3)(19)(2)");

                    }

                    else if (data.Drivetrain.SpeedKmh < 15)
                    {
                        Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(6)");
                        Controller.WriteController.SetLeftTrigger(Controller.Types.Machine, "(0)(9)(4)(3)(19)(2)");
                    }
                    else if (data.Drivetrain.SpeedKmh < 20)
                    {
                        Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(5)");
                        Controller.WriteController.SetLeftTrigger(Controller.Types.Machine, "(0)(9)(4)(3)(19)(2)");
                    }
                    else if (data.Drivetrain.SpeedKmh < 25)
                    {
                        Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(4)");
                        Controller.WriteController.SetLeftTrigger(Controller.Types.Machine, "(0)(9)(4)(3)(19)(2)");
                    }
                    else if (data.Drivetrain.SpeedKmh < 30)
                    {
                        Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(3)");
                        Controller.WriteController.SetLeftTrigger(Controller.Types.Machine, "(0)(9)(4)(3)(19)(2)");
                    }
                    else if (data.Drivetrain.SpeedKmh < 35)
                    {
                        Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(2)");
                        Controller.WriteController.SetRightTrigger(Controller.Types.Normal);
                    }

                    else if (data.Drivetrain.SpeedKmh < 40)
                    {
                        Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(1)");
                        Controller.WriteController.SetRightTrigger(Controller.Types.Normal);
                    }
                }
            }
            catch
            {
            }
        }

        public struct Controller
        {
            public enum Types
            {
                Normal = 1,
                Hard = 2,
                VeryHard = 3,
                Machine = 4,
                Resistance = 5,
            }
            public enum Triggers
            {
                LeftTrigger = 1,
                RightTrigger = 2,
            }

            public struct WriteController
            {
                public static void SetRightTrigger(Controller.Types type, string customargs = "(0)(0)")
                {
                    var MyIni = new IniFile(@"C:\DualsenseX_GameTriggers.txt");

                    switch (type)
                    {
                        case Controller.Types.Normal:
                            MyIni.Write("RightTrigger", "Normal");
                            break;
                        case Controller.Types.Hard:
                            MyIni.Write("RightTrigger", "Hard");
                            break;
                        case Controller.Types.VeryHard:
                            MyIni.Write("RightTrigger", "VeryHard");
                            break;
                        case Controller.Types.Resistance:
                            MyIni.Write("RightTrigger", "Resistance");
                            MyIni.Write("ForceRightTrigger", customargs);
                            break;
                    }
                }

                public static void SetLeftTrigger(Controller.Types type, string customargs = "(0)(0)")
                {
                    var MyIni = new IniFile(@"C:\DualsenseX_GameTriggers.txt");

                    switch (type)
                    {
                        case Controller.Types.Normal:
                            MyIni.Write("LeftTrigger", "Normal");
                            break;
                        case Controller.Types.Hard:
                            MyIni.Write("LeftTrigger", "Hard");
                            break;
                        case Controller.Types.VeryHard:
                            MyIni.Write("LeftTrigger", "VeryHard");
                            break;
                        case Controller.Types.Machine:
                            MyIni.Write("LeftTrigger", "Machine");
                            MyIni.Write("ForceLeftTrigger", customargs);
                            break;
                    }
                }

                public static void ResetTrigger(Controller.Triggers trigger)
                {
                    var MyIni = new IniFile(@"C:\DualsenseX_GameTriggers.txt");
                    switch (trigger)
                    {
                        case Controller.Triggers.LeftTrigger:
                            MyIni.Write("LeftTrigger", "Normal");
                            break;
                        case Controller.Triggers.RightTrigger:
                            MyIni.Write("RightTrigger", "Normal");
                            break;
                    }
                }
            }
        }
    }
}
