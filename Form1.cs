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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shared;
using System.Threading;

namespace ETS2_DualSenseAT_Mod
{
    public partial class Form1 : Form
    {

        static UdpClient client;
        static IPEndPoint endPoint;
        static bool Connect()
        {
            try
            {
                client = new UdpClient();
                var portNumber = File.ReadAllText(@"C:\Temp\DualSenseX\DualSenseX_PortNumber.txt");
                endPoint = new IPEndPoint(Triggers.localhost, Convert.ToInt32(portNumber));
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        static void Send(Packet data)
        {
            var RequestData = Encoding.ASCII.GetBytes(Triggers.PacketToJson(data));
            client.Send(RequestData, RequestData.Length, endPoint);
        }

        public Ets2SdkTelemetry Telemetry;
        static int speed_limit_led_step = 0;
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

            //if (!File.Exists(Application.StartupPath + "\\DualSenseX_CommandLineArgs.bat"))
            //{
            //    statusLbl.Text = "Status: DualSenseX Batch file not found!";
            //}
            //else
            //{
            //    //Process.Start(Application.StartupPath + "\\DualSenseX_CommandLineArgs.bat");
            //    statusLbl.Text = "Status: Ready!";
            //}
            //Substr
            statusLbl.Text = "Status: Ready!";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Process[] pname = Process.GetProcessesByName("eurotrucks2");
            if (pname.Length == 0)
            {
                MessageBox.Show("Euro Truck Simulator 2 is not running, please open game first!", "DualSense AT Mod");
                Application.Exit();
            }

            //if (!File.Exists(Application.StartupPath + "\\DualSenseX_CommandLineArgs.bat"))
            //{
            //    MessageBox.Show("DualSenseX Command Line not found.", "DualSense AT Mod");
            //    Application.Exit();
            //}

            if (!Connect())
            {
                MessageBox.Show("Failed to connect to the DSX UDP Server ("+ Triggers.localhost, Convert.ToInt32(File.ReadAllText(@"C:\Temp\DualSenseX\DualSenseX_PortNumber.txt")) + ")");
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
            //Thread.Sleep(350);

            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new TelemetryData(Telemetry_Data), new object[2] { data, updated });
                    return;
                }

                Packet p = new Packet();

                int controllerIndex = 0;

                int inst_index = 0;

                p.instructions = new Instruction[10];



                if (data.Paused)
                {
                    p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                    p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Normal };

                    inst_index += +1;

                    p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                    p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Normal };

                    inst_index += +1;

                    p.instructions[inst_index].type = InstructionType.RGBUpdate;
                    p.instructions[inst_index].parameters = new object[] { controllerIndex, 252, 186, 3 };
                    inst_index += +1;
                    Send(p);
                    statusLbl.Text = "Status: Game paused.";
                    return;
                }
                else
                {
                    statusLbl.Text = "Status: Running.";
                    //Here's our algorithm that changes how the triggers work according to how fast the truck is

                    if (data.Drivetrain.SpeedKmh < 1)
                    {
                        if (AcceleratorFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Machine, 0, 9, 3, 3, 10, 2 };
                            inst_index += +1;
                        }
                        if (BrakeFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Normal };
                            inst_index += +1;
                        }

                        Send(p);
                        //return;
                    }
                    else if (data.Drivetrain.SpeedKmh < 5)
                    {
                        if (AcceleratorFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Machine, 0, 9, 3, 3, 10, 2 };
                            inst_index += +1;
                        }
                        if (BrakeFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Normal, 0, 0, 0, 0, 0, 0 };
                            inst_index += +1;
                        }
                        // Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(7)");
                        //Controller.WriteController.SetLeftTrigger(Controller.Types.Normal, "(0)(0)(0)(0)(0)(0)"); //0,9,4,3,19,2
                        Send(p);
                    }

                    else if (data.Drivetrain.SpeedKmh < 10)
                    {
                        if (AcceleratorFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Machine, 0, 9, 3, 3, 20, 2 };
                            inst_index += +1;
                        }
                        if (BrakeFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Machine, 0, 9, 4, 3, 19, 2 };
                            inst_index += +1;
                        }
                        // Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(7)");
                        //Controller.WriteController.SetLeftTrigger(Controller.Types.Machine, "(0)(9)(4)(3)(19)(2)");
                        Send(p);
                    }

                    else if (data.Drivetrain.SpeedKmh < 15)
                    {
                        if (AcceleratorFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Machine, 0, 9, 3, 3, 30, 2 };
                            inst_index += +1;
                        }
                        if (BrakeFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Machine, 0, 9, 4, 3, 19, 2 };
                            inst_index += +1;
                        }
                        //Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(6)");
                        //Controller.WriteController.SetLeftTrigger(Controller.Types.Machine, "(0)(9)(4)(3)(19)(2)");
                        Send(p);
                    }
                    else if (data.Drivetrain.SpeedKmh < 20)
                    {
                        if (AcceleratorFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Machine, 0, 9, 3, 3, 30, 2 };
                            inst_index += +1;
                        }
                        if (BrakeFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Machine, 0, 9, 4, 3, 19, 2 };
                            inst_index += +1;
                        }
                        //Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(5)");
                        //Controller.WriteController.SetLeftTrigger(Controller.Types.Machine, "(0)(9)(4)(3)(19)(2)");
                        Send(p);
                    }
                    else if (data.Drivetrain.SpeedKmh < 25)
                    {
                        if (AcceleratorFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Resistance, 0, 4 };
                            inst_index += +1;
                        }
                        if (BrakeFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Machine, 0, 9, 4, 3, 19, 2 };
                            inst_index += +1;
                        }
                        // Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(4)");
                        //Controller.WriteController.SetLeftTrigger(Controller.Types.Machine, "(0)(9)(4)(3)(19)(2)");
                        Send(p);
                    }
                    else if (data.Drivetrain.SpeedKmh < 30)
                    {
                        if (AcceleratorFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Resistance, 0, 3 };
                            inst_index += +1;
                        }
                        if (BrakeFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Machine, 0, 9, 4, 3, 19, 2 };
                            inst_index += +1;
                        }
                        //Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(3)");
                        //Controller.WriteController.SetLeftTrigger(Controller.Types.Machine, "(0)(9)(4)(3)(19)(2)");
                        Send(p);
                    }
                    else if (data.Drivetrain.SpeedKmh < 35)
                    {
                        if (AcceleratorFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Resistance, 0, 2 };
                            inst_index += +1;
                        }
                        if (BrakeFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Machine, 0, 9, 4, 3, 19, 2 };
                            inst_index += +1;
                        }
                        //Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(2)");
                        //Controller.WriteController.SetRightTrigger(Controller.Types.Normal);
                        Send(p);
                    }

                    else if (data.Drivetrain.SpeedKmh < 40)
                    {
                        if (AcceleratorFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Resistance, 0, 1 };
                            inst_index += +1;
                        }
                        if (BrakeFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Machine, 0, 9, 4, 3, 19, 2 };
                            inst_index += +1;
                        }
                        // Controller.WriteController.SetRightTrigger(Controller.Types.Resistance, "(0)(1)");
                        //Controller.WriteController.SetRightTrigger(Controller.Types.Normal);
                        Send(p);
                    }
                    else if (data.Drivetrain.SpeedKmh > 41)
                    {
                        if (AcceleratorFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Resistance, 4, 2 };
                            inst_index += +1;
                        }
                        //AcceleratorBreak
                        if (BrakeFeedback.Checked)
                        {
                            p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Machine, 0, 9, 4, 3, 19, 2 };
                            inst_index += +1;
                        }
                        Send(p);
                    }

                    if (data.Drivetrain.Speed > data.Job.SpeedLimit)
                    {
                        if (LedAlarmsOnSpeedLimit.Checked)
                        {
                            if (speed_limit_led_step == 0)
                            {
                                p.instructions[inst_index].type = InstructionType.RGBUpdate;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, 237, 61, 7 };
                                inst_index += +1;
                                // PLAYER LED 1-5 true/false state
                                p.instructions[inst_index].type = InstructionType.PlayerLED;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, true, false, false, false, false };
                                inst_index += +1;
                                speed_limit_led_step = 1;
                            }
                            else if (speed_limit_led_step == 1)
                            {
                                p.instructions[inst_index].type = InstructionType.RGBUpdate;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, 252, 0, 0 };
                                inst_index += +1;
                                // PLAYER LED 1-5 true/false state
                                p.instructions[inst_index].type = InstructionType.PlayerLED;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, false, true, false, false, false };
                                inst_index += +1;
                                speed_limit_led_step = 2;
                            }
                            else if (speed_limit_led_step == 2)
                            {
                                p.instructions[inst_index].type = InstructionType.RGBUpdate;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, 148, 22, 0 };
                                inst_index += +1;
                                // PLAYER LED 1-5 true/false state
                                p.instructions[inst_index].type = InstructionType.PlayerLED;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, false, false, true, false, false };
                                inst_index += +1;

                                speed_limit_led_step = 3;
                            }
                            else if (speed_limit_led_step == 3)
                            {
                                p.instructions[inst_index].type = InstructionType.RGBUpdate;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, 237, 61, 7 };
                                inst_index += +1;
                                // PLAYER LED 1-5 true/false state
                                p.instructions[inst_index].type = InstructionType.PlayerLED;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, false, false, false, true, false };
                                inst_index += +1;

                                speed_limit_led_step = 4;
                            }
                            else if (speed_limit_led_step == 4)
                            {
                                p.instructions[inst_index].type = InstructionType.RGBUpdate;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, 148, 22, 0 };
                                inst_index += +1;
                                // PLAYER LED 1-5 true/false state
                                p.instructions[inst_index].type = InstructionType.PlayerLED;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, false, false, false, false, true };
                                inst_index += +1;

                                speed_limit_led_step = 0;
                            }
                        }
                    }
                    else
                    {
                        p.instructions[inst_index].type = InstructionType.RGBUpdate;
                        p.instructions[inst_index].parameters = new object[] { controllerIndex, 119, 3, 252 };
                        inst_index += +1;

                        // PLAYER LED 1-5 true/false state
                        //p.instructions[inst_index].type = InstructionType.PlayerLED;
                        //p.instructions[inst_index].parameters = new object[] { controllerIndex, true, false, false, false, true };
                        //inst_index += +1;
                    }

                    if (!AcceleratorFeedback.Checked)
                    {
                        p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                        p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Normal };
                        inst_index += +1;
                    }
                    if (!BrakeFeedback.Checked)
                    {
                        p.instructions[inst_index].type = InstructionType.TriggerUpdate;
                        p.instructions[inst_index].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Normal };
                        inst_index += +1;
                    }


                    if (TruckLightsIndicator.Checked)
                    {
                        if (data.Drivetrain.Speed < data.Job.SpeedLimit)
                        {
                            if (data.Lights.HighBeams)
                            {
                                // PLAYER LED 1-5 true/false state
                                p.instructions[inst_index].type = InstructionType.PlayerLED;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, true, false, false, false, true };
                                inst_index += +1;
                                //label1.Text = "HighBeams";
                            }
                            else if (data.Lights.LowBeams)
                            {
                                // label1.Text = "LowBeams";
                                // PLAYER LED 1-5 true/false state
                                p.instructions[inst_index].type = InstructionType.PlayerLED;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, false, true, false, true, false };
                                inst_index += +1;
                            }
                            else
                            {
                                // label1.Text = "OFF LIGHTS";
                                p.instructions[inst_index].type = InstructionType.PlayerLED;
                                p.instructions[inst_index].parameters = new object[] { controllerIndex, false, false, false, false, false };
                                inst_index += +1;
                            }
                        }
                    }
                    else
                    {
                        p.instructions[inst_index].type = InstructionType.PlayerLED;
                        p.instructions[inst_index].parameters = new object[] { controllerIndex, false, false, false, false, false };
                        inst_index += +1;
                    }

                    //Show Red Lights on LEDs on Breake
                    if (BrakeRedLights.Checked)
                    {
                        if (data.Lights.BrakeLight)
                        {
                            p.instructions[inst_index].type = InstructionType.RGBUpdate;
                            p.instructions[inst_index].parameters = new object[] { controllerIndex, 252, 3, 3 };
                            inst_index += +1;
                        }
                    }

                    Send(p);
                }
            }
            catch
            {
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Packet p = new Packet();

            int controllerIndex = 0;

            p.instructions = new Instruction[4];

            p.instructions[0].type = InstructionType.TriggerUpdate;
            p.instructions[0].parameters = new object[] { controllerIndex, Trigger.Right, TriggerMode.Normal };


            p.instructions[1].type = InstructionType.TriggerUpdate;
            p.instructions[1].parameters = new object[] { controllerIndex, Trigger.Left, TriggerMode.Normal };


            p.instructions[2].type = InstructionType.RGBUpdate;
            p.instructions[2].parameters = new object[] { controllerIndex, 66, 135, 245 };

            Send(p);
            statusLbl.Text = "Status: Closing";
        }
    }
}
