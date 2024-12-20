using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Receiver
{
    internal class ManageFirewallRule
    {
        public static void AddFirewallRule(string senderIP)
        {
            string ruleName = "Allow HTTP from specific IP";
            string direction = "in";
            string action = "allow";
            string protocol = "TCP";
            string localPort = "8888";
            string profile = "any";

            try
            {
                if(FirewallRuleExists())
                {
                    RemoveFirewallRule();
                }
                string command = $"advfirewall firewall add rule name=\"{ruleName}\" " +
                    $"dir={direction} action={action} protocol={protocol} localport={localPort} remoteip={senderIP} profile={profile}";

                Process process = new();
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = command;
                process.StartInfo.Verb = "runas";
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine($"Firewall rule: {ruleName} added successfully");
                }
                else
                {
                    Console.WriteLine($"Failed to add firewall rule: {ruleName}. Exit code: {process.ExitCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static bool FirewallRuleExists()
        {
            string ruleName = "Allow HTTP from specific IP";
            try
            {
                //advfirewall firewall show rule name =\"Allow HTTP from specific IP\"
                Process process = new();
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = $"advfirewall firewall show rule name=\"{ruleName}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Check if the output contains the rule name
                return output.Contains(ruleName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while checking for rule: {ex.Message}");
                return false;
            }
        }

        public static void RemoveFirewallRule()
        {
            string ruleName = "Allow HTTP from specific IP";

            try
            {
                Process process = new();
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = $"advfirewall firewall delete rule name=\"{ruleName}\"";
                process.StartInfo.Verb = "runas";
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine($"Firewall rule '{ruleName}' successfully deleted.");
                }
                else
                {
                    Console.WriteLine($"Failed to delete firewall rule {ruleName}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
