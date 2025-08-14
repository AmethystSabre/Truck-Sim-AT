using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SCSSdkClient;
using SCSSdkClient.Object;

namespace TruckSimAT
{
    class Program
    {
        static UdpClient? udpClient;
        static IPEndPoint? dsxEndpoint;

        static void Main(string[] args)
        {
            // Setup UDP client to send data to DualSenseX
            try
            {
                string portFile = @"C:\Temp\DualSenseX\DualSenseX_PortNumber.txt";
                int dsxPort = int.Parse(System.IO.File.ReadAllText(portFile));
                dsxEndpoint = new IPEndPoint(IPAddress.Loopback, dsxPort);
                udpClient = new UdpClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to setup UDP client: " + ex.Message);
                return;
            }

            // Setup telemetry client
            var telemetry = new SCSSdkTelemetry();
            telemetry.Data += Telemetry_Data;

            Console.WriteLine("Listening for telemetry data... Press Enter to quit.");
            Console.ReadLine();

            // Cleanup
            telemetry.Dispose();
            udpClient?.Close();
        }

        private static void Telemetry_Data(SCSTelemetry data, bool updated)
        {
            if (!updated) return;

            try
            {
                var packet = new Packet
                {
                    instructions = new Instruction[2]
                };

                int controllerIndex = 0;
                float speedKmh = data.TruckValues.CurrentValues.DashboardValues.Speed.Kph;
                float brakeInput = data.ControlValues?.InputValues?.Brake ?? 0f;

                if (data.Paused)
                {
                    // Normal mode for both triggers when paused
                    packet.instructions[0] = new Instruction
                    {
                        type = "TriggerUpdate",
                        parameters = new object[] { controllerIndex, (int)Trigger.Right, (int)TriggerMode.Normal }
                    };
                    packet.instructions[1] = new Instruction
                    {
                        type = "TriggerUpdate",
                        parameters = new object[] { controllerIndex, (int)Trigger.Left, (int)TriggerMode.Normal }
                    };
                }
                else
                {
                    // --- Right trigger: Accelerator ---
                    if (speedKmh < 1)
                    {
                        // Idle vibration
                        packet.instructions[0] = new Instruction
                        {
                            type = "TriggerUpdate",
                            parameters = new object[]
                            {
                                controllerIndex,
                                (int)Trigger.Right,
                                (int)TriggerMode.Machine,
                                new List<int> { 0, 9, 3, 3, 10, 2 }
                            }
                        };
                    }
                    else if (speedKmh < 40)
                    {
                        int resistance = Math.Max(1, 10 - (int)(speedKmh / 3));
                        packet.instructions[0] = new Instruction
                        {
                            type = "TriggerUpdate",
                            parameters = new object[]
                            {
                                controllerIndex,
                                (int)Trigger.Right,
                                (int)TriggerMode.Resistance,
                                0,
                                resistance
                            }
                        };
                    }
                    else if (speedKmh > 41)
                    {
                        packet.instructions[0] = new Instruction
                        {
                            type = "TriggerUpdate",
                            parameters = new object[]
                            {
                                controllerIndex,
                                (int)Trigger.Right,
                                (int)TriggerMode.Resistance,
                                4, 2
                            }
                        };
                    }

                    // --- Left trigger: Brake ---
                    if (brakeInput > 0.1f)
                    {
                        packet.instructions[1] = new Instruction
                        {
                            type = "TriggerUpdate",
                            parameters = new object[]
                            {
                                controllerIndex,
                                (int)Trigger.Left,
                                (int)TriggerMode.Machine,
                                new List<int> { 0, 9, 4, 3 ,19, 2 }
                            }
                        };
                    }
                    else
                    {
                        packet.instructions[1] = new Instruction
                        {
                            type = "TriggerUpdate",
                            parameters = new object[] { controllerIndex, (int)Trigger.Left, (int)TriggerMode.Normal }
                        };
                    }
                }

                SendPacket(packet);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing telemetry data: " + ex.Message);
            }
        }

        private static void SendPacket(Packet packet)
        {
            if (udpClient == null || dsxEndpoint == null)
            {
                Console.WriteLine("UDP client or DSX endpoint is not initialized.");
                return;
            }

            string json = Triggers.PacketToJson(packet);

            Console.WriteLine("Sending JSON to DSX:");
            Console.WriteLine(json);

            byte[] bytes = Encoding.UTF8.GetBytes(json);
            udpClient.Send(bytes, bytes.Length, dsxEndpoint);
        }
    }
}
