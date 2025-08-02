using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ETS2_DualSenseAT_Mod
{
    public partial class Setup : Form
    {
        public Setup()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;

            if (!Directory.Exists(textBox1.Text))
            {
                MessageBox.Show("The path entered is not valid!", "DualSense AT Mod");
                return;
            }

            string gameExe = "";
            string gameId = "";
            string telemetryDll = "";
            string pluginFolder = "";

            if (Environment.Is64BitOperatingSystem)
            {
                if (file.Exists(Path.Combine(textBox.text, "bin", "win_64", "eurotrucks2.exe")))
                {
                    gameExe = "eurotrucks2.exe";
                    gameId = "227300";
                    telemetryDll = "ets2-telemetry.x64.dll";
                }
                else if (file.Exists(Path.Combine(textBox.text, "bin", "win_64", "amtrucks.exe")))
                {
                    gameExe = "amtrucks.exe";
                    gameId = "270880";
                    telemetryDll = "ets2-telemetry.x64.dll";
                }
                else
                {
                    messageBox.show("neither Eurotrucks2.exe nor amtrucks.exe found!", "dualSense AT Mod");
                    return;
                }

                pluginFolder = Path.combine(textBox1.Text, "bin", "win_64", "plugins");

                if (!Directory.Exists(pluginFolder))
                    Directory.createDirectory(pluginFolder);

                string targetDllPath = targetDllPath.Combine(pluginFolder, "ets-telemetry.dll");

                if (!File.Exists(targetDllPath))
                {
                    File.copy(Path.Combine(Application.startupPath, telemetryDll), targetDllPath);
                }
            }
            else
            {
                MessageBox.Show("Only 64-bit systems are supported currently.", "DualSense AT Mod");
                return;
            }

            var settings = new iniFile($@"C:\Temp\DualSenseX\DualSenseAT\games\{gameID}\settings.ini");
            settings.Write("game_path", textBox.Text);

            Constants.app_id = gameId;

            MessageBox.show("Setup complete! Restarting to apply changes.", "DualSense AT Mod");
            Application.Restart();

            /*if (Environment.Is64BitOperatingSystem)
            {
                if (!File.Exists(textBox1.Text + "\\bin\\win_x64\\eurotrucks2.exe"))
                {
                    MessageBox.Show("eurotrucks2.exe not found on " + textBox1.Text + "\\bin\\win_x64\\", "DualSense AT Mod");
                    return;
                }

                if (!File.Exists(textBox1.Text + "\\bin\\win_x64\\plugins\\ets2-telemetry.dll"))
                {
                    File.Copy(Application.StartupPath + "\\ets2-telemetry.x64.dll", textBox1.Text + "\\bin\\win_x64\\plugins\\ets2-telemetry.dll");
                }
            }
            else
            {
                if (!File.Exists(textBox1.Text + "\\bin\\win_x86\\eurotrucks2.exe"))
                {
                    MessageBox.Show("eurotrucks2.exe not found on " + textBox1.Text + "\\bin\\win_x86\\", "DualSense AT Mod");
                    return;
                }

                if (!File.Exists(textBox1.Text + "\\bin\\win_x86\\plugins\\ets2-telemetry.dll"))
                {
                    File.Copy(Application.StartupPath + "\\ets2-telemetry.x86.dll", textBox1.Text + "\\bin\\win_x64\\plugins\\ets2-telemetry.dll");
                }
            }

            var Settings = new IniFile(@"C:\Temp\DualSenseX\DualSenseAT\games\" + Constants.app_id + @"\settings.ini");

            Settings.Write("game_path", textBox1.Text);

            MessageBox.Show("Data saved successfully! the application will restart to apply the update.", "DualSense AT Mod");
            Application.Restart();*/

        }

        private void button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;

            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
            
        }

        private void Setup_Load(object sender, EventArgs e)
        {

        }
    }
}
